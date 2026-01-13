using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CapaEntidad;
using CapaUtilidades;

namespace CapaDatos
{
    public class EstudianteRepositorioJson : IRepositorioEntidad<Estudiante>
    {
        private string rutaArchivo = "estudiantes.json";

        public void Agregar(Estudiante estudiante)
        {
            try
            {
                List<Estudiante> lista = LeerArchivo();
                lista.Add(estudiante);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteRepositorioJson), nameof(Agregar));
                throw;
            }
        }

        public void Modificar(Estudiante estudiante)
        {
            try
            {
                List<Estudiante> lista = LeerArchivo();
                int index = lista.FindIndex(e => e.Id == estudiante.Id);
                if (index >= 0)
                {
                    lista[index] = estudiante;
                    GuardarArchivo(lista);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteRepositorioJson), nameof(Modificar));
                throw;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                List<Estudiante> lista = LeerArchivo();
                lista.RemoveAll(e => e.Id == id);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteRepositorioJson), nameof(Eliminar));
                throw;
            }
        }

        public List<Estudiante> Listar()
        {
            try
            {
                return LeerArchivo();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteRepositorioJson), nameof(Listar));
                throw;
            }
        }

        public Estudiante BuscarPorId(int id)
        {
            try
            {
                List<Estudiante> lista = LeerArchivo();
                return lista.Find(e => e.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(EstudianteRepositorioJson), nameof(BuscarPorId));
                throw;
            }
        }

        // ---------------- METODOS PRIVADOS ----------------

        private List<Estudiante> LeerArchivo()
        {
            if (!File.Exists(rutaArchivo))
                return new List<Estudiante>();

            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Estudiante>>(json) ?? new List<Estudiante>();
        }

        private void GuardarArchivo(List<Estudiante> lista)
        {
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(rutaArchivo, json);
        }

        List<Estudiante> IRepositorioEntidad<Estudiante>.Listar()
        {
            throw new System.NotImplementedException();
        }
    }
}


