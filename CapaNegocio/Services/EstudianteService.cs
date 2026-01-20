using System;
using System.Collections.Generic;
using CapaDatos.Entidades;
using CapaEntidad.EntidadPersona;
using CapaUtilidades;

namespace CapaNegocio.Services
{
    public class EstudianteService
    {
        private EstudianteRepositorioJson repositorio;

        public EstudianteService()
        {
            repositorio = new EstudianteRepositorioJson();
        }

        public List<Estudiante> ListarEstudiantes()
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

        public void RegistrarEstudiante(Estudiante estudiante)
        {
            try
            {
                if (estudiante == null)
                    throw new ArgumentNullException(nameof(estudiante), "El estudiante no puede ser null");

                if (string.IsNullOrWhiteSpace(estudiante.Nombre))
                    throw new Exception("El nombre del estudiante no puede estar vacío");

                repositorio.Agregar(estudiante);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteService), nameof(RegistrarEstudiante));
                throw;
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