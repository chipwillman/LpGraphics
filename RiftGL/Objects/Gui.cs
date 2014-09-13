namespace RiftGL.Objects
{
    using RiftGL.View;

    public class Gui : GlObject
    {
        public float CurrentTime { get; set; }

        public int EnemiesLeft { get; set; }

        protected override void OnDraw(Camera camera)
        {
            ViewPort.Document.getElementById("cameraLocationX").value = camera.Location.X;
            ViewPort.Document.getElementById("cameraLocationY").value = camera.Location.Y;
            ViewPort.Document.getElementById("cameraLocationZ").value = camera.Location.Z;

            ViewPort.Document.getElementById("cameraRotationX").value = camera.Rotation.X;
            ViewPort.Document.getElementById("cameraRotationY").value = camera.Rotation.Y;
            ViewPort.Document.getElementById("cameraRotationZ").value = camera.Rotation.Z;
        }
    }
}
