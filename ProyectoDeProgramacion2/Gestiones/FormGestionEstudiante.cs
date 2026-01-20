using CapaEntidad;
using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2.Gestion
{
    public partial class FormGestionEstudiantes : Form
    {
        private EstudianteService estudianteService;
        private DataGridView dgvEstudiantes;
        private TextBox txtNombre, txtApellido, txtEmail, txtTelefono, txtCarrera;
        private NumericUpDown numSemestre;
        private Button btnNuevo, btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private Estudiante estudianteActual;
        private bool modoEdicion = false;

        public FormGestionEstudiantes()
        {
            estudianteService = new EstudianteService();
            InicializarComponentes();
            CargarEstudiantes();
            BloquearCampos();
        }

        private void InicializarComponentes()
        {
            Text = "Gestión de Estudiantes";
            Size = new Size(1100, 600);
            BackColor = Color.White;

            // Título
            Label lblTitulo = new Label
            {
                Text = "GESTIÓN DE ESTUDIANTES",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 20),
                AutoSize = true
            };
            Controls.Add(lblTitulo);

            // Panel de formulario
            Panel panelForm = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(400, 450),
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle
            };

            int y = 20;

            // Nombre
            Label lblNombre = new Label { Text = "Nombre:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtNombre = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblNombre, txtNombre });
            y += 40;

            // Apellido
            Label lblApellido = new Label { Text = "Apellido:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtApellido = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblApellido, txtApellido });
            y += 40;

            // Email
            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtEmail = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblEmail, txtEmail });
            y += 40;

            // Teléfono
            Label lblTelefono = new Label { Text = "Teléfono:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtTelefono = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblTelefono, txtTelefono });
            y += 40;

            // Carrera
            Label lblCarrera = new Label { Text = "Carrera:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtCarrera = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblCarrera, txtCarrera });
            y += 40;

            // Semestre
            Label lblSemestre = new Label { Text = "Semestre:", Location = new Point(20, y), Size = new Size(100, 20) };
            numSemestre = new NumericUpDown
            {
                Location = new Point(130, y),
                Size = new Size(100, 25),
                Minimum = 1,
                Maximum = 12,
                Value = 1
            };
            panelForm.Controls.AddRange(new Control[] { lblSemestre, numSemestre });
            y += 50;

            // Botones
            btnNuevo = CrearBoton("Nuevo", new Point(20, y), Color.FromArgb(46, 204, 113));
            btnGuardar = CrearBoton("Guardar", new Point(110, y), Color.FromArgb(52, 152, 219));
            btnCancelar = CrearBoton("Cancelar", new Point(200, y), Color.FromArgb(231, 76, 60));

            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            panelForm.Controls.AddRange(new Control[] { btnNuevo, btnGuardar, btnCancelar });
            Controls.Add(panelForm);

            // DataGridView
            dgvEstudiantes = new DataGridView
            {
                Location = new Point(440, 70),
                Size = new Size(630, 350),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            Controls.Add(dgvEstudiantes);

            // Botones de acción
            btnEditar = CrearBoton("Editar Seleccionado", new Point(440, 440), Color.FromArgb(241, 196, 15));
            btnEliminar = CrearBoton("Eliminar Seleccionado", new Point(590, 440), Color.FromArgb(192, 57, 43));

            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            Controls.AddRange(new Control[] { btnEditar, btnEliminar });
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

        private void CargarEstudiantes()
        {
            try
            {
                var estudiantes = estudianteService.ListarEstudiantes();
                dgvEstudiantes.DataSource = null;
                dgvEstudiantes.DataSource = estudiantes;

                if (dgvEstudiantes.Columns.Count > 0)
                {
                    // Ocultar ID si existe
                    if (dgvEstudiantes.Columns["Id"] != null)
                        dgvEstudiantes.Columns["Id"].Visible = false;

                    // CORRECCIÓN: Verificar si existe la columna de la lista antes de ocultarla
                    if (dgvEstudiantes.Columns.Contains("CursosInscritos"))
                    {
                        dgvEstudiantes.Columns["CursosInscritos"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar estudiantes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            DesbloquearCampos();
            modoEdicion = false;
            estudianteActual = null;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos())
                    return;

                if (modoEdicion && estudianteActual != null)
                {
                    estudianteActual.Nombre = txtNombre.Text.Trim();
                    estudianteActual.Apellido = txtApellido.Text.Trim();
                    estudianteActual.Email = txtEmail.Text.Trim();
                    estudianteActual.Telefono = txtTelefono.Text.Trim();
                    estudianteActual.Carrera = txtCarrera.Text.Trim();
                    estudianteActual.Semestre = (int)numSemestre.Value;

                    estudianteService.ModificarEstudiante(estudianteActual);
                    MessageBox.Show("Estudiante actualizado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var nuevoEstudiante = new Estudiante
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Telefono = txtTelefono.Text.Trim(),
                        Carrera = txtCarrera.Text.Trim(),
                        Semestre = (int)numSemestre.Value,
                        FechaRegistro = DateTime.Now,
                        Activo = true
                    };

                    estudianteService.AgregarEstudiante(nuevoEstudiante);
                    MessageBox.Show("Estudiante registrado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarEstudiantes();
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
            if (dgvEstudiantes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un estudiante para editar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            estudianteActual = (Estudiante)dgvEstudiantes.SelectedRows[0].DataBoundItem;
            txtNombre.Text = estudianteActual.Nombre;
            txtApellido.Text = estudianteActual.Apellido;
            txtEmail.Text = estudianteActual.Email;
            txtTelefono.Text = estudianteActual.Telefono;
            txtCarrera.Text = estudianteActual.Carrera;
            numSemestre.Value = estudianteActual.Semestre;

            DesbloquearCampos();
            modoEdicion = true;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un estudiante para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var estudiante = (Estudiante)dgvEstudiantes.SelectedRows[0].DataBoundItem;
            var resultado = MessageBox.Show(
                $"¿Está seguro de eliminar al estudiante '{estudiante.NombreCompleto}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    estudianteService.EliminarEstudiante(estudiante.Id);
                    MessageBox.Show("Estudiante eliminado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarEstudiantes();
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
            estudianteActual = null;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("El email es inválido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtCarrera.Clear();
            numSemestre.Value = 1;
        }

        private void BloquearCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtEmail.Enabled = false;
            txtTelefono.Enabled = false;
            txtCarrera.Enabled = false;
            numSemestre.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtEmail.Enabled = true;
            txtTelefono.Enabled = true;
            txtCarrera.Enabled = true;
            numSemestre.Enabled = true;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
        }
    }
}