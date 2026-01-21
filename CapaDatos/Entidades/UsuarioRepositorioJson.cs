using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CapaEntidad;
using CapaUtilidades; 

namespace CapaDatos.Entidades
{
    public class UsuarioRepositorioJson : IRepositorioEntidad<Usuario>
    {
        private string rutaArchivo = "usuarios.json";

        public void Agregar(Usuario usuario)
        {
            try
            {
                List<Usuario> lista = LeerArchivo();

                usuario.Id = lista.Count > 0 ? lista.Max(u => u.Id) + 1 : 1;

                lista.Add(usuario);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(UsuarioRepositorioJson), nameof(Agregar));
                throw;
            }
        }

        public void Modificar(Usuario usuario)
        {
            try
            {
                List<Usuario> lista = LeerArchivo();
                int index = lista.FindIndex(u => u.Id == usuario.Id);
                if (index >= 0)
                {
                    lista[index] = usuario;
                    GuardarArchivo(lista);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(UsuarioRepositorioJson), nameof(Modificar));
                throw;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                List<Usuario> lista = LeerArchivo();
                lista.RemoveAll(u => u.Id == id);
                GuardarArchivo(lista);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(UsuarioRepositorioJson), nameof(Eliminar));
                throw;
            }
        }

        public List<Usuario> Listar()
        {
            try
            {
                return LeerArchivo();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(UsuarioRepositorioJson), nameof(Listar));
                throw;
            }
        }

        public Usuario BuscarPorId(int id)
        {
            try
            {
                List<Usuario> lista = LeerArchivo();
                return lista.Find(u => u.Id == id);
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(UsuarioRepositorioJson), nameof(BuscarPorId));
                throw;
            }
        }

        // Métodos autenticación
        public Usuario BuscarPorNombreUsuario(string nombreUsuario)
        {
            try
            {
                List<Usuario> lista = LeerArchivo();
                return lista.Find(u => u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(UsuarioRepositorioJson), nameof(BuscarPorNombreUsuario));
                throw;
            }
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            try
            {
                return BuscarPorNombreUsuario(nombreUsuario) != null;
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(UsuarioRepositorioJson), nameof(ExisteNombreUsuario));
                throw;
            }
        }

        // Métodos privados
        private List<Usuario> LeerArchivo()
        {
            if (!File.Exists(rutaArchivo))
                return new List<Usuario>();

            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        private void GuardarArchivo(List<Usuario> lista)
        {
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(rutaArchivo, json);
        }

        List<Usuario> IRepositorioEntidad<Usuario>.Listar()
        {
            throw new NotImplementedException();
        }
    }
}