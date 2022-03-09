using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncountifyAPI.Models
{
    public class Token
    {
        public string CurrentToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
