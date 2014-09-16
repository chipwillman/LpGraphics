namespace RiftGL.Objects
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    using JSIL;

    /// <summary>
    /// The vend request.
    /// </summary>
    public class RiftRequest
    {
        public RiftRequest(string url, string username, string password)
        {
            this.Url = url;
            this.Username = username;
            this.Password = password;
        }

        public string Url { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public void Get(string path, MapCallback callback)
        {
            var jquery = Builtins.Global["$"];
            jquery.getJSON(path, callback);
        }

        //public string PostRequest(string path, string data)
        //{
        //    try
        //    {
        //        using (var request = new WebClient())
        //        {
        //            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
        //                                                   Encoding.ASCII.GetBytes(this.Username + ":" + this.Password));
        //            request.Headers["Content-Type"] = "application/json";

        //            var response = request.UploadData(this.Url + path, "POST", Encoding.Default.GetBytes(data));
        //            return Encoding.Default.GetString(response);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }
        //}

        //public string Post(string path, string data)
        //{
        //    try
        //    {
        //        var request = WebRequest.Create(this.Url + path);
        //        request.Method = "POST";
        //        string authInfo = this.Username + ":" + this.Password;
        //        request.Credentials = new NetworkCredential(this.Username, this.Password);

        //        request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));
        //        request.ContentType = "application/json";
        //        var postString = data;
        //        request.ContentLength = postString.Length;

        //        StreamWriter writer = new StreamWriter(request.GetRequestStream());
        //        writer.Write(postString);
        //        writer.Close();

        //        string postResponse;

        //        var response = (HttpWebResponse)request.GetResponse();
        //        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //        {
        //            postResponse = reader.ReadToEnd();
        //            reader.Close();
        //        }

        //        return postResponse;
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }
        //}

        //public object PostWebRequest(string path)
        //{
        //    WebRequest request = WebRequest.Create(this.Url + path);

        //    request.ContentType = "Content-type: text/xml";
        //    request.Method = "POST";
        //    string authInfo = this.Username + ":" + this.Password;
        //    request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));

        //    byte[] buffer = Encoding.GetEncoding("UTF-8").GetBytes("<workspace><name>my_workspace</name></workspace>");
        //    Stream reqstr = request.GetRequestStream();
        //    reqstr.Write(buffer, 0, buffer.Length);
        //    reqstr.Close();

        //    WebResponse response = request.GetResponse();
        //    return response;
        //}
    }
}
