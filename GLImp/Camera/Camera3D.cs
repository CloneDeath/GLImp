using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GLImp
{
	public class Camera3D : Camera
	{
		public Vector3 CameraUp = Vector3.UnitZ;
		public Vector3d Position;
		private Vector3d CameraLook;
		public float FieldOfView = 90.0f;
		public float NearPlane = 0.1f;
		public float FarPlane = 1000f;

		public Camera3D()
		{
			Position = Vector3d.Zero;
			CameraLook = Vector3d.UnitY;
			CameraManager.Add(this);
		}

		public Camera3D(double X, double Y, double Z)
		{
			Position = new Vector3d(X, Y, Z);
			CameraLook = Vector3d.UnitY;
			CameraManager.Add(this);
		}

		public Camera3D(Vector3d position)
		{
			this.Position = position;
			CameraLook = Vector3d.UnitY;
			CameraManager.Add(this);
		}

		public Camera3D(Vector3d position, Vector3d lookat)
		{
			this.Position = position;
			this.CameraLook = lookat;
			CameraManager.Add(this);
		}

		private void Begin3D()
		{
			if (GraphicsManager.DisableDepthTest) {
				GL.Disable(EnableCap.DepthTest);
			} else {
				GL.Enable(EnableCap.DepthTest);
			}

			GL.MatrixMode(MatrixMode.Projection);

			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		internal override void Draw()
		{
			Begin3D();

			Matrix4 projectoionview = Matrix4.CreatePerspectiveFieldOfView((float)((Math.PI / 180) * FieldOfView),
				((float)Viewport.Width) / ((float)Viewport.Height), NearPlane, FarPlane);
			GL.LoadMatrix(ref projectoionview);

			GL.MatrixMode(MatrixMode.Modelview);
			Matrix4 modelview = Matrix4.LookAt((Vector3)Position, (Vector3)CameraLook, CameraUp);
			GL.LoadMatrix(ref modelview);

			if (OnRender != null) {
				OnRender();
			}
		}

		public void LookAt(Vector3d lookat){
			CameraLook = lookat;
		}

		public void LookAt(double X, double Y, double Z)
		{
			CameraLook = new Vector3d(X, Y, Z);
		}

		public Vector3d GetLookingAt()
		{
			return CameraLook;
		}

		public void RotateCamera(double yaw, double pitch)
		{
			yaw = (double)(yaw * Math.PI / 180);
			pitch = (double)(pitch * Math.PI / 180);
			CameraLook = new Vector3d(((Math.Cos(yaw) * Math.Cos(pitch)) + Position.X),
										((Math.Sin(yaw) * Math.Cos(pitch)) + Position.Y),
										(Math.Sin(pitch) + Position.Z));
		}
	}
}
