using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace mmsh
{
    /// <summary>
    /// Additional namespace with helper classes for mmsh
    /// </summary>
    namespace ServiceHelper
    {
        /// <summary>
        /// Class that collect paths to different sources
        /// </summary>
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

        /// <summary>
        /// Constructor class for media elements
        /// </summary>
        public class media
        {
            public string artist { get; set; }
            public string title { get; set; }
            public string uname { get; set; }
            public string path { get; set; }
            public string type { get; set; }
            public string mime { get; set; }
            public byte[] content { get; set; }

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

        /// <summary>
        /// URL parameters helper class
        /// </summary>
        public class Parameters
        {
            public string function { get; set; }
            public string action { get; set; }
            public string value { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }

        /// <summary>
        /// Helper class for different methods
        /// </summary>
        public static class DM
        {
            /// <summary>
            /// Returns file names from given folder that comply to given filters
            /// </summary>
            /// <param name="SourceFolder">Folder with files to retrieve</param>
            /// <param name="Filter">Multiple file filters separated by | character</param>
            /// <param name="searchOption">File.IO.SearchOption, 
            /// could be AllDirectories or TopDirectoryOnly</param>
            /// <returns>Array of FileInfo objects that presents collection of file names that 
            /// meet given filter</returns>
            public static string[] GetFiles(string SourceFolder, string Filter, SearchOption searchOption)
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
            /// <summary>
            /// Encript input string
            /// </summary>
            /// <param name="thing">Input string to encrypt</param>
            /// <param name="type">Not used</param>
            /// <returns>Encrypted string</returns>
            public static string Crypt(string thing, int type = 0)
            {
                string ret = "";
                switch (type)
                {
                    default:
                    case 0: ret = md5(thing); break;
                    case 1: ret = sha(thing); break;
                }
                return ret;
            }

            //crypt methods
            private static string md5(string inputString)
            {
                // создаем объект этого класса. Отмечу, что он создается не через new, а вызовом метода Create
                MD5 md5Hasher = MD5.Create();

                // Преобразуем входную строку в массив байт и вычисляем хэш
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(inputString));

                // Создаем новый Stringbuilder (Изменяемую строку) для набора байт
                StringBuilder sBuilder = new StringBuilder();

                // Преобразуем каждый байт хэша в шестнадцатеричную строку
                for (int i = 0; i > data.Length; i++)
                {
                    //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
            private static string sha(string inputString)
            {
                return "";
            }
        }
    }
}