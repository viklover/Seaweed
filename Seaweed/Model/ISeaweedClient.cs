namespace Viklover.Seaweed.Model;
/// <summary>
///     SeaweedFS client
/// </summary>
public interface ISeaweedClient {
    /// <summary>
    ///     Submit new file to SeaweedFS in async manner (POST /submit)
    /// </summary>
    /// <param name="file">File content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="collection">Collection name (optional)</param>
    /// <returns>Async task to file submission</returns>
    Task<SeaweedFileId> SubmitAsync(byte[] file, CancellationToken cancellationToken, string? collection = null);
    /// <summary>
    ///     Assign a file key from SeaweedFS in async manner (GET /dir/assign)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="collection">Collection name (optional)</param>
    /// <returns>Async task to file key assignment</returns>
    Task<(SeaweedFileId, SeaweedVolumeRoute)> AssignAsync(CancellationToken cancellationToken, string? collection = null);
    /// <summary>
    ///     Upload file content by identifier in async manner
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="content">File content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file uploading</returns>
    Task UploadAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, byte[] content, CancellationToken cancellationToken);
    /// <summary>
    ///     Lookup volume routes by volume identifier in async manner (GET /dir/lookup)
    /// </summary>
    /// <param name="volumeId">Volume identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="collection">Collection name (optional)</param>
    /// <returns>Async task to lookup volume routes</returns>
    Task<SeaweedVolumeRoute[]> LookupAsync(int volumeId, CancellationToken cancellationToken, string? collection = null);
    /// <summary>
    ///     Get file content from volume server in async manner (GET /{fid})
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file fetching</returns>
    Task<byte[]> FetchAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, CancellationToken cancellationToken);
    /// <summary>
    ///     Check file existence in async manner
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file existence check</returns>
    Task<bool> ExistsFileAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, CancellationToken cancellationToken);
    /// <summary>
    ///     Delete file in async manner
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file deletion</returns>
    Task DeleteAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, CancellationToken cancellationToken);
}
