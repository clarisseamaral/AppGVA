using System;

namespace GVA.Entidades
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Observacoes { get; set; }

    }
}