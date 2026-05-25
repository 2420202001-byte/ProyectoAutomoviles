using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Drawing;

namespace AutoMovilAppCliente
{
    public partial class FormActualizarMantenimiento : Form
    {
        private const string BASE_URL = "http://localhost:8080";

        private readonly Color colorPrimario = Color.FromArgb(15, 23, 42);
        private readonly Color colorAcento = Color.FromArgb(234, 179, 8);
        private readonly Color colorFondo = Color.FromArgb(241, 245, 249);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(30, 41, 59);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);

        private TextBox txtIdBuscar, txtId, txtTipo, txtDescripcion, txtCosto, txtKilometraje;
        private DateTimePicker dtpFecha;
        private ComboBox cboAuto;
        private Panel panelForm;

        public FormActualizarMantenimiento()
        {
            CrearFormulario();
            CargarAutos();
        }

        private void CrearFormulario()
        {
            this.Text = "AutoMóvil — Actualizar Mantenimiento";
            this.Size = new Size(520, 620);
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
            lblTitulo.Text = "✏️  Actualizar Mantenimiento";
            lblTitulo.Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold);
            lblTitulo.ForeColor = colorAcento;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 18);
            panelHeader.Controls.Add(lblTitulo);

            // Barra búsqueda
            var panelBusqueda = new Panel();
            panelBusqueda.Dock = DockStyle.Top;
            panelBusqueda.Height = 55;
            panelBusqueda.BackColor = colorPanel;
            panelBusqueda.Padding = new Padding(15, 10, 15, 10);

            var lblPaso = new Label();
            lblPaso.Text = "Paso 1 — ID Mantenimiento";
            lblPaso.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblPaso.ForeColor = colorSubTexto;
            lblPaso.Location = new Point(15, 10);
            lblPaso.AutoSize = true;
            panelBusqueda.Controls.Add(lblPaso);

            txtIdBuscar = new TextBox();
            txtIdBuscar.Location = new Point(15, 27);
            txtIdBuscar.Width = 220;
            txtIdBuscar.Font = new Font("Segoe UI", 9f);
            txtIdBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtIdBuscar.BackColor = colorFondo;
            panelBusqueda.Controls.Add(txtIdBuscar);

            var btnBuscar = new Button();
            btnBuscar.Text = "🔍  Buscar";
            btnBuscar.Location = new Point(245, 25);
            btnBuscar.Size = new Size(110, 28);
            btnBuscar.BackColor = colorAcento;
            btnBuscar.ForeColor = colorPrimario;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.Click += BtnBuscar_Click;
            panelBusqueda.Controls.Add(btnBuscar);

            // Panel formulario
            panelForm = new Panel();
            panelForm.Dock = DockStyle.Fill;
            panelForm.BackColor = colorPanel;
            panelForm.Enabled = false;
            panelForm.Padding = new Padding(20);

            var layout = new FlowLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.FlowDirection = FlowDirection.TopDown;
            layout.WrapContents = false;
            layout.AutoScroll = true;

            string[] labels = { "ID:", "Tipo:", "Descripción:", "Costo ($):", "Kilometraje:", "Fecha:", "Automóvil:" };

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
                lbl.Margin = new Padding(0, 8, 0, 2);
                layout.Controls.Add(lbl);
                layout.Controls.Add(campos[i]);
            }

            var btnActualizar = new Button();
            btnActualizar.Text = "✏️  Actualizar";
            btnActualizar.Size = new Size(130, 35);
            btnActualizar.BackColor = Color.FromArgb(22, 163, 74);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnActualizar.Cursor = Cursors.Hand;
            btnActualizar.Margin = new Padding(0, 15, 0, 0);
            btnActualizar.Click += BtnActualizar_Click;
            layout.Controls.Add(btnActualizar);

            panelForm.Controls.Add(layout);

            this.Controls.Add(panelForm);
            this.Controls.Add(panelBusqueda);
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
                ForeColor = colorTexto
            };
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

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtIdBuscar.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Ingrese un ID."); return; }

            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/mantenimientos/{id}", Method.Get);
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                MessageBox.Show("No se encontró ningún mantenimiento con ID: " + id);
                return;
            }

            var m = JsonSerializer.Deserialize<JsonObject>(response.Content);
            txtId.Text = m["idMantenimiento"]?.ToString();
            txtTipo.Text = m["tipo"]?.ToString();
            txtDescripcion.Text = m["descripcion"]?.ToString();
            txtCosto.Text = m["costo"]?.ToString();
            txtKilometraje.Text = m["kilometraje"]?.ToString();

            if (DateTime.TryParse(m["fechaMantenimiento"]?.ToString(), out DateTime fecha))
                dtpFecha.Value = fecha;

            var auto = m["automovilGasolina"]?.AsObject();

if (auto != null)
{
    string idAuto = auto["id"]?.ToString();

    for (int i = 1; i < cboAuto.Items.Count; i++)
    {
        if (cboAuto.Items[i].ToString().StartsWith(idAuto))
        {
            cboAuto.SelectedIndex = i;
            break;
        }
    }
}

            panelForm.Enabled = true;
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
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
                var request = new RestRequest($"/mantenimientos/{txtIdBuscar.Text.Trim()}", Method.Put);
                request.AddJsonBody(mantenimiento);
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