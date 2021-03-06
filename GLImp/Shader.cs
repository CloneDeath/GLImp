﻿using System;
using System.IO;
using System.Reflection;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GLImp {
	public class Shader {
		protected int iVertexShader;
		protected int iFragShader;

		protected int iProgram;

		public Shader(string vertex, string frag) {
			iProgram = GL.CreateProgram();
			int result;

			iVertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(iVertexShader, vertex);
			GL.CompileShader(iVertexShader);
			GL.GetShader(iVertexShader, ShaderParameter.CompileStatus, out result);
			if(result == 0) {
				Console.WriteLine("Failed to compile vertex shader!");
				Console.WriteLine(GL.GetShaderInfoLog(iVertexShader));
				System.Diagnostics.Debug.WriteLine("Failed to compile vertex shader!");
				System.Diagnostics.Debug.WriteLine(GL.GetShaderInfoLog(iVertexShader));
			}

			iFragShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(iFragShader, frag);
			GL.CompileShader(iFragShader);
			GL.GetShader(iFragShader, ShaderParameter.CompileStatus, out result);
			if(result == 0) {
				Console.WriteLine("Failed to compile fragment shader!");
				Console.WriteLine(GL.GetShaderInfoLog(iFragShader));
				System.Diagnostics.Debug.WriteLine("Failed to compile fragment shader!");
				System.Diagnostics.Debug.WriteLine(GL.GetShaderInfoLog(iFragShader));
			}

			GL.AttachShader(iProgram, iVertexShader);
			GL.AttachShader(iProgram, iFragShader);

			GL.LinkProgram(iProgram);

			GL.GetProgram(iProgram, ProgramParameter.LinkStatus, out result);
			if(result == 0) {
				System.Diagnostics.Debug.WriteLine("Failed to link shader program!");
				System.Diagnostics.Debug.WriteLine(GL.GetProgramInfoLog(iProgram));
			}
		}

		#region SetUniform
		//1
		public void SetUniform(string name, int value) {
			int loc = GL.GetUniformLocation(iProgram, name);
			GL.Uniform1(loc, value);
		}
		public void SetUniform(string name, float value) {
			int loc = GL.GetUniformLocation(iProgram, name);
			GL.Uniform1(loc, value);
		}
		
		//2
		public void SetUniform(string name, Vector2 value) {
			int loc = GL.GetUniformLocation(iProgram, name);
			GL.Uniform2(loc, ref value);
		}
		public void SetUniform(string name, int v0, int v1) {
			int loc = GL.GetUniformLocation(iProgram, name);
			GL.Uniform2(loc, v0, v1);
		}
		
		//3
		public void SetUniform(string name, Vector3 value) {
			int loc = GL.GetUniformLocation(iProgram, name);
			GL.Uniform3(loc, ref value);
		}

		//Texture
		public void SetUniform(string name, Texture value, int slot) {
			int loc = GL.GetUniformLocation(iProgram, name);
			GL.ActiveTexture(TextureUnit.Texture0 + slot);
			GL.BindTexture(TextureTarget.Texture2D, value.ID);
			GL.Uniform1(loc, slot);
			GL.ActiveTexture(TextureUnit.Texture0);
		}
		#endregion

		public virtual void Activate() {
			GL.UseProgram(iProgram);
		}

		public virtual void Deactivate() {
			GL.UseProgram(0);
		}
	}
}