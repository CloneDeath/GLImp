using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using GLImpUnitTest.Tests;

namespace GLImpUnitTest
{
    public class TestingDisplay : WindowControl
    {
        public void Initialize()
        {
			AddTest(new ShaderTest());
			AddTest(new Line3DTest());
			AddTest(new InsideCubeTest());
			AddTest(new Alpha2DTest());
			AddTest(new BasicTest());
			AddTest(new InputTest());
        }

        List<TestSetup> Tests = new List<TestSetup>();
        public void AddTest(TestSetup test)
        {
            Tests.Add(test);
			lb.AddRow(test.Name);
        }

        ListBox lb;
        public TestingDisplay() : base(MainCanvas.GetCanvas())
        {
            this.Dock = Gwen.Pos.Left;
            this.Width = 300;
            this.IsClosable = false;

            lb = new ListBox(this);
            lb.SetPosition(10, 10);
            lb.SetSize(265, 400);

            Initialize();

            lb.SelectedRowIndex = 0;
        }

        public void Render2D()
        {
            Tests[lb.SelectedRowIndex].Render2D();
        }

		public void Render3D() {
			Tests[lb.SelectedRowIndex].Render3D();
		}
    }
}
