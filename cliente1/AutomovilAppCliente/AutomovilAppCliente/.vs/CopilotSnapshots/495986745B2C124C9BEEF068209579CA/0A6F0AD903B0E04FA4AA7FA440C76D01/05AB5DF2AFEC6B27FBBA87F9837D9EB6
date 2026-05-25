using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormAdicionarElectrico : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtId, txtMarca, txtModelo, txtAnio, txtColor, txtPrecio, txtAutonomia, txtTiempoCarga;
        private ComboBox cboBateria;

        public FormAdicionarElectrico()
        {
            CrearFormulario();
            CargarBaterias();
        }

        private void CrearFormulario()
        {
            this.Text = "Adicionar Automóvil Eléctrico";
            this.Size = new System.Drawing.Size(420, 560);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.FlowDirection = FlowDirection.TopDown;
            panel.WrapContents = false;
            panel.AutoScroll = true;
            panel.Padding = new Padding(15);

            string[] labels = { "ID:", "Marca:", "Modelo:", "Año:", "Color:", "Precio ($):", "Autonomía (km):", "T. Carga (h):", "Batería:" };

            txtId = new TextBox { Width = 350 };
            txtMarca = new TextBox { Width = 350 };
            txtModelo = new TextBox { Width = 350 };
            txtAnio = new TextBox { Width = 350 };
            txtColor = new TextBox { Width = 350 };
            txtPrecio = new TextBox { Width = 350 };
            txtAutonomia = new TextBox { Width = 350 };
            txtTiempoCarga = new TextBox { Width = 350 };
            cboBateria = new ComboBox { Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };

            System.Windows.Forms.Control[] campos = {
                txtId, txtMarca, txtModelo, txtAnio, txtColor,
                txtPrecio, txtAutonomia, txtTiempoCarga, cboBateria
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

        private void CargarBaterias()
        {
            try
            {
                cboBateria.Items.Clear();
                cboBateria.Items.Add("-- Sin batería --");

                var client = new RestClient(BASE_URL);
                var request = new RestRequest("/baterias/", Method.Get);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var lista = JsonSerializer.Deserialize<JsonArray>(response.Content);
                    foreach (var item in lista)
                        cboBateria.Items.Add(item["idBateria"]?.ToString() + " - " + item["marca"]?.ToString());
                }
                cboBateria.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando baterías: " + ex.Message);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                object bateria = null;
                if (cboBateria.SelectedIndex > 0)
                {
                    string idBat = cboBateria.SelectedItem.ToString().Split('-')[0].Trim();
                    var clientBat = new RestClient(BASE_URL);
                    var reqBat = new RestRequest($"/baterias/{idBat}", Method.Get);
                    var resBat = clientBat.Execute(reqBat);
                    if (resBat.IsSuccessful)
                        bateria = JsonSerializer.Deserialize<JsonObject>(resBat.Content);
                }

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
                    bateria = bateria
                };

                var client = new RestClient(BASE_URL);
                var request = new RestRequest("/electricos/", Method.Post);
                request.AddJsonBody(auto);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Automóvil eléctrico guardado exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AutoObservable.GetInstancia().NotificarObservers();
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
            cboBateria.SelectedIndex = 0;
        }
    }
}