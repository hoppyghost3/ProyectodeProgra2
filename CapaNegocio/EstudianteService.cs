using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CapaDatos;
using CapaEntidad;
using CapaUtilidades;

namespace CapaNegocio  // ← CAMBIAR namespace
{
    public class EstudianteService
    {
        private IRepositorioEntidad<Estudiante> repositorio;  // ✅ CORREGIDO

        public EstudianteService()
        {
            // Inicializar con el repositorio JSON
            repositorio = new EstudianteRepositorioJson();
        }

        public EstudianteService(IRepositorioEntidad<Estudiante> repo)  // ✅ CORREGIDO
        {
            repositorio = repo;
        }

        public List<Estudiante> ListarEstudiantes()  // ✅ CORREGIDO
        {
            try
            {
                return repositorio.Listar();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(ListarEstudiantes));
                throw;
            }
        }

        // Método para registrar un estudiante con manejo de excepciones
        public void RegistrarEstudiante(Estudiante estudiante)  // ✅ CORREGIDO
        {
            try
            {
                // Validación: estudiante no puede ser null
                if (estudiante == null)
                    throw new ArgumentNullException(nameof(estudiante), "El estudiante no puede ser null");

                // Validación de formato: nombre obligatorio
                if (string.IsNullOrWhiteSpace(estudiante.Nombre))  // ✅ AHORA SÍ FUNCIONA
                    throw new FormatException("El nombre del estudiante no puede estar vacío");

                // Se intenta registrar en el repositorio
                repositorio.Agregar(estudiante);
            }
            catch (FormatException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(RegistrarEstudiante));
                throw new Exception("Error de formato al intentar registrar el estudiante.", ex);
            }
            catch (FileNotFoundException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(RegistrarEstudiante));
                throw new Exception("No se encontró el archivo de estudiantes.", ex);
            }
            catch (JsonException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(RegistrarEstudiante));
                throw new Exception("Error al procesar los datos JSON de estudiantes.", ex);
            }
            catch (IOException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(RegistrarEstudiante));
                throw new Exception("Error de lectura o escritura del archivo estudiantes.", ex);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(RegistrarEstudiante));
                throw new Exception("Ocurrió un error inesperado al registrar el estudiante.", ex);
            }
        }

        public void AgregarEstudiante(Estudiante estudiante)
        {
            try
            {
                if (estudiante == null)
                    throw new ArgumentNullException(nameof(estudiante));

                repositorio.Agregar(estudiante);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(AgregarEstudiante));
                throw;
            }
        }

        public Estudiante BuscarEstudiantePorId(int id)
        {
            try
            {
                return repositorio.BuscarPorId(id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(BuscarEstudiantePorId));
                throw;
            }
        }

        public void ModificarEstudiante(Estudiante estudiante)
        {
            try
            {
                if (estudiante == null)
                    throw new ArgumentNullException(nameof(estudiante));

                repositorio.Modificar(estudiante);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(ModificarEstudiante));
                throw;
            }
        }

        public void EliminarEstudiante(int id)
        {
            try
            {
                repositorio.Eliminar(id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(EliminarEstudiante));
                throw;
            }
        }
    }
}