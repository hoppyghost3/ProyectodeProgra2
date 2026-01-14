using System;
using System.Collections.Generic;

namespace CapaEntidad
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Carrera { get; set; }
        public int Semestre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
        public List<int> CursosInscritos { get; set; }

        public Estudiante()
        {
            FechaRegistro = DateTime.Now;
            Activo = true;
            CursosInscritos = new List<int>();
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}