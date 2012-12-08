using System;
using OpenTK.Graphics.OpenGL;
using Mila.Data;
using Mila.Utility.MM;

namespace GLImp
{
	public class ModelManager
	{

		public static Mila.Data.Model LoadModel(string path)
		{
			Mila.Data.Model model = null;

			Mila.Utility.MM.MMReader reader = new Mila.Utility.MM.MMReader(path);

			if (reader.readMMFile(ref model) != Mila.Utility.MM.MMReaderErrors.Success)
			{
				model = null;
			}

			return model;
		}

		public static void DrawModel(Mila.Data.Model model, Texture texture)
		{
			int CurrentTexture = 0;
			if(texture != null) {
				GL.BindTexture(TextureTarget.Texture2D, texture.ID);
				CurrentTexture = texture.ID;
			}
			foreach (Mila.Data.FaceSet faceSet in model.FaceSets)
			{
				foreach (Mila.Data.Face face in faceSet.faces)
				{
					Vertex v1 = model.Vertices[face.vertices[0]];
					VertexAttributes a1 = model.VertexAttributes[face.vertexAttributes[0]];
					Vertex v2 = model.Vertices[face.vertices[1]];
					VertexAttributes a2 = model.VertexAttributes[face.vertexAttributes[1]];
					Vertex v3 = model.Vertices[face.vertices[2]];
					VertexAttributes a3 = model.VertexAttributes[face.vertexAttributes[2]];

					if(texture == null) {
						if(CurrentTexture != faceSet.materialId) {
							GL.BindTexture(TextureTarget.Texture2D, faceSet.materialId);
							CurrentTexture = faceSet.materialId;
						}
					}
					
					GL.Begin(BeginMode.Triangles);
					{
						//double ambient = 0.2f;
						//double light = a1.normal.X / 1.73f - a1.normal.Y / 1.73f + a1.normal.Z / 1.73f;
						//light = light * (1 - ambient) + ambient;
						//GL.Color3(light, light, light);
						GL.TexCoord2(a1.u, a1.v);
						GL.Vertex3(v1.x, v1.y, v1.z);

						//light = a2.normal.X / 1.73f - a2.normal.Y / 1.73f + a2.normal.Z / 1.73f;
						//light = light * (1 - ambient) + ambient;
						//GL.Color3(light, light, light);
						GL.TexCoord2(a2.u, a2.v);
						GL.Vertex3(v2.x, v2.y, v2.z);

						//light = a3.normal.X / 1.73f - a3.normal.Y / 1.73f + a3.normal.Z / 1.73f;
						//light = light * (1 - ambient) + ambient;
						//GL.Color3(light, light, light);
						GL.TexCoord2(a3.u, a3.v);
						GL.Vertex3(v3.x, v3.y, v3.z);
					}
					GL.End();
				}
			}
			GL.Color3(1.0f, 1.0f, 1.0f);
		}
	}
}
