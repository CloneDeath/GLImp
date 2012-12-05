using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Drawing;

namespace GLImpUnitTest.Tests
{
    public class BasicTest : TestSetup
    {
        public override void Render2DPre()
        {
            for (int i = 0; i < 29*29; i++)
            {
                int XPos = (i % 29);
                int YPos = (i / 29);
                if (i % 2 == 0)
                {
                    GraphicsManager.DrawRectangle(XPos * 10, YPos * 10, 10, 10, Color.Green);
                }
            }
        }
    }
}
