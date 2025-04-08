using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaTime.Services
{
    public class SessionService
    {
        private static SessionService? _instance;

        // Patrón Singleton: Usa una propiedad estática para acceder a la instancia única
        public static SessionService Instance => _instance ??= new SessionService();

        // Variable compartida entre pantallas
        public string Usuario { get; set; } = string.Empty;

        // Constructor privado para evitar que se creen instancias fuera de esta clase
        private SessionService() { }
    }



}
