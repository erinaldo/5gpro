﻿using _5gpro.Daos;
using _5gpro.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5gpro
{
    public partial class fmMain : Form
    {
        public fmMain()
        {
            InitializeComponent();

        }

        private void cadastrosDePessoasToolStripMenuItem_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD:5gpro/Forms/formMain.cs
            formCadastroPessoas formCadPessoas = new formCadastroPessoas();
            formCadPessoas.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cn = new ConexaoDAO();
            cn.AbrirConexao();
        }

        private void cadastroDePaisesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formCadastroPais formCadPais = new formCadastroPais();
            formCadPais.Show();
        }
=======
            var formCadPessoa = new fmCadastroPessoa();
            formCadPessoa.Show(this);
        }

        private void cadastroDeFuncionáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formCadFuncionario = new fmCadastroFuncionario();
            formCadFuncionario.Show(this);
        }

        private void cadastroDeItensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formCadItens = new fmCadastroItens();
            formCadItens.Show(this);
        }
>>>>>>> master:5gpro/Forms/fmMain.cs
    }
}