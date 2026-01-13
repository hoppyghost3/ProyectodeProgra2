using CapaEntidad;
using CapaDatos;
using CapaUtilidades; // O CapaEntidad si pusiste ErrorLogger ahí
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System;

namespace CapaNegocio
{
    public class AutenticacionService
    {
        private UsuarioRepositorioJson repositorio;

        public AutenticacionService()
        {
            repositorio = new UsuarioRepositorioJson();
        }

        // Clase para resultado de registro
        public class ResultadoRegistro
        {
            public bool exito { get; set; }
            public string mensaje { get; set; }
        }

        // Clase para resultado de login
        public class ResultadoLogin
        {
            public bool exito { get; set; }
            public string mensaje { get; set; }
            public Usuario usuario { get; set; }
        }

        // Registrar nuevo usuario
        public ResultadoRegistro RegistrarUsuario(string nombreUsuario, string contrasena,
            string nombreCompleto, string rol, string email)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(nombreUsuario))
                    return new ResultadoRegistro { exito = false, mensaje = "El nombre de usuario es obligatorio" };

                if (nombreUsuario.Length < 4)
                    return new ResultadoRegistro { exito = false, mensaje = "El nombre de usuario debe tener al menos 4 caracteres" };

                if (string.IsNullOrWhiteSpace(contrasena))
                    return new ResultadoRegistro { exito = false, mensaje = "La contraseña es obligatoria" };

                if (contrasena.Length < 6)
                    return new ResultadoRegistro { exito = false, mensaje = "La contraseña debe tener al menos 6 caracteres" };

                if (string.IsNullOrWhiteSpace(nombreCompleto))
                    return new ResultadoRegistro { exito = false, mensaje = "El nombre completo es obligatorio" };

                if (string.IsNullOrWhiteSpace(email))
                    return new ResultadoRegistro { exito = false, mensaje = "El email es obligatorio" };

                if (!email.Contains("@"))
                    return new ResultadoRegistro { exito = false, mensaje = "El email no es válido" };

                // Verificar si el usuario ya existe
                if (repositorio.ExisteNombreUsuario(nombreUsuario))
                    return new ResultadoRegistro { exito = false, mensaje = "El nombre de usuario ya está registrado" };

                // Crear usuario con contraseña encriptada
                var usuario = new Usuario
                {
                    NombreUsuario = nombreUsuario,
                    Contrasena = EncriptarContrasena(contrasena),
                    NombreCompleto = nombreCompleto,
                    Rol = rol,
                    Email = email,
                    FechaRegistro = DateTime.Now,
                    Activo = true
                };

                repositorio.Agregar(usuario);
                return new ResultadoRegistro { exito = true, mensaje = "Usuario registrado exitosamente" };
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(AutenticacionService), nameof(RegistrarUsuario));
                return new ResultadoRegistro { exito = false, mensaje = "Error al registrar usuario: " + ex.Message };
            }
        }

        // Iniciar sesión
        public ResultadoLogin IniciarSesion(string nombreUsuario, string contrasena)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
                    return new ResultadoLogin { exito = false, mensaje = "Usuario y contraseña son obligatorios", usuario = null };

                var usuario = repositorio.BuscarPorNombreUsuario(nombreUsuario);

                if (usuario == null)
                    return new ResultadoLogin { exito = false, mensaje = "Usuario no encontrado", usuario = null };

                if (!usuario.Activo)
                    return new ResultadoLogin { exito = false, mensaje = "Usuario inactivo. Contacte al administrador", usuario = null };

                // Verificar contraseña
                if (usuario.Contrasena != EncriptarContrasena(contrasena))
                    return new ResultadoLogin { exito = false, mensaje = "Contraseña incorrecta", usuario = null };

                return new ResultadoLogin { exito = true, mensaje = "Inicio de sesión exitoso", usuario = usuario };
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(AutenticacionService), nameof(IniciarSesion));
                return new ResultadoLogin { exito = false, mensaje = "Error al iniciar sesión: " + ex.Message, usuario = null };
            }
        }

        // Encriptar contraseña usando SHA256
        private string EncriptarContrasena(string contrasena)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Obtener todos los usuarios (para administración)
        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            try
            {
                return repositorio.Listar();
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(AutenticacionService), nameof(ObtenerTodosLosUsuarios));
                throw;
            }
        }
    }
}