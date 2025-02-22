using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GLImp;

public class Shader {
	protected int iFragShader;

	protected int iProgram;
	protected int iVertexShader;

	public Shader(string vertex, string frag) {
		iProgram = GL.CreateProgram();

		iVertexShader = GL.CreateShader(ShaderType.VertexShader);
		GL.ShaderSource(iVertexShader, vertex);
		GL.CompileShader(iVertexShader);
		GL.GetShader(iVertexShader, ShaderParameter.CompileStatus, out var result);
		if (result == 0) {
			Console.WriteLine("Failed to compile vertex shader!");
			Console.WriteLine(GL.GetShaderInfoLog(iVertexShader));
			Debug.WriteLine("Failed to compile vertex shader!");
			Debug.WriteLine(GL.GetShaderInfoLog(iVertexShader));
		}

		iFragShader = GL.CreateShader(ShaderType.FragmentShader);
		GL.ShaderSource(iFragShader, frag);
		GL.CompileShader(iFragShader);
		GL.GetShader(iFragShader, ShaderParameter.CompileStatus, out result);
		if (result == 0) {
			Console.WriteLine("Failed to compile fragment shader!");
			Console.WriteLine(GL.GetShaderInfoLog(iFragShader));
			Debug.WriteLine("Failed to compile fragment shader!");
			Debug.WriteLine(GL.GetShaderInfoLog(iFragShader));
		}

		GL.AttachShader(iProgram, iVertexShader);
		GL.AttachShader(iProgram, iFragShader);

		GL.LinkProgram(iProgram);

		GL.GetProgram(iProgram, ProgramParameter.LinkStatus, out result);
		if (result != 0) return;

		throw new Exception(GL.GetProgramInfoLog(iProgram));
	}

	public virtual void Activate() {
		GL.UseProgram(iProgram);
	}

	public virtual void Deactivate() {
		GL.UseProgram(0);
	}

	#region SetUniform
	//1
	public void SetUniform(string name, int value) {
		var loc = GL.GetUniformLocation(iProgram, name);
		GL.Uniform1(loc, value);
	}

	public void SetUniform(string name, float value) {
		var loc = GL.GetUniformLocation(iProgram, name);
		GL.Uniform1(loc, value);
	}

	//2
	public void SetUniform(string name, Vector2 value) {
		var loc = GL.GetUniformLocation(iProgram, name);
		GL.Uniform2(loc, ref value);
	}

	public void SetUniform(string name, int v0, int v1) {
		var loc = GL.GetUniformLocation(iProgram, name);
		GL.Uniform2(loc, v0, v1);
	}

	//3
	public void SetUniform(string name, Vector3 value) {
		var loc = GL.GetUniformLocation(iProgram, name);
		GL.Uniform3(loc, ref value);
	}

	//Texture
	public void SetUniform(string name, Texture value, int slot) {
		var loc = GL.GetUniformLocation(iProgram, name);
		GL.ActiveTexture(TextureUnit.Texture0 + slot);
		GL.BindTexture(TextureTarget.Texture2D, value.ID);
		GL.Uniform1(loc, slot);
		GL.ActiveTexture(TextureUnit.Texture0);
	}
	#endregion
}