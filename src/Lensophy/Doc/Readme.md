# Introduction
This API provides developers with the ability to enhance their comment systems (such as forums, blogs, or websites) by analyzing, filtering, and encouraging good behavior through suggested responses that guide towards a friendly perspective rather than censoring it. 

An [OpenAI account](https://platform.openai.com/signup) is required to use it

# Getting Started
Installing the package with the desired version (last version is always recommended).
```
dotnet add package Lensophy --version X.Y.Z
```

In your `appSettings.json`, add the following configuration:

```
{
  "OpenAiConfig": {
    "Secret": "your OpenAi secret"
  }
}
```

In `Program.cs`, perform the following registration:

```
var builder = WebApplication.CreateBuilder(args);

...
//code hidden for brevity.

var secret = builder.Configuration.GetSection("openaiconfig:secret").Value;
builder.Services.AddLensophy(secret);
```

In the `SampleController`, inject the dependency:
```
[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly LensophyService _lensophyService;

    public SampleController(LensophyService lensophyService) => _lensophyService = lensophyService;
}
```

Call the `Analyse` routine at the desired place.

```
[HttpPost(Name = "Analyse")]
public async Task<ContentAnalysed> Analyse([FromBody]ContentAnalyse contentToAnalyse)
{
    var contentAnalysed = await _lensophyService.AnalyseAsync(contentToAnalyse).ConfigureAwait(false);
    return contentAnalysed;
}
```

# Release notes
Every version governs a basic principle of change, although it may carry other minor improvements.

### v1.0.0
**[![Lensophy last build](https://github.com/raphaelmoreira/lensophy/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/raphaelmoreira/lensophy/actions/workflows/dotnet.yml)**
[![Percentage of issues still open](http://isitmaintained.com/badge/open/raphaelmoreira/lensophy.svg)](http://isitmaintained.com/project/raphaelmoreira/lensophy "Percentage of issues still open")
[![Average time to resolve an issue](http://isitmaintained.com/badge/resolution/raphaelmoreira/lensophy.svg)](http://isitmaintained.com/project/raphaelmoreira/lensophy "Average time to resolve an issue")

- Provides developers with the ability to enhance their comment systems (such as forums, blogs, or websites) by analyzing, filtering, and encouraging good behavior through suggested responses that guide towards a friendly perspective

# Examples
A suggestion for your message system.

[![How to use it](https://dev.azure.com/raphaelmoreira/758a305a-a7fa-4b96-95aa-962c2cf2248c/_apis/git/repositories/27fc1054-4851-4e6d-af8a-a463841dc8c1/items?path=/how-to-use-it.png&versionDescriptor%5BversionOptions%5D=0&versionDescriptor%5BversionType%5D=0&versionDescriptor%5Bversion%5D=main&resolveLfs=true&%24format=octetStream)](Readme.md)

# Contributing & Feedback
If you encounter a bug or have a feature request, please use the [Issue Tracker](https://github.com/raphaelmoreira/lensophy/issues) or the [Project Board](https://github.com/users/raphaelmoreira/projects/3). The project is also open to contributions, so feel free to [fork the project](https://github.com/raphaelmoreira/lensophy/fork) and open pull requests. Contributions are highly appreciated! Please make sure it is covered by unit or integrations tests.

# About the project
Remind people that, even physically distant from the interlocutor, respect must be maintained. Check the [Lensowiki](https://github.com/raphaelmoreira/lensophy/wiki) if you wanna read more about it.

# License
Copyright © Raphael Moreira | MIT
