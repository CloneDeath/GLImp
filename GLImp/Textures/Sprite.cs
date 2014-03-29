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

		public bool Flipped = false;

		public double XOrigin { get; set; }
		public double YOrigin { get; set; }
		public double XOffset {
			get {
				return -XOrigin;
			}

			set {
				XOrigin = -value;
			}
		}
		public double YOffset {
			get {
				return -YOrigin;
			}
			set {
				YOrigin = -value;
			}
		}

		#region Constructors
		public Sprite() {

		}

		public Sprite(Image frame) {
			Add(frame);
		}

		public Sprite(IEnumerable<Image> frames) {
			foreach(Image tf in frames) {
				Add(tf);
			}
		}
		#endregion

		public void Add(Image frame) {
			Frames.Add(frame);
		}

		public override void Draw(Vector2d Position, Vector2d Size)
		{
			if (Frames.Count > 0) {
				Frames[(int)Math.Floor(CurrentFrame)].Draw(Position.X - XOrigin, Position.Y - YOrigin, Size.X, Size.Y);
				CurrentFrame += PlaybackSpeed;
			}
		}
	}
}
