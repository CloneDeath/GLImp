using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GLImp;

public class MouseCompatability {
	public MouseCompatability(GameWindow window){
		window.MouseDown += args => ButtonDown?.Invoke(this, args);
		window.MouseUp += args => ButtonUp?.Invoke(this, args);
		window.MouseMove += args => Move?.Invoke(this, args);
		window.MouseWheel += args => WheelChanged?.Invoke(this, args);
	}
	public Action<object, MouseButtonEventArgs>? ButtonDown { get; set; }
	public Action<object, MouseButtonEventArgs>? ButtonUp { get; set; }
	public Action<object, MouseMoveEventArgs>? Move { get; set; }
	public Action<object, MouseWheelEventArgs>? WheelChanged { get; set; }
}