using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace PuntoV
{
    public partial class FVenta : Form
    {
        readonly PuntoVEntities PV = new PuntoVEntities();
        public FVenta()
        {
            InitializeComponent();
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            string codigoBarras = txtCodigoBarras.Text.Trim();
            decimal cantidad = Convert.ToDecimal(numericventa.Value);

            if (cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad mayor a 0");
                return;
            }

            using (var context = new PuntoVEntities())
            {
                var producto = context.Productos
                                      .FirstOrDefault(p => p.CodigoOficial == codigoBarras
                                                        || p.CodigoInterno == codigoBarras);

                if (producto == null)
                {
                    MessageBox.Show("Producto no encontrado.");
                    return;
                }

                string Product = producto.Producto;
                decimal PrecioVenta = producto.PrecioVenta;
                decimal importeT = PrecioVenta * cantidad;
                int IDProducto = producto.IDProducto;
                decimal CostoP = producto.Costo;
                // Buscar si ya existe el producto en el DataGridView
                DataGridViewRow filaExistente = null;
                foreach (DataGridViewRow row in dataGridV.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (row.Cells["CodigoB"].Value != null &&
                        row.Cells["CodigoB"].Value.ToString() == codigoBarras)
                    {
                        filaExistente = row;
                        break;
                    }
                }

                if (filaExistente != null)
                {
                    // Actualizar cantidad e importe
                    decimal cantidadActual = Convert.ToDecimal(filaExistente.Cells["Cantidad"].Value);
                    cantidadActual += cantidad;
                    filaExistente.Cells["Cantidad"].Value = cantidadActual;

                    decimal nuevoImporte = PrecioVenta * cantidadActual;
                    filaExistente.Cells["Importe"].Value = nuevoImporte;
                }
                else
                {
                    // Agregar nueva fila con botones
                    int index = dataGridV.Rows.Add(
                        IDProducto,
                        codigoBarras,
                        Product,
                        PrecioVenta,
                        cantidad,
                        importeT,
                        CostoP
                       
                    );

                    // Botón Editar
                    DataGridViewButtonCell btnEditar = new DataGridViewButtonCell();
                    btnEditar.Value = "Editar";
                    btnEditar.Style.BackColor = Color.RoyalBlue;
                    dataGridV.Rows[index].Cells["Editar"] = btnEditar;

                    // Botón Eliminar
                    DataGridViewButtonCell btnEliminar = new DataGridViewButtonCell();
                    btnEliminar.Value = "Eliminar";
                    btnEliminar.Style.BackColor = Color.Red;
                    btnEliminar.Style.ForeColor = Color.White;
                    dataGridV.Rows[index].Cells["Eliminar"] = btnEliminar;



                

                }

                txtCodigoBarras.Text = "";
                txtProducto.Text = "";
                numericventa.Value = 1;

                RecalcularTotales();
                //// Recalcular totales
                //decimal totalPrecio = 0;
                //int totalCantidad = 0;

                //foreach (DataGridViewRow row in dataGridV.Rows)
                //{
                //    if (row.IsNewRow) continue;
                //    totalPrecio += Convert.ToDecimal(row.Cells["Importe"].Value);
                //    totalCantidad += Convert.ToInt32(row.Cells["Cantidad"].Value);
                //}

                //labeltotal.Text = totalPrecio.ToString("N2");
            }
        }
        private void dataGridV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow fila = dataGridV.Rows[e.RowIndex];
            // Verificar si la fila está vacía
            bool filaVacia = true;
            foreach (DataGridViewCell celda in fila.Cells)
            {
                if (celda.Value != null && !string.IsNullOrWhiteSpace(celda.Value.ToString()))
                {
                    filaVacia = false;
                    break;
                }
            }

            if (filaVacia)
            {               
                return;
            }

            if (dataGridV.Columns[e.ColumnIndex].Name == "Editar")
            {
               
                decimal cantidadActual = Convert.ToDecimal(fila.Cells["Cantidad"].Value);
                decimal precio = Convert.ToDecimal(fila.Cells["PrecioU"].Value);

                // Abrir formulario modal
                using (FormEditarCantidad form = new FormEditarCantidad(cantidadActual))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        decimal nuevaCantidad = form.NuevaCantidad;

                        // Actualizar fila
                        fila.Cells["Cantidad"].Value = nuevaCantidad;
                        fila.Cells["Importe"].Value = precio * nuevaCantidad;

                        // Recalcular totales
                        RecalcularTotales();
                    }
                }
            }
            else if (dataGridV.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                dataGridV.Rows.RemoveAt(e.RowIndex);
                RecalcularTotales();
            }
        }

        private void RecalcularTotales()
        {
            decimal totalPrecio = 0;
            int totalCantidad = 0;

            foreach (DataGridViewRow row in dataGridV.Rows)
            {
                if (row.IsNewRow) continue;

                totalPrecio += Convert.ToDecimal(row.Cells["Importe"].Value);
                totalCantidad += Convert.ToInt32(row.Cells["Cantidad"].Value);
            }

            labeltotal.Text = totalPrecio.ToString("N2");
            //labelCantidad.Text = totalCantidad.ToString();
            RecalcularFeria();
        }
        private void RecalcularFeria()
        {
            decimal cantidad = Convert.ToDecimal(numericUpCobrar.Value);
            decimal totalPrecio = Convert.ToDecimal(labeltotal.Text);

            decimal cambio = cantidad- totalPrecio ;

            textBoxFeria.Text = cambio.ToString("N2");

            if (cambio < 0)
            {
                textBoxFeria.ForeColor = Color.Red;
            }
            else
            {
                textBoxFeria.ForeColor = Color.Black;
            }

        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            RecalcularFeria();
        }

        private void buttonCobrar_Click(object sender, EventArgs e)
        {
            using (FormMetodoPago form = new FormMetodoPago())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string metodoPago = form.MetodoPago;

                    // Guardar en la base de datos junto con la venta
                    //MessageBox.Show($"Venta registrada con método de pago: {metodoPago}");

                    // Aquí puedes actualizar tu tabla Ventas con el campo MetodoPago
                    decimal totalPrecio = Convert.ToDecimal(labeltotal.Text);
                    DateTime Ahora = DateTime.Now;

                    // Cultura actual (puedes cambiar a "es-MX" o "es-ES")
                    CultureInfo cultura = CultureInfo.CurrentCulture;
                    Calendar calendario = cultura.Calendar;

                    // Regla para calcular semana (primer día de semana y regla de semana)
                    CalendarWeekRule regla = cultura.DateTimeFormat.CalendarWeekRule;
                    DayOfWeek primerDia = cultura.DateTimeFormat.FirstDayOfWeek;

                    int semanaActual = calendario.GetWeekOfYear(Ahora, regla, primerDia);
                    using (PuntoVEntities PV = new PuntoVEntities())
                    {
                        // Crear objeto Venta
                        Ventas nuevaVenta = new Ventas
                        {
                            FechaHora = Ahora,
                            Total = totalPrecio,
                            Metodo_pago = metodoPago, // "Efectivo", "Transferencia", "Crédito Cliente"
                            VentaY = Ahora.Year,
                            Semana = semanaActual
                        };
                        // Guardar en la base de datos
                        PV.Ventas.Add(nuevaVenta);
                        PV.SaveChanges();
                        int ID = nuevaVenta.IDVenta;
                        // Agregar detalles desde el DataGridView
                        foreach (DataGridViewRow fila in dataGridV.Rows)
                        {
                            if (fila.IsNewRow) continue;

                            VentaDetalle detalle = new VentaDetalle
                            {
                                IDProducto = Convert.ToInt32(fila.Cells["IDProducto"].Value),
                                Cantidad = Convert.ToDecimal(fila.Cells["Cantidad"].Value),
                                PrecioUnitario = Convert.ToDecimal(fila.Cells["PrecioU"].Value),
                                SubTotal = Convert.ToDecimal(fila.Cells["Importe"].Value),
                                IDVenta = ID,
                                CostoUnitarioVenta = Convert.ToDecimal(fila.Cells["CostoP"].Value)

                            };

                            PV.VentaDetalle.Add(detalle);
                        }
                        PV.SaveChanges();

                        foreach (DataGridViewRow fila in dataGridV.Rows)
                        {
                            if (fila.IsNewRow) continue;

                            int idProducto = Convert.ToInt32(fila.Cells["IDProducto"].Value);
                            decimal cantidadVendida = Convert.ToDecimal(fila.Cells["Cantidad"].Value);

                            var producto = PV.Productos.FirstOrDefault(p => p.IDProducto == idProducto);
                            if (producto != null)
                            {
                                if (producto.StockActual >= cantidadVendida)
                                {
                                    // Stock suficiente → descontar normalmente
                                    producto.StockActual -= cantidadVendida;
                                }
                                else
                                {
                                  
                                    // Registrar solo lo que hay
                                    producto.StockActual = 0;
                                }
                            }
                        }
                        PV.SaveChanges();


                        var caja = PV.Caja.FirstOrDefault();
                        if (metodoPago== "Efectivo")
                        {
                            caja.Efectivo += totalPrecio;
                        }
                        else if (metodoPago == "Transferencia")
                        {
                            caja.Trasferencia += totalPrecio;
                        }
                        else if (metodoPago == "Crédito")
                        {
                            caja.Trasferencia += totalPrecio;
                        }
                        PV.SaveChanges();
                        MessageBox.Show("Venta guardada correctamente ✅");
                        Limpiar();
                    }




                }
            }
        }


        private void Limpiar()
        {
            // Limpiar DataGridView
            dataGridV.Rows.Clear();
            // Reiniciar labels
            labeltotal.Text = "0.00";
            labelCantidad.Text = "0";           
            numericUpCobrar.Text = "0.00";
            textBoxFeria.Text = "0.00";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia del formulario de ventas
            FVentaRap formVentas = new FVentaRap();

            // Mostrarlo como ventana independiente
            formVentas.Show(); // abre en paralelo
        }
    }
}
