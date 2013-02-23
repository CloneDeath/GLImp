using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLImpUnitTest
{
    public abstract class TestSetup
    {
		public string Name = "no name";

		public virtual void Update() {

		}

        public virtual void Render2D() {

        }

		public virtual void Render3D() {

		}
    }
}
