using System;
using System.IO;
using JSIL;

namespace RiftGL
{
    using RiftGL.Objects;
    using RiftGL.View;

    public static class Page
    {
        //public static dynamic GL;
        public static dynamic Document;
        public static dynamic Canvas;

        //public static readonly AttributeCollection Attributes = new AttributeCollection();
        //public static readonly UniformCollection Uniforms = new UniformCollection();
        //public static readonly BufferCollection Buffers = new BufferCollection();

        public static bool[] HeldKeys = new bool[255];
        public static int LastTime = 0;

        //public static float Z = -5;
        //public static float RotationX, RotationY;
        //public static float SpeedX = 3, SpeedY = -3;

        public static void Load()
        {
            Document = Builtins.Global["document"];
            Canvas = Document.getElementById("canvas");

            if (InitGL())
            {
                Crate = new Crate { Speed = new Vector(3.0f, -3.0f, 0), Position = new Vector(0, 0, -5) };

                InitMatrices();
                InitShaders();
                InitBuffers();
                InitTexture();

                ViewPort.GL.clearColor(0f, 0f, 0f, 1f);
                ViewPort.GL.enable(ViewPort.GL.DEPTH_TEST);

                Document.onkeydown = (Action<dynamic>)OnKeyDown;
                Document.onkeyup = (Action<dynamic>)OnKeyUp;

                Tick();
            }
        }

        public static ViewPort ViewPort;

        public static Crate Crate;

        public static bool InitGL()
        {
            object gl = null;

            try
            {
                gl = Canvas.getContext("experimental-webgl");
            }
            catch
            {
            }

            if (Builtins.IsTruthy(gl))
            {
                ViewPort = new ViewPort { GL = gl };
                Console.WriteLine("Initialized WebGL");
                return true;
            }
            Builtins.Global["alert"]("Could not initialize WebGL");
            return false;
        }

        public static void InitMatrices()
        {
            ViewPort.InitMatrices(Canvas);
        }

        public static void InitShaders()
        {
            Crate.InitShaders(ViewPort);

            ViewPort.GL.enableVertexAttribArray(ViewPort.Attributes.VertexPosition);
            ViewPort.GL.enableVertexAttribArray(ViewPort.Attributes.VertexNormal);
            ViewPort.GL.enableVertexAttribArray(ViewPort.Attributes.TextureCoord);
        }

        public static void InitBuffers()
        {
            ViewPort.GL.bindBuffer(ViewPort.GL.ARRAY_BUFFER, ViewPort.Buffers.CubeVertexPositions = ViewPort.GL.createBuffer());
            ViewPort.GL.bufferData(ViewPort.GL.ARRAY_BUFFER, CubeData.Positions, ViewPort.GL.STATIC_DRAW);

            ViewPort.GL.bindBuffer(ViewPort.GL.ARRAY_BUFFER, ViewPort.Buffers.CubeVertexNormals = ViewPort.GL.createBuffer());
            ViewPort.GL.bufferData(ViewPort.GL.ARRAY_BUFFER, CubeData.Normals, ViewPort.GL.STATIC_DRAW);

            ViewPort.GL.bindBuffer(ViewPort.GL.ARRAY_BUFFER, ViewPort.Buffers.CubeTextureCoords = ViewPort.GL.createBuffer());
            ViewPort.GL.bufferData(ViewPort.GL.ARRAY_BUFFER, CubeData.TexCoords, ViewPort.GL.STATIC_DRAW);

            ViewPort.GL.bindBuffer(ViewPort.GL.ELEMENT_ARRAY_BUFFER, ViewPort.Buffers.CubeIndices = ViewPort.GL.createBuffer());
            ViewPort.GL.bufferData(ViewPort.GL.ELEMENT_ARRAY_BUFFER, CubeData.Indices, ViewPort.GL.STATIC_DRAW);
        }

        public static void InitTexture()
        {
            Crate.InitTexture(ViewPort, Document);
        }

        public static void Tick()
        {
            Builtins.Global["requestAnimFrame"]((Action)Tick);

            HandleKeys();
            try
            {
                DrawScene();
            }
            catch { }
            Animate();
        }

        public static void HandleKeys()
        {
            if (HeldKeys[33])
            {
                // Page Up
                Crate.Position.Z -= 0.05f;
            }
            if (HeldKeys[34])
            {
                // Page Down
                Crate.Position.Z += 0.05f;
            }
            if (HeldKeys[37])
            {
                // Left cursor key
                Crate.Speed.Y -= 1f;
            }
            if (HeldKeys[39])
            {
                // Right cursor key
                Crate.Speed.Y += 1f;
            }
            if (HeldKeys[38])
            {
                // Up cursor key
                Crate.Speed.X -= 1f;
            }
            if (HeldKeys[40])
            {
                // Down cursor key
                Crate.Speed.X += 1f;
            }
        }

        public static void DrawScene()
        {
            ViewPort.GL.viewport(0, 0, Canvas.width, Canvas.height);
            ViewPort.GL.clear(ViewPort.GL.COLOR_BUFFER_BIT | ViewPort.GL.DEPTH_BUFFER_BIT);

            bool lighting = Document.getElementById("lighting").@checked;
            ViewPort.GL.uniform1i(ViewPort.Uniforms.UseLighting, lighting ? 1 : 0);

            if (lighting)
            {
                ViewPort.DrawLighting(Document);
            }

            Crate.Draw(ViewPort);
        }

        public static void Animate()
        {
            var now = Environment.TickCount;
            if (LastTime != 0)
            {
                var elapsed = now - LastTime;
                if (elapsed > 0)
                {
                    Crate.Animate(elapsed);
                }
            }

            LastTime = now;
        }

        public static void OnKeyDown(dynamic e)
        {
            HeldKeys[e.keyCode] = true;
        }

        public static void OnKeyUp(dynamic e)
        {
            HeldKeys[e.keyCode] = false;
        }
    }
}
