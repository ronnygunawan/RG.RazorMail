using System.Reflection;

namespace RG.RazorMail {
	/// <summary>
	/// Options for configuring RazorMailRenderer
	/// </summary>
	public class RazorMailRendererOptions {
		/// <summary>
		/// Assembly containing the .cshtml View files
		/// </summary>
		public Assembly? ViewsAssembly { get; set; }
	}
}
