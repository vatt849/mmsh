using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

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
    /// <summary>
    /// Сводное описание для helper
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
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
        public string getfiles(string function, string action, string value)
        {
            string outJSON = null;
            if (action == "list")
            {
                outJSON = "{tracks:[";
                try
                {
                    string[] answer = Directory.GetFiles(paths.get(function));
                    foreach (string _s in answer)
                    {
                        if (_s.Split('.').Last() == "mp3")
                        {
                            TagLib.File tagFile = TagLib.File.Create(_s);
                            outJSON = outJSON + "{";
                            outJSON = outJSON + "'artist':'" + tagFile.Tag.FirstAlbumArtist + "',";
                            outJSON = outJSON + "'title':'" + tagFile.Tag.Title + "',";
                            outJSON = outJSON + "'path':'" + _s + "',";
                            outJSON = outJSON + "}";

                        }
                    }
                }
                catch (Exception e)
                {
                    outJSON = "{error: '" + e.ToString() + "'}";
                }
                //out to page
                //main.InnerText = output;
                //out to response
                //outJSON;
            }
            else if (action == "get")
            {

            }

            return outJSON;
        }
    }
}
