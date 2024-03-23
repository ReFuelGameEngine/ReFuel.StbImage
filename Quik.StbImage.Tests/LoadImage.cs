using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Quik.Stb.Tests
{
    [TestClass]
    [TestCategory("Load Image")]
    public class LoadImage
    {
        [TestMethod("Set Global Options")]
        public void SetGlobals()
        {
            Quik.Stb.StbImage.FlipVerticallyOnLoad = true;
            Quik.Stb.StbImage.UnpremultiplyOnLoad = true;
        }

        private Stream GetImage(string path)
        {
            Stream? str = GetType().Assembly.GetManifestResourceStream(path);
            Assert.IsNotNull(str, $"Could not find test image resource {path}.");
            return str;
        }

        private unsafe void TestImage(string path, int width, int height)
        {
            StbImage image = StbImage.Load(GetImage(path));

            Assert.IsNotNull(image);

            Assert.AreEqual(width, image.Width, "Width does not match.");
            Assert.AreEqual(height, image.Height, "Height does not match.");

            image.Dispose();
        }

        const int WIDTH = 768;
        const int HEIGHT = 512;

        [TestMethod("Load a single frame GIF")]
        public unsafe void LoadGif() => TestImage("Quik.Stb.Tests.res.kodim.kodim23.gif", WIDTH, HEIGHT);
        [TestMethod("Load a JPEG")]
        public unsafe void LoadJpg() => TestImage("Quik.Stb.Tests.res.kodim.kodim23.jpg", WIDTH, HEIGHT);
        [TestMethod("Load a PNG")] public unsafe void LoadPng() => TestImage("Quik.Stb.Tests.res.kodim.kodim23.png", WIDTH, HEIGHT);
    }
}