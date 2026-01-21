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

            FormLogin formLogin = new FormLogin();

            if (formLogin.ShowDialog() == DialogResult.OK)
            {
                // Menú principal
                var usuario = formLogin.UsuarioAutenticado;
                Application.Run(new FormMenuPrincipal(usuario));
            }
            else
            {
                // Cerrar aplicación
                Application.Exit();
            }
        }
    }
}