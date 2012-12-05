﻿using System;
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
            AddTest(new BasicTest());
        }

        List<TestSetup> Tests = new List<TestSetup>();
        public void AddTest(TestSetup test)
        {
            Tests.Add(test);
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

            lb.AddRow("Test1");

            lb.SelectedRowIndex = 0;
        }

        public void Render2DPre()
        {
            Tests[lb.SelectedRowIndex].Render2DPre();
        }
    }
}
