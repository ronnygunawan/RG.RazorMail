using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RG.RazorMail;
using Tests.Components;
using Tests.ViewModels;
using Xunit;

namespace Tests {
	public class MailRendererTests {
		[Fact]
		public void CanBeInjected() {
			ServiceCollection services = new();
			services.AddRazorMail(options => {
				options.ViewsAssembly = Assembly.GetEntryAssembly();
			});
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
		}

		[Fact]
		public void CanBeRegisteredTwice() {
			ServiceCollection services = new();
			services.AddRazorMail(options => {
				options.ViewsAssembly = Assembly.GetEntryAssembly();
			});
			services.AddRazorMail(options => {
				options.ViewsAssembly = Assembly.GetEntryAssembly();
			});
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
		}

		[Fact]
		public async Task CanRenderToStringAsync() {
			ServiceCollection services = new();
			services.AddRazorMail(options => {
				options.ViewsAssembly = typeof(MailRendererTests).Assembly;
			});
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
			string html = await razorMailRenderer.RenderAsync("Views/HelloWorld.cshtml", new HelloWorldViewModel("John"));
			html.Should().Be("<p>Hello John</p>");
		}

		[Fact]
		public async Task CanRenderComponentToStringAsync() {
			ServiceCollection services = new();
			services.AddRazorMail();
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
			string html = await razorMailRenderer.RenderComponentAsync<HelloWorld>(new Dictionary<string, object?> {
				{ "Name", "John" }
			});
			html.Should().Be("<p>Hello John</p>");
		}

		[Fact]
		public async Task CanRenderToStringAndInlineCssAsync() {
			ServiceCollection services = new();
			services.AddRazorMail(options => {
				options.ViewsAssembly = typeof(MailRendererTests).Assembly;
			});
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
			string html = await razorMailRenderer.RenderAndInlineCssAsync("Views/HelloCss.cshtml", new HelloWorldViewModel("John"));
			html.Should().Be(
				"<html><head>\n" +
				"        <title>Hello Css</title>\n" +
				"        \n" +        
				"    </head>\n" +
				"    <body>\n" +
				"        <p style=\"color: darkslategray\">Hello John</p>\n" +
				"    \n" +
				"</body></html>"
			);
		}

		[Fact]
		public async Task CanRenderComponentToStringAndInlineCssAsync() {
			ServiceCollection services = new();
			services.AddRazorMail();
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
			string html = await razorMailRenderer.RenderComponentAndInlineCssAsync<HelloCss>(new Dictionary<string, object?> {
				{ "Name", "John" }
			});
			html.Should().Be(
				"<html><head><title>Hello Css</title>\n" +
				"        </head>\n" +
				"    <body><p style=\"color: darkslategray\">Hello John</p></body></html>"
			);
		}

		[Fact]
		public async Task CanPreserveStyleTagUsingDataPremailerIgnoreAsync() {
			ServiceCollection services = new();
			services.AddRazorMail(options => {
				options.ViewsAssembly = typeof(MailRendererTests).Assembly;
			});
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
			string html = await razorMailRenderer.RenderAndInlineCssAsync("Views/HelloCustomFont.cshtml", new HelloWorldViewModel("John"));
			html.Should().Be(
				"<html><head>\n" +
				"        <title>Hello Custom Font</title>\n" +
				"        <style type=\"text/css\" data-premailer=\"ignore\">\n" +
				"            @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@600&display=swap');\n" +
				"        </style>\n" +
				"        \n" +
				"    </head>\n" +
				"    <body>\n" +
				"        <p style=\"color: darkslategray;font-family: 'Poppins', sans-serif\">Hello John</p>\n" +
				"    \n" +
				"</body></html>"
			);
		}

		[Fact]
		public async Task CanRenderToStringAndInjectCssAsync() {
			ServiceCollection services = new();
			services.AddRazorMail(options => {
				options.ViewsAssembly = typeof(MailRendererTests).Assembly;
			});
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
			string html = await razorMailRenderer.RenderAndInlineCssAsync("Views/HelloWorld.cshtml", new HelloWorldViewModel("John"), css: "p { color: darkslategray; }");
			html.Should().Be("<html><head></head><body><p style=\"color: darkslategray\">Hello John</p></body></html>");
		}

		[Fact]
		public async Task CanRenderComponentToStringAndInjectCssAsync() {
			ServiceCollection services = new();
			services.AddRazorMail();
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			RazorMailRenderer razorMailRenderer = serviceProvider.GetRequiredService<RazorMailRenderer>();
			razorMailRenderer.Should().NotBeNull();
			string html = await razorMailRenderer.RenderComponentAndInlineCssAsync<HelloWorld>(
				parameters: new Dictionary<string, object?> {
					{ "Name", "John" }
				},
				css: "p { color: darkslategray; }"
			);
			html.Should().Be("<html><head></head><body><p style=\"color: darkslategray\">Hello John</p></body></html>");
		}
	}
}
