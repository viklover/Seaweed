# SeaweedFS .NET Client

Simple HTTP client implementation for [SeaweedFS](https://github.com/seaweedfs/seaweedfs) API.

[![NuGet](https://badge.fury.io/nu/Viklover.Seaweed.svg)](https://www.nuget.org/packages/Viklover.Seaweed)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
[![CI/CD](https://github.com/viklover/seaweed/actions/workflows/integration-tests.yml/badge.svg)](https://github.com/viklover/seaweed/actions/workflows/integration-tests.yml)

## 📚 Features

- Complete file lifecycle: assign, upload, download, check existence, delete
- Single-step and two-step upload modes
- Volume server discovery via `/dir/lookup`
- Fully asynchronous API
- Strongly typed models
- Optional collection support for logical file grouping

## 🚀 Quick start

```shell
dotnet add package Viklover.Seaweed --version 1.0.0
```

```csharp
using Viklover.Seaweed.Process;

// Create client with master server URI
var masterUri = new Uri("http://localhost:9333");
using var client = new SeaweedHttpClient(masterUri);

// Read file from filesystem
byte[] fileContent = await File.ReadAllBytesAsync("path/to/file");

// Single-step submit (collection is optional)
SeaweedFileId fileId = await client.SubmitAsync(fileContent, cancellationToken, "MyCollection");

// Two-step: assign + upload (collection is optional on assign)
var (fileId, volumeRoute) = await client.AssignAsync(cancellationToken, "MyCollection");
await client.UploadAsync(volumeRoute, fileId, fileContent, cancellationToken);

// Download
byte[] content = await client.FetchAsync(volumeRoute, fileId, cancellationToken);

// Check existence
bool exists = await client.ExistsFileAsync(volumeRoute, fileId, cancellationToken);

// Delete
await client.DeleteAsync(volumeRoute, fileId, cancellationToken);

// Lookup volume routes
SeaweedVolumeRoute[] routes = await client.LookupAsync(fileId.VolumeId, cancellationToken, "MyCollection");
```

## 🛠️ Contribution

- Report bugs 🐛
- Suggest features 💡
- Submit pull requests 🔄

## License

[MIT](LICENSE)
