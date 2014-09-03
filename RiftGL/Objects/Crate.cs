namespace RiftGL.Objects
{
    using System;
    using System.IO;

    using JSIL;

    using RiftGL.View;

    public class Crate : GlObject
    {
        public Crate()
            : base()
        {
            Rotation = new Vector();
        }

        public static object ShaderProgram;
        public static object CrateTexture;

        public Vector Rotation;

        public Vector Speed;

        public void InitShaders(ViewPort viewPort)
        {
            var fragmentShader = CompileShader(viewPort, "crate.fs");
            var vertexShader = CompileShader(viewPort, "crate.vs");

            ShaderProgram = viewPort.GL.createProgram();
            viewPort.GL.attachShader(ShaderProgram, vertexShader);
            viewPort.GL.attachShader(ShaderProgram, fragmentShader);
            viewPort.GL.linkProgram(ShaderProgram);

            bool linkStatus = viewPort.GL.getProgramParameter(ShaderProgram, viewPort.GL.LINK_STATUS);
            if (!linkStatus)
            {
                Builtins.Global["alert"]("Could not link shader");
                return;
            }

            viewPort.GL.useProgram(ShaderProgram);

            ViewPort.Attributes.VertexPosition = viewPort.GL.getAttribLocation(ShaderProgram, "aVertexPosition");
            ViewPort.Attributes.VertexNormal = viewPort.GL.getAttribLocation(ShaderProgram, "aVertexNormal");
            ViewPort.Attributes.TextureCoord = viewPort.GL.getAttribLocation(ShaderProgram, "aTextureCoord");

            ViewPort.Uniforms.ProjectionMatrix = viewPort.GL.getUniformLocation(ShaderProgram, "uPMatrix");
            ViewPort.Uniforms.ModelViewMatrix = viewPort.GL.getUniformLocation(ShaderProgram, "uMVMatrix");
            ViewPort.Uniforms.NormalMatrix = viewPort.GL.getUniformLocation(ShaderProgram, "uNMatrix");
            ViewPort.Uniforms.Sampler = viewPort.GL.getUniformLocation(ShaderProgram, "uSampler");
            ViewPort.Uniforms.UseLighting = viewPort.GL.getUniformLocation(ShaderProgram, "uUseLighting");
            ViewPort.Uniforms.AmbientColor = viewPort.GL.getUniformLocation(ShaderProgram, "uAmbientColor");
            ViewPort.Uniforms.LightingDirection = viewPort.GL.getUniformLocation(ShaderProgram, "uLightingDirection");
            ViewPort.Uniforms.DirectionalColor = viewPort.GL.getUniformLocation(ShaderProgram, "uDirectionalColor");
        }

        public static dynamic CompileShader(ViewPort viewPort, string filename)
        {
            var extension = Path.GetExtension(filename).ToLower();

            dynamic shaderObject;

            switch (extension)
            {
                case "fs":
                    shaderObject = viewPort.GL.createShader(viewPort.GL.FRAGMENT_SHADER);
                    break;

                case "vs":
                    shaderObject = viewPort.GL.createShader(viewPort.GL.VERTEX_SHADER);
                    break;

                default:
                    throw new NotImplementedException(extension);
            }

            var shaderText = File.ReadAllText(filename);

            viewPort.GL.shaderSource(shaderObject, shaderText);
            viewPort.GL.compileShader(shaderObject);

            bool compileStatus = viewPort.GL.getShaderParameter(shaderObject, viewPort.GL.COMPILE_STATUS);
            if (!compileStatus)
            {
                Builtins.Global["alert"](viewPort.GL.getShaderInfoLog(shaderObject));
                return null;
            }

            Console.WriteLine("Loaded " + filename);
            return shaderObject;
        }

        public void InitTexture(ViewPort viewPort, dynamic document)
        {
            CrateTexture = viewPort.GL.createTexture();

            var imageElement = document.createElement("img");
            imageElement.onload = (Action)(
                () => viewPort.UploadTexture(CrateTexture, imageElement)
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
        }

        public static float DegreesToRadians(float degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        protected override void OnAnimate(float deltaTime)
        {
            base.OnAnimate(deltaTime);
            Rotation.X += (Speed.X * deltaTime) / 1000f;
            Rotation.Y += (Speed.Y * deltaTime) / 1000f;
        }

        protected override void OnDraw(ViewPort camera)
        {
            ViewPort.GLMatrix4.identity(ViewPort.Matrices.ModelView);
            ViewPort.GLMatrix4.translate(ViewPort.Matrices.ModelView, new[] { 0, 0, Position.Z });
            ViewPort.GLMatrix4.rotate(ViewPort.Matrices.ModelView, DegreesToRadians(Rotation.X), new[] { 1f, 0, 0 });
            ViewPort.GLMatrix4.rotate(ViewPort.Matrices.ModelView, DegreesToRadians(Rotation.Y), new[] { 0, 1f, 0 });

            camera.GL.bindBuffer(camera.GL.ARRAY_BUFFER, ViewPort.Buffers.CubeVertexPositions);
            camera.GL.vertexAttribPointer(ViewPort.Attributes.VertexPosition, 3, camera.GL.FLOAT, false, 0, 0);

            camera.GL.bindBuffer(camera.GL.ARRAY_BUFFER, ViewPort.Buffers.CubeVertexNormals);
            camera.GL.vertexAttribPointer(ViewPort.Attributes.VertexNormal, 3, camera.GL.FLOAT, false, 0, 0);

            camera.GL.bindBuffer(camera.GL.ARRAY_BUFFER, ViewPort.Buffers.CubeTextureCoords);
            camera.GL.vertexAttribPointer(ViewPort.Attributes.TextureCoord, 2, camera.GL.FLOAT, false, 0, 0);

            camera.GL.activeTexture(camera.GL.TEXTURE0);
            camera.GL.bindTexture(camera.GL.TEXTURE_2D, CrateTexture);
            camera.GL.uniform1i(ViewPort.Uniforms.Sampler, 0);

            camera.GL.bindBuffer(camera.GL.ELEMENT_ARRAY_BUFFER, ViewPort.Buffers.CubeIndices);

            camera.GL.uniformMatrix4fv(ViewPort.Uniforms.ProjectionMatrix, false, ViewPort.Matrices.Projection);
            camera.GL.uniformMatrix4fv(ViewPort.Uniforms.ModelViewMatrix, false, ViewPort.Matrices.ModelView);

            ViewPort.GLMatrix4.toInverseMat3(ViewPort.Matrices.ModelView, ViewPort.Matrices.Normal);
            ViewPort.GLMatrix3.transpose(ViewPort.Matrices.Normal);
            camera.GL.uniformMatrix3fv(ViewPort.Uniforms.NormalMatrix, false, ViewPort.Matrices.Normal);

            camera.GL.drawElements(camera.GL.TRIANGLES, CubeData.Indices.Length, camera.GL.UNSIGNED_SHORT, 0);
        }
    }
}
