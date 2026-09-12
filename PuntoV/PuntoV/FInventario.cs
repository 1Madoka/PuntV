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
    public partial class FInventario : Form
    {
        readonly PuntoVEntities PV = new PuntoVEntities();
        // Variable global para alternar el orden
        private bool ordenAscendente = true;
        public FInventario()
        {
            InitializeComponent();
        }

       

        private void FormProductos_Load(object sender, EventArgs e)
        {
            // Traer categorías desde la base
            var categorias = PV.Categorias.OrderBy(x=>x.Categoria).ToList();

            // Insertar opción "Todas" al inicio usando el mismo tipo
            categorias.Insert(0, new Categorias { IDCategoria = 0, Categoria = "Todas" });

            comboCategoria.DataSource = categorias;
            comboCategoria.DisplayMember = "Categoria";   // lo que se muestra
            comboCategoria.ValueMember = "IDCategoria";   // el valor real
            CargarProductos();

            // Suscribir evento de clic en encabezado
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;

            var NoStock = PV.Productos.ToList();

            labelStock.Text = NoStock.Where(x => x.StockActual == 0).Count().ToString();
            labelInversion.Text = NoStock.Where(x => x.StockActual != 0).Sum(x=>x.Costo).ToString("N2");
        }

        private void CargarProductos()
        {
            var listaProductos = PV.Productos
                .Include(p => p.Categorias)
                .Select(p => new
                {
                    p.IDProducto,
                    p.Producto,
                    p.PrecioVenta,
                    p.Costo,
                    p.StockActual,
                    p.Descripcion,
                    p.Categorias.Categoria, 
                    p.CodigoOficial,
                    p.CodigoInterno
                })
                .ToList();

            dataGridView1.DataSource = listaProductos;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nombreColumna = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

            // Traemos los datos primero
            var lista = PV.Productos
                .Include(p => p.Categorias)
                .Select(p => new
                {
                    p.IDProducto,
                    p.Producto,
                    p.PrecioVenta,
                    p.Costo,
                    p.StockActual,
                    p.Descripcion,
                    p.Categorias.Categoria,
                     p.CodigoOficial,
                    p.CodigoInterno
                })
                .ToList(); // <-- importante: materializamos aquí

            // Ahora ordenamos en memoria
            if (ordenAscendente)
            {
                lista = lista.OrderBy(x => x.GetType().GetProperty(nombreColumna).GetValue(x, null)).ToList();
            }
            else
            {
                lista = lista.OrderByDescending(x => x.GetType().GetProperty(nombreColumna).GetValue(x, null)).ToList();
            }

            dataGridView1.DataSource = lista;

            ordenAscendente = !ordenAscendente;
        }

    

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            string producto = txtProducto.Text.Trim();
            string codigoBarras = txtCodigoBarras.Text.Trim();
            int? idCategoria = comboCategoria.SelectedValue as int?;

            var query = PV.Productos.Include(p => p.Categorias).AsQueryable();

            // Solo filtrar por categoría si no es "Todas"
            if (idCategoria > 0)
            {
                query = query.Where(p => p.IDCategoria == idCategoria.Value);
            }
            // Filtrar por producto
            if (!string.IsNullOrEmpty(producto))
            {
                query = query.Where(p => p.Producto.Contains(producto));
            }

            // Filtrar por código de barras
            if (!string.IsNullOrEmpty(codigoBarras))
            {
                query = query.Where(p => p.CodigoOficial.Contains(codigoBarras) || p.CodigoInterno.Contains(codigoBarras));
            }
            

          

            var listaFiltrada = query
                .Select(p => new
                {
                    p.IDProducto,
                    p.Producto,
                    p.PrecioVenta,
                    p.Costo,
                    p.StockActual,
                    p.Descripcion,
                    p.Categorias.Categoria,
                    p.CodigoOficial,
                    p.CodigoInterno
                })
                .ToList();

            dataGridView1.DataSource = listaFiltrada;
        }
    }
}
