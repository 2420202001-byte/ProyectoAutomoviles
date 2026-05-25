using System;
using System.Drawing;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormListarBateria : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        // Paleta Eléctrico (Azul)
        private readonly Color colorPrimario = Color.FromArgb(30, 58, 138);
        private readonly Color colorAcento = Color.FromArgb(56, 189, 248);
        private readonly Color colorFondo = Color.FromArgb(240, 249, 255);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(15, 23, 42);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);

        private DataGridView dgv;
        private Label lblTotal;

        public FormListarBateria()
        {
            CrearFormulario();
            CargarDatos();
            this.BackColor = colorFondo;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void CrearFormulario()
        {
            this.Text = "Listado de Baterías";
            this.Size = new System.Drawing.Size(760, 440);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = colorPrimario };
            var lblTitulo = new Label { Text = "⚡  Listado de Baterías", Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold), ForeColor = colorAcento, AutoSize = true, Location = new Point(20, 18) };
            panelHeader.Controls.Add(lblTitulo);

            var panelNorth = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            var btnRefrescar = new Button { Text = "🔄 Refrescar", BackColor = colorAcento, ForeColor = colorPrimario, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            btnRefrescar.FlatAppearance.BorderSize = 0; btnRefrescar.Click += (s, e) => CargarDatos();
            panelNorth.Controls.Add(btnRefrescar);

            dgv = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = colorPanel, BorderStyle = BorderStyle.None };
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = colorPrimario;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            lblTotal = new Label { Dock = DockStyle.Bottom, Height = 25, Text = "Total: 0 registros", Font = new Font("Segoe UI", 9f), ForeColor = colorSubTexto };

            this.Controls.Add(dgv);
            this.Controls.Add(lblTotal);
            this.Controls.Add(panelNorth);
            this.Controls.Add(panelHeader);
        }

        private void CargarDatos()
        {
            var client = new RestClient(BASE_URL);
            var request = new RestRequest("/baterias/", Method.Get);
            var response = client.Execute(request);

            if (!response.IsSuccessful) return;

            dgv.Rows.Clear();
            dgv.Columns.Clear();

            dgv.Columns.Add("id", "ID Batería");
            dgv.Columns.Add("marca", "Marca");
            dgv.Columns.Add("capacidad", "Capacidad (kWh)");
            dgv.Columns.Add("ciclos", "Ciclos de vida");
            dgv.Columns.Add("voltaje", "Voltaje (V)");

            var lista = JsonSerializer.Deserialize<JsonArray>(response.Content);
            foreach (var item in lista)
            {
                dgv.Rows.Add(
                    item["idBateria"]?.ToString(),
                    item["marca"]?.ToString(),
                    item["capacidadKwh"]?.ToString(),
                    item["ciclosVida"]?.ToString(),
                    item["voltaje"]?.ToString()
                );
            }
            lblTotal.Text = "Total: " + lista.Count + " baterías";
        }
    }
}