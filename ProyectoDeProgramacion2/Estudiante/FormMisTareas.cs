using CapaEntidad;
using CapaEntidad.EntidadObjeto;
using CapaNegocio.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormMisTareas : Form
    {
        private Usuario usuarioActual;
        private GestionAcademicaService gestionService;
        private DataGridView dgvTareasPendientes;
        private TextBox txtContenido;
        private Button btnEntregar;

        public FormMisTareas(Usuario usuario)
        {
            usuarioActual = usuario;
            gestionService = new GestionAcademicaService();

            this.Text = "Mis Tareas";
            this.Size = new Size(900, 600);

            Label lblT = new Label { Text = "Tareas de mis cursos:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            dgvTareasPendientes = new DataGridView { Location = new Point(20, 50), Size = new Size(840, 250), ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };

            Label lblC = new Label { Text = "Contenido de la entrega (Texto o Link):", Location = new Point(20, 320) };
            txtContenido = new TextBox { Location = new Point(20, 350), Size = new Size(600, 100), Multiline = true };

            btnEntregar = new Button { Text = "Enviar Entrega", Location = new Point(20, 470), Size = new Size(200, 40), BackColor = Color.Teal, ForeColor = Color.White };
            btnEntregar.Click += BtnEntregar_Click;

            this.Controls.AddRange(new Control[] { lblT, dgvTareasPendientes, lblC, txtContenido, btnEntregar });

            CargarTareas();
        }

        private void CargarTareas()
        {
            // Obtener cursos inscritos y luego buscar las tareas de cada curso
            var inscripciones = gestionService.ObtenerInscripcionesPorEstudiante(usuarioActual.Id);
            List<Tarea> todasLasTareas = new List<Tarea>();

            foreach (var ins in inscripciones)
            {
                var tareasCurso = gestionService.ObtenerTareasPorCurso(ins.CursoId);
                todasLasTareas.AddRange(tareasCurso);
            }

            dgvTareasPendientes.DataSource = todasLasTareas;
        }

        private void BtnEntregar_Click(object sender, EventArgs e)
        {
            if (dgvTareasPendientes.SelectedRows.Count == 0) { MessageBox.Show("Selecciona una tarea"); return; }
            if (string.IsNullOrWhiteSpace(txtContenido.Text)) { MessageBox.Show("Escribe contenido en la entrega"); return; }

            var tarea = (Tarea)dgvTareasPendientes.SelectedRows[0].DataBoundItem;

            var entrega = new EntregaTarea
            {
                TareaId = tarea.Id,
                EstudianteId = usuarioActual.Id,
                NombreEstudiante = usuarioActual.NombreCompleto,
                Contenido = txtContenido.Text,
                FechaEntrega = DateTime.Now
            };

            var res = gestionService.EntregarTarea(entrega);
            MessageBox.Show(res.mensaje);
            txtContenido.Clear();
        }
    }
}