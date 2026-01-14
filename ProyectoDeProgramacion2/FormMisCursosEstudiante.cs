using CapaEntidad;
using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormMisCursosEstudiante : Form
    {
        private Usuario usuarioActual;
        private GestionAcademicaService gestionService;
        private DataGridView dgvCursos;

        public FormMisCursosEstudiante(Usuario usuario)
        {
            usuarioActual = usuario;
            gestionService = new GestionAcademicaService();

            this.Text = "Mis Cursos Inscritos";
            this.Size = new Size(800, 500);

            Label lbl = new Label { Text = "MIS CURSOS", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            dgvCursos = new DataGridView { Location = new Point(20, 60), Size = new Size(740, 380), ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            this.Controls.Add(lbl);
            this.Controls.Add(dgvCursos);

            dgvCursos.DataSource = gestionService.ObtenerInscripcionesPorEstudiante(usuarioActual.Id);
        }
    }
}