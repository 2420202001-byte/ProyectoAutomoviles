using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormListarBateria : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private DataGridView dgv;
        private Label lblTotal;

        public FormListarBateria()
        {
            CrearFormulario();
            CargarDatos();
        }

        private void CrearFormulario()
        {
            this.Text = "Listado de Baterías";
            this.Size = new System.Drawing.Size(700, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelNorth = new FlowLayoutPanel();
            panelNorth.Dock = DockStyle.Top;
            panelNorth.Height = 50;
            panelNorth.Padding = new Padding(10);

            var btnRefrescar = new Button();
            btnRefrescar.Text = "🔄 Refrescar";
            btnRefrescar.Click += (s, e) => CargarDatos();
            panelNorth.Controls.Add(btnRefrescar);

            dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            lblTotal = new Label();
            lblTotal.Dock = DockStyle.Bottom;
            lblTotal.Height = 25;
            lblTotal.Text = "Total: 0 registros";

            this.Controls.Add(dgv);
            this.Controls.Add(lblTotal);
            this.Controls.Add(panelNorth);
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