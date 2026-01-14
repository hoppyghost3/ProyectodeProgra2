using CapaEntidad;
using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormMisCursosDocente : Form
    {
        private Usuario usuarioActual;
        private CursoService cursoService;
        private DataGridView dgvCursos;

        public FormMisCursosDocente(Usuario usuario)
        {
            usuarioActual = usuario;
            cursoService = new CursoService();
            InicializarComponentesManual();
            CargarCursos();
        }

        private void InicializarComponentesManual()
        {
            this.Text = "Mis Cursos Asignados";
            this.Size = new Size(800, 500);
            this.BackColor = Color.White;

            Label lblTitulo = new Label
            {
                Text = "MIS CURSOS DOCENTE",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitulo);

            dgvCursos = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(740, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            this.Controls.Add(dgvCursos);
        }

        private void CargarCursos()
        {
            try
            {
                var cursos = cursoService.ObtenerCursosPorDocente(usuarioActual.Id);
                dgvCursos.DataSource = cursos;
                if (dgvCursos.Columns["EstudiantesInscritos"] != null)
                    dgvCursos.Columns["EstudiantesInscritos"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}