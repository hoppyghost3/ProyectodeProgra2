using System;
using System.Collections.Generic;
using CapaDatos.Objeto;
using CapaEntidad.EntidadObjeto;
using CapaNegocio;

namespace CapaNegocio.Services
{
    public class CursoService
    {
        private CursoRepositorioJson repositorio;

        public CursoService()
        {
            repositorio = new CursoRepositorioJson();
        }

        public void AgregarCurso(Curso curso)
        {
            try
            {
                if (curso == null)
                    throw new ArgumentNullException(nameof(curso));

                if (string.IsNullOrWhiteSpace(curso.Nombre))
                    throw new Exception("El nombre del curso es obligatorio");

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

        public List<Curso> ObtenerCursosPorDocente(int docenteId)
        {
            try
            {
                var cursos = repositorio.Listar();
                return cursos.FindAll(c => c.ProfesorId == docenteId);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoService), nameof(ObtenerCursosPorDocente));
                throw;
            }
        }
    }
}