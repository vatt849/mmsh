using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web;
using mmsh.ServiceHelper;

namespace mmsh
{
    [ServiceContract(Namespace = "")]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class helper
    {
        // Чтобы использовать протокол HTTP GET, добавьте атрибут [WebGet]. (По умолчанию ResponseFormat имеет значение WebMessageFormat.Json.)
        // Чтобы создать операцию, возвращающую XML,
        //     добавьте [WebGet(ResponseFormat=WebMessageFormat.Xml)]
        //     и включите следующую строку в текст операции:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public void DoWork()
        {
            // Добавьте здесь реализацию операции
            return;
        }
        [OperationContract]
        [WebGet]
        public string hello()
        {
            return "hello";
        }
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
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

            string[] answer = DM.GetFiles(paths.get(par.type), search, SearchOption.AllDirectories);
            int i = answer.Count();
            foreach (string _s in answer)
            {
                TagLib.File tagFile = TagLib.File.Create(_s);
                media _a = new media(tagFile.Tag.FirstAlbumArtist, tagFile.Tag.Title, _s);
                list.Add(_a);
                //break;
            }

            return list;
        }
        [OperationContract]
        [WebGet]
        public Stream stream(string uname)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = MimeMapping.GetMimeMapping(uname);
            FileStream streamedFile = new FileStream(@uname, FileMode.Open);
            return streamedFile;
        }
    }
}
