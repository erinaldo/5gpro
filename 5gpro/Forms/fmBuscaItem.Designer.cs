﻿namespace _5gpro.Forms
{
    partial class fmBuscaItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.tbDenomCompra = new System.Windows.Forms.TextBox();
            this.tbDescricao = new System.Windows.Forms.TextBox();
            this.lbDescricao = new System.Windows.Forms.Label();
            this.lbDenomCompra = new System.Windows.Forms.Label();
            this.gbTipoItem = new System.Windows.Forms.GroupBox();
            this.cbServico = new System.Windows.Forms.CheckBox();
            this.cbProduto = new System.Windows.Forms.CheckBox();
            this.btBuscarItens = new System.Windows.Forms.Button();
            this.btImprimir = new System.Windows.Forms.Button();
            this.lbReferencia = new System.Windows.Forms.Label();
            this.tbReferencia = new System.Windows.Forms.TextBox();
            this.buscaSubGrupoItem = new _5gpro.Controls.BuscaSubGrupoItem();
            this.buscaGrupoItem = new _5gpro.Controls.BuscaGrupoItem();
            this.gbFiltros = new System.Windows.Forms.GroupBox();
            this.tbCodigoInterno = new System.Windows.Forms.TextBox();
            this.lbCodigoInterno = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.gbTipoItem.SuspendLayout();
            this.gbFiltros.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvItens
            // 
            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.AllowUserToDeleteRows = false;
            this.dgvItens.AllowUserToOrderColumns = true;
            this.dgvItens.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            this.dgvItens.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItens.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItens.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvItens.Location = new System.Drawing.Point(12, 203);
            this.dgvItens.MultiSelect = false;
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.ReadOnly = true;
            this.dgvItens.RowHeadersVisible = false;
            this.dgvItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItens.Size = new System.Drawing.Size(892, 361);
            this.dgvItens.TabIndex = 0;
            this.dgvItens.TabStop = false;
            this.dgvItens.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvItens_CellDoubleClick);
            // 
            // tbDenomCompra
            // 
            this.tbDenomCompra.Location = new System.Drawing.Point(9, 32);
            this.tbDenomCompra.Name = "tbDenomCompra";
            this.tbDenomCompra.Size = new System.Drawing.Size(274, 20);
            this.tbDenomCompra.TabIndex = 1;
            this.tbDenomCompra.TextChanged += new System.EventHandler(this.TbDenomCompra_TextChanged);
            // 
            // tbDescricao
            // 
            this.tbDescricao.Location = new System.Drawing.Point(9, 71);
            this.tbDescricao.Name = "tbDescricao";
            this.tbDescricao.Size = new System.Drawing.Size(274, 20);
            this.tbDescricao.TabIndex = 4;
            this.tbDescricao.TextChanged += new System.EventHandler(this.TbDescricao_TextChanged);
            // 
            // lbDescricao
            // 
            this.lbDescricao.AutoSize = true;
            this.lbDescricao.Location = new System.Drawing.Point(6, 55);
            this.lbDescricao.Name = "lbDescricao";
            this.lbDescricao.Size = new System.Drawing.Size(55, 13);
            this.lbDescricao.TabIndex = 5;
            this.lbDescricao.Text = "Descrição";
            // 
            // lbDenomCompra
            // 
            this.lbDenomCompra.AutoSize = true;
            this.lbDenomCompra.Location = new System.Drawing.Point(6, 16);
            this.lbDenomCompra.Name = "lbDenomCompra";
            this.lbDenomCompra.Size = new System.Drawing.Size(127, 13);
            this.lbDenomCompra.TabIndex = 6;
            this.lbDenomCompra.Text = "Denominação de Compra";
            // 
            // gbTipoItem
            // 
            this.gbTipoItem.Controls.Add(this.cbServico);
            this.gbTipoItem.Controls.Add(this.cbProduto);
            this.gbTipoItem.Location = new System.Drawing.Point(289, 16);
            this.gbTipoItem.Name = "gbTipoItem";
            this.gbTipoItem.Size = new System.Drawing.Size(149, 43);
            this.gbTipoItem.TabIndex = 8;
            this.gbTipoItem.TabStop = false;
            this.gbTipoItem.Text = "Tipo";
            // 
            // cbServico
            // 
            this.cbServico.AutoSize = true;
            this.cbServico.Location = new System.Drawing.Point(76, 23);
            this.cbServico.Name = "cbServico";
            this.cbServico.Size = new System.Drawing.Size(62, 17);
            this.cbServico.TabIndex = 1;
            this.cbServico.Text = "Serviço";
            this.cbServico.UseVisualStyleBackColor = true;
            this.cbServico.Click += new System.EventHandler(this.CbServico_Click);
            // 
            // cbProduto
            // 
            this.cbProduto.AutoSize = true;
            this.cbProduto.Location = new System.Drawing.Point(7, 23);
            this.cbProduto.Name = "cbProduto";
            this.cbProduto.Size = new System.Drawing.Size(63, 17);
            this.cbProduto.TabIndex = 0;
            this.cbProduto.Text = "Produto";
            this.cbProduto.UseVisualStyleBackColor = true;
            this.cbProduto.Click += new System.EventHandler(this.CbProduto_Click);
            // 
            // btBuscarItens
            // 
            this.btBuscarItens.Location = new System.Drawing.Point(445, 105);
            this.btBuscarItens.Name = "btBuscarItens";
            this.btBuscarItens.Size = new System.Drawing.Size(75, 23);
            this.btBuscarItens.TabIndex = 9;
            this.btBuscarItens.Text = "Pesquisar";
            this.btBuscarItens.UseVisualStyleBackColor = true;
            this.btBuscarItens.Click += new System.EventHandler(this.BtBuscarItens_Click);
            // 
            // btImprimir
            // 
            this.btImprimir.Location = new System.Drawing.Point(526, 105);
            this.btImprimir.Name = "btImprimir";
            this.btImprimir.Size = new System.Drawing.Size(75, 23);
            this.btImprimir.TabIndex = 12;
            this.btImprimir.Text = "Imprimir";
            this.btImprimir.UseVisualStyleBackColor = true;
            this.btImprimir.Click += new System.EventHandler(this.BtImprimir_Click);
            // 
            // lbReferencia
            // 
            this.lbReferencia.AutoSize = true;
            this.lbReferencia.Location = new System.Drawing.Point(6, 94);
            this.lbReferencia.Name = "lbReferencia";
            this.lbReferencia.Size = new System.Drawing.Size(59, 13);
            this.lbReferencia.TabIndex = 13;
            this.lbReferencia.Text = "Referência";
            // 
            // tbReferencia
            // 
            this.tbReferencia.Location = new System.Drawing.Point(9, 110);
            this.tbReferencia.Name = "tbReferencia";
            this.tbReferencia.Size = new System.Drawing.Size(274, 20);
            this.tbReferencia.TabIndex = 14;
            this.tbReferencia.TextChanged += new System.EventHandler(this.TbReferencia_TextChanged);
            // 
            // buscaSubGrupoItem
            // 
            this.buscaSubGrupoItem.Location = new System.Drawing.Point(439, 55);
            this.buscaSubGrupoItem.Name = "buscaSubGrupoItem";
            this.buscaSubGrupoItem.Size = new System.Drawing.Size(442, 39);
            this.buscaSubGrupoItem.TabIndex = 11;
            this.buscaSubGrupoItem.Leave += new System.EventHandler(this.BuscaSubGrupoItem_Leave);
            // 
            // buscaGrupoItem
            // 
            this.buscaGrupoItem.Location = new System.Drawing.Point(439, 10);
            this.buscaGrupoItem.Name = "buscaGrupoItem";
            this.buscaGrupoItem.Size = new System.Drawing.Size(442, 39);
            this.buscaGrupoItem.TabIndex = 10;
            this.buscaGrupoItem.Text_Changed += new _5gpro.Controls.BuscaGrupoItem.text_changedEventHandler(this.BuscaGrupoItem_Text_Changed);
            this.buscaGrupoItem.Leave += new System.EventHandler(this.BuscaGrupoItem_Leave);
            // 
            // gbFiltros
            // 
            this.gbFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFiltros.Controls.Add(this.tbCodigoInterno);
            this.gbFiltros.Controls.Add(this.lbCodigoInterno);
            this.gbFiltros.Controls.Add(this.lbDenomCompra);
            this.gbFiltros.Controls.Add(this.btImprimir);
            this.gbFiltros.Controls.Add(this.tbReferencia);
            this.gbFiltros.Controls.Add(this.btBuscarItens);
            this.gbFiltros.Controls.Add(this.buscaSubGrupoItem);
            this.gbFiltros.Controls.Add(this.tbDenomCompra);
            this.gbFiltros.Controls.Add(this.buscaGrupoItem);
            this.gbFiltros.Controls.Add(this.lbReferencia);
            this.gbFiltros.Controls.Add(this.gbTipoItem);
            this.gbFiltros.Controls.Add(this.lbDescricao);
            this.gbFiltros.Controls.Add(this.tbDescricao);
            this.gbFiltros.Location = new System.Drawing.Point(12, 12);
            this.gbFiltros.Name = "gbFiltros";
            this.gbFiltros.Size = new System.Drawing.Size(892, 185);
            this.gbFiltros.TabIndex = 15;
            this.gbFiltros.TabStop = false;
            this.gbFiltros.Text = "Filtros";
            // 
            // tbCodigoInterno
            // 
            this.tbCodigoInterno.Location = new System.Drawing.Point(9, 149);
            this.tbCodigoInterno.Name = "tbCodigoInterno";
            this.tbCodigoInterno.Size = new System.Drawing.Size(274, 20);
            this.tbCodigoInterno.TabIndex = 16;
            // 
            // lbCodigoInterno
            // 
            this.lbCodigoInterno.AutoSize = true;
            this.lbCodigoInterno.Location = new System.Drawing.Point(6, 133);
            this.lbCodigoInterno.Name = "lbCodigoInterno";
            this.lbCodigoInterno.Size = new System.Drawing.Size(75, 13);
            this.lbCodigoInterno.TabIndex = 15;
            this.lbCodigoInterno.Text = "Código interno";
            // 
            // fmBuscaItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(916, 572);
            this.Controls.Add(this.gbFiltros);
            this.Controls.Add(this.dgvItens);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(932, 611);
            this.Name = "fmBuscaItem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Busca de Itens";
            this.Load += new System.EventHandler(this.FmBuscaItem_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FmBuscaItem_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.gbTipoItem.ResumeLayout(false);
            this.gbTipoItem.PerformLayout();
            this.gbFiltros.ResumeLayout(false);
            this.gbFiltros.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItens;
        private System.Windows.Forms.TextBox tbDenomCompra;
        private System.Windows.Forms.TextBox tbDescricao;
        private System.Windows.Forms.Label lbDescricao;
        private System.Windows.Forms.Label lbDenomCompra;
        private System.Windows.Forms.GroupBox gbTipoItem;
        private System.Windows.Forms.CheckBox cbServico;
        private System.Windows.Forms.CheckBox cbProduto;
        private System.Windows.Forms.Button btBuscarItens;
        private Controls.BuscaGrupoItem buscaGrupoItem;
        private Controls.BuscaSubGrupoItem buscaSubGrupoItem;
        private System.Windows.Forms.Button btImprimir;
        private System.Windows.Forms.Label lbReferencia;
        private System.Windows.Forms.TextBox tbReferencia;
        private System.Windows.Forms.GroupBox gbFiltros;
        private System.Windows.Forms.TextBox tbCodigoInterno;
        private System.Windows.Forms.Label lbCodigoInterno;
    }
}