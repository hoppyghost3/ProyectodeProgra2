using System;
using System.Windows.Forms;
using CapaEntidad;

namespace ProyectoDeProgramacion2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Mostrar formulario de login
            FormLogin formLogin = new FormLogin();

            if (formLogin.ShowDialog() == DialogResult.OK)
            {
                // Login exitoso
                var usuario = formLogin.UsuarioAutenticado;

                MessageBox.Show(
                    $"Bienvenido {usuario.NombreCompleto}\nRol: {usuario.Rol}",
                    "Acceso Concedido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Aquí abrirías tu Form1 o el formulario correspondiente
                // Application.Run(new Form1(usuario));
            }
        }
    }
}