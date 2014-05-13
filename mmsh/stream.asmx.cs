using System;
using System.Collections;
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
        public static string audio = @"C:\Users\Kettu\Music";
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
    public class media
    {
        public string artist;
        public string title;
        public string path;
        public string mime;

        public media(string _artist, string _title, string _path)
        {
            artist = _artist;
            title = _title;
            path = _path;
            mime = MimeMapping.GetMimeMapping(path);
        }
        public media()
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
        public string name { get; set; }
        public string type { get; set; }
    }
    /// <summary>
    /// Сводное описание для helper
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [ScriptService]
    public class stream : System.Web.Services.WebService
    {

        [WebMethod]
        public string helloworld()
        {
            return "Привет всем!";
        }

        [WebMethod]
        public void getfiles(string path)
        {
            streamFile(path);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public List<media> getlist(Parameters par)
        {
            List<media> list = new List<media>();

            string search = "";
            switch (par.type)
            {
                case "audio": search = "*.mp3|*.wav|*.wma"; break;
                case "video": search = "*.mp4|*.avi|*.wmv"; break;
                case "pics": search = "*.jpg|*.jpeg|*.png|*.gif"; break;
            }

            string[] answer = GetFiles(paths.get(par.type), search, SearchOption.AllDirectories);
            int i = answer.Count();
            foreach (string _s in answer)
            {
                TagLib.File tagFile = TagLib.File.Create(_s);
                media _a = new media(tagFile.Tag.FirstAlbumArtist, tagFile.Tag.Title, _s);
                list.Add(_a);
            }

            return list;
        }

        public void streamFile(string filePath)//, string userFilename)
        {
            // Process the file in 4K blocks
            byte[] dataBlock = new byte[0x1000];
            long fileSize;
            int bytesRead;
            long totalBytesRead = 0;

            string mime = MimeMapping.GetMimeMapping(filePath);

            using (var fs = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileSize = fs.Length;

                Context.Response.Clear();
                Context.Response.ContentType = @mime;
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
        /// <summary>
        /// Returns file names from given folder that comply to given filters
        /// </summary>
        /// <param name="SourceFolder">Folder with files to retrieve</param>
        /// <param name="Filter">Multiple file filters separated by | character</param>
        /// <param name="searchOption">File.IO.SearchOption, 
        /// could be AllDirectories or TopDirectoryOnly</param>
        /// <returns>Array of FileInfo objects that presents collection of file names that 
        /// meet given filter</returns>
        public string[] GetFiles(string SourceFolder, string Filter,
         System.IO.SearchOption searchOption)
        {
            // ArrayList will hold all file names
            ArrayList alFiles = new ArrayList();

            // Create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters)
            {
                // add found file names to array list
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }

            // returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        }
    }
}
