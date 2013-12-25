using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace GLImp
{
	public class Camera3D : Camera
	{
		public Vector3d CameraUp = Vector3d.UnitZ;
		public Vector3d Position;
		private Vector3d CameraLook;
		public double FieldOfView = 90.0;
		public double NearPlane = 0.1;
		public double FarPlane = 1000;
		private Matrix4d projection = new Matrix4d();
		public Matrix4d ProjectionMatrix
		{
			get
			{
				if (UseDefaultProjectionMatrix) {
					this.UseDefaultProjection();
				}
				return projection;
			}
			set
			{
				UseDefaultProjectionMatrix = false;
				projection = value;
			}
		}
		private bool UseDefaultProjectionMatrix = true;

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

		public void UseDefaultProjection(){
			UseDefaultProjectionMatrix = true;
			ProjectionMatrix = Matrix4d.CreatePerspectiveFieldOfView((Math.PI / 180) * FieldOfView,
					((double)Viewport.Width) / ((double)Viewport.Height), NearPlane, FarPlane);
		}

		internal override void Draw()
		{
			Begin3D();

			Matrix4d projectoionview = ProjectionMatrix;
			GL.LoadMatrix(ref projectoionview);

			GL.MatrixMode(MatrixMode.Modelview);
			Matrix4d modelview = ModelView;
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
			if (CameraUp == Vector3d.UnitZ) {
				yaw = (double)(yaw * Math.PI / 180);
				pitch = (double)(pitch * Math.PI / 180);
				CameraLook = new Vector3d(((Math.Cos(yaw) * Math.Cos(pitch)) + Position.X),
											((Math.Sin(yaw) * Math.Cos(pitch)) + Position.Y),
											(Math.Sin(pitch) + Position.Z));
			} else if (CameraUp == Vector3d.UnitY) {
				yaw = (double)(yaw * Math.PI / 180);
				pitch = (double)(pitch * Math.PI / 180);
				CameraLook = new Vector3d(((Math.Cos(yaw) * Math.Cos(pitch)) + Position.X),
											(Math.Sin(pitch) + Position.Y),
											((Math.Sin(yaw) * Math.Cos(pitch)) + Position.Z));
			} else {
				throw new NotImplementedException("Camera can only be rotated if CameraUp is set to UnitY or UnitZ.");
			}
		}

		public Matrix4d ModelView
		{
			get
			{
				return Matrix4d.LookAt(Position, CameraLook, CameraUp);
			}
		}

		/// <summary>
		/// Converts a 2D screen space coordinate into a 3D world space coordinate.
		/// </summary>
		/// <param name="ScreenPoint">The X,Y coordinate in the viewport that you wish to use to convert.</param>
		/// <returns>Returns a world coordinate of the 3D point</returns>
		public Vector3d ScreenPointToPosition(Vector2d ScreenPoint)
		{
			return UnProject(ProjectionMatrix, ModelView, Viewport.Size, ScreenPoint).Xyz;
		}

		/// <summary>
		/// Converts a 2D screen space coordinate into a 3D ray.
		/// </summary>
		/// <param name="ScreenPoint">The X,Y coordinate in the viewport that you wish to convert.</param>
		/// <returns>A ray relative to the position of the camera.</returns>
		public Vector3d ScreenPointToRay(Vector2d ScreenPoint)
		{
			return Vector3d.Normalize(ScreenPointToPosition(ScreenPoint) - Position);
		}

		private static Vector4d UnProject(Matrix4d projection, Matrix4d view, Size viewport, Vector2d mouse)
		{
			Vector4d vec;

			vec.X = 2.0f * mouse.X / (float)viewport.Width - 1;
			vec.Y = -(2.0f * mouse.Y / (float)viewport.Height - 1);
			vec.Z = 0;
			vec.W = 1.0f;

			Matrix4d viewInv = Matrix4d.Invert(view);
			Matrix4d projInv = Matrix4d.Invert(projection);

			Vector4d.Transform(ref vec, ref projInv, out vec);
			Vector4d.Transform(ref vec, ref viewInv, out vec);

			if (vec.W > float.Epsilon || vec.W < float.Epsilon) {
				vec.X /= vec.W;
				vec.Y /= vec.W;
				vec.Z /= vec.W;
			}

			return vec;
		}
	}
}
