using System;
using System.Collections.Generic;
using OpenTK;

namespace GLImp {
	public class Sprite {
		float FrameOn = 0;
		List<TextureFrame> Frames = new List<TextureFrame>();

		public float Speed = 1.0f;

		public Sprite() {
		}

		public Sprite(TextureFrame[] frames) {
			foreach(TextureFrame tf in frames) {
				Add(tf);
			}
		}

		public void Add(TextureFrame frame) {
			Frames.Add(frame);
		}

		public void Draw(Vector2 p1, Vector2 p2) {
			if(Frames.Count > 0) {
				Frames[(int)Math.Floor(FrameOn)].Draw(p1, p2);
				FrameOn = (FrameOn + Speed) % Frames.Count;
			}
		}
	}
}
