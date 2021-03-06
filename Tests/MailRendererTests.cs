using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RG.RazorMail;
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
	}
}
