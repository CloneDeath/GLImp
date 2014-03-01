using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace GLImp {
	public class TileSheet {
		public int TileWidth;
		public int TileHeight;
		public Texture Texture;
		public int HOffset;
		public int VOffset;
		public int HSeparation;
		public int VSeparation;

		public TileSheet(Texture tex, int TileWidth, int TileHeight) {
			this.Texture = tex;
			this.TileWidth = TileWidth;
			this.TileHeight = TileHeight;
			this.HOffset = 0;
			this.VOffset = 0;
			this.HSeparation = 0;
			this.VSeparation = 0;
		}

		public int HorizontalTiles {
			get {
				return (Texture.Width - HOffset) / (TileWidth + HSeparation);
			}
		}

		public int VerticalTiles {
			get {
				return (Texture.Height - VOffset) / (TileHeight + VSeparation);
			}
		}

		public SubImage GetSubImage(int x, int y) {
			return Texture.Subimage(((TileWidth + HOffset) * x) + HOffset, ((TileHeight + VOffset) * y) + VOffset, TileWidth, TileHeight);
		}

		public SubImage this[int x, int y] {
			get {
				return this.GetSubImage(x, y);
			}
		}
	}
}
