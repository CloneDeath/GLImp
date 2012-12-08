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
	partial class GraphicsManager{
		/// <summary>
		/// Draws a triangle strip with the given input 
		/// </summary>
		/// <param name="pts">The vectors to connect</param>
		public static void DrawPolygon(List<Vector2d> pts) {
			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.TriangleStrip);
			foreach (Vector2d v in pts) {
				GL.Vertex2(v);
			}
			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}

		public static void DrawLines(List<Vector2d> pts) {
			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Lines);
			foreach (Vector2d v in pts) {
				GL.Vertex2(v);
			}
			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}

		//Draw Strings
		public static void DrawString(double x, double y, string msg) {
			PushMatrix();
			GL.Translate(x, y, 0);
			GL.Scale(16, 16, 1);
			int offset = 0;
			if (msg != null) {
				GL.BindTexture(TextureTarget.Texture2D, Texture.Font);
				for (int i = 0; i < msg.Length; i++) {
					if (msg.ToCharArray()[i] == '\n') {
						GL.Translate(-offset, 1, 0);
						offset = 0;
					} else {
						DrawChar(msg.ToCharArray()[i]);
						GL.Translate((16.0f / 16.0f), 0.0f, 0.0f);
						offset += 1;
					}
				}
			}
			PopMatrix();
		}
		private static void DrawChar(int charAt) {
			int xSize = 16;
			int ySize = 16;
			int c = charAt;

			int cx = c / xSize;
			int cy = c % xSize;
			double top = (cy) * (1.0f / ySize);
			double bottom = (cy + 1) * (1.0f / ySize);
			double right = (cx + 1) * (1.0f / xSize);
			double left = (cx) * (1.0f / xSize);
			GL.Begin(BeginMode.Quads);
				GL.TexCoord2(top, left); GL.Vertex2(0.0f, 0.0f);
				GL.TexCoord2(top, right); GL.Vertex2(0.0f, 1.0f);
				GL.TexCoord2(bottom, right); GL.Vertex2(1.0f, 1.0f);
				GL.TexCoord2(bottom, left); GL.Vertex2(1.0f, 0.0f);
			GL.End();
		}

		//Draw Line
		public static void DrawLine(Vector2d p1, Vector2d p2, Color color) {
			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Lines);
			GL.Color3(color);
				GL.Vertex2(p1);
				GL.Vertex2(p2);
			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}

		public static void DrawTriangle(Vector2d p1, Vector2d p2, Vector2d p3, Color color) {
			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Triangles);
			GL.Color3(color);
				GL.Vertex2(p1);
				GL.Vertex2(p2);
				GL.Vertex2(p3);
			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}

		//Rectangle
		public static void DrawRectangle(double x, double y, double width, double height, Color color) {
			DrawRectangle(new Vector2d(x, y), new Vector2d(x + width, y + height), color);
		}
		public static void DrawRectangle(Vector2d p1, Vector2d p2, Color color) {
			SetColor(color);
			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Quads);
			GL.Vertex2(p1);
			GL.Vertex2(p2.X, p1.Y);
			GL.Vertex2(p2);
			GL.Vertex2(p1.X, p2.Y);
			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}
		public static void DrawRectangle(double x, double y, double width, double height, Texture Texture) {
			DrawRectangle(new Vector2d(x, y), new Vector2d(x + width, y + height), Texture);
		}
		public static void DrawRectangle(Vector2d p1, Vector2d p2, Texture Texture) {
			SetColor(Color.White);
			GL.BindTexture(TextureTarget.Texture2D, Texture.ID);
			GL.Begin(BeginMode.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(p1.X, p1.Y);
			GL.TexCoord2(1, 0); GL.Vertex2(p2.X, p1.Y);
			GL.TexCoord2(1, 1); GL.Vertex2(p2.X, p2.Y);
			GL.TexCoord2(0, 1); GL.Vertex2(p1.X, p2.Y);
			GL.End();
		}
		public static void DrawRectangleHollow(double x, double y, double width, double height, Color color) {
			int lineWidth = 1;
			DrawRectangle(x - lineWidth / 2, y - lineWidth / 2, lineWidth, height, color);
			DrawRectangle(x + lineWidth / 2, y + lineWidth / 2, width, lineWidth, color);
			DrawRectangle(x + width, y, lineWidth, height + lineWidth / 2, color);
			DrawRectangle(x, y + height, width + lineWidth / 2, lineWidth, color);
		}

	}
}
