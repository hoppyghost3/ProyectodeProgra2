using CapaEntidad;
using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormMisCalificaciones : Form
    {
        private Usuario usuarioActual;
        private GestionAcademicaService gestionService;
        private DataGridView dgvNotas;

        public FormMisCalificaciones(Usuario usuario)
        {
            usuarioActual = usuario;
            gestionService = new GestionAcademicaService();

            this.Text = "Mis Calificaciones";
            this.Size = new Size(800, 500);

            Label lbl = new Label { Text = "HISTORIAL ACADÉMICO", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            dgvNotas = new DataGridView { Location = new Point(20, 60), Size = new Size(740, 380), ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            this.Controls.Add(lbl);
            this.Controls.Add(dgvNotas);

            dgvNotas.DataSource = gestionService.ObtenerCalificacionesPorEstudiante(usuarioActual.Id);
        }
    }
}