using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GLImp;

/// <summary>
///     Holds all the relevant information for a texture.
/// </summary>
public class Texture : Image {
	internal static int Font = TextureManager.CreateTextureFromBitmap(GraphicsManager.GetFont(), false, true);
	internal static int Error = TextureManager.CreateTextureFromBitmap(GraphicsManager.GetError(), false, true);
	private bool _clamp;
	private SixLabors.ImageSharp.Image? _img;
	private bool _linearFilter;

	/// <summary>
	///     User data associated with this texture. This is never touched by GLImp, and exists purely for user convenience.
	/// </summary>
	public object? UserData;

	/// <summary>
	///     Creates a new texture with the specified parameters.
	/// </summary>
	/// <param name="Location">Location to load the texture file from.</param>
	/// <param name="LinearFilter">Controls the linear filtering.</param>
	/// <param name="Clamp">Controls the clamping.</param>
	public Texture(string Location, bool LinearFilter = true, bool Clamp = false) {
		ID = TextureManager.CreateTextureFromFile(Location, LinearFilter, Clamp);
		_img = SixLabors.ImageSharp.Image.Load(Location);

		this.Location = Location;
		_linearFilter = LinearFilter;
		_clamp = Clamp;
	}

	/// <summary>
	///     Creates a new texture with the specified parameters.
	/// </summary>
	/// <param name="Image">Bitmap image to create a texture from.</param>
	/// <param name="LinearFilter">Controls the linear filtering.</param>
	/// <param name="Clamp">Controls the clamping.</param>
	public Texture(SixLabors.ImageSharp.Image Image, bool LinearFilter = true, bool Clamp = false) {
		ID = TextureManager.CreateTextureFromBitmap(Image, LinearFilter, Clamp);
		_img = Image;

		Location = null;
		_linearFilter = LinearFilter;
		_clamp = Clamp;
	}

	/// <summary>
	///     The location this file was loaded from. Null if the file was created from a Bitmap.
	/// </summary>
	public string? Location { get; private set; }

	/// <summary>
	///     The OpenGL generated TextureID.
	/// </summary>
	public int ID { get; private set; }

	/// <summary>
	///     Used to control the Linear Filter of this image.
	///     True = Smooth
	///     False = Pixely
	/// </summary>
	public bool LinearFilter {
		get => _linearFilter;
		set {
			_linearFilter = value;
			Reload();
		}
	}

	/// <summary>
	///     Used to control what happens if you draw beyond the edge of the image.
	///     True = Edge pixels are repeated (clamping the texture cooradinates at [0, 1])
	///     False = The texture is repeated.
	/// </summary>
	public bool Clamp {
		get => _clamp;
		set {
			_clamp = value;
			Reload();
		}
	}

	public SixLabors.ImageSharp.Image? Image {
		get => _img;
		set {
			_img = value;
			Reload();
		}
	}

	public int Width {
		get => _img?.Width ?? 0;
		set => throw new FieldAccessException(
				   "Can not set the width of a texture. The width of the bitmap must be changed.");
	}

	public int Height {
		get => _img?.Height ?? 0;
		set => throw new FieldAccessException(
				   "Can not set the height of a texture. The height of the bitmap must be changed.");
	}

	public void Draw(Vector2d Position, Vector2d Size) {
		GraphicsManager.DrawRectangle(Position.X, Position.Y, Size.X, Size.Y, this);
	}

	private void Reload() {
		if (Image == null) return;

		GL.DeleteTexture(ID);
		ID = TextureManager.CreateTextureFromBitmap(Image, LinearFilter, Clamp);
		Width = Image.Width;
		Height = Image.Height;
	}

	public SubImage Subimage(int x, int y, int width, int height) => new(this, x, y, width, height);

	public void Free() {
		GL.DeleteTexture(ID);
		ID = -1;
		_img = null;
		Width = 0;
		Height = 0;
		Location = string.Empty;
	}
}