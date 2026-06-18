# SeaweedFS .NET Client

Simple HTTP client implementation for [SeaweedFS](https://github.com/seaweedfs/seaweedfs) API.

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
using Viklover.Seaweed.Core.Process;

// Create client with master server URI and optional collection name

var masterUri = new Uri("http://localhost:9333");
using var client = new SeaweedHttpClient(masterUri, "MyCollection");

// Single-step upload
var fileId = await client.UploadAsync(fileContent, cancellationToken);

// Two-step upload (allows custom volume selection)
var (assignedId, volumeRoute) = await client.CreateFileAsync(cancellationToken);
await client.UploadAsync(volumeRoute, assignedId, fileContent, cancellationToken);

// Download
var content = await client.GetFileAsync(volumeRoute, fileId, cancellationToken);

// Check existence
var exists = await client.ExistsFileAsync(volumeRoute, fileId, cancellationToken);

// Delete
await client.DeleteAsync(volumeRoute, fileId, cancellationToken);

// Lookup volume routes
var routes = await client.LookupVolumeRoutesAsync(fileId.VolumeId, cancellationToken);
```

## 🛠️ Contribution

- Report bugs 🐛
- Suggest features 💡
- Submit pull requests 🔄

## License

[MIT](LICENSE)
