namespace RiftGL.Objects
{
    using System.Text;

    public delegate void MapCallback(Map data);

    public class RiftRepository
    {

        public RiftRepository(string url, string username, string password)
        {
            this.Url = url;
            this.Username = username;
            this.Password = password;
        }

        public string Url { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public void LoadMap(Vector location, float distance, MapCallback callback)
        {
            var sb = new StringBuilder();
            sb.Append("locationX=" + location.X + "&");
            sb.Append("locationY=" + location.Y + "&");
            sb.Append("locationZ=" + location.Z + "&");
            sb.Append("distance=" + distance);

            var paramString = sb.ToString();
            new RiftRequest(Url, Username, Password).Get("/api/world?" + paramString, callback);
        }
    }
}
