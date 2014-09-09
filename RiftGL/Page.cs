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
                InitMatrices();
                InitShaders();
                InitBuffers();
                InitTexture();

                Document.onkeydown = (Action<dynamic>)OnKeyDown;
                Document.onkeyup = (Action<dynamic>)OnKeyUp;
                Document.onmousemove = (Action<dynamic>)OnMouseMove;

                Tick();
            }
        }

        // public static ViewPort ViewPort;

        public static Camera Camera;

        public static World World;

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
                Camera = new Camera { GL = gl, position = new Vector(0, 0, -3) };
                ViewPort.Document = Document;
                ViewPort.Canvas = Canvas;
                World = new World(Camera);
                Console.WriteLine("Initialized WebGL");
                return true;
            }
            Builtins.Global["alert"]("Could not initialize WebGL");
            return false;
        }

        public static void InitMatrices()
        {
            Camera.InitMatrices(Canvas);
        }

        public static void InitShaders()
        {
            //Crate.InitShaders(ViewPort);

            //ViewPort.GL.enableVertexAttribArray(ViewPort.Attributes.VertexPosition);
            //ViewPort.GL.enableVertexAttribArray(ViewPort.Attributes.VertexNormal);
            //ViewPort.GL.enableVertexAttribArray(ViewPort.Attributes.TextureCoord);
        }

        public static void InitBuffers()
        {
            Camera.GL.bindBuffer(Camera.GL.ARRAY_BUFFER, ViewPort.Buffers.VertexPositions = Camera.GL.createBuffer());
            Camera.GL.bufferData(Camera.GL.ARRAY_BUFFER, CubeData.Positions, Camera.GL.STATIC_DRAW);

            Camera.GL.bindBuffer(Camera.GL.ARRAY_BUFFER, ViewPort.Buffers.VertexNormals = Camera.GL.createBuffer());
            Camera.GL.bufferData(Camera.GL.ARRAY_BUFFER, CubeData.Normals, Camera.GL.STATIC_DRAW);

            Camera.GL.bindBuffer(Camera.GL.ARRAY_BUFFER, ViewPort.Buffers.TextureCoords = Camera.GL.createBuffer());
            Camera.GL.bufferData(Camera.GL.ARRAY_BUFFER, CubeData.TexCoords, Camera.GL.STATIC_DRAW);

            Camera.GL.bindBuffer(Camera.GL.ELEMENT_ARRAY_BUFFER, ViewPort.Buffers.Indices = Camera.GL.createBuffer());
            Camera.GL.bufferData(Camera.GL.ELEMENT_ARRAY_BUFFER, CubeData.Indices, Camera.GL.STATIC_DRAW);
        }

        public static void InitTexture()
        {
        }

        public static void Tick()
        {
            Builtins.Global["requestAnimFrame"]((Action)Tick);

            HandleKeys();

            OnPrepare();

            World.Prepare();

            Animate();

            try
            {
                DrawScene();
            }
            catch { }
        }

        public static void HandleKeys()
        {
            if (HeldKeys[33])
            {
                // Page Up
                Camera.velocity.Y += 0.2f;
            }

            if (HeldKeys[34])
            {
                // Page Down
                Camera.velocity.Y += -0.2f;
            }

            if (HeldKeys[37])
            {
                // Left cursor key
                Camera.velocity += new Vector(1.0f, 0, 0);
            }

            if (HeldKeys[39])
            {
                // Right cursor key
                Camera.velocity += new Vector(-1.0f, 0, 0);
            }

            if (HeldKeys[38])
            {
                // Up cursor key
                Camera.velocity += new Vector(0, 0, 2f);
            }

            if (HeldKeys[40])
            {
                // Down cursor key
                Camera.velocity += new Vector(0, 0, -2f);
            }
        }

        public static void DrawScene()
        {
            Camera.GL.viewport(0, 0, Canvas.width, Canvas.height);
            Camera.GL.clear(Camera.GL.COLOR_BUFFER_BIT | Camera.GL.DEPTH_BUFFER_BIT);

            World.Draw(Camera);

            bool lighting = Document.getElementById("lighting").@checked;
            Camera.GL.uniform1i(ViewPort.Uniforms.UseLighting, lighting ? 1 : 0);

            if (lighting)
            {
                Camera.DrawLighting();
            }
        }

        public static void Animate()
        {
            var now = Environment.TickCount;
            if (LastTime != 0)
            {
                var elapsed = now - LastTime;
                if (elapsed > 0)
                {
                    Camera.Animate(elapsed);
                    World.Animate(elapsed);
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

        static float oldX;
        static float oldY;
        static float yaw = 0.0f;
        static float pitch = 0.0f;

        private static int mouseSensitivity = 10;

        public static void OnMouseMove(dynamic e)
        {
            var centerX = Camera.Canvas.width / 2;
            var centerY = Camera.Canvas.height /2;
               
	        float mX, mY;

	        mX = (float)e.x;
	        mY = (float)e.y;

	        if (mX < centerX/2)
		        Camera.yaw -= 0.25f*mouseSensitivity;
	        if (mX > centerX/2)
		        Camera.yaw += 0.25f*mouseSensitivity;

	        if (mY < centerY/2)
		        Camera.pitch += 0.25f*mouseSensitivity;
	        if (mY > centerY/2)
		        Camera.pitch -= 0.25f*mouseSensitivity;

	        oldX = mX;
	        oldY = mY;            
        }

        private static void OnPrepare()
        {
            Camera.GL.clearColor(0f, 0f, 0f, 1f);
            Camera.GL.enable(Camera.GL.DEPTH_TEST);
        }
    }
}
