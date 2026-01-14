using CapaEntidad;
using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormGestionTareas : Form
    {
        private Usuario usuarioActual;
        private GestionAcademicaService gestionService;
        private CursoService cursoService;

        private ComboBox cboCursos;
        private TabControl tabControl;

        // Controles Crear Tarea
        private TextBox txtTitulo, txtDesc, txtPuntaje;
        private DateTimePicker dtpEntrega;
        private Button btnCrearTarea;

        // Controles Calificar
        private DataGridView dgvTareas, dgvEntregas;
        private TextBox txtNotaEntrega, txtRetro;
        private Button btnCalificarEntrega;

        public FormGestionTareas(Usuario usuario)
        {
            usuarioActual = usuario;
            gestionService = new GestionAcademicaService();
            cursoService = new CursoService();
            InicializarComponentesManual();
            CargarCursos();
        }

        private void InicializarComponentesManual()
        {
            this.Size = new Size(1000, 700);
            this.Text = "Gestión de Tareas";

            Label lblCurso = new Label { Text = "Curso:", Location = new Point(20, 15), AutoSize = true };
            cboCursos = new ComboBox { Location = new Point(70, 12), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboCursos.SelectedIndexChanged += (s, e) => CargarTareas();

            tabControl = new TabControl { Location = new Point(20, 50), Size = new Size(940, 600) };

            // TAB 1: Crear Tarea
            TabPage tabCrear = new TabPage("Crear Nueva Tarea");
            txtTitulo = new TextBox { Location = new Point(100, 30), Size = new Size(300, 25) };
            txtDesc = new TextBox { Location = new Point(100, 70), Size = new Size(300, 100), Multiline = true };
            dtpEntrega = new DateTimePicker { Location = new Point(100, 190), Size = new Size(300, 25) };
            txtPuntaje = new TextBox { Location = new Point(100, 230), Size = new Size(100, 25) };
            btnCrearTarea = new Button { Text = "Crear Tarea", Location = new Point(100, 270), Size = new Size(150, 40) };
            btnCrearTarea.Click += BtnCrearTarea_Click;

            tabCrear.Controls.Add(new Label { Text = "Título:", Location = new Point(20, 30) });
            tabCrear.Controls.Add(txtTitulo);
            tabCrear.Controls.Add(new Label { Text = "Descripción:", Location = new Point(20, 70) });
            tabCrear.Controls.Add(txtDesc);
            tabCrear.Controls.Add(new Label { Text = "Fecha Entrega:", Location = new Point(20, 190) });
            tabCrear.Controls.Add(dtpEntrega);
            tabCrear.Controls.Add(new Label { Text = "Puntaje Max:", Location = new Point(20, 230) });
            tabCrear.Controls.Add(txtPuntaje);
            tabCrear.Controls.Add(btnCrearTarea);

            // TAB 2: Calificar Entregas
            TabPage tabCalificar = new TabPage("Calificar Entregas");
            dgvTareas = new DataGridView { Location = new Point(20, 20), Size = new Size(400, 250), ReadOnly = true };
            dgvTareas.SelectionChanged += (s, e) => CargarEntregas();

            dgvEntregas = new DataGridView { Location = new Point(20, 290), Size = new Size(400, 250), ReadOnly = true };

            txtNotaEntrega = new TextBox { Location = new Point(450, 310), Size = new Size(100, 25) };
            txtRetro = new TextBox { Location = new Point(450, 360), Size = new Size(300, 100), Multiline = true };
            btnCalificarEntrega = new Button { Text = "Calificar", Location = new Point(450, 480), Size = new Size(150, 40) };
            btnCalificarEntrega.Click += BtnCalificarEntrega_Click;

            tabCalificar.Controls.Add(new Label { Text = "Selecciona Tarea:", Location = new Point(20, 5) });
            tabCalificar.Controls.Add(dgvTareas);
            tabCalificar.Controls.Add(new Label { Text = "Entregas Recibidas:", Location = new Point(20, 275) });
            tabCalificar.Controls.Add(dgvEntregas);
            tabCalificar.Controls.Add(new Label { Text = "Nota:", Location = new Point(450, 290) });
            tabCalificar.Controls.Add(txtNotaEntrega);
            tabCalificar.Controls.Add(new Label { Text = "Retroalimentación:", Location = new Point(450, 340) });
            tabCalificar.Controls.Add(txtRetro);
            tabCalificar.Controls.Add(btnCalificarEntrega);

            tabControl.TabPages.Add(tabCrear);
            tabControl.TabPages.Add(tabCalificar);

            this.Controls.Add(lblCurso);
            this.Controls.Add(cboCursos);
            this.Controls.Add(tabControl);
        }

        private void CargarCursos()
        {
            cboCursos.DataSource = cursoService.ObtenerCursosPorDocente(usuarioActual.Id);
            cboCursos.DisplayMember = "Nombre";
            cboCursos.ValueMember = "Id";
        }

        private void CargarTareas()
        {
            if (cboCursos.SelectedValue == null) return;
            dgvTareas.DataSource = gestionService.ObtenerTareasPorCurso((int)cboCursos.SelectedValue);
        }

        private void CargarEntregas()
        {
            if (dgvTareas.SelectedRows.Count == 0) return;
            var tarea = (Tarea)dgvTareas.SelectedRows[0].DataBoundItem;
            dgvEntregas.DataSource = gestionService.ObtenerEntregasPorTarea(tarea.Id);
        }

        private void BtnCrearTarea_Click(object sender, EventArgs e)
        {
            if (cboCursos.SelectedItem == null) return;
            var curso = (Curso)cboCursos.SelectedItem;

            var tarea = new Tarea
            {
                CursoId = curso.Id,
                NombreCurso = curso.Nombre,
                Titulo = txtTitulo.Text,
                Descripcion = txtDesc.Text,
                FechaEntrega = dtpEntrega.Value,
                PuntajeMaximo = int.Parse(txtPuntaje.Text)
            };

            var res = gestionService.CrearTarea(tarea);
            MessageBox.Show(res.mensaje);
            CargarTareas();
        }

        private void BtnCalificarEntrega_Click(object sender, EventArgs e)
        {
            if (dgvEntregas.SelectedRows.Count == 0) return;
            var entrega = (EntregaTarea)dgvEntregas.SelectedRows[0].DataBoundItem;

            if (double.TryParse(txtNotaEntrega.Text, out double nota))
            {
                var res = gestionService.CalificarTarea(entrega.Id, nota, txtRetro.Text);
                MessageBox.Show(res.mensaje);
                CargarEntregas();
            }
        }
    }
}