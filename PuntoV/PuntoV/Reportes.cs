using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuntoV
{
    public partial class Reportes : Form
    {
        readonly PuntoVEntities PV = new PuntoVEntities();
        public Reportes()
        {
            InitializeComponent();
        }
        private void ReporteVentasForm_Load(object sender, EventArgs e)
        {
            using (PuntoVEntities PV = new PuntoVEntities())
            {
                DateTime hoy = DateTime.Today;

                // 🔹 Resumen diario
                var reporteDiario = PV.Ventas
                    .Where(v => DbFunctions.TruncateTime(v.FechaHora) == hoy)
                    .GroupBy(v => 1)
                    .Select(g => new
                    {
                        TotalVendido = g.Sum(v => v.Total),
                        NumeroTickets = g.Count()
                    })
                    .FirstOrDefault();

                var detallesHoy = PV.VentaDetalle
                    .Where(d => DbFunctions.TruncateTime(d.Ventas.FechaHora) == hoy)
                    .Select(d => new
                    {
                        d.Cantidad,
                        d.PrecioUnitario,
                        d.CostoUnitarioVenta // suponiendo que tu tabla Productos tiene campo Costo
                    })
                    .ToList();

                decimal reinversion = detallesHoy.Sum(d => d.CostoUnitarioVenta * d.Cantidad);
                decimal ventasTotales = detallesHoy.Sum(d => d.PrecioUnitario * d.Cantidad);
                decimal ganancias = ventasTotales - reinversion;


                if (reporteDiario != null)
                {
                    labelTotalD.Text = reporteDiario.TotalVendido.ToString("C");
                    labelTickDia.Text = reporteDiario.NumeroTickets.ToString();
                    lblReinversion.Text = reinversion.ToString("C");
                    lblGanancias.Text = ganancias.ToString("C");
                }
                else
                {
                    labelTotalD.Text = "0.00";
                    labelTickDia.Text = "0";
                    lblReinversion.Text = "0.00";
                    lblGanancias.Text = "0.00";
                }

                // 🔹 Detalle por método de pago
                var reportePorMetodo = PV.Ventas
                    .Where(v => DbFunctions.TruncateTime(v.FechaHora) == hoy)
                    .GroupBy(v => v.Metodo_pago)
                    .Select(g => new
                    {
                        MetodoPago = g.Key,
                        Total = g.Sum(v => v.Total),
                        Tickets = g.Count()
                    })
                    .ToList();

                // 🔹 Calcular totales
                decimal totalGeneral = reportePorMetodo.Sum(r => r.Total);
                int ticketsGeneral = reportePorMetodo.Sum(r => r.Tickets);

                // 🔹 Agregar fila de total general como objeto
                reportePorMetodo.Add(new
                {
                    MetodoPago = "Total General",
                    Total = totalGeneral,
                    Tickets = ticketsGeneral
                });

                // 🔹 Asignar al DataGridView
                dataGridReporte.DataSource = reportePorMetodo;

              
            }
        }

        private void dataGridReporte_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dataGridReporte.Rows)
            {
                if (fila.Cells["MetodoPago"].Value != null &&
                    fila.Cells["MetodoPago"].Value.ToString() == "Total General")
                {
                    fila.DefaultCellStyle.BackColor = Color.LightYellow;
                    fila.DefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    fila.DefaultCellStyle.ForeColor = Color.DarkBlue;
                }
            }
        }

        private void buttonDetalles_Click(object sender, EventArgs e)
        {
            panelVENTAS.Visible = false;
            panelDetalles.Visible = true;
        }

        private void buttonRV_Click(object sender, EventArgs e)
        {
            panelVENTAS.Visible = true;
            panelDetalles.Visible = false;
        }
    }
}
