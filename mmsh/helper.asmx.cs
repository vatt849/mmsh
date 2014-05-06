using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace mmsh
{
    public static class paths
    {
        public static string audio = @"C:\\Users\\Kettu\\Music";
        public static string video = @"C:\Users\Kettu\Videos";
        public static string pics = @"C:\Users\Kettu\Pictures";
        public static string get(string type)
        {
            string answer = "";
            switch (type)
            {
                case "audio": answer = paths.audio; break;
                case "video": answer = paths.audio; break;
                case "pics": answer = paths.pics; break;

            }
            return answer;
        }
    }
    public class audio
    {
        public string artist;
        public string title;
        public string path;
        public string mime;

        public audio(string _artist, string _title, string _path)
        {
            artist = _artist;
            title = _title;
            path = _path;
            mime = MimeMapping.GetMimeMapping(path);
        }
        public audio()
        {
            artist = "VA";
            title = "track";
            path = "";
            mime = MimeMapping.GetMimeMapping(path);
        }
    }
    public class Parameters
    {
        public string function { get; set; }
        public string action { get; set; }
        public string value { get; set; }
    }
    /// <summary>
    /// Сводное описание для helper
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [ScriptService]
    public class helper : System.Web.Services.WebService
    {

        [WebMethod]
        public string helloworld()
        {
            return "Привет всем!";
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getfiles(Parameters par)
        {
            string outJSON = null;
            if (par.action == "list")
            {
                outJSON = "{tracks:[";
                try
                {
                    string[] answer = Directory.GetFiles(paths.get(par.function));
                    int i = answer.Count();
                    foreach (string _s in answer)
                    {
                        if (_s.Split('.').Last() == "mp3")
                        {
                            TagLib.File tagFile = TagLib.File.Create(_s);
                            audio _a = new audio(tagFile.Tag.FirstAlbumArtist, tagFile.Tag.Title, _s);
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            /*outJSON = outJSON + "{";
                            outJSON = outJSON + "artist:'" + tagFile.Tag.FirstAlbumArtist + "',";
                            outJSON = outJSON + "title:'" + tagFile.Tag.Title + "',";
                            outJSON = outJSON + "path:'" + _s + "',";
                            outJSON = outJSON + "mime:'" + MimeMapping.GetMimeMapping(_s) +"'";
                            outJSON = outJSON + "},";*/
                            outJSON = outJSON + js.Serialize(_a);
                        }
                    }
                    outJSON = outJSON.Remove(outJSON.Length - 1);
                }
                catch
                {
                    outJSON = "false";
                }
                outJSON = outJSON + "]}";

                //JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(outJSON);
            }
            else if (par.action == "get")
            {
                streamFile("", "");
            }

            //return outJSON;
        }

        public void streamFile(string secureFilePath, string userFilename/*, string contentType = @"application/octet-stream"*/)
        {
            // Process the file in 4K blocks
            byte[] dataBlock = new byte[0x1000];
            long fileSize;
            int bytesRead;
            long totalBytesRead = 0;

            string mime = MimeMapping.GetMimeMapping(Directory.GetFiles(paths.audio)[0]);

            using (var fs = new FileStream(Directory.GetFiles(paths.audio)[0]/*Server.MapPath(secureFilePath)*/,
                FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileSize = fs.Length;

                Context.Response.Clear();
                Context.Response.ContentType = @mime;//contentType;
                //Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + userFilename);

                while (totalBytesRead < fileSize)
                {
                    if (!Context.Response.IsClientConnected)
                        break;

                    bytesRead = fs.Read(dataBlock, 0, dataBlock.Length);
                    Context.Response.OutputStream.Write(dataBlock, 0, bytesRead);
                    Context.Response.Flush();
                    totalBytesRead += bytesRead;
                }

                Context.Response.Close();
            }
        }
    }
}
