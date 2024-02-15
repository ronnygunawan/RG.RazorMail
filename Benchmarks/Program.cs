using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using RG.RazorMail;
using Tests;
using Tests.Components;
using Tests.ViewModels;

BenchmarkRunner.Run<EmailRendererBenchmarks>();

[RankColumn, MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 10)]
public class EmailRendererBenchmarks {
	private static RazorMailRenderer? _renderer;
	private string? _html;

	[GlobalSetup]
	public void Setup() {
		ServiceCollection services = new();
		services.AddRazorMail(options => {
			options.ViewsAssembly = typeof(MailRendererTests).Assembly;
		});
		using ServiceProvider serviceProvider = services.BuildServiceProvider();
		_renderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
	}

	[Benchmark(Description = "Render .cshtml View")]
	public async Task RenderCshtmlAsync() {
		_html = await _renderer!.RenderAsync(
			viewName: "Views/HelloWorld.cshtml",
			model: new HelloWorldViewModel("John")
		);
	}

	[Benchmark(Description = "Render .razor Component")]
	public async Task RenderRazorAsync() {
		_html = await _renderer!.RenderComponentAsync<HelloWorld>(
			parameters: new Dictionary<string, object?> {
				{ "Name", "John" }
			}
		);
	}

	[Benchmark(Description = "Render+Inline .cshtml View")]
	public async Task RenderCshtmlAndInlineCssAsync() {
		_html = await _renderer!.RenderAndInlineCssAsync(
			viewName: "Views/HelloWorld.cshtml",
			model: new HelloWorldViewModel("John"),
			css: "p { color: darkslategray; }"
		);
	}

	[Benchmark(Description = "Render+Inline .razor Component")]
	public async Task RenderRazorAndInlineCssAsync() {
		_html = await _renderer!.RenderComponentAndInlineCssAsync<HelloWorld>(
			parameters: new Dictionary<string, object?> {
				{ "Name", "John" }
			},
			css: "p { color: darkslategray; }"
		);
	}
}
