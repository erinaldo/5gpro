﻿using _5gpro.Daos;
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
    public partial class fmBuscaGrupoPessoa : Form
    {

        List<GrupoPessoa> listagrupopessoa;
        public GrupoPessoa grupoPessoaSelecionado = null;
        static ConexaoDAO connection = new ConexaoDAO();
        GrupoPessoaDAO grupoPessoaDAO = new GrupoPessoaDAO(connection);

        public fmBuscaGrupoPessoa()
        {
            InitializeComponent();
        }

        private void BuscaGrupoPessoa()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Código", typeof(string));
            table.Columns.Add("Nome", typeof(string));

            listagrupopessoa = grupoPessoaDAO.BuscaTodos(tbNomeGrupoPessoa.Text).ToList();

            foreach (GrupoPessoa g in listagrupopessoa)
            {
                table.Rows.Add(g.GrupoPessoaID, g.Nome);
            }
            dgvGrupoPessoa.DataSource = table;
        }

        private void FmBuscaGrupoPessoa_Load(object sender, EventArgs e)
        {
            BuscaGrupoPessoa();
        }

        private void TbNomeGrupoPessoa_TextChanged(object sender, EventArgs e)
        {
            BuscaGrupoPessoa();
        }

        private void BtPesquisar_Click(object sender, EventArgs e)
        {
            BuscaGrupoPessoa();
        }

        private void DgvGrupoPessoa_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = dgvGrupoPessoa.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dgvGrupoPessoa.Rows[selectedRowIndex];
            grupoPessoaSelecionado = listagrupopessoa.Find(g => g.GrupoPessoaID == Convert.ToInt32(selectedRow.Cells[0].Value)); // FAZ UMA BUSCA NA LISTA ONDE A CONDIÇÃO É ACEITA
            this.Close();
        }
    }
}