using System;
using System.Windows.Forms;
using RestSharp;

namespace AutoMovilAppCliente
{
    public partial class FormAdicionarGasolina : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        private TextBox txtId, txtMarca, txtModelo, txtAnio, txtColor, txtPrecio, txtConsumo, txtTanque, txtCilindraje;
        private ComboBox cboCombustible;

        public FormAdicionarGasolina()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Adicionar Automóvil a Gasolina";
            this.Size = new System.Drawing.Size(420, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            var layout = new TableLayoutPanel();
            layout.ColumnCount = 2;
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(15);

            string[] labels = { "ID:", "Marca:", "Modelo:", "Año:", "Color:", "Precio ($):", "Consumo (L/100km):", "Tanque (L):", "Cilindraje (cc):", "Combustible:" };
            cboCombustible = new ComboBox();
            cboCombustible.Items.AddRange(new string[] { "Gasolina corriente", "Gasolina extra", "Premium" });
            cboCombustible.SelectedIndex = 0;
            cboCombustible.Dock = DockStyle.Fill;

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

            var btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.BackColor = System.Drawing.Color.FromArgb(205, 92, 0);
            btnGuardar.ForeColor = System.Drawing.Color.White;
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.Click += BtnGuardar_Click;

            var btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Dock = DockStyle.Fill;
            btnLimpiar.Click += (s, e) => Limpiar();

            layout.Controls.Add(btnGuardar);
            layout.Controls.Add(btnLimpiar);

            this.Controls.Add(layout);
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
                    consumoLitrosPor100Km = double.Parse(txtConsumo.Text.Trim()),
                    capacidadTanqueLitros = double.Parse(txtTanque.Text.Trim()),
                    cilindraje = int.Parse(txtCilindraje.Text.Trim()),
                    tipoCombustible = cboCombustible.SelectedItem.ToString(),
                    transmision = "Manual"
                };

                var client = new RestClient(BASE_URL);
                var request = new RestRequest("/gasolina/", Method.Post);
                request.AddJsonBody(auto);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Automóvil a gasolina guardado exitosamente.", "Éxito",
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
            txtConsumo.Text = ""; txtTanque.Text = ""; txtCilindraje.Text = "";
            cboCombustible.SelectedIndex = 0;
        }
    }
}