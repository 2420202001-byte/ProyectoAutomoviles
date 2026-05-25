using System;
using System.Drawing;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;

namespace AutoMovilAppCliente
{
    public partial class FormBuscarElectrico : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        // Paleta Eléctrico (Azul)
        private readonly Color colorPrimario = Color.FromArgb(30, 58, 138);
        private readonly Color colorAcento = Color.FromArgb(56, 189, 248);
        private readonly Color colorFondo = Color.FromArgb(240, 249, 255);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(15, 23, 42);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);

        private TextBox txtId;
        private DataGridView dgvResultado;

        public FormBuscarElectrico()
        {
            CrearFormulario();
            this.BackColor = colorFondo;
            this.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void CrearFormulario()
        {
            this.Text = "Buscar Automóvil Eléctrico";
            this.Size = new System.Drawing.Size(820, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = colorPrimario };
            var lblTitulo = new Label { Text = "🔍  Buscar Automóvil Eléctrico", Font = new System.Drawing.Font("Segoe UI Semibold", 14f, System.Drawing.FontStyle.Bold), ForeColor = colorAcento, AutoSize = true, Location = new System.Drawing.Point(20, 18) };
            panelHeader.Controls.Add(lblTitulo);

            var panelBusqueda = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            panelBusqueda.Controls.Add(new Label { Text = "ID:", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold), ForeColor = colorSubTexto });
            txtId = new TextBox { Width = 220, Font = new System.Drawing.Font("Segoe UI", 9f), BorderStyle = BorderStyle.FixedSingle, BackColor = colorFondo, ForeColor = colorTexto };
            panelBusqueda.Controls.Add(txtId);

            var btnBuscar = new Button { Text = "Buscar", BackColor = colorAcento, ForeColor = colorPrimario, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold) };
            btnBuscar.FlatAppearance.BorderSize = 0; btnBuscar.Click += BtnBuscar_Click; panelBusqueda.Controls.Add(btnBuscar);

            dgvResultado = new DataGridView();
            dgvResultado.Dock = DockStyle.Fill;
            dgvResultado.ReadOnly = true;
            dgvResultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResultado.BackgroundColor = colorPanel;
            dgvResultado.BorderStyle = BorderStyle.None;
            dgvResultado.EnableHeadersVisualStyles = false;
            dgvResultado.ColumnHeadersDefaultCellStyle.BackColor = colorPrimario;
            dgvResultado.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResultado.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);

            this.Controls.Add(dgvResultado);
            this.Controls.Add(panelBusqueda);
            this.Controls.Add(panelHeader);
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