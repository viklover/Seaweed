using Viklover.Seaweed.Process;

namespace Viklover.Seaweed.Test;
/// <summary>
///     Integration tests to <see cref="SeaweedHttpClient"/>
/// </summary>
public class SeaweedHttpClientTest : SeaweedTest {
    private readonly Uri MasterUri = new("http://localhost:9333");
    private static readonly string Collection = "TEST";

    public SeaweedHttpClientTest() {
        var masterUriRaw = Environment.GetEnvironmentVariable("TEST_MASTER_URI")
            ?? throw new ArgumentNullException("Required 'TEST_MASTER_URI'");
        MasterUri = new Uri(masterUriRaw);
    }

    [Test(Description = "Test for correct file saving and reading")]
    public async Task SaveTest() {
        var client = new SeaweedHttpClient(MasterUri);
        var content = GenerateByteArray();
        var (fileId, route) = await client.AssignAsync(CancellationToken.None, Collection);
        await client.UploadAsync(route, fileId, content, CancellationToken.None);
        var readContent = await client.FetchAsync(route, fileId, CancellationToken.None);
        Assert.That(readContent, Is.EqualTo(content).AsCollection);
    }
    [Test(Description = "Тest for correct getting route to volume servers")]
    public async Task LookupVolumeRoutesTest() {
        var client = new SeaweedHttpClient(MasterUri);
        var content = GenerateByteArray();
        var (fileId, route) = await client.AssignAsync(CancellationToken.None, Collection);
        await client.UploadAsync(route, fileId, content, CancellationToken.None);
        var lookupRouteArray = await client.LookupAsync(fileId.VolumeId, CancellationToken.None, Collection);
        Assert.That(lookupRouteArray, Is.Not.Empty);
        var lookupRoute = lookupRouteArray[0];
        var readContent = await client.FetchAsync(lookupRoute, fileId, CancellationToken.None);
        Assert.That(readContent, Is.EqualTo(content).AsCollection);
    }
    [Test(Description = "Test for correct file deletion")]
    public async Task DeleteTest() {
        var client = new SeaweedHttpClient(MasterUri);
        var content = GenerateByteArray();
        var (fileId, route) = await client.AssignAsync(CancellationToken.None, Collection);
        await client.UploadAsync(route, fileId, content, CancellationToken.None);
        var readContent = await client.FetchAsync(route, fileId, CancellationToken.None);
        Assert.That(readContent, Is.EqualTo(content).AsCollection);
        await client.DeleteAsync(route, fileId, CancellationToken.None);
        Assert.ThrowsAsync<SeaweedResponseException>(() => client.FetchAsync(route, fileId, CancellationToken.None));
    }
    [Test(Description = "Test for correct file existance checking")]
    public async Task ExistsTest1() {
        var client = new SeaweedHttpClient(MasterUri);
        var content = GenerateByteArray();
        var (fileId, route) = await client.AssignAsync(CancellationToken.None, Collection);
        await client.UploadAsync(route, fileId, content, CancellationToken.None);
        var exists = await client.ExistsFileAsync(route, fileId, CancellationToken.None);
        Assert.That(exists, Is.True);
    }
    [Test(Description = "Test for correct file existance checking after deletion")]
    public async Task ExistsTest2() {
        var client = new SeaweedHttpClient(MasterUri);
        var content = GenerateByteArray();
        var (fileId, route) = await client.AssignAsync(CancellationToken.None, Collection);
        await client.UploadAsync(route, fileId, content, CancellationToken.None);
        var _ = await client.FetchAsync(route, fileId, CancellationToken.None);
        await client.DeleteAsync(route, fileId, CancellationToken.None);
        var exists = await client.ExistsFileAsync(route, fileId, CancellationToken.None);
        Assert.That(exists, Is.False);
    }
    [Test(Description = "Test for correct file submitting with collection parameter")]
    public async Task SubmitWithCollectionTest() {
        var client = new SeaweedHttpClient(MasterUri);
        var content = GenerateByteArray();
        var fileId = await client.SubmitAsync(content, CancellationToken.None, Collection);
        var lookupRouteArray = await client.LookupAsync(fileId.VolumeId, CancellationToken.None, Collection);
        Assert.That(lookupRouteArray, Is.Not.Empty);
        var lookupRoute = lookupRouteArray[0];
        var readContent = await client.FetchAsync(lookupRoute, fileId, CancellationToken.None);
        Assert.That(readContent, Is.EqualTo(content).AsCollection);
    }
    [Test(Description = "Test for correct file submitting without collection parameter")]
    public async Task SubmitWithoutCollectionTest() {
        var client = new SeaweedHttpClient(MasterUri);
        var content = GenerateByteArray();
        var fileId = await client.SubmitAsync(content, CancellationToken.None);
        var lookupRouteArray = await client.LookupAsync(fileId.VolumeId, CancellationToken.None);
        Assert.That(lookupRouteArray, Is.Not.Empty);
        var lookupRoute = lookupRouteArray[0];
        var readContent = await client.FetchAsync(lookupRoute, fileId, CancellationToken.None);
        Assert.That(readContent, Is.EqualTo(content).AsCollection);
    }
    [Test(Description = "Test for correct file submitting with collection and retrieving via lookup without collection")]
    public async Task SubmitWithCollectionLookupWithoutCollectionTest() {
        var client = new SeaweedHttpClient(MasterUri);
        var content = GenerateByteArray();
        var fileId = await client.SubmitAsync(content, CancellationToken.None, Collection);
        var lookupRouteArray = await client.LookupAsync(fileId.VolumeId, CancellationToken.None);
        Assert.That(lookupRouteArray, Is.Not.Empty);
        var lookupRoute = lookupRouteArray[0];
        var readContent = await client.FetchAsync(lookupRoute, fileId, CancellationToken.None);
        Assert.That(readContent, Is.EqualTo(content).AsCollection);
    }
}
