﻿using _5gpro.Bll;
using _5gpro.Entities;
using System;
using System.Windows.Forms;

namespace _5gpro.Forms
{
    public partial class fmCadastroItens : Form
    {

        _Item _item;
        Unimedida unimedida = new Unimedida();
        _ItemBLL _itemBLL = new _ItemBLL();
        UnimedidaBLL unimedidaBLL = new UnimedidaBLL();

        bool editando = false;

        public fmCadastroItens()
        {
            InitializeComponent();
            AlteraBotoes();
        }



        //FUNÇÕES DE KEY PRESS
        private void tbCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        //FUNÇÕES DE LEAVE
        private void tbCodUnimedida_Leave(object sender, EventArgs e)
        {
            if (tbCodUnimedida.Text.Length > 0)
            {
                unimedida = unimedidaBLL.BuscaUnimedidaByCod(tbCodUnimedida.Text);
                PreencheCamposUnimedida(unimedida);
            }
            else
            {
                tbDescricaoUndMedida.Text = "";
            }
        }

        private void tbCodigo_Leave(object sender, EventArgs e)
        {
            tbCodigo.Text = tbCodigo.Text == "0" ? "" : tbCodigo.Text;
            if (!editando)
            {
                if (tbCodigo.Text.Length > 0)
                {
                    _Item newitem = _itemBLL.BuscaItemById(tbCodigo.Text);
                    if (newitem != null)
                    {
                        _item = newitem;
                        PreencheCampos(_item);
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
                    LimpaCampos(true);
                    Editando(false);
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
                        _Item newitem = _itemBLL.BuscaItemById(tbCodigo.Text);
                        if (newitem != null)
                        {
                            _item = newitem;
                            PreencheCampos(_item);
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
                        LimpaCampos(true);
                        Editando(false);
                    }
                }
            }

        }



        //FUNÇÕES DE CLICK
        private void btBuscaUndMedida_Click(object sender, EventArgs e)
        {
            AbreTelaBuscaUnimedida();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            _item = new _Item();
            _item.Codigo = tbCodigo.Text;
            _item.Descricao = tbDescricao.Text;
            _item.DescCompra = tbDescricaoDeCompra.Text;
            _item.Referencia = tbReferncia.Text;
            _item.TipoItem = rbProduto.Checked ? "P" : "S";
            _item.ValorEntrada = decimal.Parse(tbPrecoUltimaEntrada.Text);
            _item.ValorSaida = decimal.Parse(tbPrecoVenda.Text);
            _item.Estoquenecessario = decimal.Parse(tbEstoqueNecessario.Text);
            _item.Unimedida = tbCodUnimedida.Text;

            int resultado = _itemBLL.SalvarOuAtualizarItem(_item);


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

        private void btNovo_Click(object sender, EventArgs e)
        {
            if (editando)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
                "Aviso de alteração",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    editando = true;
                    NovoRegistro();
                    tbCodigo.Focus();
                }
                else
                {

                }
            }
            else
            {
                tbCodigo.Focus();
                editando = true;
            }
            AlteraBotoes();
        }


        //FUNÇÕES DE KEY UP
        private void tbCodUnimedida_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                e.Handled = true;
                AbreTelaBuscaUnimedida();
            }

            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                Editando(true);
            }
        }

        private void tbDescricao_KeyUp(object sender, KeyEventArgs e)
        {
            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                Editando(true);
            }
        }

        private void tbDescricaoDeCompra_KeyUp(object sender, KeyEventArgs e)
        {
            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                Editando(true);
            }
        }

        private void tbReferncia_KeyUp(object sender, KeyEventArgs e)
        {
            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                Editando(true);
            }
        }

        private void tbPrecoUltimaEntrada_KeyUp(object sender, KeyEventArgs e)
        {
            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                Editando(true);
            }
        }

        private void tbEstoqueNecessario_KeyUp(object sender, KeyEventArgs e)
        {
            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                Editando(true);
            }
        }

        private void tbPrecoVenda_KeyUp(object sender, KeyEventArgs e)
        {
            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                Editando(true);
            }
        }




        //EVENTOS DE KEY DOWN
        private void tbCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void tbDescricao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void tbDescricaoDeCompra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void tbCodUnimedida_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void tbReferncia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void tbPrecoUltimaEntrada_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void tbEstoqueNecessario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void tbPrecoVenda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }


        private void fmCadastroItens_Load(object sender, EventArgs e)
        {
            tbCodigo.Focus();
        }


        private void AlteraBotoes()
        {
            if (editando)
            {
                btNovo.Image = Properties.Resources.iosPlus_48px_black;
                btNovo.Enabled = false;
                btSalvar.Image = Properties.Resources.iosOk_48px_Green;
                btSalvar.Enabled = true;
                btBuscar.Image = Properties.Resources.iosSearch_48px_black;
                btBuscar.Enabled = false;
                btDeletar.Image = Properties.Resources.iosDelete_48px_black;
                btDeletar.Enabled = false;
            }
            else
            {
                btNovo.Image = Properties.Resources.iosPlus_48px_blue;
                btNovo.Enabled = true;
                btSalvar.Image = Properties.Resources.iosOk_48px_black;
                btSalvar.Enabled = false;
                btBuscar.Image = Properties.Resources.iosSearch_48px_Blue;
                btBuscar.Enabled = true;
                btDeletar.Image = Properties.Resources.iosDelete_48px_Red;
                btDeletar.Enabled = false;
            }
        }

        private void NovoRegistro()
        {
            LimpaCampos(true);
        }

        private void LimpaCampos(bool cod)
        {
            if (cod) { tbCodigo.Clear(); }
            tbDescricao.Clear();
            tbDescricaoDeCompra.Clear();
            tbCodUnimedida.Clear();
            tbDescricaoUndMedida.Clear();
            tbReferncia.Clear();
            tbPrecoUltimaEntrada.Clear();
            tbEstoqueNecessario.Clear();
            tbPrecoVenda.Clear();
            rbProduto.Checked = true;
            rbServico.Checked = false;
        }

        private void PreencheCamposUnimedida(Unimedida unimedida)
        {
            if (unimedida != null)
            {
                tbCodUnimedida.Text = unimedida.Codigo.ToString();
                tbDescricaoUndMedida.Text = unimedida.Descricao;
            }
        }

        private void AbreTelaBuscaUnimedida()
        {
            var buscaUnimedida = new fmBuscaUnimedida();
            buscaUnimedida.ShowDialog();
            unimedida = buscaUnimedida.Unimedida;
            PreencheCamposUnimedida(unimedida);
        }

        private void PreencheCampos(_Item _item)
        {
            LimpaCampos(false);
            tbCodigo.Text = _item.Codigo;
            tbDescricao.Text = _item.Descricao;
            tbDescricaoDeCompra.Text = _item.DescCompra;

            if (_item.Unimedida != null)
            {
                unimedida = unimedidaBLL.BuscaUnimedidaByCod(_item.Unimedida);
                PreencheCamposUnimedida(unimedida);
            }

            if (_item.TipoItem == "P")
            {
                rbProduto.Checked = true;
                rbServico.Checked = false;
            }
            else
            {
                rbProduto.Checked = false;
                rbServico.Checked = true;
            }

            tbReferncia.Text = _item.Referencia;
            tbPrecoUltimaEntrada.Text = _item.ValorEntrada.ToString();
            tbEstoqueNecessario.Text = _item.Estoquenecessario.ToString();
            tbPrecoVenda.Text = _item.ValorSaida.ToString();

        }

        private void Editando(bool edit)
        {
            editando = edit;
            AlteraBotoes();
        }

        private void btProximo_Click(object sender, EventArgs e)
        {
            //Busca o item com ID maior que o atual preenchido. Só preenche se houver algum registro maior
            //Caso não houver registro com ID maior, verifica se item existe. Se não existir busca o maior anterior ao digitado
            if (!editando && tbCodigo.Text.Length > 0)
            {
 
                _Item newitem = _itemBLL.BuscarProximoItem(tbCodigo.Text);
                if (newitem != null)
                {
                    _item = newitem;
                    PreencheCampos(_item);
                }
            }
            else if (editando && tbCodigo.Text.Length > 0)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
               "Aviso de alteração",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _Item newitem = _itemBLL.BuscarProximoItem(tbCodigo.Text);
                    if (newitem != null)
                    {
                        _item = newitem;
                        PreencheCampos(_item);
                        Editando(false);
                    }
                    else
                    {
                        newitem = _itemBLL.BuscarProximoItem(tbCodigo.Text);
                        if (newitem != null)
                        {
                            _item = newitem;
                            PreencheCampos(_item);
                            Editando(false);
                        }
                    }
                }
            }
        }

        private void btAnterior_Click(object sender, EventArgs e)
        {
            //Busca o item com ID menor que o atual preenchido. Só preenche se houver algum registro menor
            //Caso não houver registro com ID menor, verifica se item existe. Se não existir busca o proximo ao digitado
            if (!editando && tbCodigo.Text.Length > 0)
            {
                _Item newitem = _itemBLL.BuscarItemAnterior(tbCodigo.Text);
                if (newitem != null)
                {
                    _item = newitem;
                    PreencheCampos(_item);
                }
            }
            else if (editando && tbCodigo.Text.Length > 0)
            {
                if (MessageBox.Show("Tem certeza que deseja perder os dados alterados?",
               "Aviso de alteração",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _Item newitem = _itemBLL.BuscarItemAnterior(tbCodigo.Text);
                    if (newitem != null)
                    {
                        _item = newitem;
                        PreencheCampos(_item);
                        Editando(false);
                    }
                    else
                    {
                        newitem = _itemBLL.BuscarProximoItem(tbCodigo.Text);
                        if (newitem != null)
                        {
                            _item = newitem;
                            PreencheCampos(_item);
                            Editando(false);
                        }
                    }
                }
            }
        }

        private void btBuscar_Click(object sender, EventArgs e)
        {
            //ABRE O FORM DE BUSCA ITEM
            var buscaItem = new fmBuscaItem();
            buscaItem.ShowDialog();
        }
    }
}
