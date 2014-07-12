using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using mmsh.ServiceHelper;

namespace mmsh
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "streamer" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы streamer.svc или streamer.svc.cs в обозревателе решений и начните отладку.
    public class streamer : Istreamer
    {
        streamedMedia Istreamer.stream()
        {
            streamedMedia sm = new streamedMedia();
            sm.artist = "Kettu";
            sm.title = "Lorem Ipsum";
            sm.uname = "000000";
            sm.type = "text";
            sm.path = DM.Crypt(@"D:\lorem ipsum.txt");
            sm.content = Convert.ToBase64String(File.ReadAllBytes(@"D:\lorem ipsum.txt"), Base64FormattingOptions.InsertLineBreaks);

            return sm;
        }
    }

    [DataContract]
    public class streamedMedia
    {
        [DataMember]
        public string artist { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string uname { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string path { get; set; }
        [DataMember]
        public string mime { get; set; }
        [DataMember]
        public string content { get; set; }
    }
}
