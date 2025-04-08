using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Models
{
    public class RegistroHidratacionRequest
    {
        public required string Usuario {  get; set; }
        public required double  CantidadML { get; set; }

    }
}
