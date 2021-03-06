﻿using _5gpro.Funcoes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace _5gpro.Controls
{
    public partial class DecimalBox : UserControl
    {
        private readonly FuncoesAuxiliares f = new FuncoesAuxiliares();

        private decimal _valor = 0.00m;


        [Description("Valor"), Category("Appearance")]
        public decimal Valor
        {
            get
            {
                return ValorDecimal();
            }
            set
            {

                tbDecimal.Text = value.ToString("############0.00");
            }
        }

        public DecimalBox()
        {
            InitializeComponent();
            tbDecimal.Size = this.Size;
        }

        private void TbDecimal_KeyPress(object sender, KeyPressEventArgs e) => f.ValidaTeclaDigitadaDecimal(e);
        private void TbDecimal_Enter(object sender, EventArgs e)
        {
            tbDecimal.SelectAll();
        }
        private void TbDecimal_Leave(object sender, EventArgs e)
        {
            FormataCampoDecimal((TextBox)sender);
            this._valor = Convert.ToDecimal(tbDecimal.Text);
        }

        private void FormataCampoDecimal(TextBox sender)
        {
            sender.Text = sender.Text.Length > 0 ? Convert.ToDecimal(sender.Text).ToString("############0.00") : "0,00";
        }
        private decimal ValorDecimal()
        {
            decimal ret = 0.00m;

            if (tbDecimal.Text.Length > 0)
                ret = Convert.ToDecimal(tbDecimal.Text);
            return ret;
        }



        //--------------------------------------------------
        //CRIA O EVENTO Text_Changed DO USERCONTROL
        //--------------------------------------------------
        public delegate void valor_changedEventHandler(object sender, EventArgs e);

        [Category("Action")]
        [Description("É acionado quando o conteúdo é alterado")]
        public event valor_changedEventHandler Valor_Changed;
        //--------------------------------------------------

        private void TbDecimal_TextChanged(object sender, EventArgs e)
        {
            this.Valor_Changed?.Invoke(this, e);
        }


    }
}
