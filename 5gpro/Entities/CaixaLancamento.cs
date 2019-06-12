﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5gpro.Entities
{
    class CaixaLancamento
    {
        public int CaixaLancamentoID { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorPago { get; set; }
        public decimal Troco { get; set; }
        public int TipoMovimento { get; set; } // 0 = crédito 1 = débito 5 = Sangria
        public int TipoDocumento { get; set; }
        public int Lancamento { get; set; }
        public string Documento { get; set; }
        public Caixa Caixa { get; set; }
    }
}