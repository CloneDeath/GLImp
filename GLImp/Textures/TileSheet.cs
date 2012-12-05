using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace GLImp {
	public class TileSheet {
		int TileWidth;
		int TileHeight;
		Texture texture;

		public TileSheet(Texture tex, int TileWidth, int TileHeight) {
			texture = tex;
			this.TileWidth = TileWidth;
			this.TileHeight = TileHeight;
		}

		public Subimage Get(int x, int y) {
			return texture.Subimage(TileWidth * x, TileHeight * y, TileWidth, TileHeight);
		}
	}
}
