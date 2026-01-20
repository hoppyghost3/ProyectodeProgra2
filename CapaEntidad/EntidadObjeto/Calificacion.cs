using System;

namespace CapaEntidad.EntidadObjeto
{
    public class Calificacion
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public string NombreEstudiante { get; set; }
        public int CursoId { get; set; }
        public string NombreCurso { get; set; }
        public double Nota { get; set; }
        public string Periodo { get; set; } // "Parcial 1", "Parcial 2", "Final"
        public DateTime FechaRegistro { get; set; }
        public string Observaciones { get; set; }

        public Calificacion()
        {
            FechaRegistro = DateTime.Now;
        }

        public string EstadoAprobacion
        {
            get
            {
                if (Nota >= 70) return "Aprobado";
                if (Nota >= 60) return "Aprobado con observación";
                return "Reprobado";
            }
        }
    }
}