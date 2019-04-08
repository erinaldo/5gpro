﻿using _5gpro.Bll;
using _5gpro.Entities;
using _5gpro.Funcoes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace _5gpro.Forms
{
    public partial class fmSaidaEmissaoNota : Form
    {
        private readonly NotaFiscalBLL notaFiscalBLL = new NotaFiscalBLL();
        private readonly FuncoesAuxiliares f = new FuncoesAuxiliares();

        private NotaFiscal notaFiscal = new NotaFiscal();
        private NotaFiscalItem itemSelecionado;
        private List<NotaFiscalItem> itens = new List<NotaFiscalItem>();

        bool editando, ignoracheckevent = false;

        public fmSaidaEmissaoNota()
        {
            InitializeComponent();
        }

        private void FmEstoqueEntradaDocumentos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                NovoCadastro();
            }

            if (e.KeyCode == Keys.F2)
            {
                SalvaCadastro();
            }

            if (e.KeyCode == Keys.F5)
            {
                RecarregaDados(notaFiscal);
            }


            EnterTab(this.ActiveControl, e);
        }

        private void FmSaidaEmissaoNota_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                "Aviso de alteração",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
                {

                }
                else
                {
                    e.Cancel = true;
                }
            }
        }



        private void BtNovoItem_Click(object sender, EventArgs e)
        {
            LimpaCampoItem();
            buscaItem.Focus();
            itemSelecionado = null;
            btInserirItem.Text = "Inserir";
        }

        private void BtInserirItem_Click(object sender, EventArgs e)
        {
            NotaFiscalItem item = new NotaFiscalItem();
            item = itemSelecionado ?? buscaItem.nfi;
            InserirItem(item);
        }


        //MENU
        private void MenuVertical_Novo_Clicked(object sender, EventArgs e)
        {
            NovoCadastro();
        }

        private void MenuVertical_Buscar_Clicked(object sender, EventArgs e)
        {
            if (!editando)
            {
                //AbreTelaBuscaOrcamento();
            }
        }

        private void MenuVertical_Salvar_Clicked(object sender, EventArgs e)
        {
            SalvaCadastro();
        }

        private void MenuVertical_Recarregar_Clicked(object sender, EventArgs e)
        {
            RecarregaDados(notaFiscal);
        }

        private void MenuVertical_Anterior_Clicked(object sender, EventArgs e)
        {
            CadastroAnterior();
        }

        private void MenuVertical_Proximo_Clicked(object sender, EventArgs e)
        {
            ProximoCadastro();
        }

        private void MenuVertical_Excluir_Clicked(object sender, EventArgs e)
        {

        }




        private void TbQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (f.ValidaTeclaDigitadaDecimal(e))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbValorUnitItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (f.ValidaTeclaDigitadaDecimal(e))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbValorTotItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (f.ValidaTeclaDigitadaDecimal(e))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbDescontoItemPorc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (f.ValidaTeclaDigitadaDecimal(e))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbDescontoItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (f.ValidaTeclaDigitadaDecimal(e))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbDescontoDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (f.ValidaTeclaDigitadaDecimal(e))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbValorTotalDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (f.ValidaTeclaDigitadaDecimal(e))
            {
                e.Handled = true;
                return;
            }
        }



        private void TbCodigo_Leave(object sender, EventArgs e)
        {
            tbCodigo.Text = tbCodigo.Text == "0" ? "" : tbCodigo.Text;
            if (!editando)
            {
                if (tbCodigo.Text.Length > 0)
                {
                    NotaFiscal newnotafiscal = notaFiscalBLL.BuscaNotaByCod(int.Parse(tbCodigo.Text));
                    if (newnotafiscal != null)
                    {
                        notaFiscal = newnotafiscal;
                        PreencheCampos(notaFiscal);
                        Editando(false);
                    }
                    else
                    {
                        Editando(true);
                        LimpaCampos(false);
                    }
                }
                else if (tbCodigo.Text.Length == 0)
                {
                    ignoracheckevent = true;
                    LimpaCampos(true);
                    ignoracheckevent = false;
                }
            }
            else
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                "Aviso de alteração",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (tbCodigo.Text.Length > 0)
                    {
                        NotaFiscal newnotafiscal = notaFiscalBLL.BuscaNotaByCod(int.Parse(tbCodigo.Text));
                        if (newnotafiscal != null)
                        {
                            notaFiscal = newnotafiscal;
                            PreencheCampos(newnotafiscal);
                            Editando(false);
                        }
                        else
                        {
                            Editando(true);
                            LimpaCampos(false);
                        }
                    }
                    else if (tbCodigo.Text.Length == 0)
                    {
                        ignoracheckevent = true;
                        LimpaCampos(true);
                        ignoracheckevent = false;
                    }
                }
            }
        }

        private void TbQuantidade_Leave(object sender, EventArgs e)
        {
            tbQuantidade.Text = tbQuantidade.Text.Length > 0 ? Convert.ToDecimal(tbQuantidade.Text).ToString("############0.00") : "0,00";
            tbValorTotItem.Text = (Convert.ToDecimal(tbQuantidade.Text) * Convert.ToDecimal(tbValorUnitItem.Text)).ToString("############0.00");
            tbDescontoItem.Text = (Convert.ToDecimal(tbValorTotItem.Text) * Convert.ToDecimal(tbDescontoItemPorc.Text) / 100).ToString("############0.00");
        }

        private void TbValorUnitItem_Leave(object sender, EventArgs e)
        {
            tbValorUnitItem.Text = tbValorUnitItem.Text.Length > 0 ? Convert.ToDecimal(tbValorUnitItem.Text).ToString("############0.00") : "0,00";
            tbValorTotItem.Text = (Convert.ToDecimal(tbQuantidade.Text) * Convert.ToDecimal(tbValorUnitItem.Text)).ToString("############0.00");
            tbDescontoItem.Text = (Convert.ToDecimal(tbValorTotItem.Text) * Convert.ToDecimal(tbDescontoItemPorc.Text) / 100).ToString("############0.00");
        }

        private void TbValorTotItem_Leave(object sender, EventArgs e)
        {
            tbValorTotItem.Text = tbValorTotItem.Text.Length > 0 ? Convert.ToDecimal(tbValorTotItem.Text).ToString("############0.00") : "0,00";
            tbDescontoItem.Text = (Convert.ToDecimal(tbValorTotItem.Text) * Convert.ToDecimal(tbDescontoItemPorc.Text) / 100).ToString("############0.00");
        }

        private void TbDescontoItemPorc_Leave(object sender, EventArgs e)
        {
            tbDescontoItemPorc.Text = tbDescontoItemPorc.Text.Length > 0 ? Convert.ToDecimal(tbDescontoItemPorc.Text).ToString("##0.00") : "0,00";
            tbDescontoItem.Text = (Convert.ToDecimal(tbValorTotItem.Text) * Convert.ToDecimal(tbDescontoItemPorc.Text) / 100).ToString("############0.00");
        }

        private void TbDescontoItem_Leave(object sender, EventArgs e)
        {
            tbDescontoItem.Text = tbDescontoItem.Text.Length > 0 ? Convert.ToDecimal(tbDescontoItem.Text).ToString("############0.00") : "0,00";
            tbDescontoItem.Text = (Convert.ToDecimal(tbValorTotItem.Text) * Convert.ToDecimal(tbDescontoItemPorc.Text) / 100).ToString("############0.00");
        }

        private void TbDescontoDocumento_Leave(object sender, EventArgs e)
        {
            tbDescontoDocumento.Text = tbDescontoDocumento.Text.Length > 0 ? Convert.ToDecimal(tbDescontoDocumento.Text).ToString("############0.00") : "0,00";
            CalculaTotalDocumento();
        }

        private void TbValorTotalDocumento_Leave(object sender, EventArgs e)
        {
            tbValorTotalDocumento.Text = tbValorTotalDocumento.Text.Length > 0 ? Convert.ToDecimal(tbValorTotalDocumento.Text).ToString("############0.00") : "0,00";
        }

        private void BuscaItem_Codigo_Leave(object sender, EventArgs e)
        {
            if (buscaItem.nfi != null)
            {
                DataGridViewRow dr = dgvItens.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Equals(buscaItem.nfi.Item._ItemID.ToString())).FirstOrDefault();
                NotaFiscalItem item;
                if (dr == null)
                {
                    item = buscaItem.nfi;
                    btInserirItem.Text = "Inserir";
                    btExcluirItem.Enabled = false;
                }
                else
                {
                    item = itens.Where(i => i.Item._ItemID == buscaItem.nfi.Item._ItemID).First();
                    btInserirItem.Text = "Alterar";
                    btExcluirItem.Enabled = true;
                }
                PreencheCamposItem(item);

            }
        }
        




        private void TbCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 && !editando)
            {
                e.Handled = true;
                //AbreTelaBuscaDocumento();
            }
        }



        private void BuscaPessoa_Text_Changed(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void DtpEmissao_ValueChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void DtpEntrada_ValueChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void TbQuantidade_TextChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void TbValorUnitItem_TextChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void TbValorTotItem_TextChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void TbDescontoItemPorc_TextChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void TbDescontoItem_TextChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void TbDescontoDocumento_TextChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void TbValorTotalDocumento_TextChanged(object sender, EventArgs e)
        {
            Editando(true);
        }

        private void DgvItens_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvItens.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgvItens.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvItens.Rows[selectedRowIndex];
                itemSelecionado = itens.Find(i => i.Item._ItemID == Convert.ToInt32(selectedRow.Cells[0].Value));
                btInserirItem.Text = "Alterar";
                PreencheCamposItem(itemSelecionado);
                btExcluirItem.Enabled = true;
            }
        }

        private void BuscaItem_Codigo_Changed(object sender, EventArgs e)
        {
            Editando(true);
        }



        private void NovoCadastro()
        {
            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                "Aviso de alteração",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ignoracheckevent = true;
                    LimpaCampos(false);
                    tbCodigo.Text = notaFiscalBLL.BuscaProxCodigoDisponivel();
                    notaFiscal = null;
                    buscaPessoa.Focus();
                    ignoracheckevent = false;
                    Editando(true);
                }
            }
            else
            {
                ignoracheckevent = true;
                LimpaCampos(false);
                tbCodigo.Text = notaFiscalBLL.BuscaProxCodigoDisponivel();
                notaFiscal = null;
                Editando(false);
                buscaPessoa.Focus();
                ignoracheckevent = false;
                Editando(true);
            }
        }

        private void SalvaCadastro()
        {
            if (editando)
            {
                notaFiscal = new NotaFiscal
                {
                    NotaFiscalID = int.Parse(tbCodigo.Text),
                    Pessoa = buscaPessoa.pessoa,
                    DataEmissao = dtpEmissao.Value,
                    DataEntradaSaida = dtpSaida.Value,

                    ValorTotalItens = Convert.ToDecimal(tbValorTotalItens.Text),
                    DescontoTotalItens = Convert.ToDecimal(tbDescontoTotalItens.Text),
                    DescontoDocumento = Convert.ToDecimal(tbDescontoDocumento.Text),
                    ValorTotalDocumento = Convert.ToDecimal(tbValorTotalDocumento.Text),

                    NotaFiscalItem = itens
                };

                int resultado = notaFiscalBLL.SalvarOuAtualizarDocumento(notaFiscal);

                // resultado 0 = nada foi inserido (houve algum erro)
                // resultado 1 = foi inserido com sucesso
                // resultado 2 = foi atualizado com sucesso
                if (resultado == 0)
                {
                    MessageBox.Show("Problema ao salvar o registro",
                    "Problema ao salvar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                }
                else if (resultado == 1)
                {
                    tbAjuda.Text = "Dados salvos com sucesso";
                    Editando(false);
                }
                else if (resultado == 2)
                {
                    tbAjuda.Text = "Dados atualizados com sucesso";
                    Editando(false);
                }
            }
        }

        private void RecarregaDados(NotaFiscal notafiscal)
        {
            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                "Aviso de alteração",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (notafiscal != null)
                    {
                        notafiscal = notaFiscalBLL.BuscaNotaByCod(notafiscal.NotaFiscalID);
                        PreencheCampos(notafiscal);
                    }
                    else
                    {
                        ignoracheckevent = true;
                        LimpaCampos(true);
                        ignoracheckevent = false;
                    }
                    Editando(false);
                }
            }
            else
            {
                if (notafiscal != null)
                {
                    notafiscal = notaFiscalBLL.BuscaNotaByCod(notafiscal.NotaFiscalID);
                    PreencheCampos(notafiscal);
                }
                else
                {
                    ignoracheckevent = true;
                    LimpaCampos(true);
                    ignoracheckevent = false;
                }
            }

        }

        private void ProximoCadastro()
        {
            //Busca o registro com ID maior que o atual preenchido. Só preenche se houver algum registro maior
            //Caso não houver registro com ID maior, verifica se pessoa existe. Se não existir busca o maior anterior ao digitado
            if (!editando && tbCodigo.Text.Length > 0)
            {
                NotaFiscal newnotafiscal = notaFiscalBLL.BuscaProximaNotaFiscal(int.Parse(tbCodigo.Text));
                if (newnotafiscal != null)
                {
                    notaFiscal = newnotafiscal;
                    itens = notaFiscal.NotaFiscalItem.ToList();
                    PreencheCampos(notaFiscal);
                }
            }
            else if (editando && tbCodigo.Text.Length > 0)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
               "Aviso de alteração",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    NotaFiscal newnotafiscal = notaFiscalBLL.BuscaProximaNotaFiscal(int.Parse(tbCodigo.Text));
                    if (newnotafiscal != null)
                    {
                        notaFiscal = newnotafiscal;
                        itens = notaFiscal.NotaFiscalItem.ToList();
                        PreencheCampos(notaFiscal);
                        Editando(false);
                    }
                    else
                    {
                        newnotafiscal = notaFiscalBLL.BuscaNotaFiscalAnterior(int.Parse(tbCodigo.Text));
                        if (newnotafiscal != null)
                        {
                            notaFiscal = newnotafiscal;
                            itens = notaFiscal.NotaFiscalItem.ToList();
                            PreencheCampos(notaFiscal);
                            Editando(false);
                        }
                    }
                }
            }
        }

        private void CadastroAnterior()
        {
            //Busca a orcamento com ID menor que o atual preenchido. Só preenche se houver algum registro menor
            //Caso não houver registro com ID menor, verifica se pessoa existe. Se não existir busca o proximo ao digitado
            if (!editando && tbCodigo.Text.Length > 0)
            {
                NotaFiscal newnotafiscal = notaFiscalBLL.BuscaNotaFiscalAnterior(int.Parse(tbCodigo.Text));
                if (newnotafiscal != null)
                {
                    notaFiscal = newnotafiscal;
                    itens = notaFiscal.NotaFiscalItem.ToList();
                    PreencheCampos(notaFiscal);
                }
            }
            else if (editando && tbCodigo.Text.Length > 0)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
               "Aviso de alteração",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    NotaFiscal newnotafiscal = notaFiscalBLL.BuscaNotaFiscalAnterior(int.Parse(tbCodigo.Text));
                    if (newnotafiscal != null)
                    {
                        notaFiscal = newnotafiscal;
                        itens = notaFiscal.NotaFiscalItem.ToList();
                        PreencheCampos(notaFiscal);
                        Editando(false);
                    }
                    else
                    {
                        newnotafiscal = notaFiscalBLL.BuscaProximaNotaFiscal(int.Parse(tbCodigo.Text));
                        if (newnotafiscal != null)
                        {
                            notaFiscal = newnotafiscal;
                            itens = notaFiscal.NotaFiscalItem.ToList();
                            PreencheCampos(notaFiscal);
                            Editando(false);
                        }
                    }
                }
            }
        }

        private void Editando(bool edit)
        {
            if (!ignoracheckevent)
            {
                editando = edit;
                menuVertical.Editando(edit);
            }
        }

        private void LimpaCampos(bool limpaCod)
        {
            if (limpaCod) { tbCodigo.Clear(); }
            buscaPessoa.Limpa();
            dtpEmissao.Value = DateTime.Now;
            dtpSaida.Value = DateTime.Now;
            tbValorTotalItens.Text = "0,00";
            tbDescontoTotalItens.Text = "0,00";
            tbDescontoDocumento.Text = "0,00";
            tbValorTotalDocumento.Text = "0,00";
            tbAjuda.Text = "";
            dgvItens.Rows.Clear();
            dgvItens.Refresh();
            LimpaCampoItem();
        }

        private void LimpaCampoItem()
        {
            buscaItem.Limpa();
            tbQuantidade.Text = "0,00";
            tbValorUnitItem.Text = "0,00";
            tbValorTotItem.Text = "0,00";
            tbDescontoItemPorc.Text = "0,00";
            tbDescontoItem.Text = "0,00";
        }

        private void InserirItem(NotaFiscalItem item)
        {
            if (item != null)
            {
                item.Quantidade = Convert.ToDecimal(tbQuantidade.Text);
                item.ValorUnitario = Convert.ToDecimal(tbValorUnitItem.Text);
                item.ValorTotal = Convert.ToDecimal(tbValorTotItem.Text);
                item.DescontoPorc = Convert.ToDecimal(tbDescontoItemPorc.Text);
                item.Desconto = Convert.ToDecimal(tbDescontoItem.Text);
                DataGridViewRow dr = dgvItens.Rows.Cast<DataGridViewRow>().Where(r => int.Parse(r.Cells[0].Value.ToString()) == item.Item._ItemID).FirstOrDefault();
                if (dr == null)
                {
                    itens.Add(item);
                    dgvItens.Rows.Add(item.Item._ItemID, item.Item.Descricao, item.Quantidade, item.ValorUnitario, item.ValorTotal, item.DescontoPorc, item.Desconto);
                    btNovoItem.PerformClick();
                }
                else
                {
                    itens.Where(i => i.Item._ItemID == item.Item._ItemID).First().Quantidade = item.Quantidade;
                    itens.Where(i => i.Item._ItemID == item.Item._ItemID).First().ValorUnitario = item.ValorUnitario;
                    itens.Where(i => i.Item._ItemID == item.Item._ItemID).First().ValorTotal = item.ValorTotal;
                    itens.Where(i => i.Item._ItemID == item.Item._ItemID).First().DescontoPorc = item.DescontoPorc;
                    itens.Where(i => i.Item._ItemID == item.Item._ItemID).First().Desconto = item.Desconto;
                    dr.Cells[dgvtbcQuantidade.Index].Value = item.Quantidade;
                    dr.Cells[dgvtbcValorUnitario.Index].Value = item.ValorUnitario;
                    dr.Cells[dgvtbcValorTotalItem.Index].Value = item.ValorTotal;
                    dr.Cells[dgvtbcDescontoPorc.Index].Value = item.DescontoPorc;
                    dr.Cells[dgvtbcDescontoItem.Index].Value = item.Desconto;
                    dgvItens.Update();
                    dgvItens.Refresh();
                }
                CalculaTotalDocumento();
                btExcluirItem.Enabled = false;
            }
            else
            {
                MessageBox.Show("Item não encontrado no banco de dados",
                "Item não encontrado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                buscaItem.Focus();
            }
        }

        private void PreencheCampos(NotaFiscal notafiscal)
        {
            ignoracheckevent = true;
            LimpaCampos(false);
            tbCodigo.Text = notafiscal.NotaFiscalID.ToString();
            buscaPessoa.PreencheCampos(notafiscal.Pessoa);
            dtpEmissao.Value = notafiscal.DataEmissao;
            dtpSaida.Value = notafiscal.DataEntradaSaida;
            tbValorTotalItens.Text = notafiscal.ValorTotalItens.ToString("############0.00");
            tbDescontoTotalItens.Text = notafiscal.DescontoTotalItens.ToString("############0.00");
            tbDescontoDocumento.Text = notafiscal.DescontoDocumento.ToString("############0.00");
            tbValorTotalDocumento.Text = notafiscal.ValorTotalDocumento.ToString("############0.00");
            itens = notafiscal.NotaFiscalItem.ToList();
            PreencheGridItens(itens);
            btInserirItem.Text = "Inserir";
            ignoracheckevent = false;
        }

        private void PreencheGridItens(List<NotaFiscalItem> itens)
        {
            foreach (var i in itens)
            {
                dgvItens.Rows.Add(i.Item._ItemID, i.Item.Descricao, i.Quantidade, i.ValorUnitario, i.ValorTotal, i.DescontoPorc, i.Desconto);
            }
            dgvItens.Refresh();
        }

        public void PreencheCamposItem(NotaFiscalItem item)
        {
            if (item != null)
            {
                buscaItem.PreencheCampos(item);
                tbQuantidade.Text = item.Quantidade.ToString("############0.00");
                tbValorUnitItem.Text = item.ValorUnitario.ToString("############0.00");
                tbValorTotItem.Text = item.ValorTotal.ToString("############0.00");
                tbDescontoItemPorc.Text = item.DescontoPorc.ToString("##0.00");
                tbDescontoItem.Text = item.Desconto.ToString("############0.00");
            }
            else
            {
                MessageBox.Show("Item não encontrado no banco de dados",
                "Item não encontrado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                buscaItem.Focus();
            }
        }

        private void EnterTab(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void CalculaTotalDocumento()
        {
            if (itens.Count > 0)
            {
                tbValorTotalItens.Text = itens.Sum(i => i.ValorTotal).ToString("############0.00");
                tbDescontoTotalItens.Text = itens.Sum(i => i.Desconto).ToString("############0.00");
                tbValorTotalDocumento.Text = (itens.Sum(i => i.ValorTotal) - itens.Sum(i => i.Desconto) - Convert.ToDecimal(tbDescontoDocumento.Text)).ToString("############0.00");
            }
        }
    }
}
