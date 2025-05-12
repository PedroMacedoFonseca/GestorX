using Projeto1.DAL;
using Projeto1.Models;

namespace Projeto1.Services
{
    public class UsuarioService
    {
        private readonly UsuarioDAL _dal = new UsuarioDAL();

        public void Inserir(Usuario usuario) => _dal.Inserir(usuario);
        public void Atualizar(Usuario usuario) => _dal.Atualizar(usuario);
        public Usuario ObterPorId(int id) => _dal.ObterPorId(id);
        public bool CpfExiste(string cpf, int idAtual) => _dal.ExisteCPF(cpf, idAtual);
    }
}