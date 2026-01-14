using CapaEntidad;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormMenuPrincipal : Form
    {
        private Usuario usuarioActual;
        private Panel panelMenu;
        private Panel panelContenido;
        private Label lblBienvenida;

        public FormMenuPrincipal(Usuario usuario)
        {
            usuarioActual = usuario;
            InicializarComponentes();
            CargarMenuSegunRol();
        }

        private void InicializarComponentes()
        {
            this.Text = $"Sistema Académico - {usuarioActual.Rol}";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Panel de menú lateral
            panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(41, 128, 185),
                Padding = new Padding(10)
            };

            // Bienvenida en el menú
            lblBienvenida = new Label
            {
                Text = $"Bienvenido\n{usuarioActual.NombreCompleto}\n\nRol: {usuarioActual.Rol}",
                Location = new Point(10, 20),
                Size = new Size(230, 100),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.TopLeft
            };
            panelMenu.Controls.Add(lblBienvenida);

            // Panel de contenido
            panelContenido = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            this.Controls.Add(panelContenido);
            this.Controls.Add(panelMenu);
        }

        private void CargarMenuSegunRol()
        {
            int y = 140;

            switch (usuarioActual.Rol)
            {
                case "Administrador":
                    AgregarBotonMenu(" Gestionar Cursos", y, () => AbrirFormulario(new FormGestionCursos()));
                    y += 60;
                    AgregarBotonMenu(" Gestionar Docentes", y, () => AbrirFormulario(new FormGestionDocentes()));
                    y += 60;
                    AgregarBotonMenu(" Gestionar Estudiantes", y, () => AbrirFormulario(new FormGestionEstudiantes()));
                    y += 60;
                    AgregarBotonMenu(" Reportes", y, () => MessageBox.Show("Módulo de reportes en desarrollo"));
                    break;

                case "Docente":
                    AgregarBotonMenu("Mis Cursos", y, () => AbrirFormulario(new FormMisCursosDocente(usuarioActual)));
                    y += 60;
                    AgregarBotonMenu(" Calificaciones", y, () => AbrirFormulario(new FormCalificaciones(usuarioActual)));
                    y += 60;
                    AgregarBotonMenu(" Tareas", y, () => AbrirFormulario(new FormGestionTareas(usuarioActual)));
                    y += 60;
                    AgregarBotonMenu(" Estudiantes", y, () => AbrirFormulario(new FormVerEstudiantes(usuarioActual)));
                    break;

                case "Estudiante":
                    AgregarBotonMenu(" Mis Cursos", y, () => AbrirFormulario(new FormMisCursosEstudiante(usuarioActual)));
                    y += 60;
                    AgregarBotonMenu(" Inscribir Cursos", y, () => AbrirFormulario(new FormInscribirCursos(usuarioActual)));
                    y += 60;
                    AgregarBotonMenu(" Mis Calificaciones", y, () => AbrirFormulario(new FormMisCalificaciones(usuarioActual)));
                    y += 60;
                    AgregarBotonMenu(" Mis Tareas", y, () => AbrirFormulario(new FormMisTareas(usuarioActual)));
                    break;
            }

            // Botón de cerrar sesión
            AgregarBotonMenu("🚪 Cerrar Sesión", this.Height - 150, CerrarSesion);
        }

        private void AgregarBotonMenu(string texto, int y, Action accion)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = new Point(10, y),
                Size = new Size(230, 50),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => accion();

            // Efectos hover
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(41, 128, 185);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(52, 152, 219);

            panelMenu.Controls.Add(btn);
        }

        private void AbrirFormulario(Form formulario)
        {
            panelContenido.Controls.Clear();
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(formulario);
            formulario.Show();
        }

        private void CerrarSesion()
        {
            var resultado = MessageBox.Show(
                "¿Está seguro de que desea cerrar sesión?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                this.Close();
                // Abrir login nuevamente
                FormLogin login = new FormLogin();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    var nuevoMenu = new FormMenuPrincipal(login.UsuarioAutenticado);
                    nuevoMenu.Show();
                }
            }
        }
    }
}