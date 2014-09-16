namespace RiftGL.Objects
{
    public class ModelUpdate
    {
        public enum UpdateType
        {
            Map = 0,
            Object = 1
        }

        public UpdateType Type { get; set; }
        public Map World { get; set; }
        public GlObject Model { get; set; }
    }
}
