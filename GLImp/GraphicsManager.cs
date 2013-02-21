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
				return game.Width;
			}
			set {
				game.Width = value;
			}
		}
		public static int WindowHeight {
			get {
				return game.Height;
			}
			set {
				game.Height = value;
			}
		}

		private GraphicsManager(int x, int y)
            : base(x, y, new OpenTK.Graphics.GraphicsMode(32,24,8,4), "GLImp Game Window")
        {
            VSync = VSyncMode.On;
			CameraPos = Vector3d.Zero;
			CameraLook = Vector3d.UnitY;
        }

		private static Shader Billboard;

		public static bool UseExperimentalFullAlpha = false;

		#region ON GAME RUN METHODS
		/*****************************************************************
		 *						ON GAME RUN METHODS
		 *****************************************************************/
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			TextureManager.InitTexturing();
			InputManager.Init();
			GL.Enable(EnableCap.DepthTest);
			if (UseExperimentalFullAlpha) {
				GL.AlphaFunc(AlphaFunction.Always, 0f);
			} else {
				GL.AlphaFunc(AlphaFunction.Greater, 0.5f);
			}
			GL.Enable(EnableCap.AlphaTest);
		}

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
			//Console.Write("W: " + ClientRectangle.Width + " H: " + ClientRectangle.Height + "\n");
			
			GL.Viewport(this.ClientRectangle);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, ClientRectangle.Width, ClientRectangle.Height, 0, -1, 0);

            //GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			/*
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((double)Math.PI / 4, Width / (double)Height, 0.1f, 1000000.0f);
            GL.MatrixMode(MatrixMode.Projection);
			//GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref projection);
			*/
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
		private static Vector3d CameraPos;
		private static Vector3d CameraLook;
		/*****************************************************************
		 *								CAMERA
		 *****************************************************************/
		public static void SetCamera(Vector3d point)
		{
			CameraPos = point;
		}

		public static void SetLookAt(Vector3d point)
		{
			CameraLook = point;
		}

		public static void RotateCamera(double yaw, double pitch)
		{
			yaw = (double)(yaw * Math.PI / 180);
			pitch = (double)(pitch * Math.PI / 180);
			CameraLook = new Vector3d(	((Math.Cos(yaw) * Math.Cos(pitch)) + CameraPos.X),
										((Math.Sin(yaw) * Math.Cos(pitch)) + CameraPos.Y),
										(Math.Sin(pitch) + CameraPos.Z));
		}
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
		public void DrawCollisionBox(Vec3 c1, Vec3 c2) {
			if(c1 == null || c2 == null) {
				return;
			}
			Vector3d v1 = new Vector3d(c1.x, c1.y, c1.z);
			Vector3d v2 = new Vector3d(c1.x, c2.y, c1.z);
			Vector3d v3 = new Vector3d(c2.x, c2.y, c1.z);
			Vector3d v4 = new Vector3d(c2.x, c1.y, c1.z);

			Vector3d v5 = new Vector3d(c1.x, c1.y, c2.z);
			Vector3d v6 = new Vector3d(c1.x, c2.y, c2.z);
			Vector3d v7 = new Vector3d(c2.x, c2.y, c2.z);
			Vector3d v8 = new Vector3d(c2.x, c1.y, c2.z);

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

		#region DISPLAY LIST
		/*****************************************************************
		 *							DISPLAY LIST
		 *****************************************************************/

		//Generates a list number
		public static int GenList(int NumberToAllocate) {
			return GL.GenLists(NumberToAllocate);
		}

		public static void BeginList(int ListNumber) {
			GL.NewList(ListNumber, ListMode.Compile);
		}

		public static void EndList() {
			GL.EndList();
		}

		public static void CallList(int ListNumber) {
			GL.CallList(ListNumber);
		}

		#endregion

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