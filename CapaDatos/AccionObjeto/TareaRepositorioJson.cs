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
    public class TareaRepositorioJson : IRepositorioEntidad<Tarea>
    {
        private string rutaArchivo = "tareas.json";

        public void Agregar(Tarea tarea)
        {
            try
            {
                List<Tarea> lista = LeerArchivo();
                tarea.Id = lista.Count > 0 ? lista.Max(t => t.Id) + 1 : 1;
                lista.Add(tarea);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(TareaRepositorioJson), nameof(Agregar));
                throw;
            }
        }

        public void Modificar(Tarea tarea)
        {
            try
            {
                List<Tarea> lista = LeerArchivo();
                int index = lista.FindIndex(t => t.Id == tarea.Id);
                if (index >= 0)
                {
                    lista[index] = tarea;
                    GuardarArchivo(lista);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(TareaRepositorioJson), nameof(Modificar));
                throw;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                List<Tarea> lista = LeerArchivo();
                lista.RemoveAll(t => t.Id == id);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(TareaRepositorioJson), nameof(Eliminar));
                throw;
            }
        }

        public List<Tarea> Listar()
        {
            try
            {
                return LeerArchivo();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(TareaRepositorioJson), nameof(Listar));
                throw;
            }
        }

        public Tarea BuscarPorId(int id)
        {
            try
            {
                List<Tarea> lista = LeerArchivo();
                return lista.Find(t => t.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(TareaRepositorioJson), nameof(BuscarPorId));
                throw;
            }
        }

        public List<Tarea> ObtenerPorCurso(int cursoId)
        {
            try
            {
                return LeerArchivo().Where(t => t.CursoId == cursoId).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(TareaRepositorioJson), nameof(ObtenerPorCurso));
                throw;
            }
        }

        private List<Tarea> LeerArchivo()
        {
            if (!File.Exists(rutaArchivo))
                return new List<Tarea>();

            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Tarea>>(json) ?? new List<Tarea>();
        }

        private void GuardarArchivo(List<Tarea> lista)
        {
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(rutaArchivo, json);
        }
    }

    public class EntregaTareaRepositorioJson
    {
        private string rutaArchivo = "entregas_tareas.json";

        public void Agregar(EntregaTarea entrega)
        {
            try
            {
                List<EntregaTarea> lista = LeerArchivo();
                entrega.Id = lista.Count > 0 ? lista.Max(e => e.Id) + 1 : 1;
                lista.Add(entrega);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EntregaTareaRepositorioJson), nameof(Agregar));
                throw;
            }
        }

        public List<EntregaTarea> ObtenerPorTarea(int tareaId)
        {
            try
            {
                return LeerArchivo().Where(e => e.TareaId == tareaId).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EntregaTareaRepositorioJson), nameof(ObtenerPorTarea));
                throw;
            }
        }

        public List<EntregaTarea> ObtenerPorEstudiante(int estudianteId)
        {
            try
            {
                return LeerArchivo().Where(e => e.EstudianteId == estudianteId).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EntregaTareaRepositorioJson), nameof(ObtenerPorEstudiante));
                throw;
            }
        }

        public void Calificar(int entregaId, double calificacion, string retroalimentacion)
        {
            try
            {
                List<EntregaTarea> lista = LeerArchivo();
                var entrega = lista.Find(e => e.Id == entregaId);
                if (entrega != null)
                {
                    entrega.Calificacion = calificacion;
                    entrega.Retroalimentacion = retroalimentacion;
                    GuardarArchivo(lista);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EntregaTareaRepositorioJson), nameof(Calificar));
                throw;
            }
        }

        private List<EntregaTarea> LeerArchivo()
        {
            if (!File.Exists(rutaArchivo))
                return new List<EntregaTarea>();

            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<EntregaTarea>>(json) ?? new List<EntregaTarea>();
        }

        private void GuardarArchivo(List<EntregaTarea> lista)
        {
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(rutaArchivo, json);
        }
    }
}