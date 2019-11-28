using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BussinesSVC.Domain
{
    [DataContract]
    public enum ResponseType
    {
        [EnumMember]
        Success = 1,

        [EnumMember]
        Warning = 2,

        [EnumMember]
        Error = 3,

        [EnumMember]
        NoData = 4
    }

    [DataContract(IsReference = true)]
    [Serializable]
    public class Response
    {
        [DataMember]
        public ResponseType State { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    [DataContract(IsReference = true)]
    [Serializable]
    public class ResponseOperation : Response
    {
    }

   

    [DataContract(IsReference = true)]
    [Serializable]
    public class ResponseQuery<T> : Response
    {
        [DataMember]
        public List<T> ListEntities { get; set; }
    }

  
    [DataContract(IsReference = true)]
    [Serializable]
    public class ResponseObject<T> : Response
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public T Object { get; set; }
    }

    [DataContract(IsReference = true)]
    [Serializable]
    public class ResponseObject : Response
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public Object Object { get; set; }
    }
}
