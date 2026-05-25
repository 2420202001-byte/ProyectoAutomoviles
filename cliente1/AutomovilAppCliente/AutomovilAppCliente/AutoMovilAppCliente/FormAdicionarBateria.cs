using System;
using System.Drawing;
using System.Windows.Forms;
using RestSharp;

namespace AutoMovilAppCliente
{
    public partial class FormAdicionarBateria : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        // Paleta Eléctrico (Azul)
        private readonly Color colorPrimario = Color.FromArgb(30, 58, 138);
        private readonly Color colorAcento = Color.FromArgb(56, 189, 248);
        private readonly Color colorFondo = Color.FromArgb(240, 249, 255);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(15, 23, 42);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);

        private TextBox txtId, txtMarca, txtCapacidad, txtCiclos, txtVoltaje;

        public FormAdicionarBateria()
        {
            CrearFormulario();
            this.BackColor = colorFondo;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void CrearFormulario()
        {
            this.Text = "Adicionar Batería";
            this.Size = new System.Drawing.Size(480, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = colorPrimario };
            var lblTitulo = new Label { Text = "⚡  Adicionar Batería", Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold), ForeColor = colorAcento, AutoSize = true, Location = new Point(20, 18) };
            panelHeader.Controls.Add(lblTitulo);

            var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(15) };

            string[] labels = { "ID Batería:", "Marca:", "Capacidad (kWh):", "Ciclos de vida:", "Voltaje (V):" };

            txtId = CrearTextBox(); txtMarca = CrearTextBox(); txtCapacidad = CrearTextBox(); txtCiclos = CrearTextBox(); txtVoltaje = CrearTextBox();

            System.Windows.Forms.Control[] campos = { txtId, txtMarca, txtCapacidad, txtCiclos, txtVoltaje };

            for (int i = 0; i < labels.Length; i++)
            {
                var lbl = new Label { Text = labels[i], AutoSize = true, Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = colorSubTexto };
                panel.Controls.Add(lbl);
                panel.Controls.Add(campos[i]);
            }

            var panelBtns = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0, 10, 0, 0) };

            var btnGuardar = new Button { Text = "Guardar", BackColor = colorAcento, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9f, FontStyle.Bold), Width = 120 };
            btnGuardar.FlatAppearance.BorderSize = 0; btnGuardar.Click += BtnGuardar_Click;

            var btnLimpiar = new Button { Text = "Limpiar", BackColor = colorPanel, ForeColor = colorTexto, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9f, FontStyle.Bold), Width = 120 };
            btnLimpiar.FlatAppearance.BorderSize = 0; btnLimpiar.Click += (s, e) => Limpiar();

            panelBtns.Controls.Add(btnGuardar); panelBtns.Controls.Add(btnLimpiar);
            panel.Controls.Add(panelBtns);

            this.Controls.Add(panel);
            this.Controls.Add(panelHeader);
        }

        private TextBox CrearTextBox()
        {
            return new TextBox { Width = 350, BorderStyle = BorderStyle.FixedSingle, BackColor = colorFondo, ForeColor = colorTexto, Font = new Font("Segoe UI", 9f) };
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
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
                var request = new RestRequest("/baterias/", Method.Post);
                request.AddJsonBody(bateria);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Batería guardada exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                }
                else
                {
                    MessageBox.Show("Error al guardar: " + response.StatusCode, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            txtId.Text = ""; txtMarca.Text = ""; txtCapacidad.Text = "";
            txtCiclos.Text = ""; txtVoltaje.Text = "";
        }
    }
}