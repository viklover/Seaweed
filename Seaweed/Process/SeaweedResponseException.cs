using System.Net;
using Viklover.Seaweed;

namespace Viklover.Seaweed.Process;
/// <summary>
///     Response exception
/// </summary>
public class SeaweedResponseException : SeaweedException {
    /// <summary>
    ///     Status code
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="code">Http status code</param>
    /// <param name="query">Uri of request</param>
    public SeaweedResponseException(
        HttpStatusCode code, 
        Uri? query
    ) : base(
        $"Unexpected response status code = {code} (query: {query})"
    ) {
        StatusCode = code;
    }
}
