using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormEliminarGasolina : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtId;
        private DataGridView dgvResultado;

        public FormEliminarGasolina()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Eliminar Automóvil a Gasolina";
            this.Size = new System.Drawing.Size(820, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelBusqueda = new FlowLayoutPanel();
            panelBusqueda.Dock = DockStyle.Top;
            panelBusqueda.Height = 50;
            panelBusqueda.Padding = new Padding(10);

            panelBusqueda.Controls.Add(new Label { Text = "ID a eliminar:", AutoSize = true });
            txtId = new TextBox { Width = 200 };
            panelBusqueda.Controls.Add(txtId);

            var btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.BackColor = System.Drawing.Color.FromArgb(150, 50, 0);
            btnBuscar.ForeColor = System.Drawing.Color.White;
            btnBuscar.Click += BtnBuscar_Click;
            panelBusqueda.Controls.Add(btnBuscar);

            dgvResultado = new DataGridView();
            dgvResultado.Dock = DockStyle.Fill;
            dgvResultado.ReadOnly = true;
            dgvResultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var panelSur = new FlowLayoutPanel();
            panelSur.Dock = DockStyle.Bottom;
            panelSur.Height = 45;
            panelSur.FlowDirection = FlowDirection.RightToLeft;
            panelSur.Padding = new Padding(10);

            var btnEliminar = new Button();
            btnEliminar.Text = "Confirmar Eliminación";
            btnEliminar.BackColor = System.Drawing.Color.FromArgb(180, 0, 0);
            btnEliminar.ForeColor = System.Drawing.Color.White;
            btnEliminar.Click += BtnEliminar_Click;
            panelSur.Controls.Add(btnEliminar);

            this.Controls.Add(dgvResultado);
            this.Controls.Add(panelSur);
            this.Controls.Add(panelBusqueda);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Ingrese un ID."); return; }

            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/gasolina/{id}", Method.Get);
            var response = client.Execute(request);

            dgvResultado.Rows.Clear();
            dgvResultado.Columns.Clear();

            if (!response.IsSuccessful)
            {
                MessageBox.Show("No se encontró ningún automóvil con ID: " + id);
                return;
            }

            var auto = JsonSerializer.Deserialize<JsonObject>(response.Content);

            dgvResultado.Columns.Add("id", "ID");
            dgvResultado.Columns.Add("marca", "Marca");
            dgvResultado.Columns.Add("modelo", "Modelo");
            dgvResultado.Columns.Add("anio", "Año");
            dgvResultado.Columns.Add("color", "Color");
            dgvResultado.Columns.Add("precio", "Precio");
            dgvResultado.Columns.Add("cilindraje", "Cilindraje");
            dgvResultado.Columns.Add("combustible", "Combustible");
            dgvResultado.Columns.Add("transmision", "Transmisión");

            dgvResultado.Rows.Add(
                auto["id"]?.ToString(),
                auto["marca"]?.ToString(),
                auto["modelo"]?.ToString(),
                auto["anio"]?.ToString(),
                auto["color"]?.ToString(),
                auto["precio"]?.ToString(),
                auto["cilindraje"]?.ToString(),
                auto["tipoCombustible"]?.ToString(),
                auto["transmision"]?.ToString()
            );
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Busque primero un automóvil."); return; }

            var confirm = MessageBox.Show("¿Confirma la eliminación?", "Eliminar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var client = new RestClient(BASE_URL);
                var request = new RestRequest($"/gasolina/{id}", Method.Delete);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AutoObservable.GetInstancia().NotificarObservers();
                    txtId.Text = "";
                    dgvResultado.Rows.Clear();
                    dgvResultado.Columns.Clear();
                }
                else
                {
                    MessageBox.Show("Error al eliminar.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}