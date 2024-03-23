using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Quik.Stb;

namespace Quik.Stb
{
    [TestClass]
    [TestCategory("Load Font")]
    public class LoadFont
    {
        StbFont? font;

        [TestInitialize]
        public void Initialize()
        {
            using (Stream? str = GetType().Assembly.GetManifestResourceStream("Quik.Stb.Tests.res.Varicka.ttf"))
            {
                Assert.IsNotNull(str, "Test font file not packed.");
                font = StbFont.Load(str);
            }
        }

        [TestCleanup]
        public void Deinitialize()
        {
            font?.Dispose();
        }

        [TestMethod]
        public void AscendIsValid()
        {
            Assert.AreNotEqual(-1, font!.Ascend);
        }

        [TestMethod]
        public void DescendIsValid()
        {
            Assert.AreNotEqual(-1, font!.Descend);
        }

        [TestMethod]
        public void VLineGapIsValid()
        {
            Assert.AreNotEqual(-1, font!.VerticalLineGap);
        }

        [TestMethod]
        public void BBoxIsValid()
        {
            Assert.AreNotEqual(default, font!.BoundingBox);
        }

        [TestMethod]
        public void KerningTableIsValid()
        {
            Assert.IsNotNull(font!.KerningTable);
        }

        [TestMethod]
        public void GetGlyphsForAscii()
        {
            for (int i = 0; i < 128; i++)
            {
                int glyph = font!.FindGlyphIndex(i);

                Assert.AreNotEqual(-1, glyph);
            }
        }
    }
}