using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormLogin : Form
    {
        private AutenticacionService authService;
        private bool modoRegistro = false;

        // Controles
        private TextBox txtUsuario;
        private TextBox txtContrasena;
        private TextBox txtNombreCompleto;
        private TextBox txtEmail;
        private ComboBox cboRol;
        private Button btnAccion;
        private Button btnCambiarModo;
        private CheckBox chkMostrarContrasena;
        private Label lblTitulo;

        public Usuario UsuarioAutenticado { get; private set; }

        public FormLogin()
        {
            authService = new AutenticacionService();
            InicializarComponentes();
            ConfigurarModoLogin();
        }

        private void InicializarComponentes()
        {
            // Configuración del formulario
            this.Text = "Sistema - Login";
            this.Size = new Size(450, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            int y = 30;

            // Título
            lblTitulo = new Label
            {
                Text = "INICIAR SESIÓN",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                Location = new Point(50, y),
                Size = new Size(340, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            y += 60;

            // Usuario
            Label lblUsuario = new Label
            {
                Text = "Usuario:",
                Location = new Point(50, y),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10)
            };
            txtUsuario = new TextBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(340, 25),
                Font = new Font("Segoe UI", 10)
            };
            y += 65;

            // Contraseña
            Label lblContrasena = new Label
            {
                Text = "Contraseña:",
                Location = new Point(50, y),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10)
            };
            txtContrasena = new TextBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(340, 25),
                PasswordChar = '●',
                Font = new Font("Segoe UI", 10)
            };
            y += 65;

            // Mostrar contraseña
            chkMostrarContrasena = new CheckBox
            {
                Text = "Mostrar contraseña",
                Location = new Point(50, y),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9)
            };
            chkMostrarContrasena.CheckedChanged += (s, e) =>
                txtContrasena.PasswordChar = chkMostrarContrasena.Checked ? '\0' : '●';
            y += 35;

            // Nombre completo (oculto inicialmente)
            Label lblNombre = new Label
            {
                Text = "Nombre Completo:",
                Location = new Point(50, y),
                Size = new Size(150, 20),
                Visible = false,
                Font = new Font("Segoe UI", 10),
                Tag = "registro"
            };
            txtNombreCompleto = new TextBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(340, 25),
                Visible = false,
                Font = new Font("Segoe UI", 10),
                Tag = "registro"
            };
            y += 65;

            // Email (oculto inicialmente)
            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(50, y),
                Size = new Size(100, 20),
                Visible = false,
                Font = new Font("Segoe UI", 10),
                Tag = "registro"
            };
            txtEmail = new TextBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(340, 25),
                Visible = false,
                Font = new Font("Segoe UI", 10),
                Tag = "registro"
            };
            y += 65;

            // Rol (oculto inicialmente)
            Label lblRol = new Label
            {
                Text = "Rol:",
                Location = new Point(50, y),
                Size = new Size(100, 20),
                Visible = false,
                Font = new Font("Segoe UI", 10),
                Tag = "registro"
            };
            cboRol = new ComboBox
            {
                Location = new Point(50, y + 25),
                Size = new Size(340, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false,
                Font = new Font("Segoe UI", 10),
                Tag = "registro"
            };
            cboRol.Items.AddRange(Roles.ObtenerTodos().ToArray());
            cboRol.SelectedIndex = 0;

            // Botones
            btnAccion = new Button
            {
                Text = "INICIAR SESIÓN",
                Location = new Point(50, 270),
                Size = new Size(340, 40),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAccion.FlatAppearance.BorderSize = 0;
            btnAccion.Click += BtnAccion_Click;

            btnCambiarModo = new Button
            {
                Text = "¿No tienes cuenta? Regístrate",
                Location = new Point(50, 320),
                Size = new Size(340, 30),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DodgerBlue,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCambiarModo.FlatAppearance.BorderSize = 0;
            btnCambiarModo.Click += BtnCambiarModo_Click;

            // Agregar controles al formulario
            this.Controls.AddRange(new Control[]
            {
                lblTitulo, lblUsuario, txtUsuario, lblContrasena, txtContrasena,
                chkMostrarContrasena, lblNombre, txtNombreCompleto,
                lblEmail, txtEmail, lblRol, cboRol,
                btnAccion, btnCambiarModo
            });
        }

        private void ConfigurarModoLogin()
        {
            modoRegistro = false;
            this.Size = new Size(450, 450);
            lblTitulo.Text = "INICIAR SESIÓN";
            btnAccion.Text = "INICIAR SESIÓN";
            btnAccion.Location = new Point(50, 270);
            btnCambiarModo.Text = "¿No tienes cuenta? Regístrate";
            btnCambiarModo.Location = new Point(50, 320);

            // Ocultar campos de registro
            foreach (Control c in this.Controls)
            {
                if (c.Tag != null && c.Tag.ToString() == "registro")
                    c.Visible = false;
            }

            LimpiarCampos();
        }

        private void ConfigurarModoRegistro()
        {
            modoRegistro = true;
            this.Size = new Size(450, 650);
            lblTitulo.Text = "REGISTRO DE USUARIO";
            btnAccion.Text = "REGISTRARSE";
            btnAccion.Location = new Point(50, 520);
            btnCambiarModo.Text = "¿Ya tienes cuenta? Inicia sesión";
            btnCambiarModo.Location = new Point(50, 570);

            // Mostrar campos de registro
            foreach (Control c in this.Controls)
            {
                if (c.Tag != null && c.Tag.ToString() == "registro")
                    c.Visible = true;
            }

            LimpiarCampos();
        }

        private void BtnCambiarModo_Click(object sender, EventArgs e)
        {
            if (modoRegistro)
                ConfigurarModoLogin();
            else
                ConfigurarModoRegistro();
        }

        private void BtnAccion_Click(object sender, EventArgs e)
        {
            if (modoRegistro)
                Registrar();
            else
                IniciarSesion();
        }

        private void IniciarSesion()
        {
            var resultado = authService.IniciarSesion(
                txtUsuario.Text.Trim(),
                txtContrasena.Text
            );

            if (resultado.exito)
            {
                UsuarioAutenticado = resultado.usuario;
                MessageBox.Show(
                    $"Bienvenido {resultado.usuario.NombreCompleto}!\nRol: {resultado.usuario.Rol}",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(resultado.mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContrasena.Clear();
                txtContrasena.Focus();
            }
        }

        private void Registrar()
        {
            var resultado = authService.RegistrarUsuario(
                txtUsuario.Text.Trim(),
                txtContrasena.Text,
                txtNombreCompleto.Text.Trim(),
                cboRol.SelectedItem?.ToString() ?? Roles.Estudiante,
                txtEmail.Text.Trim()
            );

            if (resultado.exito)
            {
                MessageBox.Show(
                    resultado.mensaje + "\n\nAhora puedes iniciar sesión.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                ConfigurarModoLogin();
            }
            else
            {
                MessageBox.Show(resultado.mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtContrasena.Clear();
            txtNombreCompleto.Clear();
            txtEmail.Clear();
            if (cboRol.Items.Count > 0)
                cboRol.SelectedIndex = 0;
            chkMostrarContrasena.Checked = false;
            txtUsuario.Focus();
        }
    }
}