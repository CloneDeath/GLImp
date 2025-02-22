using System;
using OpenTK.Graphics.OpenGL;

namespace GLImp;

/// <summary>
///     Captures a collection of GL calls to quickly call them all at once in the future.
/// </summary>
public class DisplayList {
	private static int CurrentCapture = -1;
	private readonly int ListNumber;

	/// <summary>
	///     Generates a new Display List.
	/// </summary>
	public DisplayList() {
		ListNumber = GL.GenLists(1);
	}

	/// <summary>
	///     Begin capturing OpenGL calls. This will intercept the calls and stop them from being executed.
	/// </summary>
	public void BeginCapture() {
		if (CurrentCapture == -1) {
			GL.NewList(ListNumber, ListMode.Compile);
			CurrentCapture = ListNumber;
		} else {
			throw new Exception("An existing capture session is already running! End it before starting a new one.");
		}
	}

	/// <summary>
	///     Begin capturing OpenGL calls. This will allow the calls to also be executed, passively collecting them.
	/// </summary>
	public void PassiveCapture() {
		if (CurrentCapture == -1) {
			GL.NewList(ListNumber, ListMode.CompileAndExecute);
			CurrentCapture = ListNumber;
		} else {
			throw new Exception("An existing capture session is already running! End it before starting a new one.");
		}
	}

	/// <summary>
	///     Ends the current capture sequence.
	/// </summary>
	public void EndCapture() {
		if (CurrentCapture == ListNumber) {
			GL.EndList();
			CurrentCapture = -1;
		} else {
			throw new Exception(
				"This DisplayList is not currently capturing a session. Please call BeinCapture/PassiveCapture first.");
		}
	}

	/// <summary>
	///     Draws (or replays) the collected capture sequence. Use BeginCapture/PassiveCapture and EndCapture to collect GL
	///     calls.
	/// </summary>
	public void Draw() {
		GL.CallList(ListNumber);
	}

	public override bool Equals(object? obj) {
		if (obj is not DisplayList other) return false;
		return ListNumber == other.ListNumber;
	}

	public override int GetHashCode() => ListNumber;

	public override string ToString() => "DisplayList " + ListNumber;
}