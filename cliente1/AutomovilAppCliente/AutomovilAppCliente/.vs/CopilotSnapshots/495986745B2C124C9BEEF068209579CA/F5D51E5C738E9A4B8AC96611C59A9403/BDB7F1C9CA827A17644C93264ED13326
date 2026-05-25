using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormEliminarBateria : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtId;
        private DataGridView dgv;

        public FormEliminarBateria()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Eliminar Batería";
            this.Size = new System.Drawing.Size(700, 380);
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

            this.Controls.Add(dgv);
            this.Controls.Add(panelSur);
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

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Busque primero una batería."); return; }

            var confirm = MessageBox.Show("¿Confirma la eliminación?", "Eliminar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var client = new RestClient(BASE_URL);
                var request = new RestRequest($"/baterias/{id}", Method.Delete);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtId.Text = "";
                    dgv.Rows.Clear();
                    dgv.Columns.Clear();
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