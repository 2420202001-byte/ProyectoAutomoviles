using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormBuscarBateria : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtId;
        private DataGridView dgv;

        public FormBuscarBateria()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Buscar Batería";
            this.Size = new System.Drawing.Size(700, 350);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelBusqueda = new FlowLayoutPanel();
            panelBusqueda.Dock = DockStyle.Top;
            panelBusqueda.Height = 50;
            panelBusqueda.Padding = new Padding(10);

            panelBusqueda.Controls.Add(new Label { Text = "ID Batería:", AutoSize = true });
            txtId = new TextBox { Width = 200 };
            panelBusqueda.Controls.Add(txtId);

            var btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.BackColor = System.Drawing.Color.FromArgb(0, 100, 180);
            btnBuscar.ForeColor = System.Drawing.Color.White;
            btnBuscar.Click += BtnBuscar_Click;
            panelBusqueda.Controls.Add(btnBuscar);

            dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            this.Controls.Add(dgv);
            this.Controls.Add(panelBusqueda);
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