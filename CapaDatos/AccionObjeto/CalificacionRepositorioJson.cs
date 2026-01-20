using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CapaDatos.Entidades;
using CapaEntidad.EntidadObjeto;
using CapaUtilidades;

namespace CapaDatos.Objeto
{
    public class CalificacionRepositorioJson : IRepositorioEntidad<Calificacion>
    {
        private string rutaArchivo = "calificaciones.json";

        public void Agregar(Calificacion calificacion)
        {
            try
            {
                List<Calificacion> lista = LeerArchivo();
                calificacion.Id = lista.Count > 0 ? lista.Max(c => c.Id) + 1 : 1;
                lista.Add(calificacion);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CalificacionRepositorioJson), nameof(Agregar));
                throw;
            }
        }

        public void Modificar(Calificacion calificacion)
        {
            try
            {
                List<Calificacion> lista = LeerArchivo();
                int index = lista.FindIndex(c => c.Id == calificacion.Id);
                if (index >= 0)
                {
                    lista[index] = calificacion;
                    GuardarArchivo(lista);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CalificacionRepositorioJson), nameof(Modificar));
                throw;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                List<Calificacion> lista = LeerArchivo();
                lista.RemoveAll(c => c.Id == id);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CalificacionRepositorioJson), nameof(Eliminar));
                throw;
            }
        }

        public List<Calificacion> Listar()
        {
            try
            {
                return LeerArchivo();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CalificacionRepositorioJson), nameof(Listar));
                throw;
            }
        }

        public Calificacion BuscarPorId(int id)
        {
            try
            {
                List<Calificacion> lista = LeerArchivo();
                return lista.Find(c => c.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CalificacionRepositorioJson), nameof(BuscarPorId));
                throw;
            }
        }

        // Métodos adicionales específicos
        public List<Calificacion> ObtenerPorEstudiante(int estudianteId)
        {
            try
            {
                return LeerArchivo().Where(c => c.EstudianteId == estudianteId).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CalificacionRepositorioJson), nameof(ObtenerPorEstudiante));
                throw;
            }
        }

        public List<Calificacion> ObtenerPorCurso(int cursoId)
        {
            try
            {
                return LeerArchivo().Where(c => c.CursoId == cursoId).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CalificacionRepositorioJson), nameof(ObtenerPorCurso));
                throw;
            }
        }

        private List<Calificacion> LeerArchivo()
        {
            if (!File.Exists(rutaArchivo))
                return new List<Calificacion>();

            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Calificacion>>(json) ?? new List<Calificacion>();
        }

        private void GuardarArchivo(List<Calificacion> lista)
        {
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(rutaArchivo, json);
        }
    }
}