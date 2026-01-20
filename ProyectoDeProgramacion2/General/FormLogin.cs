using CapaEntidad;
using CapaNegocio.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormLogin : Form
    {
        private AutenticacionService authService;
        private bool modoRegistro = false;

        private TextBox txtUsuario, txtContrasena, txtNombreCompleto, txtEmail;
        private ComboBox cboRol;
        private Button btnAccion, btnCambiarModo;
        private CheckBox chkMostrarContrasena;
        private Label lblTitulo;

        // Controles Estudiantes
        private TextBox txtTelefono, txtCarrera;
        private NumericUpDown numSemestre;
        private Label lblTelefono, lblCarrera, lblSemestre;

        // Controles Docentes
        private TextBox txtEspecialidad, txtDepartamento;
        private Label lblEspecialidad, lblDepartamento;

        public Usuario UsuarioAutenticado { get; private set; }

        public FormLogin()
        {
            authService = new AutenticacionService();
            InicializarComponentes();
            ConfigurarModoLogin();
        }

        private void InicializarComponentes()
        {
            this.Text = "Sistema Académico - Acceso";
            this.Size = new Size(450, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            int y = 30;

            // Título
            lblTitulo = new Label { Text = "INICIAR SESIÓN", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.DodgerBlue, Location = new Point(50, y), Size = new Size(340, 40), TextAlign = ContentAlignment.MiddleCenter };
            y += 60;

            // Usuario
            Label lblU = new Label { Text = "Usuario:", Location = new Point(50, y), Size = new Size(100, 20) };
            txtUsuario = new TextBox { Location = new Point(50, y + 25), Size = new Size(340, 25) };
            y += 60;

            // Contraseña
            Label lblC = new Label { Text = "Contraseña:", Location = new Point(50, y), Size = new Size(100, 20) };
            txtContrasena = new TextBox { Location = new Point(50, y + 25), Size = new Size(340, 25), PasswordChar = '●' };
            y += 60;

            chkMostrarContrasena = new CheckBox { Text = "Mostrar contraseña", Location = new Point(50, y), Size = new Size(150, 20) };
            chkMostrarContrasena.CheckedChanged += (s, e) => txtContrasena.PasswordChar = chkMostrarContrasena.Checked ? '\0' : '●';
            y += 30;

            // --- CAMPOS DE REGISTRO BASE ---

            Label lblN = CrearLabel("Nombre Completo:", y);
            txtNombreCompleto = CrearTextBox(y + 25);
            y += 60;

            Label lblE = CrearLabel("Email:", y);
            txtEmail = CrearTextBox(y + 25);
            y += 60;

            Label lblR = CrearLabel("Rol:", y);
            cboRol = new ComboBox { Location = new Point(50, y + 25), Size = new Size(340, 25), DropDownStyle = ComboBoxStyle.DropDownList, Visible = false, Tag = "registro" };
            cboRol.Items.AddRange(Roles.ObtenerTodos().ToArray());
            cboRol.SelectedIndex = 0;
            cboRol.SelectedIndexChanged += CboRol_SelectedIndexChanged;
            y += 60;

            // --- CAMPOS EXTRA (Estudiante y Docente) ---

            // Teléfono
            lblTelefono = CrearLabel("Teléfono:", y);
            txtTelefono = CrearTextBox(y + 25);
            y += 60;

            // Campos  Estudiante
            lblCarrera = CrearLabel("Carrera:", y);
            txtCarrera = CrearTextBox(y + 25);

            // Posición Y Especialidad (Docente)
            lblEspecialidad = new Label { Text = "Especialidad:", Location = new Point(50, y), Size = new Size(200, 20), Visible = false, Tag = "extra_docente" };
            txtEspecialidad = new TextBox { Location = new Point(50, y + 25), Size = new Size(340, 25), Visible = false, Tag = "extra_docente" };

            y += 60;

            // Campos  Estudiante (Semestre)
            lblSemestre = CrearLabel("Semestre:", y);
            numSemestre = new NumericUpDown { Location = new Point(50, y + 25), Size = new Size(100, 25), Minimum = 1, Maximum = 12, Visible = false, Tag = "extra_estudiante" };

            // Posición Y Departamento (Docente)
            lblDepartamento = new Label { Text = "Departamento:", Location = new Point(50, y), Size = new Size(200, 20), Visible = false, Tag = "extra_docente" };
            txtDepartamento = new TextBox { Location = new Point(50, y + 25), Size = new Size(340, 25), Visible = false, Tag = "extra_docente" };


            // Botones
            btnAccion = new Button { Text = "INICIAR SESIÓN", Location = new Point(50, 300), Size = new Size(340, 40), BackColor = Color.DodgerBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Cursor = Cursors.Hand };
            btnAccion.Click += BtnAccion_Click;

            btnCambiarModo = new Button { Text = "Registrarse", Location = new Point(50, 350), Size = new Size(340, 30), ForeColor = Color.DodgerBlue, FlatStyle = FlatStyle.Flat };
            btnCambiarModo.FlatAppearance.BorderSize = 0;
            btnCambiarModo.Click += BtnCambiarModo_Click;

            this.Controls.AddRange(new Control[] { lblTitulo, lblU, txtUsuario, lblC, txtContrasena, chkMostrarContrasena,
                lblN, txtNombreCompleto, lblE, txtEmail, lblR, cboRol,
                lblTelefono, txtTelefono, lblCarrera, txtCarrera, lblSemestre, numSemestre,
                lblEspecialidad, txtEspecialidad, lblDepartamento, txtDepartamento,
                btnAccion, btnCambiarModo });
        }

        private Label CrearLabel(string texto, int y)
        {
            return new Label { Text = texto, Location = new Point(50, y), Size = new Size(200, 20), Visible = false, Tag = "registro" };
        }
        private TextBox CrearTextBox(int y)
        {
            return new TextBox { Location = new Point(50, y), Size = new Size(340, 25), Visible = false, Tag = "registro" };
        }

        private void ConfigurarModoLogin()
        {
            modoRegistro = false;
            this.Size = new Size(450, 450);
            lblTitulo.Text = "INICIAR SESIÓN";
            btnAccion.Text = "LOG IN";
            btnAccion.Location = new Point(50, 280);
            btnCambiarModo.Text = "Registrarse";
            btnCambiarModo.Location = new Point(50, 330);

            MostrarControlesRegistro(false);
            LimpiarCampos();
        }

        private void ConfigurarModoRegistro()
        {
            modoRegistro = true;
            lblTitulo.Text = "REGISTRO DE USUARIO";
            btnAccion.Text = "REGISTRARSE";
            btnCambiarModo.Text = "Volver al Login";

            MostrarControlesRegistro(true);
            CboRol_SelectedIndexChanged(null, null);
        }

        private void MostrarControlesRegistro(bool mostrar)
        {
            foreach (Control c in this.Controls)
            {
                if (c.Tag != null && c.Tag.ToString() == "registro")
                    c.Visible = mostrar;

                if (c.Tag != null && (c.Tag.ToString() == "extra_estudiante" || c.Tag.ToString() == "extra_docente"))
                    c.Visible = false;
            }
        }

        private void CboRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!modoRegistro) return;

            string rol = cboRol.SelectedItem != null ? cboRol.SelectedItem.ToString() : "";
            bool esEstudiante = rol == "Estudiante";
            bool esDocente = rol == "Docente";

            lblTelefono.Visible = esEstudiante || esDocente;
            txtTelefono.Visible = esEstudiante || esDocente;

            lblCarrera.Visible = esEstudiante;
            txtCarrera.Visible = esEstudiante;
            lblSemestre.Visible = esEstudiante;
            numSemestre.Visible = esEstudiante;

            lblEspecialidad.Visible = esDocente;
            txtEspecialidad.Visible = esDocente;
            lblDepartamento.Visible = esDocente;
            txtDepartamento.Visible = esDocente;

            if (esEstudiante || esDocente)
            {
                this.Size = new Size(450, 850);
                btnAccion.Location = new Point(50, 700);
                btnCambiarModo.Location = new Point(50, 750);
            }
            else
            {
                this.Size = new Size(450, 600);
                btnAccion.Location = new Point(50, 480);
                btnCambiarModo.Location = new Point(50, 530);
            }
        }

        private void BtnAccion_Click(object sender, EventArgs e)
        {
            if (modoRegistro)
            {
                var res = authService.RegistrarUsuario(
                    txtUsuario.Text.Trim(),
                    txtContrasena.Text,
                    txtNombreCompleto.Text.Trim(),
                    cboRol.SelectedItem.ToString(),
                    txtEmail.Text.Trim(),
                    txtTelefono.Text.Trim(),
                    txtCarrera.Text.Trim(),
                    (int)numSemestre.Value,
                    txtEspecialidad.Text.Trim(),
                    txtDepartamento.Text.Trim()
                );

                if (res.exito)
                {
                    MessageBox.Show(res.mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ConfigurarModoLogin();
                }
                else
                {
                    MessageBox.Show(res.mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var res = authService.IniciarSesion(txtUsuario.Text.Trim(), txtContrasena.Text);
                if (res.exito)
                {
                    UsuarioAutenticado = res.usuario;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(res.mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCambiarModo_Click(object sender, EventArgs e)
        {
            if (modoRegistro) ConfigurarModoLogin();
            else ConfigurarModoRegistro();
        }

        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtContrasena.Clear();
            txtNombreCompleto.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtCarrera.Clear();
            txtEspecialidad.Clear();
            txtDepartamento.Clear();
            numSemestre.Value = 1;
        }
    }
}