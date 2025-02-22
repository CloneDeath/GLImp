using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace GLImp;

public class Sprite : Image {
	private readonly List<Image> Frames = new();

	private double _currentframe;

	public bool Flipped = false;

	public double PlaybackSpeed = 1.0f;

	public Sprite() { }

	public Sprite(Image frame) {
		Add(frame);
	}

	public Sprite(IEnumerable<Image> frames) {
		foreach (var tf in frames) {
			Add(tf);
		}
	}

	public double CurrentFrame {
		//Decimal point is % of the way done with a frame (for example, half speeds)
		get => _currentframe;
		set {
			_currentframe = value % Frames.Count;
			if (_currentframe < 0) {
				_currentframe += Frames.Count;
			}
		}
	}

	public double XOrigin { get; set; }
	public double YOrigin { get; set; }

	public double XOffset {
		get => -XOrigin;

		set => XOrigin = -value;
	}

	public double YOffset {
		get => -YOrigin;
		set => YOrigin = -value;
	}

	public int Width { get; set; }
	public int Height { get; set; }

	public void Draw(Vector2d Position, Vector2d Size) {
		if (Frames.Count > 0) {
			Frames[(int)Math.Floor(CurrentFrame)].Draw(Position.X - XOrigin, Position.Y - YOrigin, Size.X, Size.Y);
			CurrentFrame += PlaybackSpeed;
		}
	}

	public void Add(Image frame) {
		Frames.Add(frame);
	}
}