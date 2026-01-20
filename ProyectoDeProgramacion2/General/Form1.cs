using CapaEntidad;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class Form1 : Form
    {
        private Usuario usuarioActual;

        public Form1(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;

            // Mostrar información del usuario en el formulario
            this.Text = $"Sistema de Gestión - {usuario.Rol}: {usuario.NombreCompleto}";

            // Aquí puedes configurar permisos según el rol
            ConfigurarPermisosPorRol();
        }

        private void ConfigurarPermisosPorRol()
        {
            switch (usuarioActual.Rol)
            {
                case "Administrador":
                    // El administrador tiene acceso a todo
                    break;

                case "Docente":
                    // Limitar acceso de docente
                    break;

                case "Estudiante":
                    // Limitar acceso de estudiante
                    break;
            }
        }
    }
}