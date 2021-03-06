﻿using _5gpro.Daos;
using _5gpro.Entities;
using _5gpro.Funcoes;
using _5gpro.StaticFiles;
using System;
using System.Linq;
using System.Windows.Forms;

namespace _5gpro.Forms
{
    public partial class fmCadastroGrupoItem : Form
    {

        private GrupoItem grupoItem = null;
        private readonly GrupoItemDAO grupoItemDAO = new GrupoItemDAO();
        private readonly SubGrupoItemDAO subgrupoitemDAO = new SubGrupoItemDAO();
        private readonly Validacao validacao = new Validacao();
        public SubGrupoItem subgrupoitemSelecionado = null;
        private int Nivel;
        private string CodGrupoUsuario;

        int codigo = 0;

        bool editando = false;
        bool ignoraCheckEvent;

        public fmCadastroGrupoItem()
        {
            InitializeComponent();
            SetarNivel();
        }

        private void SetarNivel()
        {
            CodGrupoUsuario = Logado.Usuario.Grupousuario.GrupoUsuarioID.ToString();
            Nivel = Logado.Usuario.Grupousuario.Permissoes.Find(p => p.Codigo == "010500").Nivel;
            Editando(editando);
        }

        private void FmCadastroGrupoItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 && (Nivel > 1 || CodGrupoUsuario == "999"))
            {
                Novo();
                return;
            }

            if (e.KeyCode == Keys.F2 && (Nivel > 1 || CodGrupoUsuario == "999"))
            {
                Salva();
            }

            if (e.KeyCode == Keys.F5)
            {
                Recarrega();
                return;
            }

            EnterTab(this.ActiveControl, e);
        }
        private void FmCadastroGrupoItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!editando)
                return;

            if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
            "Aviso de alteração",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        private void MenuVertical_Novo_Clicked(object sender, EventArgs e) => Novo();
        private void MenuVertical_Buscar_Clicked(object sender, EventArgs e) => Busca();
        private void MenuVertical_Salvar_Clicked(object sender, EventArgs e) => Salva();
        private void MenuVertical_Recarregar_Clicked(object sender, EventArgs e) => Recarrega();
        private void MenuVertical_Anterior_Clicked(object sender, EventArgs e) => Anterior();
        private void MenuVertical_Proximo_Clicked(object sender, EventArgs e) => Proximo();
        private void MenuVertical_Excluir_Clicked(object sender, EventArgs e)
        {

        }
        private void TbNomeGrupo_TextChanged(object sender, System.EventArgs e) => Editando(true);
        private void TbCodigo_Leave(object sender, System.EventArgs e) => CarregaDados();
        private void BtNovoSubGrupo_Click(object sender, EventArgs e) => InserirSubGrupoItem();
        private void BtRemoverSub_Click(object sender, EventArgs e) => RemoverSubGrupoItem();
        private void DgvSubGruposItens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSubGruposItens.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgvSubGruposItens.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvSubGruposItens.Rows[selectedRowIndex];
                subgrupoitemSelecionado = grupoItem.SubGrupoItens.Find(p => p.Codigo == Convert.ToInt32(selectedRow.Cells[0].Value));
                PreencheCamposSubGrupo(subgrupoitemSelecionado);
                btSalvar.Enabled = true;
                btRemoverSub.Enabled = true;
            }
        }

        private void BtSalvar_Click(object sender, EventArgs e) => SalvaSubGrupo();


        private void Novo()
        {
            if (editando)
            {
                return;
            }

            if (Nivel > 1 || CodGrupoUsuario == "999")
            {
                ignoraCheckEvent = true;
                LimpaCampos(false);
                tbCodigo.Text = grupoItemDAO.BuscaProxCodigoDisponivel().ToString();
                grupoItem = null;
                tbNomeGrupoItem.Focus();
                ignoraCheckEvent = false;
                Editando(true);
            }
        }

        private void Busca()
        {
            if (CodGrupoUsuario != "999" && Nivel <= 0)
            {
                return;
            }

            if (editando)
            {
                return;
            }

            var buscaGrupoItem = new fmBuscaGrupoItem();
            buscaGrupoItem.ShowDialog();
            if (buscaGrupoItem.grupoitemSelecionado != null)
            {
                grupoItem = buscaGrupoItem.grupoitemSelecionado;
                PreencheCampos(grupoItem);
            }
        }
        private void Salva()
        {
            if (!editando)
            {
                return;
            }
            bool ok = false;

            if (tbCodigo.Text.Length <= 0)
            {
                if (MessageBox.Show("Código em branco, deseja gerar um código automaticamente?",
                "Aviso",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    tbCodigo.Text = grupoItemDAO.BuscaProxCodigoDisponivel().ToString();
                }
                ok = false;
                return;
            }

            grupoItem = new GrupoItem();
            grupoItem.GrupoItemID = int.Parse(tbCodigo.Text);
            grupoItem.Nome = tbNomeGrupoItem.Text;

            var controls = (ControlCollection)this.Controls;

            ok = validacao.ValidarEntidade(grupoItem, controls);

            if (ok)
            {
                validacao.despintarCampos(controls);
                int resultado = grupoItemDAO.SalvarOuAtualizar(grupoItem);

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
                    btNovoSubGrupo.Enabled = true;
                    Editando(false);
                    return;
                }
                else if (resultado == 2)
                {
                    tbAjuda.Text = "Dados atualizados com sucesso";
                    Editando(false);
                    return;
                }
            }

        }
        private void Recarrega()
        {
            if (tbCodigo.Text.Length <= 0)
            {
                return;
            }

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

            if (grupoItem != null)
            {
                grupoItem = grupoItemDAO.BuscaByID(grupoItem.GrupoItemID);
                PreencheCampos(grupoItem);
                if (editando)
                {
                    Editando(false);
                }
            }
            else
            {
                ignoraCheckEvent = true;
                LimpaCampos(true);
                ignoraCheckEvent = false;
            }
        }
        private void Anterior()
        {
            if (tbCodigo.Text.Length <= 0)
            {
                return;
            }

            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                    "Aviso de alteração",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }


            var controls = (ControlCollection)this.Controls;


            validacao.despintarCampos(controls);

            var newgrupoitem = grupoItemDAO.Anterior(int.Parse(tbCodigo.Text));
            if (newgrupoitem != null)
            {
                grupoItem = newgrupoitem;
                PreencheCampos(grupoItem);
                if (editando)
                {
                    Editando(false);
                }
            }
        }
        private void Proximo()
        {
            if (tbCodigo.Text.Length <= 0)
            {
                return;
            }

            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                    "Aviso de alteração",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }


            var controls = (ControlCollection)this.Controls;


            validacao.despintarCampos(controls);

            var newgrupoitem = grupoItemDAO.Proximo(int.Parse(tbCodigo.Text));
            if (newgrupoitem != null)
            {
                grupoItem = newgrupoitem;
                PreencheCampos(grupoItem);
                if (editando)
                {
                    Editando(false);
                }
            }
        }
        private void CarregaDados()
        {
            var controls = (ControlCollection)this.Controls;

            if (tbCodigo.Text.Length == 0)
            {
                validacao.despintarCampos(controls);
                LimpaCampos(true);
                Editando(false);
                return;
            }

            int c = 0;
            if (!int.TryParse(tbCodigo.Text, out c))
            {
                tbCodigo.Clear();
            }
            else
            {
                if (c != codigo)
                {
                    if (editando)
                    {
                        if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?", "Aviso de alteração",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    codigo = c;
                }
            }

            if (codigo == 0)
            {
                LimpaCampos(true);
                Editando(false);
                return;
            }

            if (grupoItem?.GrupoItemID == codigo)
            {
                return;
            }


            var newGrupoItem = grupoItemDAO.BuscaByID(int.Parse(tbCodigo.Text));
            if (newGrupoItem != null)
            {
                validacao.despintarCampos(controls);
                grupoItem = newGrupoItem;
                PreencheCampos(grupoItem);
                Editando(false);
            }
            else
            {
                validacao.despintarCampos(controls);
                Editando(true);
                LimpaCampos(false);
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
        private void LimpaCampos(bool cod)
        {
            if (cod) { tbCodigo.Clear(); }
            tbNomeGrupoItem.Clear();
            dgvSubGruposItens.Rows.Clear();
            dgvSubGruposItens.Refresh();
            btNovoSubGrupo.Enabled = false;
            LimpaCamposSubItens();
            codigo = 0;
            grupoItem = null;
        }
        private void LimpaCamposSubItens()
        {
            tbCodigoSubGrupo.Clear();
            tbNomeSubGrupo.Clear();
            subgrupoitemSelecionado = null;
        }
        private void PreencheCampos(GrupoItem grupoitem)
        {
            ignoraCheckEvent = true;
            LimpaCampos(false);
            tbCodigo.Text = grupoitem.GrupoItemID.ToString();
            tbNomeGrupoItem.Text = grupoitem.Nome;
            grupoItem = grupoitem;
            btNovoSubGrupo.Enabled = true;
            PreencheGridSubGrupoItens();
            ignoraCheckEvent = false;
        }
        private void PreencheCamposSubGrupo(SubGrupoItem subGrupoItem)
        {
            tbCodigoSubGrupo.Text = subGrupoItem.Codigo.ToString();
            tbNomeSubGrupo.Text = subGrupoItem.Nome;
        }
        private void Editando(bool edit)
        {
            if (!ignoraCheckEvent)
            {
                editando = edit;
                menuVertical.Editando(edit, Nivel, CodGrupoUsuario);
            }
        }
        private void InserirSubGrupoItem()
        {
            LimpaCamposSubItens();
            tbCodigoSubGrupo.Text = grupoItem.SubGrupoItens?.Count > 0 ? (grupoItem.SubGrupoItens?.Max(sg => sg.Codigo) + 1).ToString() : "1";
            tbNomeSubGrupo.Focus();
            btNovoSubGrupo.Enabled = false;
        }
        private void RemoverSubGrupoItem()
        {
            if (grupoItemDAO.SubGrupoUsado(subgrupoitemSelecionado))
            {
                MessageBox.Show("Este sub-grupo está sendo utilizado e não pode ser deletado.",
                "Aviso",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Warning);
                btNovoSubGrupo.Enabled = true;
                btRemoverSub.Enabled = false;
                LimpaCamposSubItens();
                return;
            }

            int retorno = grupoItemDAO.RemoverSubGrupo(subgrupoitemSelecionado);
            if (retorno > 0)
            {
                grupoItem.SubGrupoItens.Remove(subgrupoitemSelecionado);
                dgvSubGruposItens.Rows.Clear();
                PreencheGridSubGrupoItens();
                btNovoSubGrupo.Enabled = true;
                btRemoverSub.Enabled = false;
                LimpaCamposSubItens();
                tbAjuda.Text = "Sub-grupo removido com sucesso";
            }
        }
        private void SalvaSubGrupo()
        {
            if (tbCodigoSubGrupo.Text.Length <= 0 || grupoItem == null)
            {
                return;
            }

            SubGrupoItem subGrupo = null;
            if (subgrupoitemSelecionado != null)
            {
                subGrupo = subgrupoitemSelecionado;
                grupoItem.SubGrupoItens.Remove(subGrupo);
                subGrupo.Nome = tbNomeSubGrupo.Text;

                int resultado = grupoItemDAO.AtualizarSubGrupo(subGrupo);
                if(resultado > 0)
                {
                    tbAjuda.Text = "Sub-grupo atualizado com sucesso";
                    grupoItem.SubGrupoItens.Add(subGrupo);
                    btNovoSubGrupo.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Problema ao atualizar o registro",
                    "Problema ao atualizar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    return;
                }
                
            }
            else
            {
                subGrupo = new SubGrupoItem();
                subGrupo.Nome = tbNomeSubGrupo.Text;
                subGrupo.Codigo = int.Parse(tbCodigoSubGrupo.Text);
                subGrupo.GrupoItem = grupoItem;

                int resultado = grupoItemDAO.InserirSubGrupo(subGrupo);
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
                    tbAjuda.Text = "Sub-grupo salvo com sucesso";

                    grupoItem.SubGrupoItens.Add(subGrupo);
                    btNovoSubGrupo.Enabled = true;

                }
                else if (resultado == 2)
                {
                    tbAjuda.Text = "Sub-grupo atualizado com sucesso";
                    grupoItem.SubGrupoItens.Add(subGrupo);
                    btNovoSubGrupo.Enabled = true;
                }
            }
            LimpaCamposSubItens();
            PreencheGridSubGrupoItens();
        }

        private void PreencheGridSubGrupoItens()
        {
            dgvSubGruposItens.Rows.Clear();
            foreach (var sub in grupoItem.SubGrupoItens)
            {
                dgvSubGruposItens.Rows.Add(sub.Codigo,
                                           sub.Nome);
            }
            dgvSubGruposItens.Refresh();
        }
    }
}
