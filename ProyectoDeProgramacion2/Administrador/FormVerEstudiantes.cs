using CapaEntidad;
using CapaNegocio.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormVerEstudiantes : Form
    {
        private Usuario usuarioActual;
        private GestionAcademicaService gestionService;
        private CursoService cursoService;
        private ComboBox cboCursos;
        private DataGridView dgvEstudiantes;

        public FormVerEstudiantes(Usuario usuario)
        {
            usuarioActual = usuario;
            gestionService = new GestionAcademicaService();
            cursoService = new CursoService();

            this.Size = new Size(800, 600);
            this.Text = "Estudiantes Inscritos";

            Label lbl = new Label { Text = "Curso:", Location = new Point(20, 20), AutoSize = true };
            cboCursos = new ComboBox { Location = new Point(70, 18), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboCursos.SelectedIndexChanged += (s, e) => {
                if (cboCursos.SelectedValue != null)
                    dgvEstudiantes.DataSource = gestionService.ObtenerInscripcionesPorCurso((int)cboCursos.SelectedValue);
            };

            dgvEstudiantes = new DataGridView { Location = new Point(20, 60), Size = new Size(740, 480), ReadOnly = true };

            this.Controls.Add(lbl);
            this.Controls.Add(cboCursos);
            this.Controls.Add(dgvEstudiantes);

            cboCursos.DataSource = cursoService.ObtenerCursosPorDocente(usuarioActual.Id);
            cboCursos.DisplayMember = "Nombre";
            cboCursos.ValueMember = "Id";
        }
    }
}