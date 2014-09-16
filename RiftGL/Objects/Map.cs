namespace RiftGL.Objects
{
    public class Map
    {
        public float X { get; set; }
        public float Z { get; set; }
        public float SizeX { get; set; }
        public float SizeZ { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public float[] HeightMap { get; set; }
    }
}
