using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormActualizarGasolina : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtIdBuscar, txtId, txtMarca, txtModelo, txtAnio, txtColor, txtPrecio, txtConsumo, txtTanque, txtCilindraje;
        private ComboBox cboCombustible;
        private Panel panelForm;

        public FormActualizarGasolina()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Actualizar Automóvil a Gasolina";
            this.Size = new System.Drawing.Size(440, 580);
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
            btnBuscar.BackColor = System.Drawing.Color.FromArgb(150, 50, 0);
            btnBuscar.ForeColor = System.Drawing.Color.White;
            btnBuscar.Click += BtnBuscar_Click;
            panelBusqueda.Controls.Add(btnBuscar);

            panelForm = new Panel();
            panelForm.Dock = DockStyle.Fill;
            panelForm.Enabled = false;

            var layout = new TableLayoutPanel();
            layout.ColumnCount = 2;
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(15);

            cboCombustible = new ComboBox();
            cboCombustible.Items.AddRange(new string[] { "Gasolina corriente", "Gasolina extra", "Premium" });
            cboCombustible.SelectedIndex = 0;
            cboCombustible.Dock = DockStyle.Fill;

            string[] labels = { "ID:", "Marca:", "Modelo:", "Año:", "Color:", "Precio ($):", "Consumo (L/100km):", "Tanque (L):", "Cilindraje (cc):", "Combustible:" };
            System.Windows.Forms.Control[] campos = {
                txtId = new TextBox(),
                txtMarca = new TextBox(),
                txtModelo = new TextBox(),
                txtAnio = new TextBox(),
                txtColor = new TextBox(),
                txtPrecio = new TextBox(),
                txtConsumo = new TextBox(),
                txtTanque = new TextBox(),
                txtCilindraje = new TextBox(),
                cboCombustible
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
            var request = new RestRequest($"/gasolina/{id}", Method.Get);
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
            txtConsumo.Text = auto["consumoLitrosPor100Km"]?.ToString();
            txtTanque.Text = auto["capacidadTanqueLitros"]?.ToString();
            txtCilindraje.Text = auto["cilindraje"]?.ToString();
            cboCombustible.SelectedItem = auto["tipoCombustible"]?.ToString();
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
                    consumoLitrosPor100Km = double.Parse(txtConsumo.Text.Trim()),
                    capacidadTanqueLitros = double.Parse(txtTanque.Text.Trim()),
                    cilindraje = int.Parse(txtCilindraje.Text.Trim()),
                    tipoCombustible = cboCombustible.SelectedItem.ToString(),
                    transmision = "Manual"
                };

                var client = new RestClient(BASE_URL);
                var request = new RestRequest($"/gasolina/{txtIdBuscar.Text.Trim()}", Method.Put);
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