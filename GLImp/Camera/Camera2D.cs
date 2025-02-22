using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GLImp;

public class Camera2D : Camera {
	private bool Centered;
	private Vector2d Position;
	private double Zoom;

	public void SetLocation(double x, double y) {
		Position = new Vector2d(x, y);
	}

	/// <summary>
	///     Set to true if the location of the camera is the center of the screen. Set it to false for the location to be the
	///     top right.
	/// </summary>
	/// <param name="c"></param>
	public void CenterOnTarget(bool c) {
		Centered = c;
	}

	/// <summary>
	///     Set the magnification of the camera. "2" will make everything twice as big.
	/// </summary>
	/// <param name="zoom">What you want to set the zoom level to.</param>
	public void SetZoom(double zoom) {
		Zoom = zoom;
	}

	public double GetZoom() => Zoom;
	//private double Rotation; //TODO nicholas implement this when we need it

	#region Constructors
	public Camera2D() : this(0, 0, 1) { }

	public Camera2D(double X, double Y) : this(X, Y, 1) { }

	public Camera2D(double X, double Y, double Zoom) {
		Position = new Vector2d(X, Y);
		this.Zoom = Zoom;
		Layer = 0;

		CameraManager.Add(this);
	}
	#endregion

	#region Drawing
	internal override void Draw(FrameEventArgs e) {
		GraphicsManager.PushMatrix();
		BeginOrtho(Viewport.Width, Viewport.Height);

		GL.Scale(Zoom, Zoom, 1);
		if (Centered) {
			GL.Translate(Viewport.Width / 2.0 / Zoom, Viewport.Height / 2.0 / Zoom, 0);
		}

		GL.Translate(-Position.X, -Position.Y, 0);

		OnRender?.Invoke(e);

		GraphicsManager.PopMatrix();
	}

	private static void BeginOrtho(double width, double height) {
		GL.Disable(EnableCap.DepthTest);
		GL.BlendEquation(BlendEquationMode.FuncAdd);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		GL.Clear(ClearBufferMask.DepthBufferBit);
		GL.MatrixMode(MatrixMode.Projection);
		GL.LoadIdentity();
		GL.Ortho(0, width, height, 0, -1, 0);
		GL.MatrixMode(MatrixMode.Modelview);
		GL.LoadIdentity();
	}
	#endregion

	#region Properties
	public double X {
		get => Position.X;
		set => Position.X = value;
	}

	public double Y {
		get => Position.Y;
		set => Position.Y = value;
	}

	public double Width => Viewport.Width / Zoom;

	public double Height => Viewport.Height / Zoom;

	public double Top => Y;

	public double Left => X;

	public double Bottom => Y + Height;

	public double Right => X + Width;
	#endregion
}