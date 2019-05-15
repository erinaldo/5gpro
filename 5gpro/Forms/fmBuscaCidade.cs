﻿using _5gpro.Daos;
using _5gpro.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace _5gpro.Forms
{
    public partial class fmBuscaCidade : Form
    {
        public List<Cidade> Cidades;
        public Cidade cidadeSelecionada;
        private readonly CidadeDAO cidadeDAO = new CidadeDAO();




        public fmBuscaCidade()
        {
            InitializeComponent();
        }

        private void FmBuscaCidade_KeyDown(object sender, KeyEventArgs e)
        {
            EnterTab(this.ActiveControl, e);
        }


        private void BtPesquisar_Click(object sender, EventArgs e)
        {
            BuscaCidades();
        }

        private void DgvCidades_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = dgvCidades.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dgvCidades.Rows[selectedRowIndex];
            cidadeSelecionada = Cidades.Find(c => c.CidadeID == Convert.ToInt32(selectedRow.Cells[0].Value)); // FAZ UMA BUSCA NA LISTA ONDE A CONDIÇÃO É ACEITA
            this.Close();
        }

        private void TbFiltroNomeCidade_TextChanged(object sender, EventArgs e)
        {
            BuscaCidades();
        }



        private void BuscaCidades()
        {
            dgvCidades.Rows.Clear();
            int codEstado = buscaEstado.estado?.EstadoID ?? 0;
            Cidades = cidadeDAO.Busca(codEstado, tbFiltroNomeCidade.Text);
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            foreach (Cidade c in Cidades)
            {
                DataGridViewRow linha = new DataGridViewRow();
                linha.CreateCells(dgvCidades,
                c.CidadeID,
                c.Nome,
                c.Estado.EstadoID,
                c.Estado.Nome,
                c.Estado.Uf
                );
                rows.Add(linha);

                //MÉTODO QUE PRIMEIRO ADICIONA A LINHA A LISTA DEPOIS CRIA AS CÉLULAS
                //rows.Add(new DataGridViewRow());
                //rows[rows.Count - 1].CreateCells(dgvCidades,
                //c.CidadeID,
                //c.Nome,
                //c.Estado.EstadoID,
                //c.Estado.Nome,
                //c.Estado.Uf
                //);
            }
            dgvCidades.Rows.AddRange(rows.ToArray());
            dgvCidades.Refresh();

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

