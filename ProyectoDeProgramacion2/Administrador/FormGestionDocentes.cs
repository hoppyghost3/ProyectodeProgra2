using CapaEntidad.EntidadPersona;
using CapaNegocio.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormGestionDocentes : Form
    {
        private DocenteService docenteService;
        private DataGridView dgvDocentes;
        private TextBox txtNombre, txtApellido, txtEmail, txtTelefono, txtEspecialidad, txtDepartamento;
        private Button btnNuevo, btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private Docente docenteActual;
        private bool modoEdicion = false;

        public FormGestionDocentes()
        {
            docenteService = new DocenteService();
            InicializarComponentes();
            CargarDocentes();
            BloquearCampos();
        }

        private void InicializarComponentes()
        {
            this.Text = "Gestión de Docentes";
            this.Size = new Size(1100, 600);
            this.BackColor = Color.White;

            // Título
            Label lblTitulo = new Label
            {
                Text = "GESTIÓN DE DOCENTES",
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

            // Especialidad
            Label lblEspecialidad = new Label { Text = "Especialidad:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtEspecialidad = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblEspecialidad, txtEspecialidad });
            y += 40;

            // Departamento
            Label lblDepartamento = new Label { Text = "Departamento:", Location = new Point(20, y), Size = new Size(100, 20) };
            txtDepartamento = new TextBox { Location = new Point(130, y), Size = new Size(240, 25) };
            panelForm.Controls.AddRange(new Control[] { lblDepartamento, txtDepartamento });
            y += 50;

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
            dgvDocentes = new DataGridView
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
            this.Controls.Add(dgvDocentes);

            // Botones de acción
            btnEditar = CrearBoton("Editar Seleccionado", new Point(440, 440), Color.FromArgb(241, 196, 15));
            btnEliminar = CrearBoton("Eliminar Seleccionado", new Point(590, 440), Color.FromArgb(192, 57, 43));

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
                dgvDocentes.DataSource = null;
                dgvDocentes.DataSource = docentes;

                if (dgvDocentes.Columns.Count > 0)
                {
                    if (dgvDocentes.Columns["Id"] != null)
                        dgvDocentes.Columns["Id"].Visible = false;

                    if (dgvDocentes.Columns.Contains("CursosAsignados"))
                    {
                        dgvDocentes.Columns["CursosAsignados"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar docentes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            DesbloquearCampos();
            modoEdicion = false;
            docenteActual = null;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos())
                    return;

                if (modoEdicion && docenteActual != null)
                {
                    docenteActual.Nombre = txtNombre.Text.Trim();
                    docenteActual.Apellido = txtApellido.Text.Trim();
                    docenteActual.Email = txtEmail.Text.Trim();
                    docenteActual.Telefono = txtTelefono.Text.Trim();
                    docenteActual.Especialidad = txtEspecialidad.Text.Trim();
                    docenteActual.Departamento = txtDepartamento.Text.Trim();

                    docenteService.ActualizarDocente(docenteActual);
                    MessageBox.Show("Docente actualizado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var nuevoDocente = new Docente
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Telefono = txtTelefono.Text.Trim(),
                        Especialidad = txtEspecialidad.Text.Trim(),
                        Departamento = txtDepartamento.Text.Trim(),
                        FechaContratacion = DateTime.Now,
                        Activo = true
                    };

                    docenteService.RegistrarDocente(nuevoDocente);
                    MessageBox.Show("Docente registrado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarDocentes();
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
            if (dgvDocentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un docente para editar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            docenteActual = (Docente)dgvDocentes.SelectedRows[0].DataBoundItem;
            txtNombre.Text = docenteActual.Nombre;
            txtApellido.Text = docenteActual.Apellido;
            txtEmail.Text = docenteActual.Email;
            txtTelefono.Text = docenteActual.Telefono;
            txtEspecialidad.Text = docenteActual.Especialidad;
            txtDepartamento.Text = docenteActual.Departamento;

            DesbloquearCampos();
            modoEdicion = true;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDocentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un docente para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var docente = (Docente)dgvDocentes.SelectedRows[0].DataBoundItem;
            var resultado = MessageBox.Show(
                $"¿Está seguro de eliminar al docente '{docente.NombreCompleto}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    docenteService.EliminarDocente(docente.Id);
                    MessageBox.Show("Docente eliminado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDocentes();
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
            docenteActual = null;
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
            txtEspecialidad.Clear();
            txtDepartamento.Clear();
        }

        private void BloquearCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtEmail.Enabled = false;
            txtTelefono.Enabled = false;
            txtEspecialidad.Enabled = false;
            txtDepartamento.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtEmail.Enabled = true;
            txtTelefono.Enabled = true;
            txtEspecialidad.Enabled = true;
            txtDepartamento.Enabled = true;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
        }
    }
}