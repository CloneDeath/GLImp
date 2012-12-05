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
		/*****************************************************************
		 *							DRAW METHODS
		 *****************************************************************/
		public static void DrawButton(int x, int y, int width, int height, String text, Color color)
		{
			GL.Color4(1.0f, 1.0f, 1.0f, 0.5f);

			GL.Begin(BeginMode.Quads);
				GL.Vertex3(x, y, 4.0f);
				GL.Vertex3(x+width, y, 4.0f);
				GL.Vertex3(x+width, y+height, 4.0f);
				GL.Vertex3(x, y+height, 4.0f);
			GL.End();

			int len = text.Length*16;
			Text.DrawString((x+width/2-len/2), (y+height/2-len/2),text,color);
		}

		
		public static void DrawLine(Vector3 p1, Vector3 p2, Color color) {
			GL.Begin(BeginMode.Lines);
				GL.Color3(color);
				GL.Vertex3(p1);
				GL.Vertex3(p2);
			GL.End();
		}
		public static void DrawLine(Vector3 p1, Vector3 p2) {
			GL.Begin(BeginMode.Lines);
				GL.Vertex3(p1);
				GL.Vertex3(p2);
			GL.End();
		}

		

		
		//Draw Polygon
		public static void DrawPolygon(Vector3 p1, Vector3 p2, Vector3 p3, Color c)
		{
			SetColor(c);
			DrawPolygon(p1, p2, p3);
		}
		public static void DrawPolygon(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Polygon);
                GL.Vertex3(p1);
                GL.Vertex3(p2);
                GL.Vertex3(p3);
            GL.End();
			GL.Enable(EnableCap.Texture2D);
		}


		//Draw Quad
		public static void DrawQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color c) {
			SetColor(c);
			DrawQuadNoTex(p1, p2, p3, p4);
		}
		public static void DrawQuadNoTex(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Quads);
				GL.Vertex3(p1);
				GL.Vertex3(p2);
				GL.Vertex3(p3);
				GL.Vertex3(p4);
			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}
		public static void DrawQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Texture Texture) {
			SetTexture(Texture);
			DrawQuad(p1, p2, p3, p4);
		}
		public static void DrawQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
			GL.Begin(BeginMode.Quads);
				GL.TexCoord2(0, 0); GL.Vertex3(p1);
				GL.TexCoord2(1, 0); GL.Vertex3(p2);
				GL.TexCoord2(1, 1); GL.Vertex3(p3);
				GL.TexCoord2(0, 1); GL.Vertex3(p4);
			GL.End();
		}
		public static void DrawQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Texture Texture, float HSplit, float SpriteNumber) {
			SetTexture(Texture);
			DrawQuad(p1, p2, p3, p4, HSplit, SpriteNumber);
		}
		public static void DrawQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float HSplit, float SpriteNumber) {
			GL.Begin(BeginMode.Quads);
				GL.TexCoord2(SpriteNumber/HSplit, 0); GL.Vertex3(p1);
				GL.TexCoord2((SpriteNumber+1)/HSplit, 0); GL.Vertex3(p2);
				GL.TexCoord2((SpriteNumber + 1) / HSplit, 1); GL.Vertex3(p3);
				GL.TexCoord2(SpriteNumber / HSplit, 1); GL.Vertex3(p4);
			GL.End();
		}


		//Set Texture
		public static void SetTexture(Texture Texture) {
			SetColor(Color.White);
			GL.BindTexture(TextureTarget.Texture2D, Texture.ID);
		}
	}
}
