namespace RiftGL.Objects
{
    using RiftGL.View;

    public class World
    {
        public World(Camera camera)
        {
            var width = 32;
            this.Camera = camera;
            this.Terrain = new Terrain { Position = new Vector(0, 0, -10) };
            this.Crate = new Crate { Position = new Vector(0, 0, -20)};
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

        // public OgroEnemy OrgoEnemy { get; set;
        // public SodEnemy SodEnemy { get; set;

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

            //if (Camera.position.X <= Terrain.GetScanDepth())
            //{
            //    Camera.position.X = Terrain.GetScanDepth();
            //}

            //if (Camera.position.X >= Terrain.GetWidth() * Terrain.GetMul() - Terrain.GetScanDepth())
            //{
            //    Camera.position.X = Terrain.GetWidth() * Terrain.GetMul() - Terrain.GetScanDepth();
            //}

            //if (Camera.position.Z <= Terrain.GetScanDepth())
            //{
            //    Camera.position.Z = Terrain.GetScanDepth();
            //}

            //if (Camera.position.Z >= Terrain.GetWidth() * Terrain.GetMul() - Terrain.GetScanDepth())
            //{
            //    Camera.position.Z = Terrain.GetWidth() * Terrain.GetMul() - Terrain.GetScanDepth();
            //}


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

        public void LoadWorld()
        {
            
        }

        public void UnloadWorld()
        {
            
        }

        #region Implementation

        // private int numberOgros;

        // private int numberSods;

        private bool gameDone;

        public void Prepare()
        {
            //glClearColor(terrain->fogColor[0], terrain->fogColor[1], terrain->fogColor[2], terrain->fogColor[3]);
	
            Terrain.Prepare();
            Crate.Prepare();
        }

        #endregion
    }
}
