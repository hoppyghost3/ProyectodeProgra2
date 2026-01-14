using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public class FormGestionCursos : Form
    {
        private CursoService cursoService;
        private DocenteService docenteService;
        private DataGridView dgvCursos;
        private TextBox txtCodigo, txtNombre, txtDescripcion, txtHorario, txtCreditos, txtCupoMaximo;
        private ComboBox cboDocente;
        private DateTimePicker dtpInicio, dtpFin;
        private Button btnNuevo, btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private Curso cursoActual;
        private bool modoEdicion = false;

        public FormGestionCursos()
        {
            cursoService = new CursoService();
            docenteService = new DocenteService();
            InicializarComponentes();
            CargarCursos();
            CargarDocentes();
            BloquearCampos();
        }

        private void InicializarComponentes()
        {
            this.Text = "Gestión de Cursos";
            this.Size = new Size(1100, 650);
            this.BackColor = Color.White;

            // Título
            Label lblTitulo = new Label
            {
                Text = "GESTIÓN DE CURSOS",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitulo);

            // Panel de formulario
            Panel panelForm = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(400, 500),
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle
            };

            int y = 20;

            // Código
            Label lblCodigo = new Label { Text = "Código:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtCodigo = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblCodigo, txtCodigo });
            y += 35;

            // Nombre
            Label lblNombre = new Label { Text = "Nombre:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtNombre = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblNombre, txtNombre });
            y += 35;

            // Descripción
            Label lblDesc = new Label { Text = "Descripción:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtDescripcion = new TextBox { Location = new Point(130, y), Size = new Size(240, 60), Multiline = true };
            panelForm.Controls.AddRange(new Control[] { lblDesc, txtDescripcion });
            y += 75;

            // Docente
            Label lblDocente = new Label { Text = "Docente:", Location = new Point(20, y), Size = new Size(100, 20) };
            cboDocente = new ComboBox { Location = new Point(130, y), Size = new Size(240, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            panelForm.Controls.AddRange(new Control[] { lblDocente, cboDocente });
            y += 35;

            // Horario
            Label lblHorario = new Label { Text = "Horario:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtHorario = new TextBox { Location = new Point(130, y), Size = new Size(240, 25), PlaceholderText = "Ej: Lun-Mie-Vie 10:00-12:00" };
            panelForm.Controls.AddRange(new Control[] { lblHorario, txtHorario });
            y += 35;

            // Créditos
            Label lblCreditos = new Label { Text = "Créditos:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtCreditos = new TextBox { Location = new Point(130, y), Size = new Size(100, 25) };
            panelForm.Controls.AddRange(new Control[] { lblCreditos, txtCreditos });
            y += 35;

            // Cupo
            Label lblCupo = new Label { Text = "Cupo Máximo:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtCupoMaximo = new TextBox { Location = new Point(130, y), Size = new Size(100, 25) };
            panelForm.Controls.AddRange(new Control[] { lblCupo, txtCupoMaximo });
            y += 35;

            // Fecha Inicio
            Label lblInicio = new Label { Text = "Fecha Inicio:", Location = new Point(20, y), Size = new Size(100, 20) };
            dtpInicio = new DateTimePicker { Location = new Point(130, y), Size = new Size(240, 25), Format = DateTimePickerFormat.Short };
            panelForm.Controls.AddRange(new Control[] { lblInicio, dtpInicio });
            y += 35;

            // Fecha Fin
            Label lblFin = new Label { Text = "Fecha Fin:", Location = new Point(20, y), Size = new Size(100, 20) };
            dtpFin = new DateTimePicker { Location = new Point(130, y), Size = new Size(240, 25), Format = DateTimePickerFormat.Short };
            panelForm.Controls.AddRange(new Control[] { lblFin, dtpFin });
            y += 45;

            // Botones
            btnNuevo = CrearBoton("Nuevo", new Point(20, y), Color.FromArgb(46, 204, 113));
            btnGuardar = CrearBoton("Guardar", new Point(110, y), Color.FromArgb(52, 152, 219));
            btnCancelar = CrearBoton("Cancelar", new Point(200, y), Color.FromArgb(231, 76, 60));

            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            panelForm.Controls.AddRange(new Control[] { btnNuevo, btnGuardar, btnCancelar });

            this.Controls.Add(panelForm);

            // DataGridView
            dgvCursos = new DataGridView
            {
                Location = new Point(440, 70),
                Size = new Size(630, 400),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvCursos.SelectionChanged += DgvCursos_SelectionChanged;
            this.Controls.Add(dgvCursos);

            // Botones de acción del grid
            btnEditar = CrearBoton("Editar Seleccionado", new Point(440, 490), Color.FromArgb(241, 196, 15));
            btnEliminar = CrearBoton("Eliminar Seleccionado", new Point(590, 490), Color.FromArgb(192, 57, 43));

            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            this.Controls.AddRange(new Control[] { btnEditar, btnEliminar });
        }

        private Button CrearBoton(string texto, Point ubicacion, Color color)
        {
            return new Button
            {
                Text = texto,
                Location = ubicacion,
                Size = new Size(130, 35),
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private void CargarDocentes()
        {
            try
            {
                var docentes = docenteService.ObtenerTodos();
                cboDocente.DisplayMember = "NombreCompleto";
                cboDocente.ValueMember = "Id";
                cboDocente.DataSource = docentes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar docentes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarCursos()
        {
            try
            {
                var cursos = cursoService.ObtenerCursos();
                dgvCursos.DataSource = null;
                dgvCursos.DataSource = cursos;

                if (dgvCursos.Columns.Count > 0)
                {
                    dgvCursos.Columns["Id"].Visible = false;
                    dgvCursos.Columns["ProfesorId"].Visible = false;
                    dgvCursos.Columns["EstudiantesInscritos"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar cursos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            DesbloquearCampos();
            modoEdicion = false;
            cursoActual = null;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos())
                    return;

                if (cboDocente.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un docente", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var docente = (Docente)cboDocente.SelectedItem;

                if (modoEdicion && cursoActual != null)
                {
                    // Editar
                    cursoActual.Codigo = txtCodigo.Text.Trim();
                    cursoActual.Nombre = txtNombre.Text.Trim();
                    cursoActual.Descripcion = txtDescripcion.Text.Trim();
                    cursoActual.ProfesorId = (int)cboDocente.SelectedValue;
                    cursoActual.NombreProfesor = docente.NombreCompleto;
                    cursoActual.Horario = txtHorario.Text.Trim();
                    cursoActual.Creditos = int.Parse(txtCreditos.Text);
                    cursoActual.CupoMaximo = int.Parse(txtCupoMaximo.Text);
                    cursoActual.FechaInicio = dtpInicio.Value;
                    cursoActual.FechaFin = dtpFin.Value;

                    cursoService.ModificarCurso(cursoActual);
                    MessageBox.Show("Curso actualizado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Nuevo
                    var nuevoCurso = new Curso
                    {
                        Codigo = txtCodigo.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        ProfesorId = (int)cboDocente.SelectedValue,
                        NombreProfesor = docente.NombreCompleto,
                        Horario = txtHorario.Text.Trim(),
                        Creditos = int.Parse(txtCreditos.Text),
                        CupoMaximo = int.Parse(txtCupoMaximo.Text),
                        CupoDisponible = int.Parse(txtCupoMaximo.Text),
                        FechaInicio = dtpInicio.Value,
                        FechaFin = dtpFin.Value,
                        Activo = true
                    };

                    cursoService.AgregarCurso(nuevoCurso);
                    MessageBox.Show("Curso creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarCursos();
                LimpiarCampos();
                BloquearCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCursos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un curso para editar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            cursoActual = (Curso)dgvCursos.SelectedRows[0].DataBoundItem;
            CargarDatosCurso(cursoActual);
            DesbloquearCampos();
            modoEdicion = true;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCursos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un curso para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var curso = (Curso)dgvCursos.SelectedRows[0].DataBoundItem;
            var resultado = MessageBox.Show(
                $"¿Está seguro de eliminar el curso '{curso.Nombre}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    cursoService.EliminarCurso(curso.Id);
                    MessageBox.Show("Curso eliminado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarCursos();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            BloquearCampos();
            modoEdicion = false;
            cursoActual = null;
        }

        private void DgvCursos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCursos.SelectedRows.Count > 0)
            {
                var curso = (Curso)dgvCursos.SelectedRows[0].DataBoundItem;
                // Mostrar información adicional si deseas
            }
        }

        private void CargarDatosCurso(Curso curso)
        {
            txtCodigo.Text = curso.Codigo;
            txtNombre.Text = curso.Nombre;
            txtDescripcion.Text = curso.Descripcion;
            cboDocente.SelectedValue = curso.ProfesorId;
            txtHorario.Text = curso.Horario;
            txtCreditos.Text = curso.Creditos.ToString();
            txtCupoMaximo.Text = curso.CupoMaximo.ToString();
            dtpInicio.Value = curso.FechaInicio;
            dtpFin.Value = curso.FechaFin;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El código es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtCreditos.Text, out int creditos) || creditos <= 0)
            {
                MessageBox.Show("Los créditos deben ser un número positivo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtCupoMaximo.Text, out int cupo) || cupo <= 0)
            {
                MessageBox.Show("El cupo debe ser un número positivo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpFin.Value <= dtpInicio.Value)
            {
                MessageBox.Show("La fecha de fin debe ser posterior a la fecha de inicio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtHorario.Clear();
            txtCreditos.Clear();
            txtCupoMaximo.Clear();
            dtpInicio.Value = DateTime.Now;
            dtpFin.Value = DateTime.Now.AddMonths(4);
            if (cboDocente.Items.Count > 0)
                cboDocente.SelectedIndex = 0;
        }

        private void BloquearCampos()
        {
            txtCodigo.Enabled = false;
            txtNombre.Enabled = false;
            txtDescripcion.Enabled = false;
            txtHorario.Enabled = false;
            txtCreditos.Enabled = false;
            txtCupoMaximo.Enabled = false;
            cboDocente.Enabled = false;
            dtpInicio.Enabled = false;
            dtpFin.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtCodigo.Enabled = true;
            txtNombre.Enabled = true;
            txtDescripcion.Enabled = true;
            txtHorario.Enabled = true;
            txtCreditos.Enabled = true;
            txtCupoMaximo.Enabled = true;
            cboDocente.Enabled = true;
            dtpInicio.Enabled = true;
            dtpFin.Enabled = true;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
        }
    }
}