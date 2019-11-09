namespace GVA.Dominio
{
    public class ListagemVendaDTO
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }

        public string NomeCliente { get; set; }

        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string DataVenda { get; set; }
        public string DataVencimento { get; set; }
        public string DataPagamento { get; set; }
    }
}