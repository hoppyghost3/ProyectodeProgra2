using System.Text.Json;
using CapaDatos;

namespace ProyectProgra2
{
    public class EstudianteService
    {
        private IRepositorioEntidad<Estudiante> repositorio;

        public EstudianteService(IRepositorioEntidad<Estudiante> repo)
        {
            repositorio = repo;
        }
        public List<Estudiante> ListarEstudiantes()
        {
            return repositorio.Listar();
        }

        // Metodo para registrar un estudiante con manejo de excepciones
        public void RegistrarEstudiante(Estudiante estudiante)
        {
            try
            {
                // Validacion: estudiante no puede ser null
                if (estudiante == null)
                    throw new ArgumentNullException("El estudiante no puede ser null");

                // Validacion de formato: nombre obligatorio
                if (string.IsNullOrWhiteSpace(estudiante.Nombre))
                    throw new FormatException("El nombre del estudiante no puede estar vacio");

                // Se intenta registrar en el repositorio
                repositorio.Agregar(estudiante);
            }
            // Error de datos mal formados
            catch (FormatException ex)
            {
                throw new Exception("Error de formato al intentar registrar el estudiante.");
            }
            // Archivo faltante
            catch (FileNotFoundException ex)
            {
                throw new Exception("No se encontro el archivo de estudiantes.");
            }
            // Error en procesamiento de JSON
            catch (JsonException ex)
            {
                throw new Exception("Error al procesar los datos JSON de estudiantes.");
            }
            // Error del sistema de archivos
            catch (IOException ex)
            {
                throw new Exception("Error de lectura o escritura del archivo estudiantes.");
            }
            // Error desconocido
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error inesperado al registrar el estudiante.");
            }
            finally
            {
                // Se ejecuta en todos los casos
                Console.WriteLine("Operacion de registro de estudiante finalizada.");
            }
        }
    }
}