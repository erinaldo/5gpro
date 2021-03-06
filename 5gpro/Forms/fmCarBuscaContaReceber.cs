﻿using _5gpro.Daos;
using _5gpro.Entities;
using _5gpro.Funcoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _5gpro.Forms
{
    public partial class fmCarBuscaContaReceber : Form
    {
        public ContaReceber contaReceberSelecionada = null;
        private IEnumerable<ContaReceber> contasReceber;
        private readonly ContaReceberDAO contaReceberDAO = new ContaReceberDAO();
        private bool valorContaFiltro = false;
        private bool dataCadastroFiltro = false;
        private bool dataVencimentoFiltro = false;
        FuncoesAuxiliares funaux = new FuncoesAuxiliares();


        public struct Filtros
        {
            public Pessoa filtroPessoa;
            public Operacao filtroOperacao;
            public decimal filtroValorInicial;
            public decimal filtroValorFinal;
            public DateTime filtroDataCadastroInicial;
            public DateTime filtroDataCadastroFinal;
            public DateTime filtroDataVencimentoInicial;
            public DateTime filtroDataVencimentoFinal;
            public bool usarvalorContaFiltro;
            public bool usardataCadastroFiltro;
            public bool usardataVencimentoFiltro;
        }


        public fmCarBuscaContaReceber()
        {
            InitializeComponent();
            DatasIniciais();
        }

        //LOAD
        private void FmBuscaContaReceber_Load(object sender, EventArgs e)
        {

        }

        //LEAVE
        //KEYUP, KEYDOWN
        private void FmBuscaContaReceber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                return;
            }
            EnterTab(this.ActiveControl, e);
        }

        //CLICK
        private void BtPesquisar_Click(object sender, EventArgs e) => Pesquisar();
        private void DgvContas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = dgvContas.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dgvContas.Rows[selectedRowIndex];
            contaReceberSelecionada = contaReceberDAO.BuscaById(Convert.ToInt32(selectedRow.Cells[0].Value));
            this.Close();
        }

        //TEXTCHANGED
        private void CbValorConta_CheckedChanged(object sender, EventArgs e)
        {
            if (cbValorConta.Checked)
            {
                dbValorInicial.Enabled = true;
                dbValorFinal.Enabled = true;
                valorContaFiltro = true;
            }
            else
            {
                dbValorInicial.Enabled = false;
                dbValorFinal.Enabled = false;
                valorContaFiltro = false;
            }
        }

        private void CbDataCadastro_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDataCadastro.Checked)
            {
                dtpDataCadastroInicial.Enabled = true;
                dtpDataCadastroFinal.Enabled = true;
                dataCadastroFiltro = true;
            }
            else
            {
                dtpDataCadastroInicial.Enabled = false;
                dtpDataCadastroFinal.Enabled = false;
                dataCadastroFiltro = false;
            }
        }

        private void CbDataVencimentoParcela_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDataVencimentoParcela.Checked)
            {
                dtpDataVencimentoInicial.Enabled = true;
                dtpDataVencimentoFinal.Enabled = true;
                dataVencimentoFiltro = true;
            }
            else
            {
                dtpDataVencimentoInicial.Enabled = false;
                dtpDataVencimentoFinal.Enabled = false;
                dataVencimentoFiltro = false;
            }
        }

        //FUNÇÕES
        private void Pesquisar()
        {
            Filtros f = new Filtros
            {
                filtroOperacao = buscaOperacao.operacao,
                filtroPessoa = buscaPessoa.pessoa,
                filtroValorInicial = dbValorInicial.Valor,
                filtroValorFinal = dbValorFinal.Valor,
                filtroDataCadastroInicial = dtpDataCadastroInicial.Value,
                filtroDataCadastroFinal = dtpDataCadastroFinal.Value,
                filtroDataVencimentoInicial = dtpDataVencimentoInicial.Value,
                filtroDataVencimentoFinal = dtpDataVencimentoFinal.Value,
                usardataCadastroFiltro = dataCadastroFiltro,
                usardataVencimentoFiltro = dataVencimentoFiltro,
                usarvalorContaFiltro = valorContaFiltro
            };


            contasReceber = contaReceberDAO.Busca(f);

            dgvContas.Rows.Clear();

            foreach (var cr in contasReceber)
                dgvContas.Rows.Add(cr.ContaReceberID,
                                   cr.Descricao,
                                   cr.Pessoa.Nome,
                                   cr.DataCadastro.ToShortDateString(),
                                   cr.Operacao.Nome,
                                   cr.ValorOriginal,
                                   cr.Multa,
                                   cr.Juros,
                                   cr.Acrescimo,
                                   cr.Desconto,
                                   cr.ValorFinal);

            dgvContas.Refresh();
        }
        private void DatasIniciais()
        {
            dtpDataCadastroInicial.Value = DateTime.Today.AddDays(-30);
            dtpDataVencimentoInicial.Value = DateTime.Today.AddDays(-30);
        }
        private void EnterTab(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }
    }
}
