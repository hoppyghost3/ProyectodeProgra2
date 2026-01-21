using System;
using System.Collections.Generic;

namespace CapaEntidad
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        public Usuario()
        {
            FechaRegistro = DateTime.Now;
            Activo = true;
        }
    }
    public static class Roles
    {
        public const string Administrador = "Administrador";
        public const string Docente = "Docente";
        public const string Estudiante = "Estudiante";

        public static List<string> ObtenerTodos()
        {
            return new List<string>
            {
                Administrador,
                Docente,
                Estudiante
            };
        }
    }
}