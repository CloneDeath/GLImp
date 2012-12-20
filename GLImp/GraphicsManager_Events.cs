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


namespace GLImp {
	partial class GraphicsManager {
		#region Update
		public delegate void Updater();
		public static event Updater Update;
		protected override void OnUpdateFrame(FrameEventArgs e) {
			base.OnUpdateFrame(e);

			if (Update != null) {
				Update();
			}

			InputManager.Update();
		}
		#endregion

		#region Draw
		public delegate void Renderer();
		public static event Renderer Render;

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

			Render2D();
			
			GL.Flush();
			SwapBuffer();
        }

		private void Render2D() {
			Camera2D.SortCameras();

			BeginOrtho(ClientRectangle.Width, ClientRectangle.Height);
			foreach(Camera2D camera in Camera2D.Cameras){
				camera.Draw();
			}
			EndOrtho();
		}

		public static void BeginOrtho(double width, double height) {
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Clear(ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();
			//GL.Ortho(0f, width, height, 0f, -5f, 1f); 
			GL.Ortho(0, width, height, 0, -1, 0);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
		}

		public static void EndOrtho() {
			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Blend);
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
			GL.MatrixMode(MatrixMode.Modelview);
		}
		#endregion
	}
}
