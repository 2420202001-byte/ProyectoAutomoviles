using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Drawing;

namespace AutoMovilAppCliente
{
    public partial class FormListarGasolina : Form, IObserver
    {
        private const string BASE_URL = "http://localhost:8080";
        private DataGridView dgv;
        private Label lblTotal;
        private TextBox txtFiltroMarca, txtFiltroAnio;

        // Paleta de colores — tono naranja/gasolina
        private readonly Color colorPrimario = Color.FromArgb(28, 14, 5);        // Marrón muy oscuro
        private readonly Color colorAcento = Color.FromArgb(251, 146, 60);      // Naranja gasolina
        private readonly Color colorFondo = Color.FromArgb(250, 245, 240);     // Crema claro
        private readonly Color colorPanel = Color.FromArgb(255, 255, 255);     // Blanco
        private readonly Color colorTexto = Color.FromArgb(41, 20, 5);         // Marrón oscuro
        private readonly Color colorSubTexto = Color.FromArgb(130, 100, 80);      // Marrón medio
        private readonly Color colorFilaImpar = Color.FromArgb(255, 250, 245);
        private readonly Color colorFilaPar = Color.White;
        private readonly Color colorHeader = Color.FromArgb(28, 14, 5);

        public FormListarGasolina()
        {
            CrearFormulario();
            CargarDatos();
        }

        private void CrearFormulario()
        {
            this.Text = "AutoMóvil — Vehículos a Gasolina";
            this.Size = new Size(1100, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = colorFondo;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ── HEADER ──────────────────────────────────────────────
            var panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 65;
            panelHeader.BackColor = colorPrimario;

            var lblTitulo = new Label();
            lblTitulo.Text = "⛽  Listado de Automóviles a Gasolina";
            lblTitulo.Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold);
            lblTitulo.ForeColor = colorAcento;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 18);
            panelHeader.Controls.Add(lblTitulo);

            // ── BARRA DE FILTROS ─────────────────────────────────────
            var panelFiltros = new Panel();
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Height = 60;
            panelFiltros.BackColor = colorPanel;
            panelFiltros.Padding = new Padding(15, 10, 15, 10);

            panelFiltros.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(
                    new Pen(Color.FromArgb(226, 220, 210), 1),
                    0, panelFiltros.Height - 1,
                    panelFiltros.Width, panelFiltros.Height - 1
                );
            };

            int x = 15;

            var lblMarca = new Label();
            lblMarca.Text = "Marca";
            lblMarca.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblMarca.ForeColor = colorSubTexto;
            lblMarca.Location = new Point(x, 12);
            lblMarca.AutoSize = true;
            panelFiltros.Controls.Add(lblMarca);

            txtFiltroMarca = CrearTextBox(x, 28, 140);
            panelFiltros.Controls.Add(txtFiltroMarca);
            x += 150;

            var btnFiltrarMarca = CrearBoton("Filtrar marca", x, 26, colorAcento, colorPrimario);
            btnFiltrarMarca.Click += (s, e) => FiltrarPorMarca();
            panelFiltros.Controls.Add(btnFiltrarMarca);
            x += btnFiltrarMarca.Width + 25;

            var sep = new Panel();
            sep.BackColor = Color.FromArgb(226, 220, 210);
            sep.Location = new Point(x, 10);
            sep.Size = new Size(1, 38);
            panelFiltros.Controls.Add(sep);
            x += 15;

            var lblAnio = new Label();
            lblAnio.Text = "Año";
            lblAnio.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblAnio.ForeColor = colorSubTexto;
            lblAnio.Location = new Point(x, 12);
            lblAnio.AutoSize = true;
            panelFiltros.Controls.Add(lblAnio);

            txtFiltroAnio = CrearTextBox(x, 28, 90);
            panelFiltros.Controls.Add(txtFiltroAnio);
            x += 100;

            var btnFiltrarAnio = CrearBoton("Filtrar año", x, 26, colorAcento, colorPrimario);
            btnFiltrarAnio.Click += (s, e) => FiltrarPorAnio();
            panelFiltros.Controls.Add(btnFiltrarAnio);
            x += btnFiltrarAnio.Width + 25;

            var sep2 = new Panel();
            sep2.BackColor = Color.FromArgb(226, 220, 210);
            sep2.Location = new Point(x, 10);
            sep2.Size = new Size(1, 38);
            panelFiltros.Controls.Add(sep2);
            x += 15;

            var btnTodos = CrearBoton("Ver Todos", x, 26, Color.FromArgb(240, 232, 220), colorTexto);
            btnTodos.Click += (s, e) => CargarDatos();
            panelFiltros.Controls.Add(btnTodos);

            // ── PANEL TABLA ──────────────────────────────────────────
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
            dgv.GridColor = Color.FromArgb(230, 220, 210);
            dgv.BackgroundColor = colorPanel;
            dgv.RowHeadersVisible = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = colorHeader;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = colorAcento;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = colorHeader;
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.EnableHeadersVisualStyles = false;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.ForeColor = colorTexto;
            dgv.DefaultCellStyle.Padding = new Padding(8, 4, 0, 4);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 230, 200);
            dgv.DefaultCellStyle.SelectionForeColor = colorTexto;
            dgv.RowTemplate.Height = 34;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = colorFilaImpar;
            dgv.RowsDefaultCellStyle.BackColor = colorFilaPar;

            panelTabla.Controls.Add(dgv);

            // ── FOOTER ───────────────────────────────────────────────
            var panelFooter = new Panel();
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Height = 36;
            panelFooter.BackColor = colorPanel;
            panelFooter.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(
                    new Pen(Color.FromArgb(226, 220, 210), 1),
                    0, 0, panelFooter.Width, 0
                );
            };

            lblTotal = new Label();
            lblTotal.Text = "Total: 0 registros";
            lblTotal.Font = new Font("Segoe UI", 9f);
            lblTotal.ForeColor = colorSubTexto;
            lblTotal.Location = new Point(20, 10);
            lblTotal.AutoSize = true;
            panelFooter.Controls.Add(lblTotal);

            // ── AGREGAR AL FORM ──────────────────────────────────────
            this.Controls.Add(panelTabla);
            this.Controls.Add(panelFooter);
            this.Controls.Add(panelFiltros);
            this.Controls.Add(panelHeader);
        }

        // ── HELPERS ─────────────────────────────────────────────────
        private TextBox CrearTextBox(int x, int y, int width)
        {
            var tb = new TextBox();
            tb.Location = new Point(x, y);
            tb.Width = width;
            tb.Font = new Font("Segoe UI", 9f);
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.BackColor = colorFondo;
            tb.ForeColor = colorTexto;
            return tb;
        }

        private Button CrearBoton(string texto, int x, int y, Color back, Color fore)
        {
            var btn = new Button();
            btn.Text = texto;
            btn.Location = new Point(x, y);
            btn.AutoSize = false;
            btn.Size = new Size(100, 28);
            btn.BackColor = back;
            btn.ForeColor = fore;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        // ── LÓGICA ───────────────────────────────────────────────────
        private void CargarDatos()
        {
            var client = new RestClient(BASE_URL);
            var request = new RestRequest("/gasolina/", Method.Get);
            var response = client.Execute(request);
            if (!response.IsSuccessful) return;
            MostrarDatos(response.Content);
        }

        private void FiltrarPorMarca()
        {
            string marca = txtFiltroMarca.Text.Trim();
            if (string.IsNullOrEmpty(marca)) { CargarDatos(); return; }
            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/gasolina/filtrar?marca={marca}", Method.Get);
            var response = client.Execute(request);
            if (!response.IsSuccessful) return;
            MostrarDatos(response.Content);
        }

        private void FiltrarPorAnio()
        {
            string anio = txtFiltroAnio.Text.Trim();
            if (string.IsNullOrEmpty(anio)) { CargarDatos(); return; }
            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"/gasolina/filtrar?anio={anio}", Method.Get);
            var response = client.Execute(request);
            if (!response.IsSuccessful) return;
            MostrarDatos(response.Content);
        }

        private void MostrarDatos(string json)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            dgv.Columns.Add("id", "ID");
            dgv.Columns.Add("marca", "Marca");
            dgv.Columns.Add("modelo", "Modelo");
            dgv.Columns.Add("anio", "Año");
            dgv.Columns.Add("color", "Color");
            dgv.Columns.Add("precio", "Precio");
            dgv.Columns.Add("consumo", "Consumo (L/100km)");
            dgv.Columns.Add("tanque", "Tanque (L)");
            dgv.Columns.Add("cilindraje", "Cilindraje");
            dgv.Columns.Add("combustible", "Combustible");

            var lista = JsonSerializer.Deserialize<JsonArray>(json);
            foreach (var item in lista)
            {
                dgv.Rows.Add(
                    item["id"]?.ToString(),
                    item["marca"]?.ToString(),
                    item["modelo"]?.ToString(),
                    item["anio"]?.ToString(),
                    item["color"]?.ToString(),
                    item["precio"]?.ToString(),
                    item["consumoLitrosPor100Km"]?.ToString(),
                    item["capacidadTanqueLitros"]?.ToString(),
                    item["cilindraje"]?.ToString(),
                    item["tipoCombustible"]?.ToString()
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