﻿using _5gpro.Daos;
using _5gpro.Entities;
using _5gpro.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5gpro.Bll
{
    class PermissaoBLL
    {
        private readonly PermissaoDAO permissaoDAO = new PermissaoDAO();
      

        public fmCadastroGrupoUsuario.PermissoesStruct BuscaPermissoesGrupo(string cod)
        {
            return permissaoDAO.BuscaPermissoesByIdGrupo(cod);
        }

        public fmCadastroGrupoUsuario.PermissoesStruct BuscaTodasPermissoes()
        {
            return permissaoDAO.BuscaTodasPermissoes();
        }

        public int BuscarIDbyCodigo(string codpermissao)
        {
            return permissaoDAO.BuscarIDbyCodigo(codpermissao);
        }

        public int BuscarNivelPermissao(string codgrupousuario, string codpermissao)
        {
            return permissaoDAO.BuscarNivelPermissao(codgrupousuario, codpermissao);
        }

        public List<int> BuscarIDPermissoesNpraN()
        {
            return permissaoDAO.BuscarIDPermissoesNpraN();
        }

        public List<fmMain.PermissaoNivelStruct> PermissoesNiveisStructByCodGrupoUsuario(string codgrupousuario)
        {
            return permissaoDAO.PermissoesNiveisStructByCodGrupoUsuario(codgrupousuario);
        }




    }
}


