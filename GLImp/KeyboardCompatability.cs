using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GLImp;

public class KeyboardCompatability {
	public KeyboardCompatability(GameWindow window) {
		window.KeyDown += args => KeyDown?.Invoke(this, args);
		window.KeyUp += args => KeyUp?.Invoke(this, args);
	}

	public event Action<object, KeyboardKeyEventArgs>? KeyDown;
	public event Action<object, KeyboardKeyEventArgs>? KeyUp;
}