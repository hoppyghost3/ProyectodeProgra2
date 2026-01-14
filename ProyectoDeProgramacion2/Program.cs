using System;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Mostrar formulario de login primero
            FormLogin formLogin = new FormLogin();

            if (formLogin.ShowDialog() == DialogResult.OK)
            {
                // Si el login fue exitoso, abrir el menú principal
                var usuario = formLogin.UsuarioAutenticado;
                Application.Run(new FormMenuPrincipal(usuario));
            }
            else
            {
                // Usuario canceló el login, cerrar aplicación
                Application.Exit();
            }
        }
    }
}