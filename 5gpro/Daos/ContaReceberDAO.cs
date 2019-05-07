﻿using _5gpro.Entities;
using _5gpro.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace _5gpro.Daos
{
    class ContaReceberDAO
    {
        private static ConexaoDAO Connect;
        public ContaReceberDAO(ConexaoDAO c)
        {
            Connect = c;
        }
        public int SalvaOuAtualiza(ContaReceber contaReceber)
        {
            int retorno = 0;
            try
            {
                Connect.AbrirConexao();
                Connect.Comando = Connect.Conexao.CreateCommand();
                Connect.tr = Connect.Conexao.BeginTransaction();
                Connect.Comando.Connection = Connect.Conexao;
                Connect.Comando.Transaction = Connect.tr;


                Connect.Comando.CommandText = @"INSERT INTO conta_receber
                         (idconta_receber, data_cadastro, idoperacao, valor_original, multa, juros, acrescimo, valor_final, idpessoa)
                          VALUES
                         (@idconta_receber, @data_cadastro, @idoperacao, @valor_original, @multa, @juros, @acrescimo, @valor_final, @idpessoa)
                          ON DUPLICATE KEY UPDATE
                          data_cadastro = @data_cadastro, idoperacao = @idoperacao, valor_original = @valor_original,
                          multa = @multa, juros = @juros, acrescimo = @acrescimo, valor_final = @valor_final, idpessoa = @idpessoa
                          ";

                Connect.Comando.Parameters.AddWithValue("@idconta_receber", contaReceber.ContaReceberID);
                Connect.Comando.Parameters.AddWithValue("@data_cadastro", contaReceber.DataCadastro);
                Connect.Comando.Parameters.AddWithValue("@idoperacao", contaReceber.Operacao.OperacaoID);
                Connect.Comando.Parameters.AddWithValue("@valor_original", contaReceber.ValorOriginal);
                Connect.Comando.Parameters.AddWithValue("@multa", contaReceber.Multa);
                Connect.Comando.Parameters.AddWithValue("@juros", contaReceber.Juros);
                Connect.Comando.Parameters.AddWithValue("@acrescimo", contaReceber.Acrescimo);
                Connect.Comando.Parameters.AddWithValue("@valor_final", contaReceber.ValorFinal);
                Connect.Comando.Parameters.AddWithValue("@idpessoa", contaReceber.Pessoa.PessoaID);


                retorno = Connect.Comando.ExecuteNonQuery();


                if (retorno > 0) //Checa se conseguiu inserir ou atualizar pelo menos 1 registro
                {
                    Connect.Comando.CommandText = @"DELETE FROM parcela_conta_receber WHERE idconta_receber = @idconta_receber";
                    Connect.Comando.ExecuteNonQuery();

                    Connect.Comando.CommandText = @"INSERT INTO parcela_conta_receber
                                            (sequencia, data_vencimento, valor, multa, juros, acrescimo, valor_final, data_quitacao, idconta_receber, idformapagamento)
                                            VALUES
                                            (@sequencia, @data_vencimento, @valor, @multa, @juros, @acrescimo, @valor_final, @data_quitacao, @idconta_receber, @idformapagamento)";
                    foreach (var parcela in contaReceber.Parcelas)
                    {
                        Connect.Comando.Parameters.Clear();
                        Connect.Comando.Parameters.AddWithValue("@sequencia", parcela.Sequencia);
                        Connect.Comando.Parameters.AddWithValue("@data_vencimento", parcela.DataVencimento);
                        Connect.Comando.Parameters.AddWithValue("@valor", parcela.Valor);
                        Connect.Comando.Parameters.AddWithValue("@multa", parcela.Multa);
                        Connect.Comando.Parameters.AddWithValue("@juros", parcela.Juros);
                        Connect.Comando.Parameters.AddWithValue("@acrescimo", parcela.Acrescimo);
                        Connect.Comando.Parameters.AddWithValue("@valor_final", parcela.ValorFinal);
                        Connect.Comando.Parameters.AddWithValue("@data_quitacao", parcela.DataQuitacao);
                        Connect.Comando.Parameters.AddWithValue("@idconta_receber", contaReceber.ContaReceberID);
                        Connect.Comando.Parameters.AddWithValue("@idformapagamento", parcela.FormaPagamento?.FormaPagamentoID ?? null);
                        Connect.Comando.ExecuteNonQuery();
                    }
                }
                Connect.tr.Commit();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
                retorno = 0;
            }
            finally
            {
                Connect.FecharConexao();
            }
            return retorno;
        }
        public ContaReceber BuscaById(int codigo)
        {
            var contaReceber = new ContaReceber();

            try
            {
                Connect.AbrirConexao();
                Connect.Comando = new MySqlCommand("SELECT * FROM conta_receber WHERE idconta_receber = @idconta_receber", Connect.Conexao);
                Connect.Comando.Parameters.AddWithValue("@idconta_receber", codigo);

                using (var reader = Connect.Comando.ExecuteReader())
                {
                    contaReceber = LeDadosReader(reader);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                Connect.FecharConexao();
            }

            if (contaReceber != null)
                contaReceber.Parcelas = BuscaParcelasDaConta(contaReceber);
            return contaReceber;
        }
        public IEnumerable<ContaReceber> Busca(fmBuscaContaReceber.Filtros f )
        {
            var contaRecebers = new List<ContaReceber>();
            string wherePessoa = f.filtroPessoa != null ? "AND p.idpessoa = @idpessoa" : "";
            string whereOperacao = f.filtroOperacao != null ? "AND op.idoperacao = @idoperacao": "";


            Operacao operacao = null;
            Pessoa pessoa = null;

            try
            {
                Connect.AbrirConexao();
                Connect.Comando = new MySqlCommand(@"SELECT cr.idconta_receber, p.idpessoa, p.nome, cr.data_cadastro,
                                                    op.idoperacao, op.nome as nomeoperacao, cr.valor_original, cr.multa, cr.juros,
                                                    cr.valor_final
                                                    FROM 
                                                    conta_receber cr 
                                                    LEFT JOIN operacao op ON cr.idoperacao = op.idoperacao
                                                    LEFT JOIN pessoa p ON cr.idpessoa = p.idpessoa
                                                    LEFT JOIN parcela_conta_receber pa ON pa.idconta_receber = cr.idconta_receber
                                                    WHERE 1 = 1"
                                             + whereOperacao + ""
                                             + wherePessoa + "" +
                                          @" AND cr.valor_final BETWEEN @valor_conta_inicial AND @valor_conta_final
                                             AND cr.data_cadastro BETWEEN @data_cadastro_inicial AND @data_cadastro_final
                                             AND pa.data_vencimento BETWEEN @data_vencimento_inicial AND @data_vencimento_final
                                             GROUP BY cr.idconta_receber", Connect.Conexao);
                if (f.filtroPessoa != null) { Connect.Comando.Parameters.AddWithValue("@idpessoa", f.filtroPessoa.PessoaID); }
                if (f.filtroOperacao != null) { Connect.Comando.Parameters.AddWithValue("@idoperacao", f.filtroOperacao.OperacaoID); }

                Connect.Comando.Parameters.AddWithValue("@valor_conta_inicial", f.filtroValorInicial);
                Connect.Comando.Parameters.AddWithValue("@valor_conta_final", f.filtroValorFinal);
                Connect.Comando.Parameters.AddWithValue("@data_cadastro_inicial", f.filtroDataCadastroInicial);
                Connect.Comando.Parameters.AddWithValue("@data_cadastro_final", f.filtroDataCadastroFinal);
                Connect.Comando.Parameters.AddWithValue("@data_vencimento_inicial", f.filtroDataVencimentoInicial);
                Connect.Comando.Parameters.AddWithValue("@data_vencimento_final", f.filtroDataVencimentoFinal);

                using (var reader = Connect.Comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pessoa = new Pessoa
                        {
                            PessoaID = reader.GetInt32(reader.GetOrdinal("idpessoa")),
                            Nome = reader.GetString(reader.GetOrdinal("nome"))
                        };

                        operacao = new Operacao
                        {
                            OperacaoID = reader.GetInt32(reader.GetOrdinal("idoperacao")),
                            Nome = reader.GetString(reader.GetOrdinal("nomeoperacao"))
                        };

                        var contaReceber = new ContaReceber
                        {
                            ContaReceberID = reader.GetInt32(reader.GetOrdinal("idconta_receber")),
                            DataCadastro = reader.GetDateTime(reader.GetOrdinal("data_cadastro")),
                            ValorOriginal = reader.GetDecimal(reader.GetOrdinal("valor_original")),
                            Multa = reader.GetDecimal(reader.GetOrdinal("multa")),
                            Juros = reader.GetDecimal(reader.GetOrdinal("juros")),
                            Acrescimo = reader.GetDecimal(reader.GetOrdinal("acrescimo")),
                            ValorFinal = reader.GetDecimal(reader.GetOrdinal("valor_final")),
                        };

                        contaReceber.Pessoa = pessoa;
                        contaReceber.Operacao = operacao;
                        contaRecebers.Add(contaReceber);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                Connect.FecharConexao();
            }
            return contaRecebers;
        }
        public int BuscaProxCodigoDisponivel()
        {
            int proximoid = 1;
            try
            {
                Connect.AbrirConexao();
                Connect.Comando = new MySqlCommand(@"SELECT cr1.idconta_receber + 1 AS proximoid
                                             FROM conta_receber AS cr1
                                             LEFT OUTER JOIN conta_receber AS cr2 ON cr1.idconta_receber + 1 = cr2.idconta_receber
                                             WHERE cr2.idconta_receber IS NULL
                                             ORDER BY proximoid
                                             LIMIT 1;", Connect.Conexao);

                using (var reader = Connect.Comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        proximoid = reader.GetInt32(reader.GetOrdinal("proximoid"));
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                Connect.FecharConexao();
            }

            return proximoid;
        }
        public ContaReceber BuscaProximo(int codigo)
        {
            var contaReceber = new ContaReceber();
            try
            {
                Connect.AbrirConexao();
                Connect.Comando = new MySqlCommand("SELECT * FROM conta_receber WHERE idconta_receber = (SELECT min(idconta_receber) FROM conta_receber WHERE idconta_receber > @idconta_receber)", Connect.Conexao);
                Connect.Comando.Parameters.AddWithValue("@idconta_receber", codigo);

                using (var reader = Connect.Comando.ExecuteReader())
                {
                    contaReceber = LeDadosReader(reader);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                Connect.FecharConexao();
            }
            if (contaReceber != null)
                contaReceber.Parcelas = BuscaParcelasDaConta(contaReceber);
            return contaReceber;
        }
        public ContaReceber BuscaAnterior(int codigo)
        {
            var contaReceber = new ContaReceber();
            try
            {
                Connect.AbrirConexao();
                Connect.Comando = new MySqlCommand("SELECT * FROM conta_receber WHERE idconta_receber = (SELECT max(idconta_receber) FROM conta_receber WHERE idconta_receber < @idconta_receber)", Connect.Conexao);
                Connect.Comando.Parameters.AddWithValue("@idconta_receber", codigo);

                using (var reader = Connect.Comando.ExecuteReader())
                {
                    contaReceber = LeDadosReader(reader);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                Connect.FecharConexao();
            }

            if (contaReceber != null)
                contaReceber.Parcelas = BuscaParcelasDaConta(contaReceber);
            return contaReceber;
        }
        private List<ParcelaContaReceber> BuscaParcelasDaConta(ContaReceber contaReceber)
        {
            var parcelas = new List<ParcelaContaReceber>();
            try
            {
                Connect.AbrirConexao();
                Connect.Comando = new MySqlCommand(@"SELECT * FROM parcela_conta_receber p LEFT JOIN formapagamento fp
                                                     ON p.idformapagamento = fp.idformapagamento 
                                                     WHERE idconta_receber = @idconta_receber", Connect.Conexao);
                Connect.Comando.Parameters.AddWithValue("@idconta_receber", contaReceber.ContaReceberID);
                using (var reader = Connect.Comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ParcelaContaReceber parcela = new ParcelaContaReceber
                        {
                            ParcelaContaReceberID = reader.GetInt32(reader.GetOrdinal("idparcela_conta_receber")),
                            DataQuitacao = reader.IsDBNull(reader.GetOrdinal("data_quitacao")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("data_quitacao")),
                            DataVencimento = reader.GetDateTime(reader.GetOrdinal("data_vencimento")),
                            Juros = reader.GetDecimal(reader.GetOrdinal("juros")),
                            Multa = reader.GetDecimal(reader.GetOrdinal("multa")),
                            Acrescimo = reader.GetDecimal(reader.GetOrdinal("acrescimo")),
                            Sequencia = reader.GetInt32(reader.GetOrdinal("sequencia")),
                            Valor = reader.GetDecimal(reader.GetOrdinal("valor"))
                        };
                        parcelas.Add(parcela);
                        //forma de pagamento
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                Connect.FecharConexao();
            }

            return parcelas;
        }
        private ContaReceber LeDadosReader(IDataReader reader)
        {
            var contaReceber = new ContaReceber();
            if (reader.Read())
            {
                contaReceber = new ContaReceber
                {
                    ContaReceberID = reader.GetInt32(reader.GetOrdinal("idconta_receber")),
                    DataCadastro = reader.GetDateTime(reader.GetOrdinal("data_cadastro")),
                    ValorOriginal = reader.GetDecimal(reader.GetOrdinal("valor_original")),
                    Multa = reader.GetDecimal(reader.GetOrdinal("multa")),
                    Juros = reader.GetDecimal(reader.GetOrdinal("juros")),
                    Acrescimo = reader.GetDecimal(reader.GetOrdinal("acrescimo")),
                    ValorFinal = reader.GetDecimal(reader.GetOrdinal("valor_final")),
                };
                contaReceber.Operacao = new Operacao();
                contaReceber.Operacao.OperacaoID = reader.GetInt32(reader.GetOrdinal("idoperacao"));
                contaReceber.Pessoa = new Pessoa();
                contaReceber.Pessoa.PessoaID = reader.GetInt32(reader.GetOrdinal("idpessoa"));
            }
            else
            {
                contaReceber = null;
            }
            return contaReceber;
        }
    }
}
