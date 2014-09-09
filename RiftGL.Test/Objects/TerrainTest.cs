namespace RiftGL.Test.Objects
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RiftGL.Objects;

    [TestClass]
    public class TerrainTest
    {
        [TestMethod]
        public void TestGenerateHeightMap()
        {
            var width = 4;
            var maxHeight = 20f;
            var terrain = new Terrain();
            terrain.BuildTerrain(1, width, 1.0f, null);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var height = terrain.HeightMap.Values[i * width + j];
                    Assert.IsTrue(height < maxHeight);
                }
            }
        }

        [TestMethod]
        public void TestGenerateVertexPositionsBuffer()
        {
            var width = 4;
            var maxHeight = 20f;
            var terrain = new Terrain();
            terrain.BuildTerrain(1, width, 1.0f, null);
            terrain.CreateVertesArray();

            for (int i = 0; i < width; i++)
            {
                var previous = -100f;
                var expectedZ = terrain.TerrainData.Vertices[i * 12 + 2];

                for (int j = 0; j < width; j++)
                {
                    var x = terrain.TerrainData.Vertices[i * 12 + j * 3];
                    var y = terrain.TerrainData.Vertices[i * 12 + j * 3 + 1];
                    var z = terrain.TerrainData.Vertices[i * 12 + j * 3 + 2];

                    Assert.IsTrue(previous < x);
                    Assert.IsTrue(y < maxHeight);

                    Assert.IsTrue(z < (width / 4.0f));
                    Assert.AreEqual(expectedZ, z);

                    previous = x;
                }
            }
        }

        [TestMethod]
        public void TestIndices()
        {
            var width = 4;
            var terrain = new Terrain();
            terrain.BuildTerrain(1, width, 1.0f, null);
            terrain.CreateVertesArray();
            terrain.CreateIndices();

            Assert.AreEqual(4, terrain.TerrainData.Indices[0]);
            Assert.AreEqual(0, terrain.TerrainData.Indices[1]);
            Assert.AreEqual(5, terrain.TerrainData.Indices[2]);
            Assert.AreEqual(1, terrain.TerrainData.Indices[3]);
            Assert.AreEqual(6, terrain.TerrainData.Indices[4]);
            Assert.AreEqual(2, terrain.TerrainData.Indices[5]);
        }
    }
}
