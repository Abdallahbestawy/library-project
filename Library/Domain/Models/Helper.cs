using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Helper
    {
        public const string pathImageUser = "/Images/Users/";
        public const string PathSaveImageUser = "Images/Users";
        public const string Success = "success";
        public const string Error = "error";
        public const string MsgType = "msgtype";
        public const string Message = "message";
        public const string Title= "title";
        public const string Save = "save";
        public const string Update = "update";
        public const string Delete = "delete";  
        public enum eCurrentState
        {
            Active=1,
            Delete=0
        }
    }
}
