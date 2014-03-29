using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace GLImp {
	internal static class Resources {
		//This should not be exposed to the outside world;
		public static string[] GetList() {
			return Assembly.GetAssembly(typeof(Resources)).GetManifestResourceNames();
		}

		public static Stream GetStream(string FileName) {
			return Assembly.GetAssembly(typeof(Resources)).GetManifestResourceStream("GLImp." + FileName);
		}

		public static string GetText(string FileName) {
			Stream stream = Assembly.GetAssembly(typeof(Resources)).GetManifestResourceStream("GLImp." + FileName);
			StreamReader reader = new StreamReader(stream);

			string result = reader.ReadToEnd();
			return result;
		}

		public static Bitmap GetPNG(string FileName) {
			Stream stream = Assembly.GetAssembly(typeof(Resources)).GetManifestResourceStream("GLImp." + FileName);
			Bitmap bmp = new Bitmap(stream);
			return bmp;
		}
	}
}
