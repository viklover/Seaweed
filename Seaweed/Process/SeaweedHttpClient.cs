using System.Net;
using Newtonsoft.Json.Linq;
using NewtonsoftJsonHelper;
using Viklover.Seaweed.Model;

namespace Viklover.Seaweed.Process;
/// <summary>
///     SeaweedFS http client
/// </summary>
public class SeaweedHttpClient : ISeaweedClient, IDisposable {
    private readonly HttpClient _client = new();
    private readonly string? _collection;
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="masterServerUri">Base address to master server</param>
    /// <param name="collection">Collection name (optional)</param>
    public SeaweedHttpClient(Uri masterServerUri, string? collection = null) {
        _client.BaseAddress = masterServerUri;
        _collection = collection;
    }
    /// <summary>
    ///     Submit new file to SeaweedFS in async manner (POST /submit)
    /// </summary>
    /// <param name="file">File content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file submission</returns>
    public async Task<SeaweedFileId> SubmitAsync(byte[] file, CancellationToken cancellationToken) {
        using var form = new MultipartFormDataContent();
        using var requestContent = new ByteArrayContent(file);
        form.Add(requestContent, "file", "document");
        using var request = new HttpRequestMessage(HttpMethod.Post, "/submit");
        request.Content = form;
        var responseJson = await ExecuteJsonAsync(request, cancellationToken);
        var fileId = ReadFileId(responseJson);
        return fileId;
    }
    /// <summary>
    ///     Assign a file key from SeaweedFS in async manner (GET /dir/assign)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file key assignment</returns>
    public async Task<(SeaweedFileId, SeaweedVolumeRoute)> AssignAsync(CancellationToken cancellationToken) {
        var parameters = new Dictionary<string, string?> {
            ["collection"] = _collection
        };
        var query = BuildQueryUri("/dir/assign", parameters);
        using var request = new HttpRequestMessage(HttpMethod.Get, query);
        var responseJson = await ExecuteJsonAsync(request, cancellationToken);
        var fileId = ReadFileId(responseJson);
        var routeRaw = responseJson.SelectStringOrThrow("$.url");
        var route = new Uri($"http://{routeRaw}");
        var volumeRoute = new SeaweedVolumeRoute(route);
        return (fileId, volumeRoute);
    }
    /// <summary>
    ///     Upload file content by identifier in async manner
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="content">File content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file uploading</returns>
    public async Task UploadAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, byte[] content, CancellationToken cancellationToken) {
        var fileUri = new Uri(route.Route, fileId.ToString());
        using var form = new MultipartFormDataContent();
        using var request = new HttpRequestMessage(HttpMethod.Post, fileUri);
        using var requestContent = new ByteArrayContent(content);
        form.Add(requestContent, "file", "document");
        request.Content = form;
        await ExecuteJsonAsync(request, cancellationToken);
    }
    /// <summary>
    ///     Lookup volume routes by volume identifier in async manner (GET /dir/lookup)
    /// </summary>
    /// <param name="volumeId">Volume identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to lookup volume routes</returns>
    public async Task<SeaweedVolumeRoute[]> LookupAsync(int volumeId, CancellationToken cancellationToken) {
        var parameters = new Dictionary<string, string?> {
            ["volumeId"] = volumeId.ToString(),
            ["collection"] = _collection
        };
        var query = BuildQueryUri("/dir/lookup", parameters);
        using var request = new HttpRequestMessage(HttpMethod.Get, query);
        var response = await ExecuteJsonAsync(request, cancellationToken);
        var routeArray = response
            .SelectTokens("$.locations[::].url")
            .Select(token => token.ToObject<string>())
            .Select(urlRaw => new Uri($"http://{urlRaw}"))
            .Select(_ => new SeaweedVolumeRoute(_))
            .ToArray();
        return routeArray;
    }
    /// <summary>
    ///     Get file content from volume server in async manner (GET /{fid})
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file fetching</returns>
    public async Task<byte[]> FetchAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, CancellationToken cancellationToken) {
        var fileUri = new Uri(route.Route, fileId.ToString());
        using var request = new HttpRequestMessage(HttpMethod.Get, fileUri);
        using var response = await ExecuteAsync(request, cancellationToken);
        var result = new MemoryStream();
        await response.Content.CopyToAsync(result, cancellationToken);
        result.Seek(0, SeekOrigin.Begin);
        return result.ToArray();
    }
    /// <summary>
    ///     Check file existence in async manner
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file existence check</returns>
    public async Task<bool> ExistsFileAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, CancellationToken cancellationToken) {
        try {
            var fileUri = new Uri(route.Route, fileId.ToString());
            using var request = new HttpRequestMessage(HttpMethod.Head, fileUri);
            using var response = await ExecuteAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK) {
                return true;
            }
            throw new SeaweedResponseException(response.StatusCode, request.RequestUri);   
        } catch (SeaweedResponseException exception) {
            if (exception.StatusCode == HttpStatusCode.NotFound) {
                return false;
            }
            throw;
        }
    }
    /// <summary>
    ///     Delete file in async manner
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file deletion</returns>
    public async Task DeleteAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, CancellationToken cancellationToken) {
        var fileUri = new Uri(route.Route, fileId.ToString());
        using var request = new HttpRequestMessage(HttpMethod.Delete, fileUri);
        using var response = await ExecuteAsync(request, cancellationToken);
    }
    /// <summary>
    ///     Execute request and parse response json content in async manner
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to request execution</returns>
    private async Task<JToken> ExecuteJsonAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        using var response = await ExecuteAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseJson = JToken.Parse(responseContent);
        return responseJson;
    }
    /// <summary>
    ///     Execute request in async manner
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to request execution</returns>
    private async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        try {
            var response = await _client.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode == false) {
                response.Dispose();
                throw new SeaweedResponseException(response.StatusCode, request.RequestUri);
            }
            return response;
        } catch (HttpRequestException exception) {
            throw new SeaweedException($"Failed to send request: {request.RequestUri}", exception);
        }
    }
    /// <summary>
    ///     Build query uri
    /// </summary>
    /// <param name="path">Request path</param>
    /// <param name="args">Request arguments</param>
    /// <returns>Query string</returns>
    public static string BuildQueryUri(string path, Dictionary<string, string?> args) {
        var query = BuildQueryString(args);
        return $"{path}?{query}";
    }
    /// <summary>
    ///     Build query string
    /// </summary>
    /// <param name="queryStringArgs">Request args</param>
    /// <returns>Query string</returns>
    public static string BuildQueryString(Dictionary<string, string?> queryStringArgs) {
        var variables = new List<string>();
        var ordered = queryStringArgs.OrderBy(item => item.Key);
        foreach (var (key, value) in ordered) {
            if (value == null) {
                continue;
            }
            var encodedKey = Uri.EscapeDataString(key);
            var encodedValue = Uri.EscapeDataString(value);
            var variable = $"{encodedKey}={encodedValue}";
            variables.Add(variable);
        }
        return string.Join("&", variables);
    }
    /// <summary>
    ///     Read file identifier from json
    /// </summary>
    /// <param name="json">JSON</param>
    /// <returns>File identifier</returns>
    public static SeaweedFileId ReadFileId(JToken json) {
        var fileId = json.SelectStringOrThrow("$.fid");
        return new SeaweedFileId(fileId);
    }

    /// <summary>
    ///     Dispose umanaged resources
    /// </summary>
    public void Dispose() {
        _client.Dispose();
    }
    
}
