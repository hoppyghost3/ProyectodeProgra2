using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CapaEntidad;
using CapaUtilidades;

namespace CapaDatos
{
    public class InscripcionRepositorioJson : IRepositorioEntidad<Inscripcion>
    {
        private string rutaArchivo = "inscripciones.json";

        public void Agregar(Inscripcion inscripcion)
        {
            try
            {
                List<Inscripcion> lista = LeerArchivo();
                inscripcion.Id = lista.Count > 0 ? lista.Max(i => i.Id) + 1 : 1;
                lista.Add(inscripcion);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(InscripcionRepositorioJson), nameof(Agregar));
                throw;
            }
        }

        public void Modificar(Inscripcion inscripcion)
        {
            try
            {
                List<Inscripcion> lista = LeerArchivo();
                int index = lista.FindIndex(i => i.Id == inscripcion.Id);
                if (index >= 0)
                {
                    lista[index] = inscripcion;
                    GuardarArchivo(lista);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(InscripcionRepositorioJson), nameof(Modificar));
                throw;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                List<Inscripcion> lista = LeerArchivo();
                lista.RemoveAll(i => i.Id == id);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(InscripcionRepositorioJson), nameof(Eliminar));
                throw;
            }
        }

        public List<Inscripcion> Listar()
        {
            try
            {
                return LeerArchivo();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(InscripcionRepositorioJson), nameof(Listar));
                throw;
            }
        }

        public Inscripcion BuscarPorId(int id)
        {
            try
            {
                List<Inscripcion> lista = LeerArchivo();
                return lista.Find(i => i.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(InscripcionRepositorioJson), nameof(BuscarPorId));
                throw;
            }
        }

        // Métodos adicionales
        public List<Inscripcion> ObtenerPorEstudiante(int estudianteId)
        {
            try
            {
                return LeerArchivo().Where(i => i.EstudianteId == estudianteId).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(InscripcionRepositorioJson), nameof(ObtenerPorEstudiante));
                throw;
            }
        }

        public List<Inscripcion> ObtenerPorCurso(int cursoId)
        {
            try
            {
                return LeerArchivo().Where(i => i.CursoId == cursoId).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(InscripcionRepositorioJson), nameof(ObtenerPorCurso));
                throw;
            }
        }

        public bool ExisteInscripcion(int estudianteId, int cursoId)
        {
            try
            {
                return LeerArchivo().Any(i => i.EstudianteId == estudianteId &&
                                             i.CursoId == cursoId &&
                                             i.Estado == "Activo");
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(InscripcionRepositorioJson), nameof(ExisteInscripcion));
                throw;
            }
        }

        private List<Inscripcion> LeerArchivo()
        {
            if (!File.Exists(rutaArchivo))
                return new List<Inscripcion>();

            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Inscripcion>>(json) ?? new List<Inscripcion>();
        }

        private void GuardarArchivo(List<Inscripcion> lista)
        {
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(rutaArchivo, json);
        }
    }
}