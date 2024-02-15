# RG.RazorMail

[![NuGet](https://img.shields.io/nuget/v/RG.RazorMail.svg)](https://www.nuget.org/packages/RG.RazorMail/) [![.NET](https://github.com/ronnygunawan/RG.RazorMail/actions/workflows/CI.yml/badge.svg)](https://github.com/ronnygunawan/RG.RazorMail/actions/workflows/CI.yml)

Render razor view to html and inline css. This library simply wraps [RazorLight](https://github.com/toddams/RazorLight/) and [PreMailer.Net](https://github.com/milkshakesoftware/PreMailer.Net/)

## Setup

### 1. For Rendering `.cshtml` Views

#### 1.1. Add to service collection
```cs
services.AddRazorMail(options => {
    options.ViewAssembly = Assembly.GetEntryAssembly();
});
```

#### 1.2. Set `PreserveCompilationContext` to `true` in your `.csproj` file
```xml
<PropertyGroup>
  <PreserveCompilationContext>true</PreserveCompilationContext>
</PropertyGroup>
```

#### 1.3. Set build action of your razor views to `EmbeddedResource`

#### 1.4. Use relative path of your razor views as viewName
```cs
string html = await _razorMail.RenderAsync("Views/Home/Index.cshtml", model);
```

### 2. For Rendering `.razor` Components

#### 2.1. Add to service collection

```cs
services.AddRazorMail();
```

## Available methods

```cs
// Render razor view to string
Task<string> RenderAsync<T>(string viewName, T model);

// Render razor component to string
Task<string> RenderComponentAsync<TComponent>(IDictionary<string, object>? parameters);

// Render razor view to string and inline css
Task<string> RenderAndInlineCssAsync<T>(string viewName, T model, string? css = null);

// Render razor component to string and inline css
Task<string> RenderComponentAndInlineCssAsync<TComponent>(IDictionary<string, object>? parameters, string? css = null);

// Inline css only
string InlineCss(string html, string? css = null);
```

## Benchmark Results

Intel Core i7-10700 CPU 2.90GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101

| Method                           | Mean       | Error      | StdDev     | Rank | Gen0   | Gen1   | Allocated |
|--------------------------------- |-----------:|-----------:|-----------:|-----:|-------:|-------:|----------:|
| 'Render .cshtml View'            |  67.514 us |  0.8551 us |  0.5088 us |    3 | 0.8545 |      - |   7.06 KB |
| 'Render .razor Component'        |   2.808 us |  0.1450 us |  0.0759 us |    1 | 0.1984 | 0.0648 |   1.64 KB |
| 'Render+Inline .cshtml View'     | 184.443 us | 83.4985 us | 55.2291 us |    4 | 6.8359 | 0.4883 |  59.22 KB |
| 'Render+Inline .razor Component' |  43.990 us |  1.3244 us |  0.6927 us |    2 | 5.8594 | 1.7090 |  48.73 KB |