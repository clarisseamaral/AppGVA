namespace GVA.Dominio
{
    public class ListagemVendaDTO
    {
        public long Id { get; set; }
        public long IdCliente { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string DataVenda { get; set; }
        public string DataVencimento { get; set; }
        public string DataPagamento { get; set; }
    }
}