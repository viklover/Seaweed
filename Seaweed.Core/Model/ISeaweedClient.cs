namespace Viklover.Seaweed.Core.Model;
/// <summary>
///     SeaweedFS client
/// </summary>
public interface ISeaweedClient {
    /// <summary>
    ///     Upload file content in async manner
    /// </summary>
    /// <param name="file">File content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file uplaoding</returns>
    Task<SeaweedFileId> UploadAsync(byte[] file, CancellationToken cancellationToken);
    /// <summary>
    ///     Create file in async manner
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file creation</returns>
    Task<(SeaweedFileId, SeaweedVolumeRoute)> CreateFileAsync(CancellationToken cancellationToken);
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
    ///     Lookup volume routes by volume identifier in async manner
    /// </summary>
    /// <param name="volumeId">Volume identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to lookup volume routes</returns>
    Task<SeaweedVolumeRoute[]> LookupVolumeRoutesAsync(int volumeId, CancellationToken cancellationToken);
    /// <summary>
    ///     Get file content in async manner
    /// </summary>
    /// <param name="route">Volume route</param>
    /// <param name="fileId">File identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task to file downloading</returns>
    Task<byte[]> GetFileAsync(SeaweedVolumeRoute route, SeaweedFileId fileId, CancellationToken cancellationToken);
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
