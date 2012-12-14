using System;
using System.Collections.Generic;
using OpenTK;

namespace GLImp {
	public class Sprite : Image {
		private double _currentframe;
		public double CurrentFrame { //Decimal point is % of the way done with a frame (for example, half speeds)
			get {
				return _currentframe;
			}
			set {
				_currentframe = value % Frames.Count;
				if (_currentframe < 0) {
					_currentframe += Frames.Count;
				}
			}
		}
		List<Image> Frames = new List<Image>();

		public double PlaybackSpeed = 1.0f;

		public Sprite(Image frame) {
			Add(frame);
		}

		public Sprite(IEnumerable<Image> frames) {
			foreach(Image tf in frames) {
				Add(tf);
			}
		}

		public void Add(Image frame) {
			Frames.Add(frame);
		}

		public override void Draw(double x, double y) {
			if(Frames.Count > 0) {
				Frames[(int)Math.Floor(CurrentFrame)].Draw(x, y);
				CurrentFrame += PlaybackSpeed;
			}
		}

		public override void Draw(double x, double y, double width, double height) {
			if (Frames.Count > 0) {
				Frames[(int)Math.Floor(CurrentFrame)].Draw(x, y, width, height);
				CurrentFrame += PlaybackSpeed;
			}
		}
	}
}
