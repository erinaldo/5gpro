﻿using _5gpro.Daos;
using _5gpro.Entities;
using _5gpro.Funcoes;
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
    public partial class fmCapCadastroConta : Form
    {
        private ParcelaContaPagar parcelaSelecionada = null;
        private bool editando, ignoracheckevent = false;
        private ContaPagar contaPagar = null;
        private List<ParcelaContaPagar> parcelas = new List<ParcelaContaPagar>();

        private readonly FuncoesAuxiliares f = new FuncoesAuxiliares();
        private readonly static ConexaoDAO connection = new ConexaoDAO();
        private readonly ContaPagarDAO contaPagarDAO = new ContaPagarDAO(connection);
        private readonly PessoaDAO pessoaDAO = new PessoaDAO(connection);

        //Controle de Permissões
        private readonly PermissaoDAO permissaoDAO = new PermissaoDAO(connection);
        private Logado logado;
        private readonly LogadoDAO logadoDAO = new LogadoDAO(connection);
        private readonly NetworkAdapter adap = new NetworkAdapter();
        private int Nivel;
        private string CodGrupoUsuario;

        public fmCapCadastroConta()
        {
            InitializeComponent();
            SetarNivel();
        }

        private void FmCapCadastroConta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                Recarrega();
                return;
            }

            if (e.KeyCode == Keys.F1)
            {
                Novo();
                return;
            }

            if (e.KeyCode == Keys.F2)
            {
                Salva();
                return;
            }

            EnterTab(this.ActiveControl, e);
        }
        private void MenuVertical_Novo_Clicked(object sender, EventArgs e) => Novo();
        private void MenuVertical_Buscar_Clicked(object sender, EventArgs e) => Busca();
        private void MenuVertical_Salvar_Clicked(object sender, EventArgs e) => Salva();
        private void MenuVertical_Recarregar_Clicked(object sender, EventArgs e) => Recarrega();
        private void MenuVertical_Anterior_Clicked(object sender, EventArgs e) => Anterior();
        private void MenuVertical_Proximo_Clicked(object sender, EventArgs e) => Proximo();
        private void BtSalvarParcela_Click(object sender, EventArgs e) => SalvaParcela();
        private void DtpDataCadatroConta_ValueChanged(object sender, EventArgs e) => Editando(true);
        private void BuscaPessoa_Text_Changed(object sender, EventArgs e) => Editando(true);
        private void TbCodigoConta_Leave(object sender, EventArgs e) => CarregaDados();
        private void DbValorOriginalParcela_Leave(object sender, EventArgs e) => CalculaTotalParcela();
        private void DbMultaParcela_Leave(object sender, EventArgs e) => CalculaTotalParcela();
        private void DbJurosParcela_Leave(object sender, EventArgs e) => CalculaTotalParcela();
        private void DgvParcelas_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvParcelas.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgvParcelas.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvParcelas.Rows[selectedRowIndex];
                parcelaSelecionada = parcelas.Find(p => p.Sequencia == Convert.ToInt32(selectedRow.Cells[0].Value));
                PreencheCamposParcelas(parcelaSelecionada);
                btSalvarParcela.Enabled = true;
                btNovaParcela.Enabled = false;
            }
        }
        private void BtNovaParcela_Click(object sender, EventArgs e) => InserirParcela();







        private void Novo()
        {
            if (editando)
                return;


            ignoracheckevent = true;
            LimpaCampos(false);
            tbCodigoConta.Text = contaPagarDAO.BuscaProxCodigoDisponivel().ToString();
            contaPagar = null;
            dtpDataCadatroConta.Focus();
            ignoracheckevent = false;
            Editando(true);
        }
        private void Busca()
        {
            if (editando)
                return;
            var buscaContaPagar = new fmBuscaContaPagar();
            buscaContaPagar.ShowDialog();
            if (buscaContaPagar.contaPagarSelecionada != null)
            {
                contaPagar = buscaContaPagar.contaPagarSelecionada;
                PreencheCampos(contaPagar);
            }
        }
        private void Salva()
        {
            if (!editando)
                return;

            contaPagar = new ContaPagar
            {
                ContaPagarID = int.Parse(tbCodigoConta.Text),
                DataCadastro = dtpDataCadatroConta.Value,

                ValorOriginal = dbValorOriginalConta.Valor,
                Multa = dbMultaConta.Valor,
                Juros = dbJurosConta.Valor,
                ValorFinal = dbValorFinalConta.Valor,

                Parcelas = parcelas,
                Pessoa = buscaPessoa.pessoa
            };

            int resultado = contaPagarDAO.SalvaOuAtualiza(contaPagar);

            // resultado 0 = nada foi inserido (houve algum erro)
            // resultado 1 = foi inserido com sucesso
            // resultado 2 = foi atualizado com sucesso
            if (resultado == 0)
            {
                MessageBox.Show("Problema ao salvar o registro",
                "Problema ao salvar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
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
        private void Recarrega()
        {
            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                "Aviso de alteração",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            if (contaPagar != null)
            {
                contaPagar = contaPagarDAO.BuscaById(contaPagar.ContaPagarID);
                contaPagar.Pessoa = pessoaDAO.BuscaById(contaPagar.Pessoa.PessoaID);
                PreencheCampos(contaPagar);
                if (editando)
                    Editando(false);
            }
            else
            {
                ignoracheckevent = true;
                LimpaCampos(true);
                ignoracheckevent = false;
            }

        }
        private void Proximo()
        {
            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                    "Aviso de alteração",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }



            if (tbCodigoConta.Text.Length > 0)
            {
                var newcontaPagar = contaPagarDAO.BuscaProximo(int.Parse(tbCodigoConta.Text));
                if (newcontaPagar != null)
                {
                    newcontaPagar.Pessoa = pessoaDAO.BuscaById(newcontaPagar.Pessoa.PessoaID);
                    contaPagar = newcontaPagar;
                    parcelas = contaPagar.Parcelas.ToList();
                    PreencheCampos(contaPagar);
                    if (editando)
                        Editando(false);
                }
            }
        }
        private void Anterior()
        {
            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                    "Aviso de alteração",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }

            if (tbCodigoConta.Text.Length > 0)
            {
                var newcontaPagar = contaPagarDAO.BuscaAnterior(int.Parse(tbCodigoConta.Text));
                if (newcontaPagar != null)
                {
                    newcontaPagar.Pessoa = pessoaDAO.BuscaById(newcontaPagar.Pessoa.PessoaID);
                    contaPagar = newcontaPagar;
                    parcelas = contaPagar.Parcelas.ToList();
                    PreencheCampos(contaPagar);
                    if (editando)
                        Editando(false);
                }
            }
        }
        private void CarregaDados()
        {
            int codigo = 0;
            if (!int.TryParse(tbCodigoConta.Text, out codigo)) { tbCodigoConta.Clear(); }
            if (contaPagar?.ContaPagarID == codigo)
                return;

            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?", "Aviso de alteração",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }

            if (tbCodigoConta.Text.Length == 0)
            {
                LimpaCampos(true);
                Editando(false);
                return;
            }

            var newcontaPagar = contaPagarDAO.BuscaById(codigo);
            if (newcontaPagar != null)
            {
                newcontaPagar.Pessoa = pessoaDAO.BuscaById(newcontaPagar.Pessoa.PessoaID);
                contaPagar = newcontaPagar;
                PreencheCampos(contaPagar);
                Editando(false);
            }
            else
            {
                Editando(true);
                LimpaCampos(false);
            }

        }
        private void PreencheCampos(ContaPagar contaPagar)
        {
            ignoracheckevent = true;
            LimpaCampos(false);
            tbCodigoConta.Text = contaPagar.ContaPagarID.ToString();
            dtpDataCadatroConta.Value = contaPagar.DataCadastro;
            dbValorOriginalConta.Valor = contaPagar.ValorOriginal;
            dbValorFinalConta.Valor = contaPagar.ValorFinal;
            dbMultaConta.Valor = contaPagar.Multa;
            dbJurosConta.Valor = contaPagar.Juros;
            parcelas = contaPagar.Parcelas.ToList();
            buscaPessoa.PreencheCampos(contaPagar.Pessoa);
            PreencheGridParcelas(parcelas);
            ignoracheckevent = false;
        }
        private void PreencheGridParcelas(List<ParcelaContaPagar> parcelas)
        {
            foreach (var parcela in parcelas)
                dgvParcelas.Rows.Add(parcela.Sequencia,
                                     parcela.DataVencimento.ToShortDateString(),
                                     parcela.Valor.ToString("############0.00"),
                                     parcela.Multa.ToString("############0.00"),
                                     parcela.Multa.ToString("############0.00"),
                                     parcela.ValorFinal.ToString("############0.00"),
                                     parcela.DataQuitacao?.Date);
            dgvParcelas.Refresh();
        }
        private void PreencheCamposParcelas(ParcelaContaPagar parcela)
        {
            tbCodigoParcela.Text = parcela.Sequencia.ToString();
            dtpDataVencimentoParcela.Value = parcela.DataVencimento;
            dbValorOriginalParcela.Valor = parcela.Valor;
            dbMultaParcela.Valor = parcela.Multa;
            dbJurosParcela.Valor = parcela.Juros;
            dbValorFinalParcela.Valor = parcela.ValorFinal;
            tbDataQuitacao.Text = parcela.DataQuitacao != null ? parcela.DataQuitacao.Value.ToShortDateString() : "";
        }
        private void InserirParcela()
        {
            LimpaCamposParcela();
            string codigo = "1";
            if (parcelas.Count > 0)
                codigo = (parcelas.Max(p => p.Sequencia) + 1).ToString();
            tbCodigoParcela.Text = codigo;
            dtpDataVencimentoParcela.Focus();
            btNovaParcela.Enabled = false;
            Editando(true);
        }
        private void CalculaTotalParcela()
        {
            dbValorFinalParcela.Valor = dbValorOriginalParcela.Valor + dbMultaParcela.Valor + dbJurosParcela.Valor;
        }
        private void SalvaParcela()
        {
            if (tbCodigoParcela.Text.Length == 0)
                return;
            var dr = dgvParcelas.Rows.Cast<DataGridViewRow>().Where(r => int.Parse(r.Cells[0].Value.ToString()) == parcelaSelecionada?.Sequencia).FirstOrDefault();
            if (dr == null)
            {
                var parcela = new ParcelaContaPagar()
                {
                    Sequencia = Convert.ToInt32(tbCodigoParcela.Text),
                    DataVencimento = dtpDataVencimentoParcela.Value,
                    Valor = dbValorOriginalParcela.Valor,
                    Multa = dbMultaParcela.Valor,
                    Juros = dbJurosParcela.Valor,
                    FormaPagamento = buscaFormaPagamento.formaPagamento
                };
                parcelas.Add(parcela);
                dgvParcelas.Rows.Add(parcela.Sequencia,
                                     parcela.DataVencimento.ToShortDateString(),
                                     parcela.Valor.ToString("############0.00"),
                                     parcela.Multa.ToString("############0.00"),
                                     parcela.Juros.ToString("############0.00"),
                                     parcela.ValorFinal.ToString("############0.00"),
                                     parcela.DataQuitacao?.Date);
                dgvParcelas.Refresh();
                btNovaParcela.Enabled = true;
                btNovaParcela.PerformClick();
            }
            else
            {
                var ptemp = parcelas.Where(p => p.Sequencia == int.Parse(dr.Cells[0].Value.ToString())).FirstOrDefault();
                ptemp.Valor = dbValorOriginalParcela.Valor;
                ptemp.Multa = dbMultaParcela.Valor;
                ptemp.Juros = dbJurosParcela.Valor;
                ptemp.DataVencimento = dtpDataVencimentoParcela.Value;
                ptemp.FormaPagamento = buscaFormaPagamento.formaPagamento;
                parcelas.Where(p => p.Sequencia == int.Parse(dr.Cells[0].Value.ToString())).First().Valor = ptemp.Valor;
                parcelas.Where(p => p.Sequencia == int.Parse(dr.Cells[0].Value.ToString())).First().DataVencimento = ptemp.DataVencimento;
                parcelas.Where(p => p.Sequencia == int.Parse(dr.Cells[0].Value.ToString())).First().Multa = ptemp.Multa;
                parcelas.Where(p => p.Sequencia == int.Parse(dr.Cells[0].Value.ToString())).First().Juros = ptemp.Juros;
                parcelas.Where(p => p.Sequencia == int.Parse(dr.Cells[0].Value.ToString())).First().FormaPagamento = ptemp.FormaPagamento;
                dr.Cells[dgvtbcValorOriginal.Index].Value = ptemp.Valor.ToString("############0.00");
                dr.Cells[dgvtbcDataVencimento.Index].Value = ptemp.DataVencimento.ToShortDateString();
                dr.Cells[dgvtbcMulta.Index].Value = ptemp.Multa.ToString("############0.00");
                dr.Cells[dgvtbcJuros.Index].Value = ptemp.Juros.ToString("############0.00");
                dgvParcelas.Update();
                dgvParcelas.Refresh();
            }
            CalculaTotalConta();
            btNovaParcela.Enabled = true;
        }
        private void CalculaTotalConta()
        {
            if (parcelas.Count > 0)
            {
                dbValorOriginalConta.Valor = parcelas.Sum(p => p.Valor);
                dbMultaConta.Valor = parcelas.Sum(p => p.Multa);
                dbJurosConta.Valor = parcelas.Sum(p => p.Juros);
                dbValorFinalConta.Valor = parcelas.Sum(p => p.ValorFinal);
            }
        }
        private void LimpaCampos(bool limpaCod)
        {
            if (limpaCod) { tbCodigoConta.Clear(); }
            buscaPessoa.Limpa();
            dtpDataCadatroConta.Value = DateTime.Now;
            tbCodigoParcela.Clear();
            dtpDataVencimentoParcela.Value = DateTime.Now;
            dbValorOriginalConta.Valor = 0.00m;
            dbValorFinalConta.Valor = 0.00m;
            dbMultaConta.Valor = 0.00m;
            dbJurosConta.Valor = 0.00m;
            tbAjuda.Clear();
            parcelas.Clear();
            dgvParcelas.Rows.Clear();
            dgvParcelas.Refresh();
            LimpaCamposParcela();
        }
        private void LimpaCamposParcela()
        {
            dbValorOriginalParcela.Valor = 0.00m;
            dbValorFinalParcela.Valor = 0.00m;
            dbMultaParcela.Valor = 0.00m;
            dbJurosParcela.Valor = 0.00m;
            this.parcelaSelecionada = null;
        }
        private void EnterTab(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }
        private void Editando(bool edit)
        {
            if (!ignoracheckevent)
            {
                editando = edit;
                menuVertical.Editando(edit, Nivel, CodGrupoUsuario);
            }
        }
        private void SetarNivel()
        {
            //Busca o usuário logado no pc, através do MAC
            logado = logadoDAO.BuscaLogadoByMac(adap.Mac);
            CodGrupoUsuario = logado.Usuario.Grupousuario.GrupoUsuarioID.ToString();
            string Codpermissao = permissaoDAO.BuscarIDbyCodigo("060100").ToString();

            //Busca o nivel de permissão através do código do Grupo Usuario e do código da Tela
            Nivel = permissaoDAO.BuscarNivelPermissao(CodGrupoUsuario, Codpermissao);
            Editando(editando);
        }
    }
}
