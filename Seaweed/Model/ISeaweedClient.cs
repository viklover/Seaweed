namespace Viklover.Seaweed.Model;
/// <summary>
///     SeaweedFS client
/// </summary>
public interface ISeaweedClient {
    /// <summary>
    ///     Submit new file to SeaweedFS in async manner (POST /submit)
    /// </summary>
    /// <param name="collection">Collection name</param>
    /// <param name="file">File content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file submission</returns>
    Task<SeaweedFileId> SubmitAsync(string collection, byte[] file, CancellationToken cancellationToken);
    /// <summary>
    ///     Assign a file key from SeaweedFS in async manner (GET /dir/assign)
    /// </summary>
    /// <param name="collection">Collection name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file key assignment</returns>
    Task<(SeaweedFileId, SeaweedVolumeRoute)> AssignAsync(string collection, CancellationToken cancellationToken);
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
    /// <param name="collection">Collection name</param>
    /// <param name="volumeId">Volume identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to lookup volume routes</returns>
    Task<SeaweedVolumeRoute[]> LookupAsync(string collection, int volumeId, CancellationToken cancellationToken);
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
