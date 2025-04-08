using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Models
{
    public class PlanHidratacion
    {
        [JsonProperty("Cod_TipoPlan")]
        public int Cod_TipoPlan { get; set; }

        [JsonProperty("NombrePlan")]
        public string NombrePlan { get; set; }

        [JsonProperty("ML_Diarios")]
        public int ML_Diarios { get; set; }

        [JsonProperty("Icono")]
        public string Icono { get; set; }
    }
}