using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Models
{
    public class PlanHidratacionResponse
    {
        [JsonProperty("Planes")]
        public List<PlanHidratacion> Planes { get; set; }

        [JsonProperty("resultado")]
        public bool Resultado { get; set; }

        [JsonProperty("error")]
        public List<HidratacionError> Error { get; set; } // Usa la clase ya existente
    }
}
