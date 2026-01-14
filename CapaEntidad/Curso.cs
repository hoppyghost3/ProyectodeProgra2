using System;
using System.Collections.Generic;

namespace CapaEntidad
{
    public class Curso
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int ProfesorId { get; set; }
        public string NombreProfesor { get; set; }
        public string Horario { get; set; }
        public int Creditos { get; set; }
        public int CupoMaximo { get; set; }
        public int CupoDisponible { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }
        public List<int> EstudiantesInscritos { get; set; }

        public Curso()
        {
            Activo = true;
            EstudiantesInscritos = new List<int>();
            CupoDisponible = CupoMaximo;
            FechaInicio = DateTime.Now;
        }
    }
}