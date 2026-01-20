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
    public class CursoRepositorioJson : IRepositorioEntidad<Curso>
    {
        private string rutaArchivo = "cursos.json";

        public void Agregar(Curso curso)
        {
            try
            {
                List<Curso> lista = LeerArchivo();
                curso.Id = lista.Count > 0 ? lista.Max(c => c.Id) + 1 : 1;
                lista.Add(curso);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoRepositorioJson), nameof(Agregar));
                throw;
            }
        }

        public void Modificar(Curso curso)
        {
            try
            {
                List<Curso> lista = LeerArchivo();
                int index = lista.FindIndex(c => c.Id == curso.Id);
                if (index >= 0)
                {
                    lista[index] = curso;
                    GuardarArchivo(lista);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoRepositorioJson), nameof(Modificar));
                throw;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                List<Curso> lista = LeerArchivo();
                lista.RemoveAll(c => c.Id == id);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoRepositorioJson), nameof(Eliminar));
                throw;
            }
        }

        public List<Curso> Listar()
        {
            try
            {
                return LeerArchivo();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoRepositorioJson), nameof(Listar));
                throw;
            }
        }

        public Curso BuscarPorId(int id)
        {
            try
            {
                List<Curso> lista = LeerArchivo();
                return lista.Find(c => c.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(CursoRepositorioJson), nameof(BuscarPorId));
                throw;
            }
        }

        private List<Curso> LeerArchivo()
        {
            if (!File.Exists(rutaArchivo))
                return new List<Curso>();

            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Curso>>(json) ?? new List<Curso>();
        }

        private void GuardarArchivo(List<Curso> lista)
        {
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(rutaArchivo, json);
        }
    }
}