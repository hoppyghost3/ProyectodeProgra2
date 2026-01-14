using System;
using System.Collections.Generic;

namespace CapaEntidad
{
    public class Docente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Especialidad { get; set; }
        public string Departamento { get; set; }
        public DateTime FechaContratacion { get; set; }
        public bool Activo { get; set; }
        public List<int> CursosAsignados { get; set; }

        public Docente()
        {
            FechaContratacion = DateTime.Now;
            Activo = true;
            CursosAsignados = new List<int>();
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}