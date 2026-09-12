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
    public partial class Producto : Form
    {
        readonly PuntoVEntities PV = new PuntoVEntities();
        public Producto()
        {
            InitializeComponent();
        
        }
        private void FormProductos_Load(object sender, EventArgs e)
        {
            
                var categorias = PV.Categorias.OrderBy(x => x.Categoria).ToList();
            comboAltaCat.DataSource = categorias;
            comboAltaCat.DisplayMember = "Categoria";
            comboAltaCat.ValueMember = "IDCategoria";

            comboEditarCat.DataSource = categorias;
            comboEditarCat.DisplayMember = "Categoria";
            comboEditarCat.ValueMember = "IDCategoria";


          
            AutoCompleteStringCollection codigos = new AutoCompleteStringCollection();

            using (var context = new PuntoVEntities())
            {
                // Traer ambos campos y unirlos en una sola lista
                var lista = context.Productos
                                   .Select(p => new { p.CodigoInterno, p.CodigoOficial,p.Producto })
                                   .ToList();

                foreach (var item in lista)
                {
                  

                    if (!string.IsNullOrEmpty(item.CodigoInterno))
                        //codigos.Add($"{item.CodigoInterno} - {item.Producto}");
                    codigos.Add(item.CodigoInterno);

                    if (!string.IsNullOrEmpty(item.CodigoOficial))
                        // Concatenar código + nombre como referencia
                        //codigos.Add($"{item.CodigoOficial} - {item.Producto}");
                    codigos.Add(item.CodigoOficial);
                }
            }

            textEditarCodigo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textEditarCodigo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textEditarCodigo.AutoCompleteCustomSource = codigos;



            AutoCompleteStringCollection prod = new AutoCompleteStringCollection();

            using (var context = new PuntoVEntities())
            {
                // Traer ambos campos y unirlos en una sola lista
                var lista = context.Productos
                                   .Select(p => new { p.Producto })
                                   .ToList();

                foreach (var item in lista)
                {
                    if (!string.IsNullOrEmpty(item.Producto))
                        codigos.Add(item.Producto);
                }
            }

            textEditarProd.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textEditarProd.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textEditarProd.AutoCompleteCustomSource = codigos;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelAlta.Visible = false;
            panelEditar.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panelAlta.Visible=true;
            panelEditar.Visible = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string produc =    textAltaProduc.Text;
            string  descrip =  textAltaDescr.Text;
            string codigo =   textAltaCodigo.Text;    
            decimal pventa = Convert.ToDecimal( numAltaPrecio.Value);
            decimal costo = Convert.ToDecimal(numericAltaCosto.Value);
            decimal stock = Convert.ToDecimal(numericAltaStock.Value);
            int categoria = (int) comboAltaCat.SelectedValue;
            string msg = "";
            if (pventa> 0 && costo>0 && produc != "" && codigo != "")
            {

                produc = produc.ToUpper().Trim();
                var pd = PV.Productos.Where(x=> x.Producto == produc && x.CodigoOficial== codigo).ToList();

                if (pd.Count>0)
                {
                    msg = " Ya existe ese codigo de barras";
                }
                else
                {
                    Productos nvo = new Productos()
                    {
                        Descripcion = descrip,
                        IDCategoria = categoria,
                        Costo = costo,
                        CodigoOficial = codigo,
                        LastUpdate = DateTime.Now,
                        PrecioVenta = pventa,
                        Producto = produc,
                        StockActual = stock,
                    };
                    PV.Productos.Add(nvo);
                    PV.SaveChanges();


                    int idGenerado = nvo.IDProducto;

                    var upd = PV.Productos.Where(x=>x.IDProducto==idGenerado).FirstOrDefault();

                    upd.CodigoInterno = "CINT" + upd.IDProducto;

                    PV.SaveChanges();

                    if (idGenerado>0)
                    {
                        msg = "Producto Agregado Correctamente";

                        textAltaProduc.Text ="";
                        textAltaDescr.Text = "";
                        textAltaCodigo.Text = "";
                        numAltaPrecio.Text = "";
                        numericAltaCosto.Text = "";
                        numericAltaStock.Text = "";
                        comboAltaCat.SelectedValue = -1;
                    }
                 
                }
               
            }
            else
            {
              

                if (produc =="")
                {
                    msg += " Escriba un nombre de producto mas largo";                    
                }
                if (codigo == "")
                {
                    msg += " Escriba un codigo de barras";
                }
                if (pventa == 0)
                {
                    msg += " El precio de venta debe ser mayor de 0";
                }
                if (costo == 0)
                {
                    msg += " El costo debe ser mayor de 0";
                }
               
            }
            labelError.Text = msg;
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            string id = textEditID.Text;
            string descrip = textEditarDesc .Text;
         
            decimal pventa = Convert.ToDecimal(numEdiarVent.Value);
            decimal costo = Convert.ToDecimal(numEditarCosto.Value);
            decimal stock = Convert.ToDecimal(numEdiatrStock.Value);

           
            int categoria = (int)comboAltaCat.SelectedValue;
            string msg = "";
            if (pventa > 0 && costo > 0 && id !="")
            {

                int IDPro = Convert.ToInt32(textEditID.Text);

                var upd = PV.Productos.Where(x => x.IDProducto == IDPro).FirstOrDefault();

                upd.Descripcion = descrip;
                upd.LastUpdate = DateTime.Now;
                upd.IDCategoria = categoria;
                upd.PrecioVenta=pventa;
                upd.Costo = costo;
                upd.StockActual = stock;
                PV.SaveChanges();
                
                
                msg = "Producto Editado Correctamente";

            }
            else
            {

                
                if (pventa == 0)
                {
                    msg += " El precio de venta debe ser mayor de 0";
                }
                if (costo == 0)
                {
                    msg += " El costo debe ser mayor de 0";
                }

            }
            labelErrorEdit.Text = msg;
        }
        private void txtCodigoBarras_Leave(object sender, EventArgs e)
        {
            string textoSeleccionado = textEditarCodigo.Text;

            // Extraer solo el código (antes del guion)
            string codigoSolo = textoSeleccionado.Split('-')[0].Trim();

            using (var context = new PuntoVEntities())
            {
                var producto = context.Productos
                                      .FirstOrDefault(p => p.CodigoOficial == codigoSolo
                                                        || p.CodigoInterno == codigoSolo);

                if (producto != null)
                {
                    textEditarProd.Text = producto.Producto;
                    numEditarCosto.Text = producto.Costo.ToString("0.00");
                    numEdiarVent.Text = producto.PrecioVenta.ToString("0.00");
                    numEdiatrStock.Text = producto.StockActual.ToString("0.00");
                    textEditarDesc.Text = producto.Descripcion;
                    comboEditarCat.SelectedValue = producto.IDCategoria;
                    textEditID.Text = producto.IDProducto.ToString();
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
            }
        }

        private void textEditarCodigo_TextChanged(object sender, EventArgs e)
        {
            textEditarProd.Text = "";
            numEditarCosto.Text = "0.00";
            numEdiarVent.Text   = "0.00";
            numEdiatrStock.Text = "0.00";
            textEditarDesc.Text = "";
            comboEditarCat.SelectedIndex = -1;
            textEditID.Text = "";
        }
        //private void textEditarProdu_TextChanged(object sender, EventArgs e)
        //{

        //    string codigo = textEditarCodigo.Text;

        //    var pd = PV.Productos.Where(x => x.CodigoInterno == codigo || x.CodigoOficial == codigo).ToList();



        //}
    }
}
