using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Automation.Models.Request
{
    public class PutBookRequest
    {
        public string userId { get; set; }
        public string isbn { get; set; }
    }
}
