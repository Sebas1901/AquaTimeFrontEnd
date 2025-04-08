using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Models
{
    public class HidratacionError
    {
        [JsonProperty("ErrorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }

}
