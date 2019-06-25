﻿using _5gpro.Entities;
using MySQLConnection;
using System;
using System.Collections.Generic;

namespace _5gpro.Daos
{
    public class PlanoContaDAO
    {
        private static readonly ConexaoDAO Connect = new ConexaoDAO();

        public int Salva(PlanoConta planoConta)
        {
            var retorno = 0;
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.beginTransaction();
                sql.Query = @"INSERT INTO caixa_plano_contas
                            (paiid, level, codigo, descricao)
                            VALUES
                            (@paiid, @level, @codigo, @descricao)
                            ON DUPLICATE KEY UPDATE
                            descricao = @descricao";
                sql.addParam("@paiid", planoConta.PaiID);
                sql.addParam("@level", planoConta.Level);
                sql.addParam("@codigo", planoConta.Codigo);
                sql.addParam("@descricao", planoConta.Descricao);
                retorno = sql.insertQuery();
                if (retorno > 0 && planoConta.PlanoContaID == 0)
                {
                    sql.Query = "SELECT LAST_INSERT_ID() AS id;";
                    var data = sql.selectQueryForSingleRecord();
                    planoConta.PlanoContaID = Convert.ToInt32(data["id"]);
                }
                sql.Commit();
            }
            return retorno;
        }
        public int Atualiza(PlanoConta planoConta)
        {
            var retorno = 0;
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.beginTransaction();
                sql.Query = @"UPDATE caixa_plano_contas SET descricao = @descricao WHERE codigo = @codigo AND paiid = @paiid";
                sql.addParam("@paiid", planoConta.PaiID);
                sql.addParam("@codigo", planoConta.Codigo);
                sql.addParam("@descricao", planoConta.Descricao);
                retorno = sql.insertQuery();
                if (retorno > 0 && planoConta.PlanoContaID == 0)
                {
                    sql.Query = "SELECT LAST_INSERT_ID() AS id;";
                    var data = sql.selectQueryForSingleRecord();
                    planoConta.PlanoContaID = Convert.ToInt32(data["id"]);
                }
                sql.Commit();
            }
            return retorno;
        }

        public List<PlanoConta> Busca()
        {
            List<PlanoConta> planoContas = new List<PlanoConta>();
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT * FROM caixa_plano_contas";
                var data = sql.selectQuery();
                foreach(var d in data)
                {
                    planoContas.Add(LeDadosReader(d));
                }
            }
            return planoContas;
        }

        public PlanoConta BuscaByID(int Codigo)
        {
            var planoconta = new PlanoConta();
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT g.idgrupoitem AS grupoitemID, g.nome AS nomegrupoitem,
                                                   s.idsubgrupoitem AS subgrupoitemID, s.nome AS subgrupoitemnome,
                                                   s.idgrupoitem AS idgrupoitemsub, s.codigo
                                                   FROM grupoitem g 
                                                   LEFT JOIN subgrupoitem s 
                                                   ON g.idgrupoitem = s.idgrupoitem 
                                                   WHERE g.idgrupoitem = @idgrupoitem";
                sql.addParam("@idgrupoitem", Codigo);
                var data = sql.selectQuery();
                if (data == null)
                {
                    return null;
                }
               // planoconta = LeDadosReader(data);
            }
            return planoconta;
        }

        public int BuscaProxCodigoDisponivel(int paiid)
        {
            int proximocodigo = 0;
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT COALESCE(MAX(pc.codigo) + 1, 1) AS proximocodigo
                            FROM caixa_plano_contas AS pc
                            WHERE pc.paiid = @paiid";
                sql.addParam("@paiid", paiid);
                var data = sql.selectQueryForSingleRecord();
                if (data != null)
                {
                    proximocodigo = Convert.ToInt32(data["proximocodigo"]);
                }
            }
            return proximocodigo;
        }


        private PlanoConta LeDadosReader(Dictionary<string, object> dado)
        {
            PlanoConta planoConta = new PlanoConta();
            planoConta.PlanoContaID = Convert.ToInt32(dado["idcaixa_plano_contas"]);
            planoConta.Codigo = Convert.ToInt32(dado["codigo"]);
            planoConta.Level = Convert.ToInt32(dado["level"]);
            planoConta.PaiID = dado["paiid"] != null ? Convert.ToInt32(dado["paiid"]) : 0;
            planoConta.Descricao = (string)dado["descricao"];
            return planoConta;
        }
    }
}
