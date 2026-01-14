using System;

namespace CapaEntidad
{
    public class Inscripcion
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public string NombreEstudiante { get; set; }
        public int CursoId { get; set; }
        public string NombreCurso { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Estado { get; set; } // "Activo", "Retirado", "Completado"
        public double? NotaFinal { get; set; }

        public Inscripcion()
        {
            FechaInscripcion = DateTime.Now;
            Estado = "Activo";
        }
    }
}