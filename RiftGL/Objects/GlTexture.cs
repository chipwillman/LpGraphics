namespace RiftGL.Objects
{
    using System;
    using System.IO;

    using JSIL;

    using RiftGL.View;

    public class GlTexture
    {
        public object Texture { get; set; }

        public static GlTexture LoadTexture(ViewPort viewPort, string uri)
        {
            var result = new GlTexture();

            result.Texture = viewPort.GL.createTexture();

            var imageElement = ViewPort.Document.createElement("img");
            imageElement.onload = (Action)(
                () => viewPort.UploadTexture(result.Texture, imageElement)
            );

            try
            {
                var imageBytes = File.ReadAllBytes("crate.png");
                var objectUrl = Builtins.Global["JSIL"].GetObjectURLForBytes(imageBytes, "image/png");
                imageElement.src = objectUrl;
            }
            catch
            {
                // Object URLs probably aren't supported. Load the image a second time. ;/
                Console.WriteLine("Falling back to a second HTTP request for crate.png because Object URLs are not available");
                imageElement.src = "Files/crate.png";
            }

            return result;
        }
    }
}
