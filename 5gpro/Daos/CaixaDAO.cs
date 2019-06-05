﻿using _5gpro.Entities;
using MySQLConnection;
using System;
using System.Collections.Generic;

namespace _5gpro.Daos
{
    class CaixaDAO
    {
        private static readonly ConexaoDAO Connect = new ConexaoDAO();

        public int SalvaOuAtualiza(Caixa caixa)
        {
            var retorno = 0;
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"INSERT INTO caixa 
                            (codigo, nome, descricao) 
                            VALUES
                            (@codigo, nome, descricao)
                            ON DUPLICATE KEY UPDATE
                            nome = @nome, descricao = @descricao";
                sql.addParam("@codigo", caixa.Codigo);
                sql.addParam("@nome", caixa.Nome);
                sql.addParam("@descricao", caixa.Descricao);
                retorno = sql.insertQuery();
            }
            return retorno;
        }
        public Caixa Busca(int Codigo)
        {
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT * FROM caixa WHERE codigo = @codigo";
                sql.addParam("@codigo", Codigo);
                var data = sql.selectQueryForSingleRecord();
                if (data == null)
                {
                    return null;
                }
                return LeDadosReader(data);
            }
        }
        public Caixa Anterior(int Codigo)
        {
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT * 
                            FROM caixa 
                            WHERE codigo = (SELECT max(codigo) FROM caixa WHERE codigo < @codigo)";
                sql.addParam("@codigo", Codigo);
                var data = sql.selectQueryForSingleRecord();
                if (data == null)
                {
                    return null;
                }
                return LeDadosReader(data);
            }
        }
        public Caixa Proximo(int Codigo)
        {
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT * 
                            FROM caixa 
                            WHERE codigo = (SELECT min(codigo) FROM caixa WHERE codigo > @codigo)";
                sql.addParam("@codigo", Codigo);
                var data = sql.selectQueryForSingleRecord();
                if (data == null)
                {
                    return null;
                }
                return LeDadosReader(data);
            }
        }
        public int BuscaProxCodigoDisponivel()
        {
            int proximoid = 1;
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT c1.codigo + 1 AS proximoid
                            FROM caixa AS c1
                            LEFT OUTER JOIN caixa AS c2 ON c1.codigo + 1 = c2.codigo
                            WHERE c2.codigo IS NULL
                            LIMIT 1";
                var data = sql.selectQueryForSingleRecord();
                if (data != null)
                {
                    proximoid = Convert.ToInt32(data["proximoid"]);
                }
            }
            return proximoid;
        }
        private Caixa LeDadosReader(Dictionary<string, object> data)
        {
            var caixa = new Caixa();
            caixa.CaixaID = Convert.ToInt32(data["idcaixa"]);
            caixa.Codigo = Convert.ToInt32(data["codigo"]);
            caixa.Nome = (string)data["nome"];
            caixa.Descricao = (string)data["descricao"];
            caixa.DataAbertura = (DateTime?)data["dataabertura"];
            caixa.DataFechamento = (DateTime?)data["datafechamento"];
            caixa.ValorAbertura = (decimal)data["valorabertura"];
            caixa.ValorFechamento = (decimal)data["valorfechamento"];
            return caixa;
        }
    }
}
