using System;
using System.Drawing;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormBuscarBateria : Form
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
        private DataGridView dgv;

        public FormBuscarBateria()
        {
            CrearFormulario();
            this.BackColor = colorFondo;
            this.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void CrearFormulario()
        {
            this.Text = "Buscar Batería";
            this.Size = new System.Drawing.Size(700, 380);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = colorPrimario };
            var lblTitulo = new Label { Text = "🔍  Buscar Batería", Font = new System.Drawing.Font("Segoe UI Semibold", 14f, System.Drawing.FontStyle.Bold), ForeColor = colorAcento, AutoSize = true, Location = new System.Drawing.Point(20, 18) };
            panelHeader.Controls.Add(lblTitulo);

            var panelBusqueda = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            panelBusqueda.Controls.Add(new Label { Text = "ID Batería:", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold), ForeColor = colorSubTexto });
            txtId = new TextBox { Width = 220, Font = new System.Drawing.Font("Segoe UI", 9f), BorderStyle = BorderStyle.FixedSingle, BackColor = colorFondo, ForeColor = colorTexto };
            panelBusqueda.Controls.Add(txtId);

            var btnBuscar = new Button { Text = "Buscar", BackColor = colorAcento, ForeColor = colorPrimario, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold) };
            btnBuscar.FlatAppearance.BorderSize = 0; btnBuscar.Click += BtnBuscar_Click; panelBusqueda.Controls.Add(btnBuscar);

            dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = colorPanel;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = colorPrimario;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);

            this.Controls.Add(dgv);
            this.Controls.Add(panelBusqueda);
            this.Controls.Add(panelHeader);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Ingrese un ID."); return; }

            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/baterias/{id}", Method.Get);
            var response = client.Execute(request);

            dgv.Rows.Clear();
            dgv.Columns.Clear();

            if (!response.IsSuccessful)
            {
                MessageBox.Show("No se encontró ninguna batería con ID: " + id);
                return;
            }

            var b = JsonSerializer.Deserialize<JsonObject>(response.Content);

            dgv.Columns.Add("id", "ID Batería");
            dgv.Columns.Add("marca", "Marca");
            dgv.Columns.Add("capacidad", "Capacidad (kWh)");
            dgv.Columns.Add("ciclos", "Ciclos de vida");
            dgv.Columns.Add("voltaje", "Voltaje (V)");

            dgv.Rows.Add(
                b["idBateria"]?.ToString(),
                b["marca"]?.ToString(),
                b["capacidadKwh"]?.ToString(),
                b["ciclosVida"]?.ToString(),
                b["voltaje"]?.ToString()
            );
        }
    }
}