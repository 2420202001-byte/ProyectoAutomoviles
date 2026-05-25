using System;
using System.Drawing;
using System.Windows.Forms;
using RestSharp;

namespace AutoMovilAppCliente
{
    public partial class FormAdicionarGasolina : Form
    {
        private const string BASE_URL = "http://localhost:8080";
        // Paleta Gasolina (Naranja/Cálido)
        private readonly Color colorPrimario = Color.FromArgb(67, 20, 7);
        private readonly Color colorAcento = Color.FromArgb(249, 115, 22);
        private readonly Color colorFondo = Color.FromArgb(255, 247, 237);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(15, 23, 42);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);
        private TextBox txtId, txtMarca, txtModelo, txtAnio, txtColor, txtPrecio, txtConsumo, txtTanque, txtCilindraje;
        private ComboBox cboCombustible;

        public FormAdicionarGasolina()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "Adicionar Automóvil a Gasolina";
            this.Size = new System.Drawing.Size(520, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = colorFondo;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = colorPrimario };
            var lblTitulo = new Label { Text = "⛽  Adicionar Automóvil a Gasolina", Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold), ForeColor = colorAcento, AutoSize = true, Location = new Point(20, 18) };
            panelHeader.Controls.Add(lblTitulo);

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(15),
                BackColor = colorPanel
            };

            string[] labels = { "ID:", "Marca:", "Modelo:", "Año:", "Color:", "Precio ($):", "Consumo (L/100km):", "Tanque (L):", "Cilindraje (cc):", "Combustible:" };
            cboCombustible = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f), BackColor = colorFondo, ForeColor = colorTexto, Width = 350 };
            cboCombustible.Items.AddRange(new string[] { "Gasolina corriente", "Gasolina extra", "Premium" });
            cboCombustible.SelectedIndex = 0;

            txtId = CrearTextBox();
            txtMarca = CrearTextBox();
            txtModelo = CrearTextBox();
            txtAnio = CrearTextBox();
            txtColor = CrearTextBox();
            txtPrecio = CrearTextBox();
            txtConsumo = CrearTextBox();
            txtTanque = CrearTextBox();
            txtCilindraje = CrearTextBox();

            System.Windows.Forms.Control[] campos = { txtId, txtMarca, txtModelo, txtAnio, txtColor, txtPrecio, txtConsumo, txtTanque, txtCilindraje, cboCombustible };

            for (int i = 0; i < labels.Length; i++)
            {
                var lbl = new Label { Text = labels[i], AutoSize = true, Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = colorSubTexto };
                panel.Controls.Add(lbl);
                panel.Controls.Add(campos[i]);
            }

            var btnGuardar = new Button
            {
                Text = "Guardar",
                BackColor = colorAcento,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Width = 120,
                Margin = new Padding(0, 5, 10, 0)
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            var btnLimpiar = new Button
            {
                Text = "Limpiar",
                BackColor = colorPanel,
                ForeColor = colorTexto,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Width = 120,
                Margin = new Padding(0, 5, 0, 0)
            };
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Click += (s, e) => Limpiar();

            var panelBtns = new FlowLayoutPanel
            {
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            panelBtns.Controls.Add(btnGuardar);
            panelBtns.Controls.Add(btnLimpiar);

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