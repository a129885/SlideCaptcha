using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlideCaptcha.Models.Vaidate
{
    public class ValidateResponseModel : ResponseBase
    {
        public string Token { get; set; }
    }
}
