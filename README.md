# AmatoSplashPage

#Visible on:
https://giga487.github.io/AmatoSplashPage/

## Development

Requires the .NET 10 SDK and the WebAssembly build tools:

```shell
dotnet workload install wasm-tools
dotnet run --project AmatoFluent/AmatoFluent.csproj --urls http://localhost:5080
```

The SDK is constrained to .NET 10 by `global.json`. GitHub Pages publishes the
Release build using the same SDK major version and WebAssembly workload.

