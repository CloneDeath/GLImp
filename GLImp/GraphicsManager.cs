using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImp
{
    public partial class GraphicsManager : GameWindow
    {
		public static int WindowWidth {
			get {
				return Instance.ClientSize.X;
			}
			set {
				Instance.ClientSize = new Vector2i(value, Instance.ClientSize.Y);
			}
		}
		public static int WindowHeight {
			get {
				return Instance.ClientSize.Y;
			}
			set {
				Instance.ClientSize = new Vector2i(Instance.ClientSize.X, value);
			}
		}

		private GraphicsManager(int x, int y)
			: base(new GameWindowSettings(), new NativeWindowSettings{ ClientSize = new Vector2i(x, y), Title = "GLImp Game Window"})
            //: base(x, y, new GraphicsMode(32,24,8,4), )
        {
            VSync = VSyncMode.On;
        }

		public static bool UseExperimentalFullAlpha = false;
		public static bool DisableDepthTest = false;

		#region ON GAME RUN METHODS
		/*****************************************************************
		 *						ON GAME RUN METHODS
		 *****************************************************************/
		protected override void OnLoad()
		{
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

		protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

			GL.Viewport(this.ClientRectangle);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, ClientRectangle.Size.X, ClientRectangle.Size.Y, 0, -1, 0);

			OnWindowResize?.Invoke();
        }

		public delegate void Resizer();
		public static event Resizer? OnWindowResize;



		public delegate void Disposer();
		public static event Disposer? OnDispose;
		public static void Close() {
			OnDispose?.Invoke();
			((GameWindow)Instance).Close();
		}
		#endregion


		#region set things
		/*****************************************************************
		 *							SET THINGS
		 *****************************************************************/
		public static void SetResolution(int Width, int Height)
		{
			Instance.ClientSize = new Vector2i(Width, Height);
		}

		public static void SetBackground(Color color)
		{
			GL.ClearColor(color);
		}

		public static void SetColor(Color c) {
			GL.Color4(c);
		}
		#endregion



		/*****************************************************************
		 *								MISC
		 *****************************************************************/
		public static WindowState windowstate {
			get {
				return Instance.WindowState;
			}
			set {
				Instance.WindowState = value;
			}
		}

		//Draws an axis alligned bounding box
		public void DrawCollisionBox(Vector3 c1, Vector3 c2) {
			if(c1 == null || c2 == null) {
				return;
			}
			Vector3d v1 = new Vector3d(c1.X, c1.Y, c1.Z);
			Vector3d v2 = new Vector3d(c1.X, c2.Y, c1.Z);
			Vector3d v3 = new Vector3d(c2.X, c2.Y, c1.Z);
			Vector3d v4 = new Vector3d(c2.X, c1.Y, c1.Z);

			Vector3d v5 = new Vector3d(c1.X, c1.Y, c2.Z);
			Vector3d v6 = new Vector3d(c1.X, c2.Y, c2.Z);
			Vector3d v7 = new Vector3d(c2.X, c2.Y, c2.Z);
			Vector3d v8 = new Vector3d(c2.X, c1.Y, c2.Z);

			GL.Disable(EnableCap.Texture2D);
			GraphicsManager.DrawLine(v1, v2, Color.DarkGreen);
			GraphicsManager.DrawLine(v2, v3, Color.Red);
			GraphicsManager.DrawLine(v3, v4, Color.DarkGreen);
			GraphicsManager.DrawLine(v4, v1, Color.Red);

			GraphicsManager.DrawLine(v5, v6, Color.DarkGreen);
			GraphicsManager.DrawLine(v6, v7, Color.Red);
			GraphicsManager.DrawLine(v7, v8, Color.DarkGreen);
			GraphicsManager.DrawLine(v8, v5, Color.Red);

			GraphicsManager.SetColor(Color.Blue);
			GraphicsManager.DrawLine(v1, v5);
			GraphicsManager.DrawLine(v2, v6);
			GraphicsManager.DrawLine(v3, v7);
			GraphicsManager.DrawLine(v4, v8);
			GL.Enable(EnableCap.Texture2D);
		}


		public static void SwapBuffer()
		{
			Instance.SwapBuffers();
		}

		public static Bitmap GetFont() {
			return Resources.GetPNG("data.font.png");
		}

		public static Bitmap GetError() {
			return Resources.GetPNG("data.error.png");
		}

        /// <summary>
        /// Starts the game. Same exact thing as OpenWindow.
        /// </summary>
		public static void Start(double TargetUpdateFPS = 60) {
			Instance.UpdateFrequency = TargetUpdateFPS;
			Instance.Run();
        }

		public static void SetWindowState(WindowState state) {
			Instance.WindowState = state;
		}

		public static WindowState GetWindowState() {
			return Instance.WindowState;
		}

		public static void SetTitle(string title) {
			Instance.Title = title;
		}

		private static GraphicsManager game;
		public static GraphicsManager Instance
		{
			get
			{
				if (game == null)
				{
					game = new GraphicsManager(800, 600);
				}
				return game;
			}
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

		#region INPUT
		/*****************************************************************
		 *								INPUT
		 *****************************************************************/
		public static KeyboardState keyboard {
			get {
				return Instance.KeyboardState;
			}
		}
		public static MouseState mouse {
			get {
				return Instance.Mouse;
			}
		}
		#endregion


	}
}