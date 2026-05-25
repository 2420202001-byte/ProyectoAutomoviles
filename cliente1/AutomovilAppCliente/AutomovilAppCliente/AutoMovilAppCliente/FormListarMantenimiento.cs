using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Drawing;

namespace AutoMovilAppCliente
{
    public partial class FormListarMantenimiento : Form, IObserver
    {
        private const string BASE_URL = "http://localhost:8080";

        private readonly Color colorPrimario = Color.FromArgb(15, 23, 42);
        private readonly Color colorAcento = Color.FromArgb(234, 179, 8);
        private readonly Color colorFondo = Color.FromArgb(241, 245, 249);
        private readonly Color colorPanel = Color.White;
        private readonly Color colorTexto = Color.FromArgb(30, 41, 59);
        private readonly Color colorSubTexto = Color.FromArgb(100, 116, 139);
        private readonly Color colorHeader = Color.FromArgb(15, 23, 42);
        private readonly Color colorFilaImpar = Color.FromArgb(255, 251, 235);
        private readonly Color colorFilaPar = Color.White;

        private DataGridView dgv;
        private Label lblTotal;
        private TextBox txtFiltroTipo, txtFiltroAuto;

        public FormListarMantenimiento()
        {
            CrearFormulario();
            CargarDatos();
        }

        private void CrearFormulario()
        {
            this.Text = "AutoMóvil — Listado de Mantenimientos";
            this.Size = new Size(1100, 620);
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
            lblTitulo.Text = "🔧  Listado de Mantenimientos";
            lblTitulo.Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold);
            lblTitulo.ForeColor = colorAcento;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 18);
            panelHeader.Controls.Add(lblTitulo);

            // Barra filtros
            var panelFiltros = new Panel();
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Height = 60;
            panelFiltros.BackColor = colorPanel;
            panelFiltros.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(226, 232, 240), 1),
                    0, panelFiltros.Height - 1, panelFiltros.Width, panelFiltros.Height - 1);
            };

            int x = 15;

            var lblTipo = new Label();
            lblTipo.Text = "Tipo";
            lblTipo.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblTipo.ForeColor = colorSubTexto;
            lblTipo.Location = new Point(x, 12);
            lblTipo.AutoSize = true;
            panelFiltros.Controls.Add(lblTipo);

            txtFiltroTipo = CrearTextBox(x, 28, 140);
            panelFiltros.Controls.Add(txtFiltroTipo);
            x += 150;

            var btnFiltrarTipo = CrearBoton("Filtrar tipo", x, 26, colorAcento, colorPrimario);
            btnFiltrarTipo.Click += (s, e) => FiltrarPorTipo();
            panelFiltros.Controls.Add(btnFiltrarTipo);
            x += 115;

            var sep = new Panel();
            sep.BackColor = Color.FromArgb(226, 232, 240);
            sep.Location = new Point(x, 10);
            sep.Size = new Size(1, 38);
            panelFiltros.Controls.Add(sep);
            x += 15;

            var lblAuto = new Label();
            lblAuto.Text = "ID Automóvil";
            lblAuto.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblAuto.ForeColor = colorSubTexto;
            lblAuto.Location = new Point(x, 12);
            lblAuto.AutoSize = true;
            panelFiltros.Controls.Add(lblAuto);

            txtFiltroAuto = CrearTextBox(x, 28, 120);
            panelFiltros.Controls.Add(txtFiltroAuto);
            x += 130;

            var btnFiltrarAuto = CrearBoton("Filtrar auto", x, 26, colorAcento, colorPrimario);
            btnFiltrarAuto.Click += (s, e) => FiltrarPorAuto();
            panelFiltros.Controls.Add(btnFiltrarAuto);
            x += 115;

            var sep2 = new Panel();
            sep2.BackColor = Color.FromArgb(226, 232, 240);
            sep2.Location = new Point(x, 10);
            sep2.Size = new Size(1, 38);
            panelFiltros.Controls.Add(sep2);
            x += 15;

            var btnTodos = CrearBoton("Ver Todos", x, 26, Color.FromArgb(226, 232, 240), colorTexto);
            btnTodos.Click += (s, e) => CargarDatos();
            panelFiltros.Controls.Add(btnTodos);

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
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(226, 232, 240);
            dgv.BackgroundColor = colorPanel;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = colorHeader;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = colorAcento;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.ForeColor = colorTexto;
            dgv.DefaultCellStyle.Padding = new Padding(8, 4, 0, 4);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 243, 199);
            dgv.DefaultCellStyle.SelectionForeColor = colorTexto;
            dgv.RowTemplate.Height = 34;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = colorFilaImpar;
            dgv.RowsDefaultCellStyle.BackColor = colorFilaPar;

            panelTabla.Controls.Add(dgv);

            // Footer
            var panelFooter = new Panel();
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Height = 36;
            panelFooter.BackColor = colorPanel;
            panelFooter.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(226, 232, 240), 1),
                    0, 0, panelFooter.Width, 0);
            };

            lblTotal = new Label();
            lblTotal.Text = "Total: 0 registros";
            lblTotal.Font = new Font("Segoe UI", 9f);
            lblTotal.ForeColor = colorSubTexto;
            lblTotal.Location = new Point(20, 10);
            lblTotal.AutoSize = true;
            panelFooter.Controls.Add(lblTotal);

            this.Controls.Add(panelTabla);
            this.Controls.Add(panelFooter);
            this.Controls.Add(panelFiltros);
            this.Controls.Add(panelHeader);
        }

        private TextBox CrearTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = width,
                Font = new Font("Segoe UI", 9f),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = colorFondo,
                ForeColor = colorTexto
            };
        }

        private Button CrearBoton(string texto, int x, int y, Color back, Color fore)
        {
            var btn = new Button();
            btn.Text = texto;
            btn.Location = new Point(x, y);
            btn.Size = new Size(105, 28);
            btn.BackColor = back;
            btn.ForeColor = fore;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void CargarDatos()
        {
            var client = new RestClient(BASE_URL);
            var request = new RestRequest("/mantenimientos/", Method.Get);
            var response = client.Execute(request);
            if (!response.IsSuccessful) return;
            MostrarDatos(response.Content);
        }

        private void FiltrarPorTipo()
        {
            string tipo = txtFiltroTipo.Text.Trim();
            if (string.IsNullOrEmpty(tipo)) { CargarDatos(); return; }
            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/mantenimientos/filtrar?tipo={tipo}", Method.Get);
            var response = client.Execute(request);
            if (!response.IsSuccessful) return;
            MostrarDatos(response.Content);
        }

        private void FiltrarPorAuto()
        {
            string idAuto = txtFiltroAuto.Text.Trim();
            if (string.IsNullOrEmpty(idAuto)) { CargarDatos(); return; }
            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/mantenimientos/auto?idAuto={idAuto}", Method.Get);
            var response = client.Execute(request);
            if (!response.IsSuccessful) return;
            MostrarDatos(response.Content);
        }

        private void MostrarDatos(string json)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            dgv.Columns.Add("id", "ID");
            dgv.Columns.Add("tipo", "Tipo");
            dgv.Columns.Add("descripcion", "Descripción");
            dgv.Columns.Add("costo", "Costo");
            dgv.Columns.Add("fecha", "Fecha");
            dgv.Columns.Add("km", "Kilometraje");
            dgv.Columns.Add("auto", "Automóvil");
            dgv.Columns.Add("marcaAuto", "Marca Auto");

            var lista = JsonSerializer.Deserialize<JsonArray>(json);
            foreach (var item in lista)
            {
                string idAuto = "Sin auto";
                string marcaAuto = "";

                if (item["idAutomovilGasolina"] != null && item["idAutomovilGasolina"].ToString() != "null")
                {
                    idAuto = item["idAutomovilGasolina"]?.ToString();
                }
                else if (item["automovilGasolina"] != null && item["automovilGasolina"].ToString() != "null")
                {
                    idAuto = item["automovilGasolina"]["id"]?.ToString();
                }

                if (item["marcaAutomovilGasolina"] != null && item["marcaAutomovilGasolina"].ToString() != "null")
                {
                    marcaAuto = item["marcaAutomovilGasolina"]?.ToString();
                }
                else if (item["automovilGasolina"] != null && item["automovilGasolina"].ToString() != "null")
                {
                    marcaAuto = item["automovilGasolina"]["marca"]?.ToString();
                }

                dgv.Rows.Add(
                    item["idMantenimiento"]?.ToString(),
                    item["tipo"]?.ToString(),
                    item["descripcion"]?.ToString(),
                    item["costo"]?.ToString(),
                    item["fechaMantenimiento"]?.ToString(),
                    item["kilometraje"]?.ToString(),
                    idAuto,
                    marcaAuto
                );
            }
            lblTotal.Text = $"Total: {lista.Count} registros";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AutoObservable.GetInstancia().AgregarObserver(this);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            AutoObservable.GetInstancia().EliminarObserver(this);
        }

        public void Actualizar() => CargarDatos();
    }
}