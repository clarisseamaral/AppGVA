
using System.Data;
using System.Text;
using GVA.Util;

namespace GVA.DataLocal
{
    public class VendaDB : IDataLocal
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string DataVenda { get; set; }
        public string DataVencimento { get; set; }
        public string DataPagamento { get; set; }
        
        public string CaminhoImagem { get; set; }


        public VendaDB(DataRow dr)
        {
            ConvertDr(dr);
        }

        public VendaDB() { }

        public static string TableColumns {
            get {
                return @"Id INTEGER primary key autoincrement, 
                         IdCliente INTEGER, 
                         Descricao ntext, 
                         Valor ntext, 
                         DataVenda ntext, 
                         DataVencimento ntext, 
                         DataPagamento ntext,
                         CaminhoImagem ntext";
            }
        }


        public static string TableName {
            get { return "Venda"; }
        }

        public string InsertQuery {
            get {
                var sb = new StringBuilder();
                sb.AppendFormat("insert into {0} (IdCliente, Descricao, Valor, DataVenda, DataVencimento, DataPagamento, CaminhoImagem)", TableName);

                sb.Append(" values(");
                //sb.AppendFormat("'{0}',", this.Id);
                sb.AppendFormat("'{0}',", this.IdCliente);
                sb.AppendFormat("'{0}',", this.Descricao);
                sb.AppendFormat("'{0}',", this.Valor);
                sb.AppendFormat("'{0}',", this.DataVenda);
                sb.AppendFormat("'{0}',", this.DataVencimento);
                sb.AppendFormat("'{0}',", this.DataPagamento);
                sb.AppendFormat("'{0}'", this.CaminhoImagem);


                sb.Append(")");

                return sb.ToString();
            }
        }

        public string UpdateQuery {
            get {
                var sb = new StringBuilder();
                sb.AppendFormat("update {0} ", TableName);
                sb.AppendFormat("set {0} = '{1}',", "IdCliente", IdCliente);
                sb.AppendFormat("{0} = '{1}',", "Descricao", Descricao);
                sb.AppendFormat("{0} = '{1}',", "Valor", Valor);
                sb.AppendFormat("{0} = '{1}',", "DataVenda", DataVenda);
                sb.AppendFormat("{0} = '{1}',", "DataVencimento", DataVencimento);
                sb.AppendFormat("{0} = '{1}',", "DataPagamento", DataPagamento);
                sb.AppendFormat("{0} = '{1}'", "CaminhoImagem", CaminhoImagem);

                sb.AppendFormat(" where {0} = '{1}'", "Id", Id);

                return sb.ToString();
            }
        }

        public string SelectQuery {
            get {
                var sb = new StringBuilder();
                sb.AppendFormat("SELECT * from {0} ", TableName);
                return sb.ToString();
            }
        }

        public void ConvertDr(DataRow dr)
        {
            int id; int.TryParse(dr["Id"].ToString(), out id);
            Id = id;

            int idCliente; int.TryParse(dr["IdCliente"].ToString(), out idCliente);
            IdCliente = idCliente;

            Valor = dr["Valor"].ToString();
            Descricao = dr["Descricao"].ToString();
            DataVenda = dr["DataVenda"].ToString();
            DataVencimento = dr["DataVencimento"].ToString();
            DataPagamento = dr["DataPagamento"].ToString();
            CaminhoImagem = dr["CaminhoImagem"].ToString();
        }
    }
}
