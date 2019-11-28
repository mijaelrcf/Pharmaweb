using BussinesSVC.Data;
using BussinesSVC.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesSVC.Manger
{
    public class LoginManager
    {
        BD_CerberusSVFEntities contexto = new BD_CerberusSVFEntities();
        public ResponseObject Login(string user, string password)
        {
            ResponseObject resul = new ResponseObject();
            try
            {

                List<spLogin_Result> resulQuery = contexto.spLogin(user, password).ToList();

                resul.State = Convert.ToBoolean(resulQuery[0].respuesta_bo) ? ResponseType.Success : ResponseType.Warning;
                resul.Message = Convert.ToString(resulQuery[0].Log_respuesta_vc);
                resul.Object = resulQuery[0];
            }
            catch (Exception ex)
            {

                resul.State = ResponseType.Error;
                resul.Message = ex.Message;
            }
            return resul;
        }
    }
}
