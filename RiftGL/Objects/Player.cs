namespace RiftGL.Objects
{
    using RiftGL.View;

    public class Player : GlObject
    {
        public Player()
        {
            this.Size = 2f;
        }

        public void SetCamera(Camera camera)
        {
            this.Camera = camera;
        }

        public void DetachCamera()
        {
            this.Camera = null;
        }

        public void SetTerrain(Terrain terrain)
        {
            this.Terrain = terrain;
        }

        public Camera Camera;

        public Terrain Terrain;
    }
}
