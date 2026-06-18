namespace Viklover.Seaweed;
/// <summary>
///     SeaweedFS exception
/// </summary>
public class SeaweedException : Exception {
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="message">Message</param>
    public SeaweedException(string message) : base(message) {}
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="innerException">Inner exception</param>
    public SeaweedException(string message, Exception innerException) : base(message, innerException) {}
}
