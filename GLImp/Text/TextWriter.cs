using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GLImp;

//http://www.opentk.com/node/1554?page=1
class TextWriter {
	private readonly Font TextFont = new Font(SystemFonts.TryGet("roboto", out var font) ? font : throw new Exception(), 8);
	private readonly Image<Rgba32> TextBitmap;
	private List<PointF> _positions;
	private List<string> _lines;
	private List<Brush> _colours;
	private int _textureId;
	private Size _clientSize;

	public void Update(int ind, string newText) {
		if (ind < _lines.Count) {
			_lines[ind] = newText;
			UpdateText();
		}
	}


	public TextWriter(Size ClientSize, Size areaSize) {
		_positions = new List<PointF>();
		_lines = new List<string>();
		_colours = new List<Brush>();

		TextBitmap = new Image<Rgba32>(areaSize.Width, areaSize.Height);
		this._clientSize = ClientSize;
		_textureId = CreateTexture();
	}

	private int CreateTexture() {
		int textureId;
		GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);//Important, or wrong color on some computers
		var bitmap = TextBitmap;
		GL.GenTextures(1, out textureId);
		GL.BindTexture(TextureTarget.Texture2D, textureId);

		byte[] pixelBytes = new byte[bitmap.Width * bitmap.Height * Unsafe.SizeOf<Rgba32>()];
		bitmap.CopyPixelDataTo(pixelBytes);
		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixelBytes);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		//    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
		//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
		GL.Finish();
		return textureId;
	}

	public void Dispose() {
		if (_textureId > 0)
			GL.DeleteTexture(_textureId);
	}

	public void Clear() {
		_lines.Clear();
		_positions.Clear();
		_colours.Clear();
	}

	public void AddLine(string s, PointF pos, Brush col) {
		_lines.Add(s);
		_positions.Add(pos);
		_colours.Add(col);
		UpdateText();
	}

	public void UpdateText() {
		if (_lines.Count > 0) {
			TextBitmap.Mutate(gfx => {
				gfx.Clear(Color.Black);
				for (int i = 0; i < _lines.Count; i++)
					gfx.DrawText(_lines[i], TextFont, _colours[i], _positions[i]);
			});

			GL.BindTexture(TextureTarget.Texture2D, _textureId);

			byte[] pixelBytes = new byte[TextBitmap.Width * TextBitmap.Height * Unsafe.SizeOf<Rgba32>()];
			TextBitmap.CopyPixelDataTo(pixelBytes);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, TextBitmap.Width, TextBitmap.Height, PixelFormat.Bgra, PixelType.UnsignedByte, pixelBytes);
		}
	}

	public void Draw() {
		GL.PushMatrix();
		GL.LoadIdentity();

		Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, _clientSize.Width, _clientSize.Height, 0, -1, 1);
		GL.MatrixMode(MatrixMode.Projection);

		GL.PushMatrix();//
		GL.LoadMatrix(ref ortho_projection);

		//GL.Enable(EnableCap.Blend);
		//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.DstAlpha);
		//GL.Enable(EnableCap.Texture2D);
		GL.BindTexture(TextureTarget.Texture2D, _textureId);


		GL.Begin(PrimitiveType.Quads);
		GL.TexCoord2(0, 0); GL.Vertex2(0, 0);
		GL.TexCoord2(1, 0); GL.Vertex2(TextBitmap.Width, 0);
		GL.TexCoord2(1, 1); GL.Vertex2(TextBitmap.Width, TextBitmap.Height);
		GL.TexCoord2(0, 1); GL.Vertex2(0, TextBitmap.Height);
		GL.End();
		GL.PopMatrix();

		//GL.Disable(EnableCap.Blend);
		GL.Disable(EnableCap.Texture2D);

		GL.MatrixMode(MatrixMode.Modelview);
		GL.PopMatrix();
	}
}