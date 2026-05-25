using System;
using System.Drawing;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormActualizarBateria : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        // Paleta Eléctrico (Azul)
        private readonly Color colorPrimario = Color.FromArgb(30, 58, 138);
        private readonly Color colorAcento = Color.FromArgb(56, 189, 248);
        private readonly Color colorFondo = Color.FromArgb(240, 249, 255);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(15, 23, 42);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);

        private TextBox txtIdBuscar, txtId, txtMarca, txtCapacidad, txtCiclos, txtVoltaje;
        private Panel panelForm;

        public FormActualizarBateria()
        {
            CrearFormulario();
            this.BackColor = colorFondo;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void CrearFormulario()
        {
            this.Text = "Actualizar Batería";
            this.Size = new System.Drawing.Size(480, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = colorPrimario };
            var lblTitulo = new Label { Text = "⚡  Actualizar Batería", Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold), ForeColor = colorAcento, AutoSize = true, Location = new Point(20, 18) };
            panelHeader.Controls.Add(lblTitulo);

            var panelBusqueda = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            panelBusqueda.Controls.Add(new Label { Text = "Paso 1 - ID:", AutoSize = true, Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = colorSubTexto });
            txtIdBuscar = new TextBox { Width = 220, Font = new Font("Segoe UI", 9f), BorderStyle = BorderStyle.FixedSingle, BackColor = colorFondo, ForeColor = colorTexto };
            panelBusqueda.Controls.Add(txtIdBuscar);
            var btnBuscar = new Button { Text = "🔍 Buscar", BackColor = colorAcento, ForeColor = colorPrimario, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            btnBuscar.FlatAppearance.BorderSize = 0; btnBuscar.Click += BtnBuscar_Click; panelBusqueda.Controls.Add(btnBuscar);

            panelForm = new Panel { Dock = DockStyle.Fill, BackColor = colorPanel, Enabled = false, Padding = new Padding(20) };
            var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(15) };

            string[] labels = { "ID:", "Marca:", "Capacidad (kWh):", "Ciclos de vida:", "Voltaje (V):" };

            txtId = CrearTextBox(); txtMarca = CrearTextBox(); txtCapacidad = CrearTextBox(); txtCiclos = CrearTextBox(); txtVoltaje = CrearTextBox();

            System.Windows.Forms.Control[] campos = { txtId, txtMarca, txtCapacidad, txtCiclos, txtVoltaje };

            for (int i = 0; i < labels.Length; i++)
            {
                var lbl = new Label { Text = labels[i], AutoSize = true, Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = colorSubTexto };
                layout.Controls.Add(lbl);
                layout.Controls.Add(campos[i]);
            }

            var btnActualizar = new Button { Text = "Actualizar", BackColor = Color.FromArgb(46, 139, 87), ForeColor = Color.White, Width = 120, Margin = new Padding(0, 10, 0, 0), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnActualizar.FlatAppearance.BorderSize = 0; btnActualizar.Click += BtnActualizar_Click; layout.Controls.Add(btnActualizar);

            panelForm.Controls.Add(layout);
            this.Controls.Add(panelForm);
            this.Controls.Add(panelBusqueda);
            this.Controls.Add(panelHeader);
        }

        private TextBox CrearTextBox()
        {
            return new TextBox { Width = 350, BorderStyle = BorderStyle.FixedSingle, BackColor = colorFondo, ForeColor = colorTexto, Font = new Font("Segoe UI", 9f) };
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