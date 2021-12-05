using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using AnimeTime.Utilities.Colors;

namespace TestingProject
{
    [TestClass]
    public class TestingClass
    {
        [TestMethod]
        public void TestColorToHSLA()
        {
            Color c = Color.FromArgb(50, 120, 120);
            var hsla = c.ToHslaColor();

            var color = hsla.ToColor();

            var mediaColor = hsla.ToMediaColor();
        }

        [TestMethod]
        public void Test()
        {

        }
    }
}
