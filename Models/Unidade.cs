using System;

namespace Projeto1.Models
{
    public class Unidade
    {
        public int UnidadeID { get; set; }
        public string Nome { get; set; } 
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; } 
    }
}