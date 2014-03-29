using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace GLImp {
	abstract public class Image {
		public virtual int Width { get; set; }
		public virtual int Height { get; set; }
		
		/// <summary>
		/// Draws the image at the specified position and size.
		/// </summary>
		/// <param name="Position">Position to draw the image.</param>
		/// <param name="Size">Size to draw the image.</param>
		abstract public void Draw(Vector2d Position, Vector2d Size);

		/// <summary>
		/// Draws the image at the specified position.
		/// </summary>
		/// <param name="Position">Position to draw the image.</param>
		public void Draw(Vector2d Position)
		{
			this.Draw(Position, new Vector2d(Width, Height));
		}

		/// <summary>
		/// Draws the image at the specified position.
		/// </summary>
		/// <param name="X">X Position to draw the image.</param>
		/// <param name="Y">Y Position to draw the image.</param>
		public void Draw(double X, double Y)
		{
			this.Draw(new Vector2d(X, Y));
		}

		/// <summary>
		/// Draws the image at the specified position and size.
		/// </summary>
		/// <param name="X">X Position to draw the image.</param>
		/// <param name="Y">Y Position to draw the image.</param>
		/// <param name="Width">Width to draw the image.</param>
		/// <param name="Height">Height to draw the image.</param>
		public void Draw(double X, double Y, double Width, double Height)
		{
			this.Draw(new Vector2d(X, Y), new Vector2d(Width, Height));
		}
	}
}
