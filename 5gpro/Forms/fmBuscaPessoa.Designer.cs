﻿namespace _5gpro.Forms
{
    partial class fmBuscaPessoa
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
            this.lbFiltroNome = new System.Windows.Forms.Label();
            this.tbFiltroNome = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvPessoas = new System.Windows.Forms.DataGridView();
            this.tbCpfCnpj = new System.Windows.Forms.TextBox();
            this.gbFiltros = new System.Windows.Forms.GroupBox();
            this.buscaCidade = new _5gpro.Controls.BuscaCidade();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPessoas)).BeginInit();
            this.gbFiltros.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbFiltroNome
            // 
            this.lbFiltroNome.AutoSize = true;
            this.lbFiltroNome.Location = new System.Drawing.Point(6, 97);
            this.lbFiltroNome.Name = "lbFiltroNome";
            this.lbFiltroNome.Size = new System.Drawing.Size(35, 13);
            this.lbFiltroNome.TabIndex = 0;
            this.lbFiltroNome.Text = "Nome";
            // 
            // tbFiltroNome
            // 
            this.tbFiltroNome.Location = new System.Drawing.Point(9, 113);
            this.tbFiltroNome.Name = "tbFiltroNome";
            this.tbFiltroNome.Size = new System.Drawing.Size(309, 20);
            this.tbFiltroNome.TabIndex = 1;
            this.tbFiltroNome.TextChanged += new System.EventHandler(this.TbFiltroNome_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "CPF/CNPJ";
            // 
            // dgvPessoas
            // 
            this.dgvPessoas.AllowUserToAddRows = false;
            this.dgvPessoas.AllowUserToDeleteRows = false;
            this.dgvPessoas.AllowUserToOrderColumns = true;
            this.dgvPessoas.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            this.dgvPessoas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPessoas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPessoas.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPessoas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPessoas.Location = new System.Drawing.Point(12, 158);
            this.dgvPessoas.MultiSelect = false;
            this.dgvPessoas.Name = "dgvPessoas";
            this.dgvPessoas.ReadOnly = true;
            this.dgvPessoas.RowHeadersVisible = false;
            this.dgvPessoas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPessoas.Size = new System.Drawing.Size(1044, 345);
            this.dgvPessoas.TabIndex = 1;
            this.dgvPessoas.TabStop = false;
            this.dgvPessoas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvPessoas_CellDoubleClick);
            // 
            // tbCpfCnpj
            // 
            this.tbCpfCnpj.Location = new System.Drawing.Point(9, 71);
            this.tbCpfCnpj.MaxLength = 14;
            this.tbCpfCnpj.Name = "tbCpfCnpj";
            this.tbCpfCnpj.Size = new System.Drawing.Size(141, 20);
            this.tbCpfCnpj.TabIndex = 5;
            this.tbCpfCnpj.TextChanged += new System.EventHandler(this.TbCpfCnpj_TextChanged);
            // 
            // gbFiltros
            // 
            this.gbFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFiltros.Controls.Add(this.buscaCidade);
            this.gbFiltros.Controls.Add(this.lbFiltroNome);
            this.gbFiltros.Controls.Add(this.tbCpfCnpj);
            this.gbFiltros.Controls.Add(this.tbFiltroNome);
            this.gbFiltros.Controls.Add(this.label1);
            this.gbFiltros.Location = new System.Drawing.Point(12, 12);
            this.gbFiltros.Name = "gbFiltros";
            this.gbFiltros.Size = new System.Drawing.Size(1044, 140);
            this.gbFiltros.TabIndex = 0;
            this.gbFiltros.TabStop = false;
            this.gbFiltros.Text = "Filtros da pessoa";
            // 
            // buscaCidade
            // 
            this.buscaCidade.LabelText = "Cidade";
            this.buscaCidade.Location = new System.Drawing.Point(3, 16);
            this.buscaCidade.Margin = new System.Windows.Forms.Padding(0);
            this.buscaCidade.Name = "buscaCidade";
            this.buscaCidade.Size = new System.Drawing.Size(315, 39);
            this.buscaCidade.TabIndex = 3;
            this.buscaCidade.Text_Changed += new _5gpro.Controls.BuscaCidade.text_changedEventHandler(this.BuscaCidade_Text_Changed);
            this.buscaCidade.Leave += new System.EventHandler(this.BuscaCidade_Leave);
            // 
            // fmBuscaPessoa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1068, 516);
            this.Controls.Add(this.gbFiltros);
            this.Controls.Add(this.dgvPessoas);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(590, 555);
            this.Name = "fmBuscaPessoa";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Busca pessoa";
            this.Load += new System.EventHandler(this.FmBuscaPessoa_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FmBuscaPessoa_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPessoas)).EndInit();
            this.gbFiltros.ResumeLayout(false);
            this.gbFiltros.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbFiltroNome;
        private System.Windows.Forms.TextBox tbFiltroNome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPessoas;
        private System.Windows.Forms.TextBox tbCpfCnpj;
        private System.Windows.Forms.GroupBox gbFiltros;
        private Controls.BuscaCidade buscaCidade;
    }
}