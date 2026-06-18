namespace Viklover.Seaweed.Core.Model;
/// <summary>
///     Seaweed file identifier
/// </summary>
public class SeaweedFileId {
    /// <summary>
    ///     Volume identifier
    /// </summary>
    public int VolumeId { get; }
    /// <summary>
    ///     File identifier
    /// </summary>
    public string FileId { get; }
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="id">Id presented as string</param>
    public SeaweedFileId(string id) {
        var idSplit = id.Split(",");
        if (idSplit.Length != 2) {
            throw new SeaweedException($"Invalid id: {id}");
        }
        VolumeId = int.Parse(idSplit[0]);
        FileId = idSplit[1];
    }
    /// <summary>
    ///     Serialize to string
    /// </summary>
    public override string ToString() => $"{VolumeId},{FileId}";
    /// <summary>
    ///     Get hash code
    /// </summary>
    public override int GetHashCode() => FileId.GetHashCode();
    /// <summary>
    ///     Compare identifier with another object
    /// </summary>
    /// <param name="obj">Object</param>
    /// <returns>Result boolean</returns>
    public override bool Equals(object? obj) {
        if (obj is SeaweedFileId fileId) {
            return FileId == fileId.FileId;
        }
        return false;
    }
}
