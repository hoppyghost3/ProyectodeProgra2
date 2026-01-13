using CapaErrores;

namespace CapaDatos
{
    public abstract class BaseService<T> where T : class
    {
        protected IRepositorioEntidad<T> repositorio;

        protected BaseService(IRepositorioEntidad<T> repo)
        {
            repositorio = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public List<T> Listar()
        {
            return repositorio.Listar();
        }

        public void Registrar(T entidad)
        {
            if (entidad == null)
                throw new ArgumentNullException(nameof(entidad), $"La entidad {typeof(T).Name} no puede ser nula");

            ValidarEntidad(entidad);

            try
            {
                repositorio.Agregar(entidad);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, GetType().Name, nameof(Registrar));
                throw new InvalidOperationException($"Error al registrar {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Método abstracto para que cada servicio implemente sus validaciones específicas
        /// </summary>
        protected abstract void ValidarEntidad(T entidad);
    }
}
