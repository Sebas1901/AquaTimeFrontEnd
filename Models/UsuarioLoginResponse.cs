using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Models
{
    public class UsuarioLoginResponse
    {
        public bool Resultado { get; set; }
        public required List<ErrorDetail> Error { get; set; }
    }

    public class ErrorDetail
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
