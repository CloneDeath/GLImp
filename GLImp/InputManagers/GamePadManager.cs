using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLImp {
	public class GamePadManager {
		public const int MaxJoysticks = 4;
		public static GamePadDevice[] GamePads = new GamePadDevice[MaxJoysticks];

		internal static void Init() {
			for (int i = 0; i < MaxJoysticks; i++) {
				GamePads[i] = new GamePadDevice(i);
			}
		}

		internal static void Update() {
			foreach (GamePadDevice dev in GamePads) {
				dev.Update();
			}
		}
	}
}
