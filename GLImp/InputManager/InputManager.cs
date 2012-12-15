using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace GLImp {
	internal class InputManager {
		//Copy over the current state to the previous state when this is called. This will get called around 60Hz, so, make it fast!
		//Also, make sure that the current state is unchanged and independant from the previous state (ie: no shallow copy)
		internal static void Update() {
			KeyboardManager.Update();
			MouseManager.Update();
		}

		internal static void Init() {
			KeyboardManager.Init();
			MouseManager.Init();
		}
	}
}
