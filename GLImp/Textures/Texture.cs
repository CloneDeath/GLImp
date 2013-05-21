using System;
using System.Drawing;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace GLImp
{
	public class Texture : Image
	{
		public static List<Texture> AllTextures = new List<Texture>();
		public static int Font = TextureManager.CreateTextureFromBitmap(GraphicsManager.GetFont());
		public static int Error = TextureManager.CreateTextureFromBitmap(GraphicsManager.GetError());

		public string Location;
		public string Name;
		public int ID;
		public double XOffset;
		public double YOffset;

		private Bitmap _img;
		public Bitmap Image {
			get {
				return _img;
			}
			set {
				_img = value;
				Reload();
			}
		}

		private void Reload() {
			GL.DeleteTexture(ID);

			ID = TextureManager.CreateTextureFromBitmap(Image);
			Width = Image.Width;
			Height = Image.Height;
		}

		public Texture(string location) {
			init(location, 0, 0);
		}

		public Texture(string location, int x_offset, int y_offset) {
			init(location, x_offset, y_offset);
		}

		private void init(string loc, int x, int y) {
			Location = loc;
			ID = TextureManager.CreateTextureFromFile(loc);
			Name = StripName(loc);
			AllTextures.Add(this);

			_img = new Bitmap(Bitmap.FromFile(loc));
			Width = Image.Width;
			Height = Image.Height;
			XOffset = x;
			YOffset = y;
		}

		public Texture(Bitmap img, string Name, int offsetX = 0, int offsetY = 0) {
			this._img = img;
			Location = "";
			ID = TextureManager.CreateTextureFromBitmap(img);
			this.Name = Name;
            Width = img.Width;
            Height = img.Height;
            XOffset = offsetX;
            YOffset = offsetY;

			AllTextures.Add(this);
		}

		~Texture() {
			GL.DeleteTexture(ID);
		}

		private string StripName(string s) {
			string[] temp = s.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
			return temp[temp.Length - 1].Split(new char[] { '.' })[0];
		}

		public override void Draw(double x, double y) {
			GraphicsManager.DrawRectangle(x - XOffset, y - YOffset, Width, Height, this);
		}

		public override void Draw(double x, double y, double width, double height) {
			GraphicsManager.DrawRectangle(x - XOffset, y - YOffset, width, height, this);
		}

		public Subimage Subimage(int x, int y, int width, int height) {
			return new Subimage(this, x, y, width, height);
		}
	}
}
