using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;

namespace AutoMovilAppCliente
{
    public partial class FormBuscarElectrico : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtId;
        private DataGridView dgvResultado;

        public FormBuscarElectrico()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Buscar Automóvil Eléctrico";
            this.Size = new System.Drawing.Size(820, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelBusqueda = new FlowLayoutPanel();
            panelBusqueda.Dock = DockStyle.Top;
            panelBusqueda.Height = 50;
            panelBusqueda.Padding = new Padding(10);

            panelBusqueda.Controls.Add(new Label { Text = "ID:", AutoSize = true, Anchor = AnchorStyles.Left });
            txtId = new TextBox { Width = 200 };
            panelBusqueda.Controls.Add(txtId);

            var btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.BackColor = System.Drawing.Color.FromArgb(25, 70, 130);
            btnBuscar.ForeColor = System.Drawing.Color.White;
            btnBuscar.Click += BtnBuscar_Click;
            panelBusqueda.Controls.Add(btnBuscar);

            dgvResultado = new DataGridView();
            dgvResultado.Dock = DockStyle.Fill;
            dgvResultado.ReadOnly = true;
            dgvResultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResultado.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            this.Controls.Add(dgvResultado);
            this.Controls.Add(panelBusqueda);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Ingrese un ID."); return; }

            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/electricos/{id}", Method.Get);
            var response = client.Execute(request);

            dgvResultado.Rows.Clear();
            dgvResultado.Columns.Clear();

            if (!response.IsSuccessful || response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("No se encontró ningún automóvil con ID: " + id);
                return;
            }

            var auto = JsonSerializer.Deserialize<System.Text.Json.Nodes.JsonObject>(response.Content);

            dgvResultado.Columns.Add("id", "ID");
            dgvResultado.Columns.Add("marca", "Marca");
            dgvResultado.Columns.Add("modelo", "Modelo");
            dgvResultado.Columns.Add("anio", "Año");
            dgvResultado.Columns.Add("color", "Color");
            dgvResultado.Columns.Add("precio", "Precio");
            dgvResultado.Columns.Add("autonomia", "Autonomía (km)");
            dgvResultado.Columns.Add("tiempoCarga", "T. Carga (h)");

            dgvResultado.Rows.Add(
                auto["id"]?.ToString(),
                auto["marca"]?.ToString(),
                auto["modelo"]?.ToString(),
                auto["anio"]?.ToString(),
                auto["color"]?.ToString(),
                auto["precio"]?.ToString(),
                auto["autonomiaKm"]?.ToString(),
                auto["tiempoCargaHoras"]?.ToString()
            );
        }
    }
}