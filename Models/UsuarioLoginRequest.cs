using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Models
{
    public class UsuarioLoginRequest
    {
        public LoginData Login { get; set; }

        public UsuarioLoginRequest(string username, string password)
        {
            Login = new LoginData { Usuario = username, Password = password };
        }
    }

    public class LoginData
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}