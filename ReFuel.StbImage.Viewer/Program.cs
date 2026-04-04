/*
 * ReFuel.StbImage.Viewer - A simple image viewer demo for ReFuel.StbImage.
 * ------------------------------------------------------------------------
 *
 * Pass an image file as path (or drag and drop over the executable or window) to view
 * of the file formats stb_image supports. Otherwise the default embedded image will be
 * shown.
 *
 * The demo uses OpenGL2.1 for brevity sake - I did not feel like writing a shader program
 * for this demo. It is very easy to port this demo to modern OpenGL if desired. Just
 * replace the FFP calls with the equivalent programmable pipeline calls (glVertexAttribPointer,
 * glUseShader and its friends).
 */

using System.Reflection;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using ReFuel.Stb;

NativeWindow window = new NativeWindow(new NativeWindowSettings()
{
    Profile = ContextProfile.Any,
    APIVersion = new Version(2, 1),
    Title = "ReFuel StbImage Viewer",
    AutoLoadBindings = true,
    Flags = ContextFlags.Default,
});

bool quit = false;
int texture = 0;
int imageWidth = 0, imageHeight = 0;

// This flag is important for OpenGL users, as texture coordinate systems
// are the opposite of what most image formats use. Y is up not down.
// Does not matter for DX, for example.
StbImage.FlipVerticallyOnLoad = true;

Vertex[] vertices = new Vertex[]
{
    new Vertex(-1, -1, 0, 0),
    new Vertex(1, -1, 1, 0),
    new Vertex(1, 1, 1, 1),
    new Vertex(-1, -1, 0, 0),
    new Vertex(1, 1, 1, 1),
    new Vertex(-1, 1, 0, 1),
};

LoadImage(args.Length > 0 ? args[0] : null);

GL.ClearColor(Color4.Black);
GL.Enable(EnableCap.Texture2D);
GL.Enable(EnableCap.Blend);
GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); // conventional blending function.
GL.EnableClientState(ArrayCap.VertexArray);
GL.EnableClientState(ArrayCap.TextureCoordArray);
GL.EnableClientState(ArrayCap.ColorArray);

window.Closing += (_) => quit = true;
window.FramebufferResize += (_) => {
    ResizeImage();
    Paint();
};
window.FileDrop += (args) => 
    LoadImage(
        args.FileNames
            .Select(x => new FileInfo(x))
            .FirstOrDefault(x => x.Exists)?.FullName);

window.WindowState = WindowState.Normal;
while (!quit)
{
    NativeWindow.ProcessWindowEvents(true);
    Paint();
};

void Paint()
{
    GL.Clear(ClearBufferMask.ColorBufferBit);
    GL.Viewport(0, 0, window.FramebufferSize.X, window.FramebufferSize.Y);
    GL.BindTexture(TextureTarget.Texture2D, texture);

    unsafe
    {
        fixed (Vertex* pvert = vertices)
        {
            // We have to do it this way because the garbage collector may
            // move the vertex data at any time. The OpenGL client will
            // handle streaming the data at the glDrawArrays call, unlike
            // the modern method.
            GL.VertexPointer(2, VertexPointerType.Float, 32, (nint)(&pvert->X));
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 32, (nint)(&pvert->U));
            GL.ColorPointer(4, ColorPointerType.Float, 32, (nint)(&pvert->Color));
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }
    }

    window.Context.SwapBuffers();
}

void ResizeImage()
{
    // Regenerates the vertex positions to maintain the aspect ratio and fill the window.
    float windowAspect = (float)window.FramebufferSize.X / window.FramebufferSize.Y;
    float imageAspect = (float)imageWidth / imageHeight;
    float ratio = imageAspect / windowAspect;

    float widthNDC, heightNDC;
    if (ratio > 1)
    {
        widthNDC = 2.0f;
        heightNDC = 2.0f / ratio;
    }
    else
    {
        heightNDC = 2.0f;
        widthNDC = 2.0f * ratio;
    }

    vertices[0].X = -widthNDC / 2; vertices[0].Y = -heightNDC / 2;
    vertices[1].X = widthNDC / 2; vertices[1].Y = -heightNDC / 2;
    vertices[2].X = widthNDC / 2; vertices[2].Y = heightNDC / 2;
    vertices[3].X = -widthNDC / 2; vertices[3].Y = -heightNDC / 2;
    vertices[4].X = widthNDC / 2; vertices[4].Y = heightNDC / 2;
    vertices[5].X = -widthNDC / 2; vertices[5].Y = heightNDC / 2;
}

void LoadImage(string? path)
{
    // Load the given image, or the default if the path is null, or any other error happens.
    StbImage? image = null;
    if (path != null && File.Exists(path))
    {
        try
        {
            using Stream str = File.OpenRead(path);
            StbImage.TryLoad(out image, str);
        }
        catch
        {
            // Ignore
        }
    }

    if (image == null)
    {
        using Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("default.png")!;
        image = StbImage.Load(str);
    }

    // Boilerplate code for creating a new OpenGL texture.

    GL.DeleteTexture(texture);
    texture = GL.GenTexture();
    GL.BindTexture(TextureTarget.Texture2D, texture);
    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, image.Format switch
    {
        StbiImageFormat.Grey => PixelFormat.Red,
        StbiImageFormat.GreyAlpha => PixelFormat.Rg,
        StbiImageFormat.Rgb => PixelFormat.Rgb,
        StbiImageFormat.Rgba => PixelFormat.Rgba,
        _ => throw new Exception()
    }, image.IsFloat ? PixelType.Float : PixelType.UnsignedByte, image.ImagePointer);
    
    // Generate mipmaps for better minification.
    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    // Enable them.
    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

    // Set texture wrap mode to clamp to border to prevent bleeding on the image edges.
    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

    // For R or RA format images, we need to set the swizzle mask.
    switch (image.Format)
    {
        case StbiImageFormat.Grey:
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureSwizzleR, (int)TextureSwizzle.Red);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureSwizzleG, (int)TextureSwizzle.Red);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureSwizzleB, (int)TextureSwizzle.Red);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureSwizzleA, (int)TextureSwizzle.One);
            break;
        case StbiImageFormat.GreyAlpha:
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureSwizzleR, (int)TextureSwizzle.Red);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureSwizzleG, (int)TextureSwizzle.Red);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureSwizzleA, (int)TextureSwizzle.Green);
            // Yes the last channel is green, we uploaded the texture with the Rg color format.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureSwizzleB, (int)TextureSwizzle.Red);
            break;
    }

    imageWidth = image.Width;
    imageHeight = image.Height;
    ResizeImage();

    image.Dispose();
}

// Vertex struct for convenience. Padded to 32 bytes for memory alignment.
[StructLayout(LayoutKind.Sequential, Size = 32)]
struct Vertex(float x, float y, float u, float v)
{
    public float X = x, Y = y, U = u, V = v;
    public Color4 Color = Color4.White;
}

