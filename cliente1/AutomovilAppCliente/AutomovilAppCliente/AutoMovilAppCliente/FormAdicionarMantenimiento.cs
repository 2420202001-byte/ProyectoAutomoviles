using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Drawing;

namespace AutoMovilAppCliente
{
    public partial class FormAdicionarMantenimiento : Form
    {
        private const string BASE_URL = "http://localhost:8080";

        private readonly Color colorPrimario = Color.FromArgb(15, 23, 42);
        private readonly Color colorAcento = Color.FromArgb(234, 179, 8);
        private readonly Color colorFondo = Color.FromArgb(241, 245, 249);
        private readonly Color colorPanel = Color.FromArgb(255, 255, 255);
        private readonly Color colorTexto = Color.FromArgb(30, 41, 59);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);

        private TextBox txtId, txtTipo, txtDescripcion, txtCosto, txtKilometraje;
        private DateTimePicker dtpFecha;
        private ComboBox cboAuto;

        public FormAdicionarMantenimiento()
        {
            CrearFormulario();
            CargarAutos();
        }

        private void CrearFormulario()
        {
            this.Text = "AutoMóvil — Adicionar Mantenimiento";
            this.Size = new Size(520, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = colorFondo;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Header
            var panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 65;
            panelHeader.BackColor = colorPrimario;

            var lblTitulo = new Label();
            lblTitulo.Text = "🔧  Adicionar Mantenimiento";
            lblTitulo.Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold);
            lblTitulo.ForeColor = colorAcento;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 18);
            panelHeader.Controls.Add(lblTitulo);

            // Panel formulario
            var panelForm = new Panel();
            panelForm.Dock = DockStyle.Fill;
            panelForm.BackColor = colorPanel;
            panelForm.Padding = new Padding(25, 20, 25, 20);

            var layout = new FlowLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.FlowDirection = FlowDirection.TopDown;
            layout.WrapContents = false;
            layout.AutoScroll = true;
            layout.Padding = new Padding(0);

            string[] labels = { "ID Mantenimiento:", "Tipo:", "Descripción:", "Costo ($):", "Kilometraje:", "Fecha Mantenimiento:", "Automóvil a Gasolina:" };

            txtId = CrearTextBox(); txtTipo = CrearTextBox();
            txtDescripcion = CrearTextBox(); txtCosto = CrearTextBox();
            txtKilometraje = CrearTextBox();
            dtpFecha = new DateTimePicker { Width = 420, Format = DateTimePickerFormat.Short };
            cboAuto = new ComboBox { Width = 420, DropDownStyle = ComboBoxStyle.DropDownList };

            Control[] campos = { txtId, txtTipo, txtDescripcion, txtCosto, txtKilometraje, dtpFecha, cboAuto };

            for (int i = 0; i < labels.Length; i++)
            {
                var lbl = new Label();
                lbl.Text = labels[i];
                lbl.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
                lbl.ForeColor = colorSubTexto;
                lbl.AutoSize = true;
                lbl.Margin = new Padding(0, 10, 0, 2);
                layout.Controls.Add(lbl);
                campos[i].Margin = new Padding(0, 0, 0, 0);
                layout.Controls.Add(campos[i]);
            }

            // Botones
            var panelBtns = new FlowLayoutPanel();
            panelBtns.AutoSize = true;
            panelBtns.Margin = new Padding(0, 20, 0, 0);

            var btnGuardar = CrearBoton("💾  Guardar", colorAcento, colorPrimario);
            btnGuardar.Click += BtnGuardar_Click;
            var btnLimpiar = CrearBoton("🧹  Limpiar", Color.FromArgb(226, 232, 240), colorTexto);
            btnLimpiar.Click += (s, e) => Limpiar();

            panelBtns.Controls.Add(btnGuardar);
            panelBtns.Controls.Add(btnLimpiar);
            layout.Controls.Add(panelBtns);

            panelForm.Controls.Add(layout);
            this.Controls.Add(panelForm);
            this.Controls.Add(panelHeader);
        }

        private TextBox CrearTextBox()
        {
            return new TextBox
            {
                Width = 420,
                Font = new Font("Segoe UI", 9f),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = colorFondo,
                ForeColor = colorTexto,
                Margin = new Padding(0, 0, 0, 0)
            };
        }

        private Button CrearBoton(string texto, Color back, Color fore)
        {
            var btn = new Button();
            btn.Text = texto;
            btn.Size = new Size(130, 35);
            btn.BackColor = back;
            btn.ForeColor = fore;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Margin = new Padding(0, 0, 10, 0);
            return btn;
        }

        private void CargarAutos()
        {
            try
            {
                cboAuto.Items.Clear();
                cboAuto.Items.Add("-- Seleccione un automóvil --");
                var client = new RestClient(BASE_URL);
                var request = new RestRequest("/gasolina/", Method.Get);
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var lista = JsonSerializer.Deserialize<JsonArray>(response.Content);
                    foreach (var item in lista)
                        cboAuto.Items.Add(item["id"]?.ToString() + " - " + item["marca"]?.ToString() + " " + item["modelo"]?.ToString());
                }
                cboAuto.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando autos: " + ex.Message);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboAuto.SelectedIndex == 0) { MessageBox.Show("Seleccione un automóvil."); return; }
                string idAuto = cboAuto.SelectedItem.ToString().Split('-')[0].Trim();

                var mantenimiento = new
                {
                    idMantenimiento = txtId.Text.Trim(),
                    tipo = txtTipo.Text.Trim(),
                    descripcion = txtDescripcion.Text.Trim(),
                    costo = double.Parse(txtCosto.Text.Trim()),
                    fechaMantenimiento = dtpFecha.Value.ToString("yyyy-MM-ddTHH:mm:ss"),
                    kilometraje = int.Parse(txtKilometraje.Text.Trim()),
                    automovilGasolina = new { id = idAuto }
                };

                var client = new RestClient(BASE_URL);
                var request = new RestRequest("/mantenimientos/", Method.Post);
                request.AddJsonBody(mantenimiento);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("Mantenimiento guardado exitosamente.", "Éxito",
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
            txtId.Text = ""; txtTipo.Text = ""; txtDescripcion.Text = "";
            txtCosto.Text = ""; txtKilometraje.Text = "";
            dtpFecha.Value = DateTime.Now;
            cboAuto.SelectedIndex = 0;
        }
    }
}