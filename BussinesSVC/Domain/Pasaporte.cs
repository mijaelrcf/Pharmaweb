using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BussinesSVC.Domain
{
    [DataContract]
    public class Pasaporte
    {
        public Pasaporte()
        {
            Aprobado = false;
        }
        [DataMember]
        public string usuario_vc { get; set; }
        [DataMember]
        public long id_empresa { get; set; }
        [DataMember]
        public bool Aprobado { get; set; }
        [DataMember]
        public string Token { get; set; }
    }
}
