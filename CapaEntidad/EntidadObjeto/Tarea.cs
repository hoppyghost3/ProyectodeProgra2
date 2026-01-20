using System;

namespace CapaEntidad.EntidadObjeto
{
    public class Tarea
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public string NombreCurso { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int PuntajeMaximo { get; set; }
        public bool Activa { get; set; }

        public Tarea()
        {
            FechaAsignacion = DateTime.Now;
            Activa = true;
        }

        public bool EstaVencida => DateTime.Now > FechaEntrega;

        public string Estado => EstaVencida ? "Vencida" : "Activa";
    }

    public class EntregaTarea
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public int EstudianteId { get; set; }
        public string NombreEstudiante { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public double? Calificacion { get; set; }
        public string Retroalimentacion { get; set; }
        public bool EntregadaTarde { get; set; }

        public EntregaTarea()
        {
            FechaEntrega = DateTime.Now;
        }
    }
}