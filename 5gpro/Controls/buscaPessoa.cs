﻿using _5gpro.Daos;
using _5gpro.Entities;
using _5gpro.Forms;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace _5gpro.Controls
{
    public partial class BuscaPessoa : UserControl
    {
        public Pessoa pessoa = null;
        public int atuacao; //será utilizado para filtrar só fornecedor ou só cliente

        private readonly PessoaDAO pessoaDAO = new PessoaDAO();

        [Description("Texto do Label"), Category("Appearance")]
        public string LabelText
        {
            get
            {
                // Insert code here.
                return lbPessoa.Text;
            }
            set
            {
                lbPessoa.Text = value;
                if (value == "Cliente")
                {
                    atuacao = 1;
                }
                else if (value == "Fornecedor")
                {
                    atuacao = 2;
                }
                else if (value == "Vendedor(a)")
                {
                    atuacao = 3;
                }
            }
        }



        public BuscaPessoa()
        {
            InitializeComponent();
        }

        private void BtProcuraCliente_Click(object sender, System.EventArgs e)
        {
            AbreTelaBuscaPessoa();
        }

        private void TbCodigoPessoa_Leave(object sender, System.EventArgs e)
        {
            if (!int.TryParse(tbCodigoPessoa.Text, out int codigo)) { tbCodigoPessoa.Clear(); }
            if (tbCodigoPessoa.Text.Length > 0)
            {
                pessoa = pessoaDAO.BuscaByID(int.Parse(tbCodigoPessoa.Text), atuacao);
                PreencheCamposPessoa(pessoa);
            }
            else
            {
                pessoa = null;
                tbNomePessoa.Clear();
            }
        }

        private void TbCodigoPessoa_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                e.Handled = true;
                AbreTelaBuscaPessoa();
            }
        }



        private void AbreTelaBuscaPessoa()
        {
            var buscaPessoa = new fmBuscaPessoa(atuacao);
            buscaPessoa.ShowDialog();
            if (buscaPessoa.pessoaSelecionada != null)
            {
                pessoa = buscaPessoa.pessoaSelecionada;
                PreencheCamposPessoa(pessoa);
            }
        }

        private void PreencheCamposPessoa(Pessoa pessoa)
        {
            if (pessoa != null)
            {
                tbCodigoPessoa.Text = pessoa.PessoaID.ToString();
                tbNomePessoa.Text = pessoa.Nome;
            }
            else
            {
                string pessoa_tipo = "";
                switch (atuacao)
                {
                    case 1:
                        pessoa_tipo = "Cliente";
                        break;
                    case 2:
                        pessoa_tipo = "Fornecedor";
                        break;
                    case 3:
                        pessoa_tipo = "Vendedor(a)";
                        break;
                }
                MessageBox.Show($"{pessoa_tipo} não encontrado no banco de dados",
                $"{pessoa_tipo} não encontrado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                tbCodigoPessoa.Clear();
                tbCodigoPessoa.Focus();
                tbCodigoPessoa.SelectAll();
            }
        }

        public void PreencheCampos(Pessoa pessoa)
        {
            this.pessoa = pessoa;
            tbCodigoPessoa.Text = this.pessoa != null ? this.pessoa.PessoaID.ToString() : "";
            tbNomePessoa.Text = this.pessoa != null ? this.pessoa.Nome : "";
        }

        public void Limpa()
        {
            tbCodigoPessoa.Clear();
            tbNomePessoa.Clear();
            pessoa = null;
        }





        //--------------------------------------------------
        //CRIA O EVENTO Text_Changed DO USERCONTROL
        //--------------------------------------------------
        public delegate void text_changedEventHandler(object sender, EventArgs e);

        [Category("Action")]
        [Description("É acionado quando o conteúdo do código da pessoa é alterado")]
        public event text_changedEventHandler Text_Changed;
        //--------------------------------------------------

        private void TbCodigoPessoa_TextChanged(object sender, EventArgs e)
        {
            this.Text_Changed?.Invoke(this, e);
        }
    }
}
