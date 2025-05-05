using System;

namespace Projeto1.Models
{
    public class Usuario
    {
        public int ID { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string SenhaHash { get; set; }
        public string Unidade { get; set; }
        public string Perfil { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Telefone { get; set; }
        public string Matricula { get; set; }
        public bool PrimeiroAcesso { get; set; } = true;
    }
}