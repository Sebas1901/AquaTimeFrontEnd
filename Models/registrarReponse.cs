﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Models
{
    public class registrarReponse
    {
        public bool Resultado { get; set; }
        public required List<ErrorDetail> Error { get; set; }
    }
}
