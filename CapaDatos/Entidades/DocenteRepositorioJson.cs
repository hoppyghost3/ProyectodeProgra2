using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CapaEntidad.EntidadPersona;
using CapaUtilidades;

namespace CapaDatos.Entidades
{
    public class DocenteRepositorioJson : IRepositorioEntidad<Docente>
    {
        private string rutaArchivo = "docentes.json";

        public void Agregar(Docente docente)
        {
            try
            {
                List<Docente> lista = LeerArchivo();
                docente.Id = lista.Count > 0 ? lista.Max(d => d.Id) + 1 : 1;
                lista.Add(docente);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteRepositorioJson), nameof(Agregar));
                throw;
            }
        }

        public void Modificar(Docente docente)
        {
            try
            {
                List<Docente> lista = LeerArchivo();
                int index = lista.FindIndex(d => d.Id == docente.Id);
                if (index >= 0)
                {
                    lista[index] = docente;
                    GuardarArchivo(lista);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteRepositorioJson), nameof(Modificar));
                throw;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                List<Docente> lista = LeerArchivo();
                lista.RemoveAll(d => d.Id == id);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteRepositorioJson), nameof(Eliminar));
                throw;
            }
        }

        public List<Docente> Listar()
        {
            try
            {
                return LeerArchivo();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteRepositorioJson), nameof(Listar));
                throw;
            }
        }

        public Docente BuscarPorId(int id)
        {
            try
            {
                List<Docente> lista = LeerArchivo();
                return lista.Find(d => d.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(DocenteRepositorioJson), nameof(BuscarPorId));
                throw;
            }
        }

        private List<Docente> LeerArchivo()
        {
            if (!File.Exists(rutaArchivo))
                return new List<Docente>();

            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Docente>>(json) ?? new List<Docente>();
        }

        private void GuardarArchivo(List<Docente> lista)
        {
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(rutaArchivo, json);
        }
    }
}