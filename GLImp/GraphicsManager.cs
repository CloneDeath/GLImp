using System.Drawing;
using GLImp.Input;
using GLImp.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImp;

public partial class GraphicsManager : GameWindow {
	public static bool UseExperimentalFullAlpha { get; set; } = false;
	public static bool DisableDepthTest { get; set; } = false;

	private static GraphicsManager? game;

	private GraphicsManager(int x, int y)
		: base(new GameWindowSettings(),
			new NativeWindowSettings { ClientSize = new Vector2i(x, y), Title = "GLImp Game Window" })
	//: base(x, y, new GraphicsMode(32,24,8,4), )
	{
		VSync = VSyncMode.On;
	}

	public static int WindowWidth {
		get => Instance.ClientSize.X;
		set => Instance.ClientSize = new Vector2i(value, Instance.ClientSize.Y);
	}

	public static int WindowHeight {
		get => Instance.ClientSize.Y;
		set => Instance.ClientSize = new Vector2i(Instance.ClientSize.X, value);
	}

	/*****************************************************************
	 *								MISC
	 *****************************************************************/
	public static WindowState windowstate {
		get => Instance.WindowState;
		set => Instance.WindowState = value;
	}

	public static GraphicsManager Instance => game ??= new GraphicsManager(800, 600);

	//Draws an axis aligned bounding box
	public static void DrawCollisionBox(Vector3 c1, Vector3 c2) {
		var v1 = new Vector3d(c1.X, c1.Y, c1.Z);
		var v2 = new Vector3d(c1.X, c2.Y, c1.Z);
		var v3 = new Vector3d(c2.X, c2.Y, c1.Z);
		var v4 = new Vector3d(c2.X, c1.Y, c1.Z);

		var v5 = new Vector3d(c1.X, c1.Y, c2.Z);
		var v6 = new Vector3d(c1.X, c2.Y, c2.Z);
		var v7 = new Vector3d(c2.X, c2.Y, c2.Z);
		var v8 = new Vector3d(c2.X, c1.Y, c2.Z);

		GL.Disable(EnableCap.Texture2D);
		DrawLine(v1, v2, Color.DarkGreen);
		DrawLine(v2, v3, Color.Red);
		DrawLine(v3, v4, Color.DarkGreen);
		DrawLine(v4, v1, Color.Red);

		DrawLine(v5, v6, Color.DarkGreen);
		DrawLine(v6, v7, Color.Red);
		DrawLine(v7, v8, Color.DarkGreen);
		DrawLine(v8, v5, Color.Red);

		SetColor(Color.Blue);
		DrawLine(v1, v5);
		DrawLine(v2, v6);
		DrawLine(v3, v7);
		DrawLine(v4, v8);
		GL.Enable(EnableCap.Texture2D);
	}

	public static void SwapBuffer() {
		Instance.SwapBuffers();
	}

	public static SixLabors.ImageSharp.Image GetFont() => Resources.GetPNG("data.font.png");

	public static SixLabors.ImageSharp.Image GetError() => Resources.GetPNG("data.error.png");

	/// <summary>
	///     Starts the game. Same exact thing as OpenWindow.
	/// </summary>
	public static void Start(double TargetUpdateFPS = 60) {
		Instance.UpdateFrequency = TargetUpdateFPS;
		Instance.Run();
	}

	public static void SetWindowState(WindowState state) {
		Instance.WindowState = state;
	}

	public static WindowState GetWindowState() => Instance.WindowState;

	public static void SetTitle(string title) {
		Instance.Title = title;
	}

	public static void PushMatrix() {
		GL.PushMatrix();
	}

	public static void PopMatrix() {
		GL.PopMatrix();
	}

	public static void Translate(Vector3d displacement) {
		GL.Translate(displacement);
	}

	//Scale
	public static void Scale(double s) {
		Scale(s, s, s);
	}

	public static void Scale(double x, double y, double z) {
		Scale(new Vector3d(x, y, z));
	}

	public static void Scale(Vector3d v) {
		GL.Scale(v);
	}

	//Rotate
	public static void Rotate(double angle, Vector3d Up) {
		GL.Rotate(angle, Up);
	}

	#region ON GAME RUN METHODS
	/*****************************************************************
	 *						ON GAME RUN METHODS
	 *****************************************************************/
	protected override void OnLoad() {
		base.OnLoad();
		TextureManager.InitTexturing();
		InputManager.Init();
		if (DisableDepthTest) {
			GL.Disable(EnableCap.DepthTest);
		} else {
			GL.Enable(EnableCap.DepthTest);
		}

		if (UseExperimentalFullAlpha) {
			//GL.AlphaFunc(AlphaFunction.Always, 0f);
		} else {
			GL.AlphaFunc(AlphaFunction.Greater, 0.5f);
		}

		GL.Enable(EnableCap.AlphaTest);
	}

	protected override void OnResize(ResizeEventArgs e) {
		base.OnResize(e);

		GL.Viewport(ClientRectangle);
		GL.MatrixMode(MatrixMode.Projection);
		GL.LoadIdentity();
		GL.Ortho(0, ClientRectangle.Size.X, ClientRectangle.Size.Y, 0, -1, 0);

		OnWindowResize?.Invoke();
	}

	public delegate void Resizer();

	public static event Resizer? OnWindowResize;

	public delegate void Disposer();

	public static event Disposer? OnDispose;

	public new static void Close() {
		OnDispose?.Invoke();
		((GameWindow)Instance).Close();
	}
	#endregion

	#region set things
	/*****************************************************************
	 *							SET THINGS
	 *****************************************************************/
	public static void SetResolution(int Width, int Height) {
		Instance.ClientSize = new Vector2i(Width, Height);
	}

	public static void SetBackground(Color color) {
		GL.ClearColor(color);
	}

	public static void SetColor(Color c) {
		GL.Color4(c);
	}
	#endregion

	#region INPUT
	/*****************************************************************
	 *								INPUT
	 *****************************************************************/
	public static KeyboardCompatability keyboard => new KeyboardCompatability(Instance);

	public static MouseCompatability mouse => new MouseCompatability(Instance);
	#endregion
}