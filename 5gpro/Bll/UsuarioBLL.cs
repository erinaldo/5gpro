﻿using _5gpro.Daos;
using _5gpro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5gpro.Bll
{
    class UsuarioBLL
    {
        UsuarioDAO usuarioDAO = new UsuarioDAO();

        public int SalvarOuAtualizarUsuario(Usuario usuario)
        {
            return usuarioDAO.SalvarOuAtualizarUsuario(usuario);
        }

        public Usuario Logar(string login, string senha)
        {
            return usuarioDAO.Logar(login, senha);
        }

        public string BuscaProxCodigoDisponivel()
        {
            return usuarioDAO.BuscaProxCodigoDisponivel();
        }

        public Usuario BuscarUsuarioById(string cod)
        {
            return usuarioDAO.BuscarUsuarioById(cod);
        }

        public Usuario BuscarProximoUsuario(string codAtual)
        {
            return usuarioDAO.BuscarProximoUsuario(codAtual);
        }

        public Usuario BuscarUsuarioAnterior(string codAtual)
        {
            return usuarioDAO.BuscarUsuarioAnterior(codAtual);
        }
    }
}
