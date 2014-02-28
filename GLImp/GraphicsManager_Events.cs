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
		public static event Action<FrameEventArgs> Update;

		protected override void OnUpdateFrame(FrameEventArgs e) {
			base.OnUpdateFrame(e);

			if (Update != null) {
				Update(e);
			}

			InputManager.Update();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			CameraManager.Draw(e);
			
			GL.Flush();
			SwapBuffer();
        }
	}
}
