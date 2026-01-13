using CapaDatos;

namespace ProyectProgra2
{
    public class CursoService
    {
        private readonly IRepositorioEntidad<Curso> _repo;

        public CursoService(IRepositorioEntidad<Curso> repo)
        {
            _repo = repo;
        }

        protected void ValidarEntidad(Curso curso)
        {
            if (string.IsNullOrWhiteSpace(curso.Nombre))
                throw new ArgumentException("El nombre del curso no puede estar vacío", nameof(Curso.Nombre));

            if (curso.Id <= 0)
                throw new ArgumentException("El ID del curso debe ser positivo", nameof(Curso.Id));

            if (curso.DuracionHoras <= 0)
                throw new ArgumentException("La duración debe ser mayor a 0", nameof(Curso.DuracionHoras));
        }
    }
}