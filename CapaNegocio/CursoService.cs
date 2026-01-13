using System.Text.Json;
using CapaDatos;


namespace ProyectProgra2.Negocio
{
    public class CursoService
    {
        private IRepositorioEntidad<Curso> repositorio;

        public CursoService(IRepositorioEntidad<Curso> repo)
        {
            repositorio = repo;
        }

        // Metodo para registrar un curso con manejo de excepciones
        public void RegistrarCurso(Curso curso)
        {
            try
            {
                // Validacion: Evitar objetos nulos
                if (curso == null)
                    throw new ArgumentNullException("El curso no puede ser null");

                // Validacion de formato: ID debe ser positivo
                if (curso.Id <= 0)
                    throw new FormatException("El ID del curso debe ser un numero positivo");

                // Si pasa validaciones, intentamos agregarlo al repositorio
                repositorio.Agregar(curso);
            }
            // Error de formato (ideal para valores invalidos)
            catch (FormatException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("Error de formato al intentar registrar el curso.");
            }
            // Archivo no encontrado
            catch (FileNotFoundException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("No se encontro el archivo de cursos.");
            }
            // Error al parsear JSON
            catch (JsonException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("Error al procesar los datos JSON de cursos.");
            }
            // Problemas con el sistema de archivos
            catch (IOException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("Error de lectura o escritura del archivo cursos.");
            }
            // Cualquier error inesperado
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("Ocurrio un error inesperado al registrar el curso.");
            }
            finally
            {
                // Siempre se ejecuta
                Console.WriteLine("Operacion de registro de curso finalizada.");
            }
        }

        public void AgregarCurso(CursoService c)
        {
            repositorio.Agregar(c);
        }

        public List<CursoService> ListarCursos()
        {
            return repositorio.Listar();
        }
    }
}



