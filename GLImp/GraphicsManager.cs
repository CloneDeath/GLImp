using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Resources;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace GLImp
{
    public partial class GraphicsManager : GameWindow
    {
		public static int WindowWidth {
			get {
				return Instance.Width;
			}
			set {
				Instance.Width = value;
			}
		}
		public static int WindowHeight {
			get {
				return Instance.Height;
			}
			set {
				Instance.Height = value;
			}
		}

		private GraphicsManager(int x, int y)
            : base(x, y, new OpenTK.Graphics.GraphicsMode(32,24,8,4), "GLImp Game Window")
        {
            VSync = VSyncMode.On;
        }

		public static bool UseExperimentalFullAlpha = false;
		public static bool DisableDepthTest = false;

		#region ON GAME RUN METHODS
		/*****************************************************************
		 *						ON GAME RUN METHODS
		 *****************************************************************/
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
			
			GL.Viewport(this.ClientRectangle);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, ClientRectangle.Width, ClientRectangle.Height, 0, -1, 0);

			if(OnWindowResize != null) {
				OnWindowResize();
			}
			
        }

		public delegate void Resizer();
		public static event Resizer OnWindowResize;

		

		public delegate void Disposer();
		public static event Disposer OnDispose;
		public static void Close() {
			if(OnDispose != null) {
				OnDispose();
			}
			Instance.Exit();
		}
		#endregion

		#region CAMERA
		/*****************************************************************
		 *								CAMERA
		 *****************************************************************/
		
		#endregion camera

		#region set things
		/*****************************************************************
		 *							SET THINGS
		 *****************************************************************/
		public static void SetResolution(int Width, int Height)
		{
			Instance.Width = Width;
			Instance.Height = Height;
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
        /// Starts the game.
        /// </summary>
        public static void OpenWindow()
        {
			Instance.Run(60.0);
        }
        /// <summary>
        /// Starts the game. Same exact thing as OpenWindow.
        /// </summary>
        public static void Start()
        {
            OpenWindow();
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
		public static KeyboardDevice keyboard {
			get {
				return Instance.Keyboard;
			}
		}
		public static MouseDevice mouse {
			get {
				return Instance.Mouse;
			}
		}
		public static IList<JoystickDevice> joysticks{
			get {
				return Instance.Joysticks;
			}
		}
		#endregion


	}
}