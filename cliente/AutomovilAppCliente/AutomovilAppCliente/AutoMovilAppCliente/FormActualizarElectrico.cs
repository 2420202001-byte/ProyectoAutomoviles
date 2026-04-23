using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormActualizarElectrico : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtIdBuscar, txtId, txtMarca, txtModelo, txtAnio, txtColor, txtPrecio, txtAutonomia, txtTiempoCarga;
        private ComboBox cboBateria;
        private Panel panelForm;

        public FormActualizarElectrico()
        {
            CrearFormulario();
            CargarBaterias();
        }

        private void CrearFormulario()
        {
            this.Text = "Actualizar Automóvil Eléctrico";
            this.Size = new System.Drawing.Size(440, 620);
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
            btnBuscar.BackColor = System.Drawing.Color.FromArgb(25, 70, 130);
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
                layout.Controls.Add(new Label { Text = labels[i], AutoSize = true });
                campos[i].Dock = DockStyle.None;
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

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtIdBuscar.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Ingrese un ID."); return; }

            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/electricos/{id}", Method.Get);
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                MessageBox.Show("No se encontró ningún automóvil con ID: " + id);
                return;
            }

            var auto = JsonSerializer.Deserialize<JsonObject>(response.Content);
            txtId.Text = auto["id"]?.ToString();
            txtMarca.Text = auto["marca"]?.ToString();
            txtModelo.Text = auto["modelo"]?.ToString();
            txtAnio.Text = auto["anio"]?.ToString();
            txtColor.Text = auto["color"]?.ToString();
            txtPrecio.Text = auto["precio"]?.ToString();
            txtAutonomia.Text = auto["autonomiaKm"]?.ToString();
            txtTiempoCarga.Text = auto["tiempoCargaHoras"]?.ToString();

            // Seleccionar batería si tiene
            if (auto["bateria"] != null && auto["bateria"].ToString() != "null")
            {
                string idBat = auto["bateria"]["idBateria"]?.ToString();
                for (int i = 1; i < cboBateria.Items.Count; i++)
                {
                    if (cboBateria.Items[i].ToString().StartsWith(idBat))
                    {
                        cboBateria.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cboBateria.SelectedIndex = 0;
            }

            panelForm.Enabled = true;
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
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
                var request = new RestRequest($"/electricos/{txtIdBuscar.Text.Trim()}", Method.Put);
                request.AddJsonBody(auto);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AutoObservable.GetInstancia().NotificarObservers();
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