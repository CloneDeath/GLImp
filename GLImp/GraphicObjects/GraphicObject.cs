using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace GLImp.GraphicObjects {
	public class GraphicObject {
		public int VertexBufferID;
		public int ColorBufferID;
		public int TexCoordBufferID;
		public int NormalBufferID;
		public int ElementBufferID;
		public int NumElements;

		public Shape Target;

		public GraphicObject(Shape shape) {
			Target = shape;

			int bufferSize;
			if (shape.Colors != null) {
				// Generate Array Buffer Id
				GL.GenBuffers(1, out ColorBufferID);

				// Bind current context to Array Buffer ID
				GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferID);

				// Send data to buffer
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(shape.Colors.Length * sizeof(int)), shape.Colors, BufferUsageHint.StaticDraw);

				// Validate that the buffer is the correct size
				GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
				if (shape.Colors.Length * sizeof(int) != bufferSize)
					throw new ApplicationException("Vertex array not uploaded correctly");

				// Clear the buffer Binding
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			}

			// Normal Array Buffer
			if (shape.Normals != null) {
				// Generate Array Buffer Id
				GL.GenBuffers(1, out NormalBufferID);

				// Bind current context to Array Buffer ID
				GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferID);

				// Send data to buffer
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(shape.Normals.Length * Vector3.SizeInBytes), shape.Normals, BufferUsageHint.StaticDraw);

				// Validate that the buffer is the correct size
				GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
				if (shape.Normals.Length * Vector3.SizeInBytes != bufferSize)
					throw new ApplicationException("Normal array not uploaded correctly");

				// Clear the buffer Binding
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			}

			// TexCoord Array Buffer
			if (shape.TexCoords != null) {
				// Generate Array Buffer Id
				GL.GenBuffers(1, out TexCoordBufferID);

				// Bind current context to Array Buffer ID
				GL.BindBuffer(BufferTarget.ArrayBuffer, TexCoordBufferID);

				// Send data to buffer
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(shape.TexCoords.Length * 8), shape.TexCoords, BufferUsageHint.StaticDraw);

				// Validate that the buffer is the correct size
				GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
				if (shape.TexCoords.Length * 8 != bufferSize)
					throw new ApplicationException("TexCoord array not uploaded correctly");

				// Clear the buffer Binding
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			}

			// Vertex Array Buffer
			{
				// Generate Array Buffer Id
				GL.GenBuffers(1, out VertexBufferID);

				// Bind current context to Array Buffer ID
				GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);

				// Send data to buffer
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(shape.Vertices.Length * Vector3.SizeInBytes), shape.Vertices, BufferUsageHint.DynamicDraw);

				// Validate that the buffer is the correct size
				GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
				if (shape.Vertices.Length * Vector3.SizeInBytes != bufferSize)
					throw new ApplicationException("Vertex array not uploaded correctly");

				// Clear the buffer Binding
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			}

			// Element Array Buffer
			{
				// Generate Array Buffer Id
				GL.GenBuffers(1, out ElementBufferID);

				// Bind current context to Array Buffer ID
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferID);

				// Send data to buffer
				GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(shape.Indices.Length * sizeof(int)), shape.Indices, BufferUsageHint.StaticDraw);

				// Validate that the buffer is the correct size
				GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
				if (shape.Indices.Length * sizeof(int) != bufferSize)
					throw new ApplicationException("Element array not uploaded correctly");

				// Clear the buffer Binding
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			}

			// Store the number of elements for the DrawElements call
			NumElements = shape.Indices.Length;
		}

		public void Draw() {

			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, Target.Texture.ID);

			// Push current Array Buffer state so we can restore it later
			GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

			if (VertexBufferID == 0) return;
			if (ElementBufferID == 0) return;

			if (GL.IsEnabled(EnableCap.Lighting)) {
				// Normal Array Buffer
				if (NormalBufferID != 0) {
					// Bind to the Array Buffer ID
					GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferID);

					// Set the Pointer to the current bound array describing how the data ia stored
					GL.NormalPointer(NormalPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);

					// Enable the client state so it will use this array buffer pointer
					GL.EnableClientState(ArrayCap.NormalArray);
				}
			} else {
				// Color Array Buffer (Colors not used when lighting is enabled)
				if (ColorBufferID != 0) {
					// Bind to the Array Buffer ID
					GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferID);

					// Set the Pointer to the current bound array describing how the data ia stored
					GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(int), IntPtr.Zero);

					// Enable the client state so it will use this array buffer pointer
					GL.EnableClientState(ArrayCap.ColorArray);
				}
			}

			// Texture Array Buffer
			if (GL.IsEnabled(EnableCap.Texture2D)) {
				if (TexCoordBufferID != 0) {
					// Bind to the Array Buffer ID
					GL.BindBuffer(BufferTarget.ArrayBuffer, TexCoordBufferID);

					// Set the Pointer to the current bound array describing how the data ia stored
					GL.TexCoordPointer(2, TexCoordPointerType.Float, 8, IntPtr.Zero);

					// Enable the client state so it will use this array buffer pointer
					GL.EnableClientState(ArrayCap.TextureCoordArray);
				}
			}

			// Vertex Array Buffer
			{
				// Bind to the Array Buffer ID
				GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);

				// Set the Pointer to the current bound array describing how the data ia stored
				GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);

				// Enable the client state so it will use this array buffer pointer
				GL.EnableClientState(ArrayCap.VertexArray);
			}

			// Element Array Buffer
			{
				// Bind to the Array Buffer ID
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferID);

				// Draw the elements in the element array buffer
				// Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
				GL.DrawElements(BeginMode.Triangles, NumElements, DrawElementsType.UnsignedInt, IntPtr.Zero);

				// Could also call GL.DrawArrays which would ignore the ElementArrayBuffer and just use primitives
				// Of course we would have to reorder our data to be in the correct primitive order
			}

			// Restore the state
			GL.PopClientAttrib();
		}
	}
}
