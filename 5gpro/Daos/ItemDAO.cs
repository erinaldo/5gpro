﻿using _5gpro.Entities;
using MySQLConnection;
using System;
using System.Collections.Generic;


namespace _5gpro.Daos
{
    class ItemDAO
    {
        private static readonly ConexaoDAO Connect = new ConexaoDAO();

        public int SalvaOuAtualiza(Item item)
        {
            int retorno = 0;
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"INSERT INTO item 
                            (iditem, descitem, denominacaocompra, tipo, referencia, valorentrada, valorsaida, estoquenecessario, idunimedida, idsubgrupoitem, quantidade) 
                            VALUES
                            (@iditem, @descitem, @denominacaocompra, @tipo, @referencia, @valorentrada, @valorsaida, @estoquenecessario, @idunimedida, @idsubgrupoitem, @quantidade)
                            ON DUPLICATE KEY UPDATE
                            descitem = @descitem, denominacaocompra = @denominacaocompra, tipo = @tipo, referencia = @referencia, valorentrada = @valorentrada,
                            valorsaida = @valorsaida, estoquenecessario = @estoquenecessario, idunimedida = @idunimedida, idsubgrupoitem = @idsubgrupoitem, quantidade = @quantidade";

                sql.addParam("@iditem", item.ItemID);
                sql.addParam("@descitem", item.Descricao);
                sql.addParam("@denominacaocompra", item.DescCompra);
                sql.addParam("@tipo", item.TipoItem);
                sql.addParam("@referencia", item.Referencia);
                sql.addParam("@valorentrada", item.ValorEntrada);
                sql.addParam("@valorsaida", item.ValorSaida);
                sql.addParam("@estoquenecessario", item.Estoquenecessario);
                sql.addParam("@idunimedida", item.Unimedida.UnimedidaID);
                sql.addParam("@idsubgrupoitem", item.SubGrupoItem.SubGrupoItemID);
                sql.addParam("@quantidade", item.Quantidade);
                retorno = sql.insertQuery();
            }
            return retorno;
        }
        public Item BuscaByID(int Codigo)
        {
            var item = new Item();
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT *, g.nome AS grupoitemnome FROM item i
                            INNER JOIN subgrupoitem s ON i.idsubgrupoitem = s.idsubgrupoitem
                            INNER JOIN grupoitem g ON s.idgrupoitem = g.idgrupoitem
                            INNER JOIN unimedida u ON u.idunimedida = i.idunimedida
                            WHERE iditem = @iditem";
                sql.addParam("@iditem", Codigo);

                var data = sql.selectQueryForSingleRecord();
                if (data == null)
                {
                    return null;
                }
                item = LeDadosReader(data);
            }
            return item;
        }
        public Item Proximo(int Codigo)
        {
            var item = new Item();
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT *, g.nome AS grupoitemnome FROM item i
                            INNER JOIN subgrupoitem s ON i.idsubgrupoitem = s.idsubgrupoitem
                            INNER JOIN grupoitem g ON s.idgrupoitem = g.idgrupoitem
                            INNER JOIN unimedida u ON u.idunimedida = i.idunimedida
                            WHERE iditem = (SELECT min(iditem) FROM item WHERE iditem > @iditem)";
                sql.addParam("@iditem", Codigo);

                var data = sql.selectQueryForSingleRecord();
                if (data == null)
                {
                    return null;
                }
                item = LeDadosReader(data);
            }
            return item;
        }
        public Item Anterior(int Codigo)
        {
            var item = new Item();
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT *, g.nome AS grupoitemnome FROM item i 
                            INNER JOIN subgrupoitem s ON i.idsubgrupoitem = s.idsubgrupoitem
                            INNER JOIN grupoitem g ON s.idgrupoitem = g.idgrupoitem
                            INNER JOIN unimedida u ON u.idunimedida = i.idunimedida
                            WHERE iditem = (SELECT max(iditem) FROM item WHERE iditem < @iditem)";
                sql.addParam("@iditem", Codigo);

                var data = sql.selectQueryForSingleRecord();
                if (data == null)
                {
                    return null;
                }
                item = LeDadosReader(data);
            }
            return item;
        }
        public List<Item> Busca(string descItem, string denomItem, string tipoItem, SubGrupoItem subgrupoitem)
        {
            List<Item> itens = new List<Item>();
            string conDescItem = descItem.Length > 0 ? "AND i.descitem LIKE @descitem" : "";
            string conDenomItem = denomItem.Length > 0 ? "AND i.denominacaocompra LIKE @denominacaocompra" : "";
            string conTipoItem = tipoItem.Length > 0 ? "AND i.tipo LIKE @tipo" : "";
            string conSubgrupoItem = subgrupoitem != null ? "AND i.idsubgrupoitem = @idsubgrupoitem" : "";
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT *, g.nome AS grupoitemnome FROM item i
                            INNER JOIN unimedida u ON i.idunimedida = u.idunimedida
                            INNER JOIN subgrupoitem s ON i.idsubgrupoitem = s.idsubgrupoitem
                            INNER JOIN grupoitem g ON s.idgrupoitem = g.idgrupoitem
                            WHERE 1=1 "
                            + conDescItem
                            + conDenomItem
                            + conTipoItem
                            + conSubgrupoItem
                            + @" ORDER BY i.iditem";
                if (denomItem.Length > 0) { sql.addParam("@denominacaocompra", "%" + denomItem + "%"); }
                if (descItem.Length > 0) { sql.addParam("@descitem", "%" + descItem + "%"); }
                if (tipoItem.Length > 0) { sql.addParam("@tipo", "%" + tipoItem + "%"); }
                if (subgrupoitem != null) { sql.addParam("@idsubgrupoitem", subgrupoitem.SubGrupoItemID); }

                var data = sql.selectQuery();
                foreach(var d in data)
                {
                    var item = LeDadosReader(d);
                    itens.Add(item);
                }
            }
            return itens;
        }
        public int BuscaProxCodigoDisponivel()
        {
            int proximoid = 1;
            using (MySQLConn sql = new MySQLConn(Connect.Conecta))
            {
                sql.Query = @"SELECT i1.iditem + 1 AS proximoid
                            FROM item AS i1
                            LEFT OUTER JOIN item AS i2 ON i1.iditem + 1 = i2.iditem
                            WHERE i2.iditem IS NULL
                            ORDER BY proximoid
                            LIMIT 1";
                var data = sql.selectQueryForSingleRecord();
                if (data != null)
                {
                    proximoid = Convert.ToInt32(data["proximoid"]);
                }
            }
            return proximoid;
        }
        private Item LeDadosReader(Dictionary<string, object> data)
        {
            var unidadeMedida = new Unimedida();
            unidadeMedida.UnimedidaID = Convert.ToInt32(data["idunimedida"]);
            unidadeMedida.Sigla = (string)data["sigla"];
            unidadeMedida.Descricao = (string)data["descricao"];

            var grupoItem = new GrupoItem();
            grupoItem.GrupoItemID = Convert.ToInt32(data["idgrupoitem"]);
            grupoItem.Nome = (string)data["grupoitemnome"];

            var subGrupoItem = new SubGrupoItem();
            subGrupoItem.SubGrupoItemID = Convert.ToInt32(data["idsubgrupoitem"]);
            subGrupoItem.Nome = (string)data["nome"];
            subGrupoItem.GrupoItem = grupoItem;

            var item = new Item();
            item.ItemID = Convert.ToInt32(data["iditem"]);
            item.Descricao = (string)data["descitem"];
            item.DescCompra = (string)data["denominacaocompra"];
            item.TipoItem = (string)data["tipo"];
            item.Referencia = (string)data["referencia"];
            item.ValorEntrada = (decimal)data["valorentrada"];
            item.ValorSaida = (decimal)data["valorsaida"];
            item.Estoquenecessario = (decimal)data["estoquenecessario"];
            item.Quantidade = (decimal)data["quantidade"];
            item.Unimedida = unidadeMedida;
            item.SubGrupoItem = subGrupoItem;

            return item;
        }
    }

}
