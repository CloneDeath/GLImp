using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;
using SixLabors.ImageSharp;

namespace GLImp {
	internal static class Resources {
		//This should not be exposed to the outside world;
		public static string[] GetList() {
			return typeof(Resources).Assembly.GetManifestResourceNames();
		}

		public static Stream GetStream(string FileName) {
			return typeof(Resources).Assembly.GetManifestResourceStream("GLImp." + FileName);
		}

		public static string GetText(string FileName) {
			Stream stream = typeof(Resources).Assembly.GetManifestResourceStream("GLImp." + FileName);
			StreamReader reader = new StreamReader(stream);

			string result = reader.ReadToEnd();
			return result;
		}

		public static SixLabors.ImageSharp.Image GetPNG(string FileName) {
			Stream stream = typeof(Resources).Assembly.GetManifestResourceStream("GLImp." + FileName);
			SixLabors.ImageSharp.Image bmp = SixLabors.ImageSharp.Image.Load(stream);
			return bmp;
		}
	}
}
