# RG.RazorMail

[![NuGet](https://img.shields.io/nuget/v/RG.RazorMail.svg)](https://www.nuget.org/packages/RG.RazorMail/) [![.NET](https://github.com/ronnygunawan/RG.RazorMail/actions/workflows/CI.yml/badge.svg)](https://github.com/ronnygunawan/RG.RazorMail/actions/workflows/CI.yml)

Render razor view to html and inline css. This library simply wraps [RazorLight](https://github.com/toddams/RazorLight/) and [PreMailer.Net](https://github.com/milkshakesoftware/PreMailer.Net/)

## Setup
#### 1. Add to service collection
```cs
services.AddRazorMail(options => {
    options.ViewAssembly = Assembly.GetEntryAssembly();
});
```

#### 2. Set `PreserveCompilationContext` to `true` in your `.csproj` file
```
<PropertyGroup>
  <PreserveCompilationContext>true</PreserveCompilationContext>
</PropertyGroup>
```

#### 3. Set build action of your razor views to `EmbeddedResource`

#### 4. Use relative path of your razor views as viewName
```
string html = await _razorMail.RenderAsync("Views/Home/Index.cshtml", model);
```

## Available methods

```cs
// Render razor view to string
Task<string> RenderAsync<T>(string viewName, T model);

// Render razor view to string and inline css
Task<string> RenderAndInlineCssAsync<T>(string viewName, T model, string? css = null);

// Inline css only
string InlineCss(string html, string? css = null);
```
