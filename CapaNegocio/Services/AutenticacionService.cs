using CapaEntidad;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System;
using CapaDatos.Entidades;
using CapaEntidad.EntidadPersona;

namespace CapaNegocio.Services
{
    public class AutenticacionService
    {
        private UsuarioRepositorioJson repositorioUsuarios;
        private EstudianteRepositorioJson repositorioEstudiantes;
        private DocenteRepositorioJson repositorioDocentes;

        public AutenticacionService()
        {
            repositorioUsuarios = new UsuarioRepositorioJson();
            repositorioEstudiantes = new EstudianteRepositorioJson();
            repositorioDocentes = new DocenteRepositorioJson(); 
        }

        public class ResultadoRegistro
        {
            public bool exito { get; set; }
            public string mensaje { get; set; }
        }

        public class ResultadoLogin
        {
            public bool exito { get; set; }
            public string mensaje { get; set; }
            public Usuario usuario { get; set; }
        }

        public ResultadoRegistro RegistrarUsuario(string nombreUsuario, string contrasena,
            string nombreCompleto, string rol, string email,
            string telefono = "",
            string carrera = "", int semestre = 1, 
            string especialidad = "", string departamento = "") 
        {
            try
            {
                // Validaciones 
                if (string.IsNullOrWhiteSpace(nombreUsuario)) return new ResultadoRegistro { exito = false, mensaje = "El usuario es obligatorio" };
                if (string.IsNullOrWhiteSpace(contrasena)) return new ResultadoRegistro { exito = false, mensaje = "La contraseña es obligatoria" };
                if (string.IsNullOrWhiteSpace(nombreCompleto)) return new ResultadoRegistro { exito = false, mensaje = "El nombre es obligatorio" };
                if (string.IsNullOrWhiteSpace(email)) return new ResultadoRegistro { exito = false, mensaje = "El email es obligatorio" };

                if (repositorioUsuarios.ExisteNombreUsuario(nombreUsuario))
                    return new ResultadoRegistro { exito = false, mensaje = "El nombre de usuario ya existe" };

                // 1. Creacion usuario
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

                repositorioUsuarios.Agregar(usuario);

                // Nombre y Apellido 
                var partesNombre = nombreCompleto.Split(' ');
                string nombre = partesNombre[0];
                string apellido = partesNombre.Length > 1 ? partesNombre[1] : "";

                if (rol == "Estudiante")
                {
                    var nuevoEstudiante = new Estudiante
                    {
                        Nombre = nombre,
                        Apellido = apellido,
                        Email = email,
                        Telefono = telefono,
                        Carrera = carrera,
                        Semestre = semestre,
                        Activo = true,
                        FechaRegistro = DateTime.Now
                    };
                    repositorioEstudiantes.Agregar(nuevoEstudiante);
                }
                else if (rol == "Docente")
                {
                    var nuevoDocente = new Docente
                    {
                        Nombre = nombre,
                        Apellido = apellido,
                        Email = email,
                        Telefono = telefono,
                        Especialidad = especialidad,
                        Departamento = departamento,
                        Activo = true,
                        FechaContratacion = DateTime.Now
                    };
                    repositorioDocentes.Agregar(nuevoDocente);
                }

                return new ResultadoRegistro { exito = true, mensaje = "Usuario registrado exitosamente" };
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(AutenticacionService), nameof(RegistrarUsuario));
                return new ResultadoRegistro { exito = false, mensaje = "Error: " + ex.Message };
            }
        }

        public ResultadoLogin IniciarSesion(string nombreUsuario, string contrasena)
        {
            try
            {
                var usuario = repositorioUsuarios.BuscarPorNombreUsuario(nombreUsuario);

                if (usuario == null) return new ResultadoLogin { exito = false, mensaje = "Usuario no encontrado", usuario = null };
                if (!usuario.Activo) return new ResultadoLogin { exito = false, mensaje = "Usuario inactivo", usuario = null };
                if (usuario.Contrasena != EncriptarContrasena(contrasena)) return new ResultadoLogin { exito = false, mensaje = "Contraseña incorrecta", usuario = null };

                return new ResultadoLogin { exito = true, mensaje = "Bienvenido", usuario = usuario };
            }
            catch (Exception ex)
            {
                ErrorLogger.RegistrarError(ex, nameof(AutenticacionService), nameof(IniciarSesion));
                return new ResultadoLogin { exito = false, mensaje = "Error: " + ex.Message, usuario = null };
            }
        }

        private string EncriptarContrasena(string contrasena)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes) builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            return repositorioUsuarios.Listar();
        }
    }
}