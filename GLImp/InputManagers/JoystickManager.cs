using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLImp {
	public class JoystickManager {
		public const int MaxJoysticks = 4;
		public static JoystickDevice[] Joysticks = new JoystickDevice[MaxJoysticks];

		internal static void Init() {
			for(int i = 0; i < MaxJoysticks; i++) {
				Joysticks[i] = new JoystickDevice(i);
			}
		}

		internal static void Update() {
			foreach(JoystickDevice dev in Joysticks) {
				dev.Update();
			}
		}
	}
}
