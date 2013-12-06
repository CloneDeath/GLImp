 using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;
using Img = System.Drawing.Imaging;
using System.IO;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GLImp {
	internal static class TextureManager
	{
		#region internal
		/// <summary>
		/// Initialize OpenGL state to enable alpha-blended texturing.
		/// Disable again with GL.Disable(EnableCap.Texture2D).
		/// Call this before drawing any texture, when you boot your
		/// application, eg. in OnLoad() of GameWindow or Form_Load()
		/// if you're building a WinForm app.
		/// </summary>
		internal static void InitTexturing() {
			GL.Disable(EnableCap.CullFace);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.Disable(EnableCap.Multisample);
			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
		}

		internal static int CreateTextureFromImage(System.Drawing.Image img)
		{
			Bitmap b = new Bitmap(img);
			return CreateTextureFromBitmap(b);
		}


		/// <summary>
		/// Create an OpenGL texture (translucent or opaque) from a given Bitmap.
		/// 24- and 32-bit bitmaps supported.
		/// </summary>
		internal static int CreateTextureFromBitmap(Bitmap bitmap, bool LinearFilter = true)
		{
			Img.BitmapData data = bitmap.LockBits(
			  new Rectangle(0, 0, bitmap.Width, bitmap.Height),
			  Img.ImageLockMode.ReadOnly,
			  bitmap.PixelFormat);
			  //Img.PixelFormat.Format32bppArgb);
			int x = GraphicsManager.Instance.X; //NOP, need to make sure graphics context is loaded.
			int tex = GL.GenTexture();

			GL.BindTexture(TextureTarget.Texture2D, tex);

			if (LinearFilter) {
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear); //Smooth
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			} else {
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest); //Pixely
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			}

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			OpenTK.Graphics.Glu.Build2DMipmap(OpenTK.Graphics.TextureTarget.Texture2D,
				(int)PixelInternalFormat.Rgba, data.Width, data.Height, OpenTK.Graphics.PixelFormat.Bgra,
				 OpenTK.Graphics.PixelType.UnsignedByte, data.Scan0);
			bitmap.UnlockBits(data);
			return tex;
		}

		/// <summary>
		/// Create an OpenGL texture (translucent or opaque) by loading a bitmap
		/// from file. 24- and 32-bit bitmaps supported.
		/// </summary>
		internal static int CreateTextureFromFile(string path, bool LinearFilter)
		{
			try {
				return CreateTextureFromBitmap(new Bitmap(Bitmap.FromFile(path)), LinearFilter);
			} catch(Exception e) {
				MessageBox.Show("Missing Texture: [" + e.Message + "]");
				return Texture.Error;
			}
		}
		#endregion
	}
}
