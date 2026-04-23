using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoMovilAppCliente
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            Text = "AutoGestión S.A.S - Sistema de Gestión de Automóviles";
            Size = new Size(560, 480);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(245, 245, 245);

            // ── Menú ─────────────────────────────────────────────────────
            var menu = new MenuStrip { BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var mnuElectrico = new ToolStripMenuItem("⚡ Eléctrico") { ForeColor = Color.White };
            mnuElectrico.DropDownItems.Add("➕  Adicionar", null, (s, e) => new FormAdicionarElectrico().ShowDialog());
            mnuElectrico.DropDownItems.Add("🔍  Buscar", null, (s, e) => new FormBuscarElectrico().ShowDialog());
            mnuElectrico.DropDownItems.Add("✏️   Actualizar", null, (s, e) => new FormActualizarElectrico().ShowDialog());
            mnuElectrico.DropDownItems.Add("🗑️   Eliminar", null, (s, e) => new FormEliminarElectrico().ShowDialog());
            mnuElectrico.DropDownItems.Add("📋  Listar", null, (s, e) => new FormListarElectrico().ShowDialog());

            var mnuGasolina = new ToolStripMenuItem("⛽ Gasolina") { ForeColor = Color.White };
            mnuGasolina.DropDownItems.Add("➕  Adicionar", null, (s, e) => new FormAdicionarGasolina().ShowDialog());
            mnuGasolina.DropDownItems.Add("🔍  Buscar", null, (s, e) => new FormBuscarGasolina().ShowDialog());
            mnuGasolina.DropDownItems.Add("✏️   Actualizar", null, (s, e) => new FormActualizarGasolina().ShowDialog());
            mnuGasolina.DropDownItems.Add("🗑️   Eliminar", null, (s, e) => new FormEliminarGasolina().ShowDialog());
            mnuGasolina.DropDownItems.Add("📋  Listar", null, (s, e) => new FormListarGasolina().ShowDialog());

            var mnuAyuda = new ToolStripMenuItem("❓ Ayuda") { ForeColor = Color.White };
            mnuAyuda.DropDownItems.Add("Acerca de...", null, (s, e) =>
                MessageBox.Show(
                    "🚗 AutoGestión S.A.S\nVersión 1.0\n\nDesarrollo de Aplicaciones Empresariales\nUniversidad de Ibagué — 2026A\n\nIntegrantes:\n• Yaser Rondón\n• Ismael Cardozo\n• Juan Mancipe",
                    "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information));

            menu.Items.Add(mnuElectrico);
            menu.Items.Add(mnuGasolina);
            menu.Items.Add(mnuAyuda);

            // ── Panel de bienvenida ───────────────────────────────────────
            var lblTitulo = new Label
            {
                Text = "🚗 Sistema de Gestión de Automóviles",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 100, 180),
                Left = 30,
                Top = 60,
                Width = 500,
                Height = 40
            };

            var lblInstruccion = new Label
            {
                Text = "Use el menú superior para navegar entre las funciones:",
                Font = new Font("Segoe UI", 10f),
                Left = 30,
                Top = 110,
                Width = 500,
                Height = 25
            };

            string[] opcionesElectrico = {
                "⚡  Eléctrico — Adicionar, Buscar, Actualizar, Eliminar, Listar"
            };

            string[] opcionesGasolina = {
                "⛽  Gasolina — Adicionar, Buscar, Actualizar, Eliminar, Listar"
            };

            string[] opciones = {
                "➕  Adicionar    — Registrar un nuevo automóvil",
                "🔍  Buscar       — Consultar un automóvil por su ID",
                "✏️   Actualizar  — Modificar datos de un automóvil",
                "🗑️   Eliminar    — Borrar un automóvil del sistema",
                "📋  Listar       — Ver todos los automóviles (con filtros)"
            };

            int yPos = 145;
            foreach (var op in opciones)
            {
                Controls.Add(new Label
                {
                    Text = op,
                    Font = new Font("Segoe UI", 9.5f),
                    Left = 40,
                    Top = yPos,
                    Width = 480,
                    Height = 22,
                    ForeColor = Color.FromArgb(50, 50, 50)
                });
                yPos += 26;
            }

            Controls.Add(new Label
            {
                Text = "Servidor: http://localhost:8080",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = Color.Gray,
                Left = 30,
                Top = 420,
                Width = 300
            });

            Controls.AddRange(new Control[] { menu, lblTitulo, lblInstruccion });
            MainMenuStrip = menu;
        }
    }
}