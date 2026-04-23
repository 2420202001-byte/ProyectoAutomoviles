using System;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoMovilAppCliente
{
    public partial class FormListarGasolina : Form, IObserver
    {
        private const string BASE_URL = "http://localhost:8080";
        private DataGridView dgv;
        private Label lblTotal;
        private TextBox txtFiltroMarca, txtFiltroAnio;

        public FormListarGasolina()
        {
            CrearFormulario();
            CargarDatos();
        }

        private void CrearFormulario()
        {
            this.Text = "Listado de Automóviles a Gasolina";
            this.Size = new System.Drawing.Size(900, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            var panelNorth = new FlowLayoutPanel();
            panelNorth.Dock = DockStyle.Top;
            panelNorth.Height = 50;
            panelNorth.Padding = new Padding(10);

            panelNorth.Controls.Add(new Label { Text = "Filtrar por marca:", AutoSize = true });
            txtFiltroMarca = new TextBox { Width = 120 };
            panelNorth.Controls.Add(txtFiltroMarca);

            var btnFiltrarMarca = new Button();
            btnFiltrarMarca.Text = "Filtrar marca";
            btnFiltrarMarca.BackColor = System.Drawing.Color.FromArgb(150, 50, 0);
            btnFiltrarMarca.ForeColor = System.Drawing.Color.White;
            btnFiltrarMarca.Click += (s, e) => FiltrarPorMarca();
            panelNorth.Controls.Add(btnFiltrarMarca);

            panelNorth.Controls.Add(new Label { Text = "Filtrar por año:", AutoSize = true });
            txtFiltroAnio = new TextBox { Width = 80 };
            panelNorth.Controls.Add(txtFiltroAnio);

            var btnFiltrarAnio = new Button();
            btnFiltrarAnio.Text = "Filtrar año";
            btnFiltrarAnio.BackColor = System.Drawing.Color.FromArgb(150, 50, 0);
            btnFiltrarAnio.ForeColor = System.Drawing.Color.White;
            btnFiltrarAnio.Click += (s, e) => FiltrarPorAnio();
            panelNorth.Controls.Add(btnFiltrarAnio);

            var btnTodos = new Button();
            btnTodos.Text = "Ver Todos";
            btnTodos.Click += (s, e) => CargarDatos();
            panelNorth.Controls.Add(btnTodos);

            // Inicializar dgv antes de usarlo
            dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            lblTotal = new Label();
            lblTotal.Dock = DockStyle.Bottom;
            lblTotal.Height = 25;
            lblTotal.Text = "Total: 0 registros";

            this.Controls.Add(dgv);
            this.Controls.Add(lblTotal);
            this.Controls.Add(panelNorth);
        }

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
            lblTotal.Text = "Total: " + lista.Count + " registros";
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

        public void Actualizar()
        {
            CargarDatos();
        }





    }





}