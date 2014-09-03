namespace RiftGL.View
{
    using JSIL;

    using RiftGL.Objects;

    public class ViewPort
    {
        public static dynamic GLVector3, GLMatrix3, GLMatrix4;

        public static readonly AttributeCollection Attributes = new AttributeCollection();
        public static readonly UniformCollection Uniforms = new UniformCollection();
        public static readonly BufferCollection Buffers = new BufferCollection();
        public static readonly MatrixCollection Matrices = new MatrixCollection();

        public ViewPort()
        {
            GLVector3 = Builtins.Global["vec3"];
            GLMatrix4 = Builtins.Global["mat4"];
            GLMatrix3 = Builtins.Global["mat3"];
        }

        public dynamic GL;

        public void UploadTexture(object textureHandle, object imageElement)
        {
            GL.pixelStorei(GL.UNPACK_FLIP_Y_WEBGL, true);

            GL.bindTexture(GL.TEXTURE_2D, textureHandle);
            GL.texImage2D(GL.TEXTURE_2D, 0, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, imageElement);
            GL.texParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.LINEAR);
            GL.texParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.LINEAR_MIPMAP_NEAREST);
            GL.generateMipmap(GL.TEXTURE_2D);

            GL.bindTexture(GL.TEXTURE_2D, null);
        }

        public void InitMatrices(dynamic canvas)
        {
            Matrices.ModelView = GLMatrix4.create();
            Matrices.Projection = GLMatrix4.create();
            Matrices.Normal = GLMatrix3.create();

            GLMatrix4.perspective(45, canvas.width / canvas.height, 0.1, 100.0, Matrices.Projection);
        }

        public void DrawLighting(dynamic Document)
        {
            GL.uniform3f(
                Uniforms.AmbientColor,
                float.Parse(Document.getElementById("ambientR").value),
                float.Parse(Document.getElementById("ambientG").value),
                float.Parse(Document.getElementById("ambientB").value)
            );

            var lightingDirection = new[] {
                    float.Parse(Document.getElementById("lightDirectionX").value),
                    float.Parse(Document.getElementById("lightDirectionY").value),
                    float.Parse(Document.getElementById("lightDirectionZ").value)
                };

            GLVector3.normalize(lightingDirection, lightingDirection);
            GLVector3.scale(lightingDirection, -1);

            GL.uniform3fv(Uniforms.LightingDirection, lightingDirection);
            GL.uniform3f(
                Uniforms.DirectionalColor,
                float.Parse(Document.getElementById("directionalR").value),
                float.Parse(Document.getElementById("directionalG").value),
                float.Parse(Document.getElementById("directionalB").value)
            );
        }
    }
}
