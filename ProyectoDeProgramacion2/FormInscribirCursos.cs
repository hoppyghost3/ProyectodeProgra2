using CapaEntidad;
using CapaNegocio;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormInscribirCursos : Form
    {
        private Usuario usuarioActual;
        private CursoService cursoService;
        private GestionAcademicaService gestionService;

        // Controles del formulario
        private DataGridView dgvCursosDisponibles;
        private Button btnInscribir;
        private Label lblTitulo;
        private Label lblInfo;

        public FormInscribirCursos(Usuario usuario)
        {
            // Inicializar servicios y usuario
            usuarioActual = usuario;
            cursoService = new CursoService();
            gestionService = new GestionAcademicaService();

            // Configurar la interfaz gráfica manualmente
            InicializarComponentesManual();

            // Cargar datos
            CargarCursos();
        }

        private void InicializarComponentesManual()
        {
            // Configuración básica del Form
            this.Text = "Inscripción de Cursos";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Título
            lblTitulo = new Label
            {
                Text = "CATÁLOGO DE CURSOS DISPONIBLES",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185), // Un azul agradable
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Etiqueta de información
            lblInfo = new Label
            {
                Text = "Seleccione un curso de la lista para inscribirse. Solo se muestran cursos con cupo disponible.",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(22, 55),
                AutoSize = true
            };

            // Tabla (DataGridView)
            dgvCursosDisponibles = new DataGridView
            {
                Location = new Point(20, 90),
                Size = new Size(840, 380),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };

            // Botón de Inscribir
            btnInscribir = new Button
            {
                Text = "Inscribirme al curso seleccionado",
                Location = new Point(20, 490),
                Size = new Size(250, 45),
                BackColor = Color.FromArgb(46, 204, 113), // Verde esmeralda
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnInscribir.FlatAppearance.BorderSize = 0;
            btnInscribir.Click += BtnInscribir_Click;

            // Agregar controles al formulario
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblInfo);
            this.Controls.Add(dgvCursosDisponibles);
            this.Controls.Add(btnInscribir);
        }

        private void CargarCursos()
        {
            try
            {
                // Obtener todos los cursos
                var todosLosCursos = cursoService.ObtenerCursos();

                // Filtrar: Solo mostrar cursos Activos y que tengan Cupo > 0
                // (Usamos Linq para filtrar fácilmente)
                var cursosDisponibles = todosLosCursos
                                        .Where(c => c.Activo && c.CupoDisponible > 0)
                                        .ToList();

                dgvCursosDisponibles.DataSource = null;
                dgvCursosDisponibles.DataSource = cursosDisponibles;

                // Ocultar columnas que no interesan al estudiante para que se vea más limpio
                if (dgvCursosDisponibles.Columns["EstudiantesInscritos"] != null)
                    dgvCursosDisponibles.Columns["EstudiantesInscritos"].Visible = false;

                if (dgvCursosDisponibles.Columns["Activo"] != null)
                    dgvCursosDisponibles.Columns["Activo"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar cursos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnInscribir_Click(object sender, EventArgs e)
        {
            // Validar que haya una fila seleccionada
            if (dgvCursosDisponibles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un curso de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el objeto Curso de la fila seleccionada
            var cursoSeleccionado = (Curso)dgvCursosDisponibles.SelectedRows[0].DataBoundItem;

            // Confirmación opcional
            var confirmacion = MessageBox.Show(
                $"¿Desea inscribirse en el curso '{cursoSeleccionado.Nombre}'?",
                "Confirmar Inscripción",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                // Llamar al servicio de negocio para procesar la inscripción
                var resultado = gestionService.InscribirEstudianteACurso(usuarioActual.Id, cursoSeleccionado.Id);

                if (resultado.exito)
                {
                    MessageBox.Show(resultado.mensaje, "¡Inscripción Exitosa!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recargar la lista para actualizar los cupos disponibles
                    CargarCursos();
                }
                else
                {
                    MessageBox.Show("No se pudo realizar la inscripción: " + resultado.mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}