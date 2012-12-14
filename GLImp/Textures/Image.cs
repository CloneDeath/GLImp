using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLImp {
	abstract public class Image {
		public int Width;
		public int Height;
		abstract public void Draw(double x, double y);
		abstract public void Draw(double x, double y, double width, double height);
	}
}
