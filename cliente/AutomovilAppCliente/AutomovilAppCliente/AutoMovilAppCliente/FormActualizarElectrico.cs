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
        private Panel panelForm;

        public FormActualizarElectrico()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Actualizar Automóvil Eléctrico";
            this.Size = new System.Drawing.Size(440, 560);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel búsqueda
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

            // Panel formulario
            panelForm = new Panel();
            panelForm.Dock = DockStyle.Fill;
            panelForm.Enabled = false;

            var layout = new TableLayoutPanel();
            layout.ColumnCount = 2;
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(15);

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
                layout.Controls.Add(new Label { Text = labels[i], AutoSize = true, Anchor = AnchorStyles.Left });
                campos[i].Dock = DockStyle.Fill;
                layout.Controls.Add(campos[i]);
            }

            var btnActualizar = new Button();
            btnActualizar.Text = "Actualizar";
            btnActualizar.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            btnActualizar.ForeColor = System.Drawing.Color.White;
            btnActualizar.Dock = DockStyle.Fill;
            btnActualizar.Click += BtnActualizar_Click;
            layout.SetColumnSpan(btnActualizar, 2);
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
            panelForm.Enabled = true;
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
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
                MessageBox.Show("Error en campos numéricos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}