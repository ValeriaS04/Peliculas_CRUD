using CRUD.Models;
using CRUD.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class Form1 : Form
    {
        PeliculaRepository repo = new PeliculaRepository();
        string peliculaIdSeleccionada;
        byte[] imagenSeleccionada;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            cmbGenero.Items.Add("Acción");
            cmbGenero.Items.Add("Comedia");
            cmbGenero.Items.Add("Drama");
            cmbGenero.Items.Add("Ciencia ficción");
            cmbGenero.Items.Add("Terror");
            cmbGenero.Items.Add("Animación");
            cmbGenero.Items.Add("Romance");

            cmbGenero.SelectedIndex = 0;

            CargarPeliculas();
            dgvPeliculas.ReadOnly = true;
            dgvPeliculas.AllowUserToAddRows = false;
            dgvPeliculas.AllowUserToDeleteRows = false;
            dgvPeliculas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPeliculas.MultiSelect = false;
            btnAgregar.MouseEnter += Boton_MouseEnter;
            btnAgregar.MouseLeave += Boton_MouseLeave;

            btnActualizar.MouseEnter += Boton_MouseEnter;
            btnActualizar.MouseLeave += Boton_MouseLeave;

            btnEliminar.MouseEnter += Boton_MouseEnter;
            btnEliminar.MouseLeave += Boton_MouseLeave;
            nudAnio.Value = nudAnio.Minimum;
            nudDuracion.Value = nudDuracion.Minimum;
            cmbGenero.SelectedIndex = -1;
            cmbGenero.Text = "Selecciona un género";
            cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;


        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            Pelicula p = new Pelicula()
            {
                Titulo = txtTitulo.Text,
                Genero = cmbGenero.SelectedItem.ToString(),
                Director = txtDirector.Text,
                Anio = (int)nudAnio.Value,
                Duracion = (int)nudDuracion.Value,
                Imagen = imagenSeleccionada
            };

            repo.Insertar(p);
            MessageBox.Show("Película guardada");
            CargarPeliculas();
            LimpiarCampos();
        }

        private void CargarPeliculas()
        {
            dgvPeliculas.DataSource = null;
            dgvPeliculas.DataSource = repo.ObtenerTodas();
            // Opcional: cambiar títulos de columnas
            dgvPeliculas.Columns["Titulo"].HeaderText = "Título";
            dgvPeliculas.Columns["Genero"].HeaderText = "Género";
            dgvPeliculas.Columns["Director"].HeaderText = "Director";
            dgvPeliculas.Columns["Anio"].HeaderText = "Año";
            dgvPeliculas.Columns["Duracion"].HeaderText = "Duración (min)";
            dgvPeliculas.Columns["Imagen"].Visible = false;
            

            dgvPeliculas.Columns["Id"].Visible = false;
            dgvPeliculas.Columns["Imagen"].Visible = false;

            if (dgvPeliculas.Rows.Count > 0)
            {
                dgvPeliculas.Rows[0].Selected = true;
                peliculaIdSeleccionada =
                    dgvPeliculas.Rows[0].Cells["Id"].Value.ToString();
            }
        }

        private void dgvPeliculas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                peliculaIdSeleccionada =
                    dgvPeliculas.Rows[e.RowIndex].Cells["Id"].Value.ToString();

                txtTitulo.Text = dgvPeliculas.Rows[e.RowIndex].Cells["Titulo"].Value.ToString();
                cmbGenero.SelectedItem = dgvPeliculas.Rows[e.RowIndex].Cells["Genero"].Value.ToString();
                txtDirector.Text = dgvPeliculas.Rows[e.RowIndex].Cells["Director"].Value.ToString();
                nudAnio.Value = Convert.ToInt32(dgvPeliculas.Rows[e.RowIndex].Cells["Anio"].Value);
                nudDuracion.Value = Convert.ToInt32(dgvPeliculas.Rows[e.RowIndex].Cells["Duracion"].Value);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            if (dgvPeliculas.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una película primero");
                return;
            }

            string id = dgvPeliculas.CurrentRow.Cells["Id"].Value.ToString();

            Pelicula p = new Pelicula()
            {
                Id = id,
                Titulo = txtTitulo.Text,
                Genero = cmbGenero.SelectedItem.ToString(),
                Director = txtDirector.Text,
                Anio = (int)nudAnio.Value,
                Duracion = (int)nudDuracion.Value,
                Imagen = imagenSeleccionada
            };

            repo.Actualizar(id, p);

            MessageBox.Show("Película actualizada");
            CargarPeliculas();
            LimpiarCampos();

           
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (dgvPeliculas.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una película primero");
                return;
            }

            string id = dgvPeliculas.CurrentRow.Cells["Id"].Value.ToString();

            repo.Eliminar(id);

            MessageBox.Show("Película eliminada");
            CargarPeliculas();
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtDirector.Clear();

            cmbGenero.SelectedIndex = -1;
            cmbGenero.Text = "Selecciona un género";
            cmbGenero.ForeColor = Color.Gray;

            nudAnio.Value = nudAnio.Minimum;
            nudDuracion.Value = nudDuracion.Minimum;

            pbImagen.Image = null;
            imagenSeleccionada = null;

            peliculaIdSeleccionada = null;

            dgvPeliculas.ClearSelection();
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            CargarPeliculas();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El título es obligatorio");
                txtTitulo.Focus();
                return false;
            }

            if (cmbGenero.SelectedIndex < 0)
            {
                MessageBox.Show("Selecciona un género");
                cmbGenero.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDirector.Text))
            {
                MessageBox.Show("El director es obligatorio");
                txtDirector.Focus();
                return false;
            }
            if (cmbGenero.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un género");
                cmbGenero.Focus();
                return false;
            }
            if (nudDuracion.Value <= 0)
            {
                MessageBox.Show("La duración debe ser mayor a 0");
                nudDuracion.Focus();
                return false;
            }
            if (imagenSeleccionada == null)
            {
                MessageBox.Show("Debes seleccionar una imagen para la película");
                return false;
            }

            return true;
        }

        private void dgvPeliculas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvPeliculas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dgvPeliculas.Rows[e.RowIndex];

            txtTitulo.Text = fila.Cells["Titulo"].Value.ToString();
            cmbGenero.Text = fila.Cells["Genero"].Value.ToString();
            txtDirector.Text = fila.Cells["Director"].Value.ToString();

         
            int anio = Convert.ToInt32(fila.Cells["Anio"].Value);

            if (anio < nudAnio.Minimum)
                nudAnio.Value = nudAnio.Minimum;
            else if (anio > nudAnio.Maximum)
                nudAnio.Value = nudAnio.Maximum;
            else
                nudAnio.Value = anio;

            nudDuracion.Value = Convert.ToDecimal(fila.Cells["Duracion"].Value);

            if (fila.Cells["Imagen"].Value != null)
            {
                byte[] imgBytes = (byte[])fila.Cells["Imagen"].Value;

                imagenSeleccionada = imgBytes; // 🔥 ESTA LÍNEA ES LA CLAVE

                using (MemoryStream ms = new MemoryStream(imgBytes))
                {
                    pbImagen.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pbImagen.Image = null;
                imagenSeleccionada = null;
            }
        }

        
        private void MostrarImagenSeleccionada()
        {
            if (dgvPeliculas.CurrentRow == null) return;

            byte[] img = dgvPeliculas.CurrentRow.Cells["Imagen"].Value as byte[];

            if (img != null)
            {
                using (MemoryStream ms = new MemoryStream(img))
                {
                    pbImagen.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pbImagen.Image = null;
            }
        }

        private void btnCargarImagen_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imágenes|*.jpg;*.png;*.jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbImagen.Image = Image.FromFile(ofd.FileName);
                imagenSeleccionada = File.ReadAllBytes(ofd.FileName);
            }
        }

        private void dgvPeliculas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPeliculas.CurrentRow == null) return;

            var fila = dgvPeliculas.CurrentRow;

            txtTitulo.Text = fila.Cells["Titulo"].Value?.ToString();
            cmbGenero.Text = fila.Cells["Genero"].Value?.ToString();
            txtDirector.Text = fila.Cells["Director"].Value?.ToString();

            nudAnio.Value = Convert.ToInt32(fila.Cells["Anio"].Value);
            nudDuracion.Value = Convert.ToDecimal(fila.Cells["Duracion"].Value);

            // Imagen
            byte[] imgBytes = fila.Cells["Imagen"].Value as byte[];

            if (imgBytes != null)
            {
                imagenSeleccionada = imgBytes;

                using (MemoryStream ms = new MemoryStream(imgBytes))
                {
                    pbImagen.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pbImagen.Image = null;
                imagenSeleccionada = null;
            }
        }

        private void Boton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(255, 128, 128); // color al pasar mouse
        }

        private void Boton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(255, 192, 192); // color original
        }

       

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
