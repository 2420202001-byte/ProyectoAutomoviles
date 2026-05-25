using System;
using System.Windows.Forms;
using RestSharp;

namespace AutoMovilAppCliente
{
    public partial class FormAdicionarBateria : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtId, txtMarca, txtCapacidad, txtCiclos, txtVoltaje;

        public FormAdicionarBateria()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Adicionar Batería";
            this.Size = new System.Drawing.Size(420, 380);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.FlowDirection = FlowDirection.TopDown;
            panel.WrapContents = false;
            panel.AutoScroll = true;
            panel.Padding = new Padding(15);

            string[] labels = { "ID Batería:", "Marca:", "Capacidad (kWh):", "Ciclos de vida:", "Voltaje (V):" };

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
                panel.Controls.Add(new Label { Text = labels[i], AutoSize = true });
                panel.Controls.Add(campos[i]);
            }

            var panelBtns = new FlowLayoutPanel();
            panelBtns.AutoSize = true;
            panelBtns.Margin = new Padding(0, 10, 0, 0);

            var btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            btnGuardar.ForeColor = System.Drawing.Color.White;
            btnGuardar.Width = 100;
            btnGuardar.Click += BtnGuardar_Click;

            var btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Width = 100;
            btnLimpiar.Click += (s, e) => Limpiar();

            panelBtns.Controls.Add(btnGuardar);
            panelBtns.Controls.Add(btnLimpiar);
            panel.Controls.Add(panelBtns);

            this.Controls.Add(panel);
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