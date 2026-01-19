using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoDeProgramacion2
{
    public partial class FormReportes : Form
    {
        // Servicios para obtener los datos
        private EstudianteService estudianteService;
        private DocenteService docenteService;
        private CursoService cursoService;
        private AutenticacionService authService;

        public FormReportes()
        {
            // Instanciar servicios
            estudianteService = new EstudianteService();
            docenteService = new DocenteService();
            cursoService = new CursoService();
            authService = new AutenticacionService();

            InicializarComponentesManual();
            CargarEstadisticas();
        }

        private void InicializarComponentesManual()
        {
            this.Text = "Reportes y Estadísticas";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.WhiteSmoke;

            Label lblTitulo = new Label
            {
                Text = "DASHBOARD DE ESTADÍSTICAS",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitulo);

            // Contenedor para las tarjetas
            FlowLayoutPanel panelTarjetas = new FlowLayoutPanel
            {
                Location = new Point(30, 80),
                Size = new Size(940, 450),
                AutoScroll = true
            };
            this.Controls.Add(panelTarjetas);

            // Crear tarjetas (Labels que llenaremos luego)
            // 1. Estudiantes
            panelTarjetas.Controls.Add(CrearTarjeta("Total Estudiantes", "lblTotalEstudiantes", Color.FromArgb(46, 204, 113))); // Verde
            // 2. Docentes
            panelTarjetas.Controls.Add(CrearTarjeta("Total Docentes", "lblTotalDocentes", Color.FromArgb(52, 152, 219))); // Azul
            // 3. Cursos
            panelTarjetas.Controls.Add(CrearTarjeta("Cursos Creados", "lblTotalCursos", Color.FromArgb(155, 89, 182))); // Morado
            // 4. Usuarios Sistema
            panelTarjetas.Controls.Add(CrearTarjeta("Usuarios Sistema", "lblTotalUsuarios", Color.FromArgb(230, 126, 34))); // Naranja
        }

        private Panel CrearTarjeta(string titulo, string nombreLabelValor, Color colorFondo)
        {
            Panel panel = new Panel
            {
                Size = new Size(220, 150),
                BackColor = colorFondo,
                Margin = new Padding(10)
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblValor = new Label
            {
                Name = nombreLabelValor, // Importante para encontrarlo después
                Text = "Cargando...",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                Location = new Point(10, 50),
                AutoSize = true
            };

            panel.Controls.Add(lblTitulo);
            panel.Controls.Add(lblValor);

            return panel;
        }

        private void CargarEstadisticas()
        {
            try
            {
                // Obtener conteos usando los servicios existentes
                int cantEstudiantes = estudianteService.ListarEstudiantes().Count;
                int cantDocentes = docenteService.ObtenerTodos().Count;
                int cantCursos = cursoService.ObtenerCursos().Count;
                int cantUsuarios = authService.ObtenerTodosLosUsuarios().Count;

                // Buscar los labels por nombre y actualizar su texto
                ActualizarValorTarjeta("lblTotalEstudiantes", cantEstudiantes.ToString());
                ActualizarValorTarjeta("lblTotalDocentes", cantDocentes.ToString());
                ActualizarValorTarjeta("lblTotalCursos", cantCursos.ToString());
                ActualizarValorTarjeta("lblTotalUsuarios", cantUsuarios.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar estadísticas: " + ex.Message);
            }
        }

        private void ActualizarValorTarjeta(string nombreLabel, string valor)
        {
            var controles = this.Controls[1].Controls.Find(nombreLabel, true);
            if (controles.Length > 0)
            {
                controles[0].Text = valor;
            }
        }
    }
}