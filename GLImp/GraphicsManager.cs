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

		public GraphicsManager(int x, int y)
            : base(x, y, new OpenTK.Graphics.GraphicsMode(32,24,8,4), "GLImp Game Window")
        {
            VSync = VSyncMode.On;
			CameraPos = Vector3.Zero;
			CameraLook = Vector3.UnitY;
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
			KeyboardManager.Init();
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
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 0.1f, 1000000.0f);
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

		public delegate void Updater();
		public static event Updater Update;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

			if(Update != null) {
				Update();
			}

			KeyboardManager.Update();
        }

		public delegate void Disposer();
		public static event Disposer OnDispose;
		public static void Close() {
			if(OnDispose != null) {
				OnDispose();
			}
			Instance.Exit();
		}

		private static Vector3 CameraPos;
		private static Vector3 CameraLook;

		public delegate void Renderer();
		public static event Renderer Render;
		public static event Renderer Render2DPre;
		public static event Renderer Render2D;
		public static event Renderer Render2DPost;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 modelview = Matrix4.LookAt(CameraPos, CameraLook, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

			if(Render != null) {
				Render();
			}

			BeginOrtho(ClientRectangle.Width, ClientRectangle.Height);
				if(Render2DPre != null) {
					Render2DPre();
				}
				if(Render2D != null) {
					Render2D();
				}
				if(Render2DPost != null) {
					Render2DPost();
				}
			EndOrtho();
			
			GL.Flush();
			SwapBuffer();
        }
		#endregion

		#region CAMERA
		/*****************************************************************
		 *								CAMERA
		 *****************************************************************/
		public static void SetCamera(Vector3 point)
		{
			CameraPos = point;
		}

		public static void SetLookAt(Vector3 point)
		{
			CameraLook = point;
		}

		public static void RotateCamera(float yaw, float pitch)
		{
			yaw = (float)(yaw * Math.PI / 180);
			pitch = (float)(pitch * Math.PI / 180);
			CameraLook = new Vector3(	(float)((Math.Cos(yaw) * Math.Cos(pitch)) + CameraPos.X), 
										(float)((Math.Sin(yaw) * Math.Cos(pitch)) + CameraPos.Y), 
										(float)(Math.Sin(pitch) + CameraPos.Z));
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
			GL.Color3(c);
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
			Vector3 v1 = new Vector3(c1.x, c1.y, c1.z);
			Vector3 v2 = new Vector3(c1.x, c2.y, c1.z);
			Vector3 v3 = new Vector3(c2.x, c2.y, c1.z);
			Vector3 v4 = new Vector3(c2.x, c1.y, c1.z);

			Vector3 v5 = new Vector3(c1.x, c1.y, c2.z);
			Vector3 v6 = new Vector3(c1.x, c2.y, c2.z);
			Vector3 v7 = new Vector3(c2.x, c2.y, c2.z);
			Vector3 v8 = new Vector3(c2.x, c1.y, c2.z);

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
			return (Bitmap)(new global::System.Resources.ResourceManager("GLImp.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly())).GetObject("font");
		}

		public static Bitmap GetError() {
			return (Bitmap)(new global::System.Resources.ResourceManager("GLImp.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly())).GetObject("error");
		}

        static void Main()
        {
            OpenWindow();
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

		

		public static void BeginOrtho(float width, float height)
		{
			GL.Disable(EnableCap.DepthTest);
			GL.Clear(ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();
			//GL.Ortho(0f, width, height, 0f, -5f, 1f); 
			GL.Ortho(0, width, height, 0, -1, 0);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
		}

		public static void EndOrtho()
		{
			GL.Enable(EnableCap.DepthTest);
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
			GL.MatrixMode(MatrixMode.Modelview);
		}

		public static void PushMatrix() {
			GL.PushMatrix();
		}

		public static void PopMatrix() {
			GL.PopMatrix();
		}

		public static void Translate(Vector3 displacement) {
			GL.Translate(displacement);
		}

		//Scale
		public static void Scale(float s) {
			Scale(s, s, s);
		}
		public static void Scale(float x, float y, float z) {
			Scale(new Vector3(x, y, z));
		}
		public static void Scale(Vector3 v) {
			GL.Scale(v);
		}

		//Rotate
		public static void Rotate(float angle, Vector3 Up) {
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
		#endregion


	}
}