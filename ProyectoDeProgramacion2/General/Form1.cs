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

            // Mostrar info del usuario en el formulario
            this.Text = $"Sistema de Gestión - {usuario.Rol}: {usuario.NombreCompleto}";
            ConfigurarPermisosPorRol();
        }

        private void ConfigurarPermisosPorRol()
        {
            switch (usuarioActual.Rol)
            {
                case "Administrador":
                    break;

                case "Docente":
                    break;

                case "Estudiante":
                    break;
            }
        }
    }
}