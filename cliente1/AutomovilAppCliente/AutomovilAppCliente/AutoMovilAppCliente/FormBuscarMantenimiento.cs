using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Drawing;

namespace AutoMovilAppCliente
{
    public partial class FormBuscarMantenimiento : Form
    {
        private const string BASE_URL = "http://localhost:8080";

        private readonly Color colorPrimario = Color.FromArgb(15, 23, 42);
        private readonly Color colorAcento = Color.FromArgb(234, 179, 8);
        private readonly Color colorFondo = Color.FromArgb(241, 245, 249);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(30, 41, 59);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);
        private readonly Color colorHeader = Color.FromArgb(15, 23, 42);

        private TextBox txtId;
        private DataGridView dgv;

        public FormBuscarMantenimiento()
        {
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "AutoMóvil — Buscar Mantenimiento";
            this.Size = new Size(900, 420);
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
            lblTitulo.Text = "🔍  Buscar Mantenimiento";
            lblTitulo.Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold);
            lblTitulo.ForeColor = colorAcento;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 18);
            panelHeader.Controls.Add(lblTitulo);

            // Barra de búsqueda
            var panelBusqueda = new Panel();
            panelBusqueda.Dock = DockStyle.Top;
            panelBusqueda.Height = 55;
            panelBusqueda.BackColor = colorPanel;
            panelBusqueda.Padding = new Padding(15, 10, 15, 10);

            var lblId = new Label();
            lblId.Text = "ID Mantenimiento";
            lblId.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblId.ForeColor = colorSubTexto;
            lblId.Location = new Point(15, 10);
            lblId.AutoSize = true;
            panelBusqueda.Controls.Add(lblId);

            txtId = new TextBox();
            txtId.Location = new Point(15, 27);
            txtId.Width = 220;
            txtId.Font = new Font("Segoe UI", 9f);
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.BackColor = colorFondo;
            panelBusqueda.Controls.Add(txtId);

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

            // Tabla
            var panelTabla = new Panel();
            panelTabla.Dock = DockStyle.Fill;
            panelTabla.BackColor = colorFondo;
            panelTabla.Padding = new Padding(15, 10, 15, 10);

            dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = colorPanel;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(226, 232, 240);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = colorHeader;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = colorAcento;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 38;
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.ForeColor = colorTexto;
            dgv.RowTemplate.Height = 34;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 243, 199);
            dgv.DefaultCellStyle.SelectionForeColor = colorTexto;

            panelTabla.Controls.Add(dgv);

            this.Controls.Add(panelTabla);
            this.Controls.Add(panelBusqueda);
            this.Controls.Add(panelHeader);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            if (string.IsNullOrEmpty(id)) { MessageBox.Show("Ingrese un ID."); return; }

            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/mantenimientos/{id}", Method.Get);
            var response = client.Execute(request);

            dgv.Rows.Clear();
            dgv.Columns.Clear();

            if (!response.IsSuccessful)
            {
                MessageBox.Show("No se encontró ningún mantenimiento con ID: " + id);
                return;
            }

            var m = JsonSerializer.Deserialize<JsonObject>(response.Content);

            dgv.Columns.Add("id", "ID");
            dgv.Columns.Add("tipo", "Tipo");
            dgv.Columns.Add("descripcion", "Descripción");
            dgv.Columns.Add("costo", "Costo");
            dgv.Columns.Add("fecha", "Fecha");
            dgv.Columns.Add("km", "Kilometraje");
            dgv.Columns.Add("auto", "Automóvil");

            string idAuto = "Sin auto";
            string marcaAuto = "";

            if (m["idAutomovilGasolina"] != null && m["idAutomovilGasolina"].ToString() != "null")
            {
                idAuto = m["idAutomovilGasolina"]?.ToString();
            }
            else if (m["automovilGasolina"] != null && m["automovilGasolina"].ToString() != "null")
            {
                idAuto = m["automovilGasolina"]["id"]?.ToString();
            }

            if (m["marcaAutomovilGasolina"] != null && m["marcaAutomovilGasolina"].ToString() != "null")
            {
                marcaAuto = m["marcaAutomovilGasolina"]?.ToString();
            }
            else if (m["automovilGasolina"] != null && m["automovilGasolina"].ToString() != "null")
            {
                marcaAuto = m["automovilGasolina"]["marca"]?.ToString();
            }

            string auto = string.IsNullOrEmpty(marcaAuto) ? idAuto : $"{idAuto} - {marcaAuto}";

            dgv.Rows.Add(
                m["idMantenimiento"]?.ToString(),
                m["tipo"]?.ToString(),
                m["descripcion"]?.ToString(),
                m["costo"]?.ToString(),
                m["fechaMantenimiento"]?.ToString(),
                m["kilometraje"]?.ToString(),
                auto
            );
        }
    }
}