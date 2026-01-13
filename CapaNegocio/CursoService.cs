using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CapaDatos;
using CapaEntidad;
using CapaUtilidades;

namespace CapaNegocio  // ← CAMBIAR namespace
{
    public class CursoService
    {
        private IRepositorioEntidad<Curso> repositorio;

        public CursoService()
        {
            // Inicializar con el repositorio JSON
            // Asume que tienes CursoRepositorioJson en CapaDatos
            repositorio = new CursoRepositorioJson();
        }

        public CursoService(IRepositorioEntidad<Curso> repo)
        {
            repositorio = repo;
        }

        // Método para registrar un curso con manejo de excepciones
        public void RegistrarCurso(Curso curso)
        {
            try
            {
                // Validación: Evitar objetos nulos
                if (curso == null)
                    throw new ArgumentNullException(nameof(curso), "El curso no puede ser null");

                // Validación de formato: ID debe ser positivo
                if (curso.Id <= 0)
                    throw new FormatException("El ID del curso debe ser un número positivo");

                // Si pasa validaciones, intentamos agregarlo al repositorio
                repositorio.Agregar(curso);
            }
            catch (FormatException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("Error de formato al intentar registrar el curso.", ex);
            }
            catch (FileNotFoundException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("No se encontró el archivo de cursos.", ex);
            }
            catch (JsonException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("Error al procesar los datos JSON de cursos.", ex);
            }
            catch (IOException ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("Error de lectura o escritura del archivo cursos.", ex);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(RegistrarCurso));
                throw new Exception("Ocurrió un error inesperado al registrar el curso.", ex);
            }
        }

        // ✅ CORREGIDO: Debe recibir Curso, no CursoService
        public void AgregarCurso(Curso curso)
        {
            try
            {
                if (curso == null)
                    throw new ArgumentNullException(nameof(curso));

                repositorio.Agregar(curso);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(AgregarCurso));
                throw;
            }
        }

        public List<Curso> ObtenerCursos()
        {
            try
            {
                return repositorio.Listar();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(ObtenerCursos));
                throw;
            }
        }

        public Curso BuscarCursoPorId(int id)
        {
            try
            {
                return repositorio.BuscarPorId(id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(BuscarCursoPorId));
                throw;
            }
        }

        public void ModificarCurso(Curso curso)
        {
            try
            {
                if (curso == null)
                    throw new ArgumentNullException(nameof(curso));

                repositorio.Modificar(curso);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(ModificarCurso));
                throw;
            }
        }

        public void EliminarCurso(int id)
        {
            try
            {
                repositorio.Eliminar(id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(EliminarCurso));
                throw;
            }
        }
    }
}