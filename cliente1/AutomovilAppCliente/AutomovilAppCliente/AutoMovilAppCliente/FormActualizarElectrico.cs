using System;
using System.Drawing;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormActualizarElectrico : Form
    {
        private const string BASE_URL = "http://localhost:8080";

        // Paleta Eléctrico
        private readonly Color colorPrimario = Color.FromArgb(30, 58, 138);
        private readonly Color colorAcento = Color.FromArgb(56, 189, 248);
        private readonly Color colorFondo = Color.FromArgb(240, 249, 255);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(15, 23, 42);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);

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
            this.Text = "AutoMóvil — Actualizar Eléctrico";
            this.Size = new Size(520, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = colorFondo;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = colorPrimario };
            var lblTitulo = new Label
            {
                Text = "⚡  Actualizar Automóvil Eléctrico",
                Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold),
                ForeColor = colorAcento,
                AutoSize = true,
                Location = new Point(20, 18)
            };
            panelHeader.Controls.Add(lblTitulo);

            // Barra Búsqueda
            var panelBusqueda = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = colorPanel, Padding = new Padding(15, 10, 15, 10) };
            var lblPaso = new Label { Text = "Paso 1 — ID Automóvil", Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = colorSubTexto, Location = new Point(15, 10), AutoSize = true };

            txtIdBuscar = new TextBox { Location = new Point(15, 27), Width = 220, Font = new Font("Segoe UI", 9f), BorderStyle = BorderStyle.FixedSingle, BackColor = colorFondo };

            var btnBuscar = new Button
            {
                Text = "🔍 Buscar",
                Location = new Point(245, 25),
                Size = new Size(110, 28),
                BackColor = colorAcento,
                ForeColor = colorPrimario,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += BtnBuscar_Click;

            panelBusqueda.Controls.Add(lblPaso);
            panelBusqueda.Controls.Add(txtIdBuscar);
            panelBusqueda.Controls.Add(btnBuscar);

            // Panel Formulario
            panelForm = new Panel { Dock = DockStyle.Fill, BackColor = colorPanel, Enabled = false, Padding = new Padding(20) };
            var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };

            string[] labels = { "ID:", "Marca:", "Modelo:", "Año:", "Color:", "Precio ($):", "Autonomía (km):", "T. Carga (h):", "Batería:" };

            txtId = CrearTextBox(); txtMarca = CrearTextBox(); txtModelo = CrearTextBox();
            txtAnio = CrearTextBox(); txtColor = CrearTextBox(); txtPrecio = CrearTextBox();
            txtAutonomia = CrearTextBox(); txtTiempoCarga = CrearTextBox();
            cboBateria = new ComboBox { Width = 420, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f), BackColor = colorFondo, ForeColor = colorTexto };

            Control[] campos = { txtId, txtMarca, txtModelo, txtAnio, txtColor, txtPrecio, txtAutonomia, txtTiempoCarga, cboBateria };

            for (int i = 0; i < labels.Length; i++)
            {
                var lbl = new Label { Text = labels[i], Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = colorSubTexto, AutoSize = true, Margin = new Padding(0, 8, 0, 2) };
                layout.Controls.Add(lbl);
                layout.Controls.Add(campos[i]);
            }

            var btnActualizar = new Button
            {
                Text = "✏️ Actualizar",
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 15, 0, 0)
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Click += BtnActualizar_Click;

            layout.Controls.Add(btnActualizar);
            panelForm.Controls.Add(layout);

            this.Controls.Add(panelForm);
            this.Controls.Add(panelBusqueda);
            this.Controls.Add(panelHeader);
        }

        private TextBox CrearTextBox()
        {
            return new TextBox { Width = 420, Font = new Font("Segoe UI", 9f), BorderStyle = BorderStyle.FixedSingle, BackColor = colorFondo, ForeColor = colorTexto };
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

                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    try
                    {
                        var lista = JsonSerializer.Deserialize<JsonArray>(response.Content);
                        if (lista != null)
                        {
                            foreach (var item in lista)
                            {
                                var id = item["idBateria"]?.ToString() ?? "";
                                var marca = item["marca"]?.ToString() ?? "";
                                cboBateria.Items.Add(id + " - " + marca);
                            }
                        }
                    }
                    catch { /* Ignorar parseo si no viene JSON esperado */ }
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

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
            {
                MessageBox.Show("No se encontró ningún automóvil con ID: " + id);
                return;
            }

            try
            {
                var auto = JsonSerializer.Deserialize<JsonObject>(response.Content);
                txtId.Text = auto["id"]?.ToString();
                txtMarca.Text = auto["marca"]?.ToString();
                txtModelo.Text = auto["modelo"]?.ToString();
                txtAnio.Text = auto["anio"]?.ToString();
                txtColor.Text = auto["color"]?.ToString();
                txtPrecio.Text = auto["precio"]?.ToString();
                txtAutonomia.Text = auto["autonomiaKm"]?.ToString();
                txtTiempoCarga.Text = auto["tiempoCargaHoras"]?.ToString();

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
            catch (Exception ex)
            {
                MessageBox.Show("Error parseando datos: " + ex.Message);
            }
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
                    if (resBat.IsSuccessful && !string.IsNullOrEmpty(resBat.Content))
                        bateria = JsonSerializer.Deserialize<JsonObject>(resBat.Content);
                }

                var auto = new
                {
                    id = txtId.Text.Trim(),
                    marca = txtMarca.Text.Trim(),
                    modelo = txtModelo.Text.Trim(),
                    anio = int.TryParse(txtAnio.Text.Trim(), out var a) ? a : 0,
                    color = txtColor.Text.Trim(),
                    precio = double.TryParse(txtPrecio.Text.Trim(), out var p) ? p : 0,
                    autonomiaKm = double.TryParse(txtAutonomia.Text.Trim(), out var ak) ? ak : 0,
                    tiempoCargaHoras = double.TryParse(txtTiempoCarga.Text.Trim(), out var th) ? th : 0,
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
                    try { AutoObservable.GetInstancia().NotificarObservers(); } catch { }
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
