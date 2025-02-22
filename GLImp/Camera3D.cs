using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GLImp;

public class Camera3D : Camera {
	private Vector3d CameraLook;
	public Vector3d CameraUp = Vector3d.UnitZ;
	public double FarPlane = 1000;
	public double FieldOfView = 90.0;
	public double NearPlane = 0.1;
	public Vector3d Position;
	private Matrix4d projection;
	private bool UseDefaultProjectionMatrix = true;

	public Camera3D() {
		Position = Vector3d.Zero;
		CameraLook = Vector3d.UnitY;
		CameraManager.Add(this);
	}

	public Camera3D(double X, double Y, double Z) {
		Position = new Vector3d(X, Y, Z);
		CameraLook = Vector3d.UnitY;
		CameraManager.Add(this);
	}

	public Camera3D(Vector3d position) {
		Position = position;
		CameraLook = Vector3d.UnitY;
		CameraManager.Add(this);
	}

	public Camera3D(Vector3d position, Vector3d lookat) {
		Position = position;
		CameraLook = lookat;
		CameraManager.Add(this);
	}

	public Matrix4d ProjectionMatrix {
		get {
			if (UseDefaultProjectionMatrix) {
				UseDefaultProjection();
			}

			return projection;
		}
		set {
			UseDefaultProjectionMatrix = false;
			projection = value;
		}
	}

	public Matrix4d ModelView => Matrix4d.LookAt(Position, CameraLook, CameraUp);

	private static void Begin3D() {
		if (GraphicsManager.DisableDepthTest) {
			GL.Disable(EnableCap.DepthTest);
		} else {
			GL.Enable(EnableCap.DepthTest);
		}

		GL.MatrixMode(MatrixMode.Projection);

		GL.BlendEquation(BlendEquationMode.FuncAdd);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
	}

	public void UseDefaultProjection() {
		UseDefaultProjectionMatrix = true;
		ProjectionMatrix = Matrix4d.CreatePerspectiveFieldOfView(Math.PI / 180 * FieldOfView,
			Viewport.Width / (double)Viewport.Height, NearPlane, FarPlane);
	}

	internal override void Draw(FrameEventArgs e) {
		Begin3D();

		var projectoionview = ProjectionMatrix;
		GL.LoadMatrix(ref projectoionview);

		GL.MatrixMode(MatrixMode.Modelview);
		var modelview = ModelView;
		GL.LoadMatrix(ref modelview);

		OnRender?.Invoke(e);
	}

	public void LookAt(Vector3d lookat) {
		CameraLook = lookat;
	}

	public void LookAt(double X, double Y, double Z) {
		CameraLook = new Vector3d(X, Y, Z);
	}

	public Vector3d GetLookingAt() => CameraLook;

	public void RotateCamera(double yaw, double pitch) {
		if (CameraUp == Vector3d.UnitZ) {
			yaw = yaw * Math.PI / 180;
			pitch = pitch * Math.PI / 180;
			CameraLook = new Vector3d(Math.Cos(yaw) * Math.Cos(pitch) + Position.X,
				Math.Sin(yaw) * Math.Cos(pitch) + Position.Y,
				Math.Sin(pitch) + Position.Z);
		} else if (CameraUp == Vector3d.UnitY) {
			yaw = yaw * Math.PI / 180;
			pitch = pitch * Math.PI / 180;
			CameraLook = new Vector3d(Math.Cos(yaw) * Math.Cos(pitch) + Position.X,
				Math.Sin(pitch) + Position.Y,
				Math.Sin(yaw) * Math.Cos(pitch) + Position.Z);
		} else {
			throw new NotImplementedException("Camera can only be rotated if CameraUp is set to UnitY or UnitZ.");
		}
	}

	/// <summary>
	///     Converts a 2D screen space coordinate into a 3D world space coordinate.
	/// </summary>
	/// <param name="ScreenPoint">The X,Y coordinate in the viewport that you wish to use to convert.</param>
	/// <returns>Returns a world coordinate of the 3D point</returns>
	public Vector3d ScreenPointToPosition(Vector2d ScreenPoint) =>
		UnProject(ProjectionMatrix, ModelView, Viewport.Size, ScreenPoint).Xyz;

	/// <summary>
	///     Converts a 2D screen space coordinate into a 3D ray.
	/// </summary>
	/// <param name="ScreenPoint">The X,Y coordinate in the viewport that you wish to convert.</param>
	/// <returns>A ray relative to the position of the camera.</returns>
	public Vector3d ScreenPointToRay(Vector2d ScreenPoint) =>
		Vector3d.Normalize(ScreenPointToPosition(ScreenPoint) - Position);

	private static Vector4d UnProject(Matrix4d projection, Matrix4d view, Size viewport, Vector2d mouse) {
		Vector4d vec;

		vec.X = 2.0f * mouse.X / (float)viewport.Width - 1;
		vec.Y = -(2.0f * mouse.Y / (float)viewport.Height - 1);
		vec.Z = 0;
		vec.W = 1.0f;

		var viewInv = Matrix4d.Invert(view);
		var projInv = Matrix4d.Invert(projection);

		Vector4d.TransformRow(vec, projInv, out vec);
		Vector4d.TransformRow(vec, viewInv, out vec);

		if (vec.W is <= float.Epsilon and >= float.Epsilon) return vec;
		vec.X /= vec.W;
		vec.Y /= vec.W;
		vec.Z /= vec.W;
		return vec;
	}
}