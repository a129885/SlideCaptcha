using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlideCaptcha.Models
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            isOk = false;
        }
        public bool isOk { get; set; }
        public string msg { get; set; }
        public int code { get; set; }
        public object data { get; set; }
    }
}
