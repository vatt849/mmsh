using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace mmsh
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "Istreamer" в коде и файле конфигурации.
    [ServiceContract]
    public interface Istreamer
    {
        [OperationContract]
        streamedMedia stream();
    }
}
