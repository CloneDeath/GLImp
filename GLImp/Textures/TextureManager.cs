using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp.PixelFormats;

namespace GLImp;

internal static class TextureManager {
	/// <summary>
	///     Initialize OpenGL state to enable alpha-blended texturing.
	///     Disable again with GL.Disable(EnableCap.Texture2D).
	///     Call this before drawing any texture, when you boot your
	///     application, eg. in OnLoad() of GameWindow or Form_Load()
	///     if you're building a WinForm app.
	/// </summary>
	internal static void InitTexturing() {
		GL.Disable(EnableCap.CullFace);
		GL.Enable(EnableCap.Texture2D);
		GL.Enable(EnableCap.Blend);
		GL.Disable(EnableCap.Multisample);
		GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
		GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
	}

	//internal static int CreateTextureFromImage(System.Drawing.Image img)
	//{
	//    Bitmap b = new Bitmap(img);
	//    return CreateTextureFromBitmap(b);
	//}

	/// <summary>
	///     Create an OpenGL texture (translucent or opaque) by loading a bitmap
	///     from file. 24- and 32-bit bitmaps supported.
	/// </summary>
	internal static int CreateTextureFromFile(string path, bool LinearFilter, bool Clamp) =>
		CreateTextureFromBitmap(SixLabors.ImageSharp.Image.Load(path), LinearFilter, Clamp);

	/// <summary>
	///     Create an OpenGL texture (translucent or opaque) from a given Bitmap.
	///     24- and 32-bit bitmaps supported.
	/// </summary>
	internal static int
		CreateTextureFromBitmap(SixLabors.ImageSharp.Image bitmap, bool LinearFilter, bool ClampToEdge) {
		var image = bitmap.CloneAs<Rgba32>();
		var pixelBytes = new byte[image.Width * image.Height * Unsafe.SizeOf<Rgba32>()];
		image.CopyPixelDataTo(pixelBytes);
		var x = GraphicsManager.Instance.ClientLocation.X; //NOP, need to make sure graphics context is loaded.
		var tex = GL.GenTexture();

		GL.BindTexture(TextureTarget.Texture2D, tex);

		if (LinearFilter) {
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int)TextureMagFilter.Linear); //Smooth
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int)TextureMinFilter.LinearMipmapLinear);
		} else {
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int)TextureMagFilter.Nearest); //Pixely
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int)TextureMinFilter.Nearest);
		}

		if (ClampToEdge) {
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR,
				(int)TextureWrapMode.Clamp); //Just Uses Last Pixel
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
		} else {
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR,
				(int)TextureWrapMode.Repeat); //Loops Texture
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
		}

		GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		GL.TexImage2D(TextureTarget.Texture2D, 0,
			PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte,
			pixelBytes);

		return tex;
	}
}