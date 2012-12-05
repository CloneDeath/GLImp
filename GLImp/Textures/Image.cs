using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLImp {
	abstract public class Image {
		abstract public void Draw(float x, float y);
		abstract public void Draw(float x, float y, float width, float height);
	}
}
