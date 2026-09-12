using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuntoV
{
    public partial class FormPriccs : Form
    {
        public FormPriccs()
        {
            InitializeComponent();
            AbrirFormularioEnPanel(new FVenta());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            AbrirFormularioEnPanel(new Producto());
        }
        private void AbrirFormularioEnPanel(Form formHijo)
        {
            panelContenedor.Controls.Clear();

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;

            panelContenedor.Controls.Add(formHijo);
            formHijo.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FVenta());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FConfig());
        }

        private void buttonInv_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FInventario());
        }
    }
}
