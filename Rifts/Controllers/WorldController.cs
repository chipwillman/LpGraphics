namespace Rifts.Controllers
{
    using System;
    using System.Web;
    using System.Web.Http;

    using RiftGL.Objects;

    public class WorldController : ApiController
    {
        public static Terrain GameWorld
        {
            get
            {
                if (gameWorld == null)
                {
                    gameWorld = new Terrain();
                    gameWorld.BuildTerrain(42, 1024, 0.5f, null);
                }
                return gameWorld;
            }
        }

        private static Terrain gameWorld;

        // GET api/world
        public Map Get()
        {

            var queryString = Request.RequestUri.Query;

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                string locationX = HttpUtility.ParseQueryString(queryString.Substring(1))["locationX"];
                string locationY = HttpUtility.ParseQueryString(queryString.Substring(1))["locationY"];
                string locationZ = HttpUtility.ParseQueryString(queryString.Substring(1))["locationZ"];
                string distanceQuery = HttpUtility.ParseQueryString(queryString.Substring(1))["distance"];
                if (!string.IsNullOrEmpty(locationX) && !string.IsNullOrEmpty(locationY)
                    && !string.IsNullOrEmpty(locationZ))
                {
                    var location = new Vector(float.Parse(locationX), float.Parse(locationY), float.Parse(locationZ));
                    var distance = int.Parse(distanceQuery);
                    var result = new Map
                                     {
                                         X = (int)location.X,
                                         Z = (int)location.Z,
                                         Rows = distance,
                                         Columns = distance,
                                         HeightMap = FilterRows(location, distance)
                                     };

                    return result;
                }
            }

            return new Map
            {
                X = -128f,
                Z = -128f,
                Rows = 256,
                Columns = 256,
                HeightMap = gameWorld.HeightMap.Values
            };

        }

        private float[] FilterRows(Vector location, int distance)
        {
            var result = new float[distance * distance];
            try
            {
                var startX = (int)((location.X - distance / 2f) + GameWorld.Width / 2f);
                var startZ = (int)((location.Z - distance / 2f) + GameWorld.Width / 2f);
                float maxX = startX > GameWorld.Width - distance ? GameWorld.Width - distance : startX + distance;
                float maxZ = startZ > GameWorld.Width - distance ? GameWorld.Width - distance : startZ + distance;
                for (int j = startZ; j < maxZ; j++)
                {
                    for (int i = startX; i < maxX; i++)
                    {
                        result[(j - startZ) * distance + (i - startX)] = gameWorld.HeightMap.Values[j * gameWorld.Width + i];
                    }
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return result;
        }

        // GET api/world/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/world
        public void Post([FromBody]string value)
        {
        }

        // PUT api/world/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/world/5
        public void Delete(int id)
        {
        }
    }
}
