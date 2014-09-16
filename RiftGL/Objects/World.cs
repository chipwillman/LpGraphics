namespace RiftGL.Objects
{
    using System;

    using RiftGL.View;

    public class World
    {
        public World(Camera camera)
        {
            var width = 32;
            this.Camera = camera;
            this.Terrain = new Terrain { Position = new Vector(-width/2f, 0, -width/2f) };
            this.Crate = new Crate { Position = new Vector(0, 0, 3), Size = 1};
            this.Crate.InitTexture(camera);
            this.Crate.InitShaders(camera);

            this.Terrain.BuildTerrain(1, width, 1.0f, camera);

            this.Player = new Player();
            this.Gui = new Gui();

            this.Player.AttachTo(this.Terrain);
            this.Player.SetCamera(this.Camera);
            this.Player.SetTerrain(this.Terrain);
            this.timeStart = 300f;
            this.timeEnd = 0f;
            this.Gui.CurrentTime = timeStart;
            this.Gui.EnemiesLeft = 0;
        }

        public Terrain Terrain { get; set; }

        public Crate Crate { get; set; }

        public Camera Camera { get; set; }

        public Player Player { get; set; }

        public AudioSystem AudioSystem { get; set; }

        public Audio WorldSound { get; set; }

        public Gui Gui { get; set; }

        public int MapRequestSent;

        public float timeStart { get; set; }

        public float timeElapsed { get; set; }

        public float timeEnd { get; set; }

        public void Animate(float deltaTime)
        {
            var terrainHeight = Terrain.GetHeight(Camera.Location.X, Camera.Location.Z);
            if (Camera.Location.Y < terrainHeight + Player.Size)
            {
                Camera.Location.Y = terrainHeight + Player.Size;
            }
            Player.Position = Camera.Location;

            terrainHeight = Terrain.GetHeight(Crate.Position.X, Crate.Position.Z);
            if (Crate.Position.Y < terrainHeight + Crate.Size)
            {
                Crate.Position.Y = terrainHeight + Crate.Size;
            }

            //Terrain.EnsurePlayerMap(Player);

            Terrain.Animate(deltaTime);

            Gui.CurrentTime = timeStart - timeElapsed;

            if (!gameDone)
                timeElapsed += deltaTime;
            else
                timeElapsed = timeStart;
        }

        public void Draw(Camera camera)
        {
            //ViewPort.GLMatrix4.identity(ViewPort.Matrices.ModelView);
            ViewPort.GLMatrix4.translate(ViewPort.Matrices.ModelView, ViewPort.Matrices.ModelView, new[] { camera.Location.X, camera.Location.Y, camera.Location.Z });

            Terrain.Draw(camera);
            Crate.Draw(camera);
            //Gui.Draw();
        }

        public void LoadWorld(dynamic worldMap, Camera camera)
        {
            Terrain.InitBuffers(camera);
        }

        public void Prepare()
        {
            Camera.GL.clearColor(Terrain.fogColor[0], Terrain.fogColor[1], Terrain.fogColor[2], Terrain.fogColor[3]);
            var terrain2D = new Vector(Terrain.Position.X, 0, Terrain.Position.Z);
            var camera2D = new Vector(Camera.Location.X, 0, Camera.Location.Z);

            if (Math.Abs((terrain2D - camera2D).Length()) > Terrain.Width / 4f)
            {
                RequestMapUpdate();
            }

            Terrain.Prepare();
            Crate.Prepare();
        }

        public void UnloadWorld()
        {
            
        }

        #region Implementation

        // private int numberOgros;

        // private int numberSods;

        private bool gameDone;

        private void RequestMapUpdate()
        {
            var now = Environment.TickCount;
            if ((now - MapRequestSent) > 15000)
            {
                MapRequestSent = Environment.TickCount;
                var repository = new RiftRepository("lpmud.local", "user", "password");
                repository.LoadMap(Camera.Location, 128f, Page.LoadWorldCallback);
            }
        }

        #endregion
    }
}
