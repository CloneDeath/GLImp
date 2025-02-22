using System;
using System.IO;

namespace GLImp;

internal static class Resources {
	//This should not be exposed to the outside world;
	public static string[] GetList() => typeof(Resources).Assembly.GetManifestResourceNames();

	public static Stream? GetStream(string FileName) =>
		typeof(Resources).Assembly.GetManifestResourceStream("GLImp." + FileName);

	public static string GetText(string FileName) {
		var stream = typeof(Resources).Assembly.GetManifestResourceStream("GLImp." + FileName) ??
					 throw new NullReferenceException();
		var reader = new StreamReader(stream);

		var result = reader.ReadToEnd();
		return result;
	}

	public static SixLabors.ImageSharp.Image GetPNG(string FileName) {
		var stream = typeof(Resources).Assembly.GetManifestResourceStream("GLImp." + FileName) ??
					 throw new NullReferenceException();
		var bmp = SixLabors.ImageSharp.Image.Load(stream);
		return bmp;
	}
}