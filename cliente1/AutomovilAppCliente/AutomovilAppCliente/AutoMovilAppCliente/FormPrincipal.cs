using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoMovilAppCliente
{
    public partial class FormPrincipal : Form
    {
        // Paleta de colores neutra y moderna para el Dashboard
        private readonly Color colorFondo = Color.FromArgb(241, 245, 249);  // Slate 100
        private readonly Color colorHeader = Color.FromArgb(15, 23, 42);    // Slate 900
        private readonly Color colorAcento = Color.FromArgb(37, 99, 235);   // Blue 600
        private readonly Color colorTarjeta = Color.White;
        private readonly Color colorTextoPrincipal = Color.FromArgb(30, 41, 59); // Slate 800
        private readonly Color colorTextoSecundario = Color.FromArgb(100, 116, 139); // Slate 500

        public FormPrincipal()
        {
            Text = "AutoGestión S.A.S - Sistema de Gestión";
            Size = new Size(600, 520);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = colorFondo;

            // ── Menú Superior ────────────────────────────────────────────
            var menu = new MenuStrip
            {
                BackColor = colorHeader,
                ForeColor = Color.White,
                Padding = new Padding(5, 8, 0, 8),
                Font = new Font("Segoe UI", 9.5f)
            };

            var mnuBateria = new ToolStripMenuItem("🔋 Batería");
            mnuBateria.DropDownItems.Add("➕  Adicionar", null, (s, e) => new FormAdicionarBateria().Show());
            mnuBateria.DropDownItems.Add("🔍  Buscar", null, (s, e) => new FormBuscarBateria().Show());
            mnuBateria.DropDownItems.Add("✏️   Actualizar", null, (s, e) => new FormActualizarBateria().Show());
            mnuBateria.DropDownItems.Add("🗑️   Eliminar", null, (s, e) => new FormEliminarBateria().Show());
            mnuBateria.DropDownItems.Add("-"); // Separador
            mnuBateria.DropDownItems.Add("📋  Listar", null, (s, e) => new FormListarBateria().Show());

            var mnuElectrico = new ToolStripMenuItem("⚡ Eléctrico");
            mnuElectrico.DropDownItems.Add("➕  Adicionar", null, (s, e) => new FormAdicionarElectrico().Show());
            mnuElectrico.DropDownItems.Add("🔍  Buscar", null, (s, e) => new FormBuscarElectrico().Show());
            mnuElectrico.DropDownItems.Add("✏️   Actualizar", null, (s, e) => new FormActualizarElectrico().Show());
            mnuElectrico.DropDownItems.Add("🗑️   Eliminar", null, (s, e) => new FormEliminarElectrico().Show());
            mnuElectrico.DropDownItems.Add("-");
            mnuElectrico.DropDownItems.Add("📋  Listar", null, (s, e) => new FormListarElectrico().Show());

            var mnuGasolina = new ToolStripMenuItem("⛽ Gasolina");
            mnuGasolina.DropDownItems.Add("➕  Adicionar", null, (s, e) => new FormAdicionarGasolina().Show());
            mnuGasolina.DropDownItems.Add("🔍  Buscar", null, (s, e) => new FormBuscarGasolina().Show());
            mnuGasolina.DropDownItems.Add("✏️   Actualizar", null, (s, e) => new FormActualizarGasolina().Show());
            mnuGasolina.DropDownItems.Add("🗑️   Eliminar", null, (s, e) => new FormEliminarGasolina().Show());
            mnuGasolina.DropDownItems.Add("-");
            mnuGasolina.DropDownItems.Add("📋  Listar", null, (s, e) => new FormListarGasolina().Show());

            var mnuMantenimiento = new ToolStripMenuItem("🔧 Mantenimiento");
            mnuMantenimiento.DropDownItems.Add("➕  Adicionar", null, (s, e) => new FormAdicionarMantenimiento().Show());
            mnuMantenimiento.DropDownItems.Add("🔍  Buscar", null, (s, e) => new FormBuscarMantenimiento().Show());
            mnuMantenimiento.DropDownItems.Add("✏️   Actualizar", null, (s, e) => new FormActualizarMantenimiento().Show());
            mnuMantenimiento.DropDownItems.Add("🗑️   Eliminar", null, (s, e) => new FormEliminarMantenimiento().Show());
            mnuMantenimiento.DropDownItems.Add("-");
            mnuMantenimiento.DropDownItems.Add("📋  Listar", null, (s, e) => new FormListarMantenimiento().Show());

            var mnuAyuda = new ToolStripMenuItem("❓ Ayuda");
            mnuAyuda.DropDownItems.Add("Acerca de...", null, (s, e) =>
                MessageBox.Show(
                    "🚗 AutoGestión S.A.S\nVersión 1.0\n\nDesarrollo de Aplicaciones Empresariales\nUniversidad de Ibagué — 2026A\n\nIntegrantes:\n• Yaser Rondón\n• Ismael Cardozo\n• Juan Mancipe",
                    "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information));

            menu.Items.AddRange(new ToolStripItem[] { mnuElectrico, mnuGasolina, mnuBateria, mnuMantenimiento, mnuAyuda });

            // ── Header Visual (Hero) ─────────────────────────────────────
            var pnlHeader = new Panel
            {
                BackColor = colorAcento,
                Dock = DockStyle.Top,
                Height = 100
            };

            var lblTitulo = new Label
            {
                Text = "🚗 Sistema de Gestión de Automóviles",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(25, 30)
            };
            pnlHeader.Controls.Add(lblTitulo);

            // ── Tarjeta de Instrucciones (Card) ──────────────────────────
            var pnlCard = new Panel
            {
                BackColor = colorTarjeta,
                Size = new Size(520, 260),
                Location = new Point(30, 150),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblInstruccion = new Label
            {
                Text = "Guía rápida de operaciones:",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = colorTextoPrincipal,
                Location = new Point(20, 20),
                AutoSize = true
            };
            pnlCard.Controls.Add(lblInstruccion);

            var lblSubInstruccion = new Label
            {
                Text = "Utilice el menú superior oscuro para navegar entre los módulos.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = colorTextoSecundario,
                Location = new Point(20, 50),
                AutoSize = true
            };
            pnlCard.Controls.Add(lblSubInstruccion);

            string[] opciones = {
                "➕   Adicionar     Registrar un nuevo elemento en la base de datos.",
                "🔍   Buscar        Consultar información detallada mediante ID.",
                "✏️    Actualizar   Modificar los registros existentes.",
                "🗑️    Eliminar     Borrar definitivamente un registro del sistema.",
                "📋   Listar        Visualizar la tabla completa con todos los datos."
            };

            int yPos = 95;
            foreach (var op in opciones)
            {
                pnlCard.Controls.Add(new Label
                {
                    Text = op,
                    Font = new Font("Segoe UI", 10f),
                    Location = new Point(20, yPos),
                    Width = 480,
                    Height = 25,
                    ForeColor = colorTextoPrincipal
                });
                yPos += 30;
            }

            // ── Footer ───────────────────────────────────────────────────
            var lblServidor = new Label
            {
                Text = "🟢 Servidor conectado: http://localhost:8080",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.SeaGreen,
                Location = new Point(30, 435),
                AutoSize = true
            };

            // ── Ensamblaje del Formulario ────────────────────────────────
            Controls.Add(pnlCard);
            Controls.Add(pnlHeader);
            Controls.Add(menu);
            Controls.Add(lblServidor);
            MainMenuStrip = menu;
        }
    }
}