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
    public partial class FormEditarCantidad : Form
    {
        public decimal NuevaCantidad { get; private set; }
        public FormEditarCantidad(decimal cantidadActual)
        {
            InitializeComponent();
            numericCantidad.Value = cantidadActual;
        }

        

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            NuevaCantidad = numericCantidad.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
