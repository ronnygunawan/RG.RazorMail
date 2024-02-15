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
