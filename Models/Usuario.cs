using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Models
{
   public class Usuario
    {
        public string usuario { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string password { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public decimal peso { get; set; }
        public string avatar { get; set; }
        public int Cod_TipoPlan { get; set; }

    }
}
