using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormActualizarBateria : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtIdBuscar, txtId, txtMarca, txtCapacidad, txtCiclos, txtVoltaje;
        private Panel panelForm;

        public FormActualizarBateria()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Actualizar Batería";
            this.Size = new System.Drawing.Size(420, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelBusqueda = new FlowLayoutPanel();
            panelBusqueda.Dock = DockStyle.Top;
            panelBusqueda.Height = 50;
            panelBusqueda.Padding = new Padding(10);
            panelBusqueda.Controls.Add(new Label { Text = "Paso 1 - ID:", AutoSize = true });
            txtIdBuscar = new TextBox { Width = 150 };
            panelBusqueda.Controls.Add(txtIdBuscar);

            var btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.BackColor = System.Drawing.Color.FromArgb(0, 100, 180);
            btnBuscar.ForeColor = System.Drawing.Color.White;
            btnBuscar.Click += BtnBuscar_Click;
            panelBusqueda.Controls.Add(btnBuscar);

            panelForm = new Panel();
            panelForm.Dock = DockStyle.Fill;
            panelForm.Enabled = false;

            var layout = new FlowLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.FlowDirection = FlowDirection.TopDown;
            layout.WrapContents = false;
            layout.AutoScroll = true;
            layout.Padding = new Padding(15);

            string[] labels = { "ID:", "Marca:", "Capacidad (kWh):", "Ciclos de vida:", "Voltaje (V):" };

            txtId = new TextBox { Width = 350 };
            txtMarca = new TextBox { Width = 350 };
            txtCapacidad = new TextBox { Width = 350 };
            txtCiclos = new TextBox { Width = 350 };
            txtVoltaje = new TextBox { Width = 350 };

            System.Windows.Forms.Control[] campos = {
                txtId, txtMarca, txtCapacidad, txtCiclos, txtVoltaje
            };

            for (int i = 0; i < labels.Length; i++)
            {
                layout.Controls.Add(new Label { Text = labels[i], AutoSize = true });
                layout.Controls.Add(campos[i]);
            }

            var btnActualizar = new Button();
            btnActualizar.Text = "Actualizar";
            btnActualizar.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            btnActualizar.ForeColor = System.Drawing.Color.White;
            btnActualizar.Width = 120;
            btnActualizar.Margin = new Padding(0, 10, 0, 0);
            btnActualizar.Click += BtnActualizar_Click;
            layout.Controls.Add(btnActualizar);

            panelForm.Controls.Add(layout);
            this.Controls.Add(panelForm);
            this.Controls.Add(panelBusqueda);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtIdBuscar.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Ingrese un ID."); return; }

            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/baterias/{id}", Method.Get);
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                MessageBox.Show("No se encontró ninguna batería con ID: " + id);
                return;
            }

            var b = JsonSerializer.Deserialize<JsonObject>(response.Content);
            txtId.Text = b["idBateria"]?.ToString();
            txtMarca.Text = b["marca"]?.ToString();
            txtCapacidad.Text = b["capacidadKwh"]?.ToString();
            txtCiclos.Text = b["ciclosVida"]?.ToString();
            txtVoltaje.Text = b["voltaje"]?.ToString();
            panelForm.Enabled = true;
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                var bateria = new
                {
                    idBateria = txtId.Text.Trim(),
                    marca = txtMarca.Text.Trim(),
                    capacidadKwh = double.Parse(txtCapacidad.Text.Trim()),
                    ciclosVida = int.Parse(txtCiclos.Text.Trim()),
                    voltaje = double.Parse(txtVoltaje.Text.Trim())
                };

                var client = new RestClient(BASE_URL);
                var request = new RestRequest($"/baterias/{txtIdBuscar.Text.Trim()}", Method.Put);
                request.AddJsonBody(bateria);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Batería actualizada correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelForm.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Error al actualizar: " + response.StatusCode, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}