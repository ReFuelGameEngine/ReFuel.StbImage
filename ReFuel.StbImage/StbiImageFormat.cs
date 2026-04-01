using ReFuel.Stb.Native;

namespace ReFuel.Stb
{
    /// <summary>
    /// Enumeration of supported STBI image formats.
    /// </summary>
    public enum StbiImageFormat
    {
        Default     = (int)StbiEnum.STBI_default,
        Grey        = (int)StbiEnum.STBI_grey,
        GreyAlpha   = (int)StbiEnum.STBI_grey_alpha,
        Rgb         = (int)StbiEnum.STBI_rgb,
        Rgba        = (int)StbiEnum.STBI_rgb_alpha
    }
}