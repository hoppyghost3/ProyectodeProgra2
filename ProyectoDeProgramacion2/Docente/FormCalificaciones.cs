using CapaEntidad;
using CapaEntidad.EntidadObjeto;
using CapaNegocio.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormCalificaciones : Form
    {
        private Usuario usuarioActual;
        private GestionAcademicaService gestionService;
        private CursoService cursoService;

        private ComboBox cboCursos;
        private DataGridView dgvInscritos;
        private TextBox txtNota;
        private TextBox txtObservaciones;
        private Button btnCalificar;

        public FormCalificaciones(Usuario usuario)
        {
            usuarioActual = usuario;
            gestionService = new GestionAcademicaService();
            cursoService = new CursoService();
            InicializarComponentesManual();
            CargarCursosDocente();
        }

        private void InicializarComponentesManual()
        {
            this.Size = new Size(900, 600);
            this.Text = "Gestión de Calificaciones";
            this.BackColor = Color.White;

            Label lblCurso = new Label { Text = "Seleccionar Curso:", Location = new Point(20, 20), AutoSize = true };
            cboCursos = new ComboBox { Location = new Point(140, 20), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboCursos.SelectedIndexChanged += CboCursos_SelectedIndexChanged;

            dgvInscritos = new DataGridView { Location = new Point(20, 60), Size = new Size(500, 400), ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };

            // Panel de Calificación
            Panel panelCalif = new Panel { Location = new Point(540, 60), Size = new Size(300, 400), BorderStyle = BorderStyle.FixedSingle };

            Label lblNota = new Label { Text = "Nota (0-100):", Location = new Point(10, 20) };
            txtNota = new TextBox { Location = new Point(10, 45), Size = new Size(100, 25) };

            Label lblObs = new Label { Text = "Observaciones:", Location = new Point(10, 80) };
            txtObservaciones = new TextBox { Location = new Point(10, 105), Size = new Size(260, 100), Multiline = true };

            btnCalificar = new Button { Text = "Registrar Nota", Location = new Point(10, 220), Size = new Size(260, 40), BackColor = Color.Green, ForeColor = Color.White };
            btnCalificar.Click += BtnCalificar_Click;

            panelCalif.Controls.AddRange(new Control[] { lblNota, txtNota, lblObs, txtObservaciones, btnCalificar });

            this.Controls.AddRange(new Control[] { lblCurso, cboCursos, dgvInscritos, panelCalif });
        }

        private void CargarCursosDocente()
        {
            cboCursos.DataSource = cursoService.ObtenerCursosPorDocente(usuarioActual.Id);
            cboCursos.DisplayMember = "Nombre";
            cboCursos.ValueMember = "Id";
        }

        private void CboCursos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCursos.SelectedValue != null)
            {
                int cursoId = (int)cboCursos.SelectedValue;
                dgvInscritos.DataSource = gestionService.ObtenerInscripcionesPorCurso(cursoId);
            }
        }

        private void BtnCalificar_Click(object sender, EventArgs e)
        {
            if (dgvInscritos.SelectedRows.Count == 0) { MessageBox.Show("Seleccione un estudiante"); return; }
            if (!double.TryParse(txtNota.Text, out double nota)) { MessageBox.Show("Nota inválida"); return; }

            var inscripcion = (Inscripcion)dgvInscritos.SelectedRows[0].DataBoundItem;
            var curso = (Curso)cboCursos.SelectedItem;

            var calificacion = new Calificacion
            {
                EstudianteId = inscripcion.EstudianteId,
                NombreEstudiante = inscripcion.NombreEstudiante,
                CursoId = curso.Id,
                NombreCurso = curso.Nombre,
                Nota = nota,
                Observaciones = txtObservaciones.Text,
                Periodo = "Final" // Puedes cambiar esto según lógica
            };

            var resultado = gestionService.RegistrarCalificacion(calificacion);
            MessageBox.Show(resultado.mensaje);
        }
    }
}