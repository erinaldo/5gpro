﻿using _5gpro.Bll;
using _5gpro.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5gpro.Forms
{
    public partial class formCadastroPais : Form
    {
        public formCadastroPais()
        {
            InitializeComponent();
        }

        private void salvar(Pais pais)
        {
            paisBLL pbl = new paisBLL();

            pais.idpais = int.Parse(tbIdpais.Text);
            pais.nome = tbNomepais.Text;
            pais.sigla = tbSigla.Text;

            pbl.salvar(pais);

            MessageBox.Show("País adicionado com sucesso!");


        }

        private void btSavepais_Click(object sender, EventArgs e)
        {
            Pais pais = new Pais();
            salvar(pais);
        }
    }
}