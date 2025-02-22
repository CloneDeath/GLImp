using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

namespace GLImp;

partial class GraphicsManager {
	public static event Action<FrameEventArgs>? Update;

	protected override void OnUpdateFrame(FrameEventArgs e) {
		base.OnUpdateFrame(e);
		Update?.Invoke(e);
		InputManager.Update();
	}

	protected override void OnRenderFrame(FrameEventArgs e) {
		base.OnRenderFrame(e);

		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		CameraManager.Draw(e);

		GL.Flush();
		SwapBuffer();
	}
}