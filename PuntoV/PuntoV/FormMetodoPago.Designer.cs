using System.Drawing;

namespace PuntoV
{
    partial class FormMetodoPago
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEfectivo = new System.Windows.Forms.Button();
            this.btnCredito = new System.Windows.Forms.Button();
            this.btnTransferencia = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEfectivo
            // 
            this.btnEfectivo.BackColor = System.Drawing.Color.SeaGreen;
            this.btnEfectivo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnEfectivo.ForeColor = System.Drawing.Color.White;
            this.btnEfectivo.Location = new System.Drawing.Point(24, 52);
            this.btnEfectivo.Name = "btnEfectivo";
            this.btnEfectivo.Size = new System.Drawing.Size(160, 90);
            this.btnEfectivo.TabIndex = 2;
            this.btnEfectivo.Text = "Efectivo";
            this.btnEfectivo.UseVisualStyleBackColor = false;
            this.btnEfectivo.Click += new System.EventHandler(this.btnEfectivo_Click_1);
            // 
            // btnCredito
            // 
            this.btnCredito.BackColor = System.Drawing.Color.DarkOrange;
            this.btnCredito.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCredito.ForeColor = System.Drawing.Color.White;
            this.btnCredito.Location = new System.Drawing.Point(394, 52);
            this.btnCredito.Name = "btnCredito";
            this.btnCredito.Size = new System.Drawing.Size(160, 90);
            this.btnCredito.TabIndex = 1;
            this.btnCredito.Text = "Crédito Cliente";
            this.btnCredito.UseVisualStyleBackColor = false;
            this.btnCredito.Click += new System.EventHandler(this.btnCredito_Click_1);
            // 
            // btnTransferencia
            // 
            this.btnTransferencia.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnTransferencia.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnTransferencia.ForeColor = System.Drawing.Color.White;
            this.btnTransferencia.Location = new System.Drawing.Point(210, 52);
            this.btnTransferencia.Name = "btnTransferencia";
            this.btnTransferencia.Size = new System.Drawing.Size(160, 90);
            this.btnTransferencia.TabIndex = 0;
            this.btnTransferencia.Text = "Transferencia";
            this.btnTransferencia.UseVisualStyleBackColor = false;
            this.btnTransferencia.Click += new System.EventHandler(this.btnTransferencia_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(147, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "¿Cómo se realizó el pago?";
            // 
            // FormMetodoPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 194);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTransferencia);
            this.Controls.Add(this.btnCredito);
            this.Controls.Add(this.btnEfectivo);
            this.Name = "FormMetodoPago";
            this.Text = "Selecciona método de pago";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEfectivo;
        private System.Windows.Forms.Button btnCredito;
        private System.Windows.Forms.Button btnTransferencia;
        private System.Windows.Forms.Label label1;
    }
}