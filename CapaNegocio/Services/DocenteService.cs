using System;
using System.Collections.Generic;
using CapaDatos.Entidades;
using CapaEntidad.EntidadPersona;
using CapaUtilidades;

namespace CapaNegocio.Services
{
    public class DocenteService
    {
        private DocenteRepositorioJson repositorio;

        public DocenteService()
        {
            repositorio = new DocenteRepositorioJson();
        }

        public void RegistrarDocente(Docente docente)
        {
            try
            {
                if (docente == null)
                    throw new ArgumentNullException(nameof(docente));

                if (string.IsNullOrWhiteSpace(docente.Nombre))
                    throw new Exception("El nombre es obligatorio");

                repositorio.Agregar(docente);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteService), nameof(RegistrarDocente));
                throw;
            }
        }

        public List<Docente> ObtenerTodos()
        {
            try
            {
                return repositorio.Listar();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteService), nameof(ObtenerTodos));
                throw;
            }
        }

        public Docente BuscarPorId(int id)
        {
            try
            {
                return repositorio.BuscarPorId(id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteService), nameof(BuscarPorId));
                throw;
            }
        }

        public void ActualizarDocente(Docente docente)
        {
            try
            {
                repositorio.Modificar(docente);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteService), nameof(ActualizarDocente));
                throw;
            }
        }

        public void EliminarDocente(int id)
        {
            try
            {
                repositorio.Eliminar(id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteService), nameof(EliminarDocente));
                throw;
            }
        }
    }
}