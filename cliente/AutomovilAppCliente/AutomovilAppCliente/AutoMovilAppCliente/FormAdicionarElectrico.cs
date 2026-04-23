using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;

namespace AutoMovilAppCliente
{
    public partial class FormAdicionarElectrico : Form
    {
        private const string BASE_URL = "http://localhost:8080";

        private TextBox txtId, txtMarca, txtModelo, txtAnio, txtColor, txtPrecio, txtAutonomia, txtTiempoCarga;

        public FormAdicionarElectrico()
        {
            
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Adicionar Automóvil Eléctrico";
            this.Size = new System.Drawing.Size(420, 480);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panel = new TableLayoutPanel();
            panel.ColumnCount = 2;
            panel.RowCount = 10;
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(15);

            string[] labels = { "ID:", "Marca:", "Modelo:", "Año:", "Color:", "Precio ($):", "Autonomía (km):", "T. Carga (h):" };
            TextBox[] campos = {
                txtId = new TextBox(),
                txtMarca = new TextBox(),
                txtModelo = new TextBox(),
                txtAnio = new TextBox(),
                txtColor = new TextBox(),
                txtPrecio = new TextBox(),
                txtAutonomia = new TextBox(),
                txtTiempoCarga = new TextBox()
            };

            for (int i = 0; i < labels.Length; i++)
            {
                panel.Controls.Add(new Label { Text = labels[i], Anchor = AnchorStyles.Left, AutoSize = true });
                campos[i].Dock = DockStyle.Fill;
                panel.Controls.Add(campos[i]);
            }

            var btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            btnGuardar.ForeColor = System.Drawing.Color.White;
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.Click += BtnGuardar_Click;

            var btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Dock = DockStyle.Fill;
            btnLimpiar.Click += (s, e) => Limpiar();

            panel.Controls.Add(btnGuardar);
            panel.Controls.Add(btnLimpiar);

            this.Controls.Add(panel);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var auto = new
                {
                    id = txtId.Text.Trim(),
                    marca = txtMarca.Text.Trim(),
                    modelo = txtModelo.Text.Trim(),
                    anio = int.Parse(txtAnio.Text.Trim()),
                    color = txtColor.Text.Trim(),
                    precio = double.Parse(txtPrecio.Text.Trim()),
                    autonomiaKm = double.Parse(txtAutonomia.Text.Trim()),
                    tiempoCargaHoras = double.Parse(txtTiempoCarga.Text.Trim()),
                    bateria = (object)null
                };

                var client = new RestClient(BASE_URL);
                var request = new RestRequest("/electricos/", Method.Post);
                request.AddJsonBody(auto);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Automóvil eléctrico guardado exitosamente.", "Éxito",
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
            txtId.Text = ""; txtMarca.Text = ""; txtModelo.Text = "";
            txtAnio.Text = ""; txtColor.Text = ""; txtPrecio.Text = "";
            txtAutonomia.Text = ""; txtTiempoCarga.Text = "";
        }
    }
}