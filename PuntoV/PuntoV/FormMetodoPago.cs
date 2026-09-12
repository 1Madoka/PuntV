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
    public partial class FormMetodoPago : Form
    {
        public string MetodoPago { get; private set; }

        public FormMetodoPago()
        {
            InitializeComponent();
        }


        private void btnEfectivo_Click_1(object sender, EventArgs e)
        {
            MetodoPago = "Efectivo";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnTransferencia_Click_1(object sender, EventArgs e)
        {

            MetodoPago = "Transferencia";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCredito_Click_1(object sender, EventArgs e)
        {

            MetodoPago = "Crédito";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
