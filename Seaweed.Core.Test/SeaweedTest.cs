namespace Viklover.Seaweed.Core.Test;
/// <summary>
///     Abstract test to share utility methods
/// </summary>
public abstract class SeaweedTest {
    protected int GenerateInt() => Random.Shared.Next();
    protected int GenerateInt(int min, int max) => Random.Shared.Next(min, max);
    protected byte GenerateByte() => GenerateInt() % 2 == 0 ? (byte) 0x00 : (byte) 0x01;
    protected byte[] GenerateByteArray() {
        var count = GenerateInt(1, 100);
        var array = new byte[count];
        for (var i = 0; i < count; ++i) {
            array[i] = GenerateByte();
        }
        return array;
    }
}
