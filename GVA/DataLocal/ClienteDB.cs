using System.Data;
using System.Text;
using GVA.Util;

namespace GVA.DataLocal
{
    public class ClienteDB : IDataLocal
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Observacoes { get; set; }

        public ClienteDB(DataRow dr)
        {
            ConvertDr(dr);
        }

        public ClienteDB() { }

        public static string TableColumns {
            get {
                return @"IdCliente INTEGER primary key autoincrement, 
                         Nome ntext, 
                         Email ntext, 
                         Telefone ntext, 
                         DataNascimento ntext, 
                         Observacoes ntext,
                         Endereco ntext";
            }
        }


        public static string TableName {
            get { return "Cliente"; }
        }

        public string InsertQuery {
            get {
                var sb = new StringBuilder();
                sb.AppendFormat("insert into {0} (Nome, Email, Telefone, DataNascimento, Observacoes,Endereco)", TableName);

                sb.Append(" values(");
                //sb.AppendFormat("'{0}',", this.IdCliente);
                sb.AppendFormat("'{0}',", this.Nome);
                sb.AppendFormat("'{0}',", this.Email);
                sb.AppendFormat("'{0}',", this.Telefone);
                sb.AppendFormat("'{0}',", this.DataNascimento);
                sb.AppendFormat("'{0}',", this.Observacoes);
                sb.AppendFormat("'{0}'", this.Endereco);

                sb.Append(")");

                return sb.ToString();
            }
        }

        public string UpdateQuery {
            get {
                var sb = new StringBuilder();
                sb.AppendFormat("update {0} ", TableName);
                sb.AppendFormat("set {0} = '{1}',", "Nome", Nome);
                sb.AppendFormat("{0} = '{1}',", "Email", Email);
                sb.AppendFormat("{0} = '{1}',", "Telefone", Telefone);
                sb.AppendFormat("{0} = '{1}',", "DataNascimento", DataNascimento);
                sb.AppendFormat("{0} = '{1}',", "Endereco", Endereco);
                sb.AppendFormat("{0} = '{1}'", "Observacoes", Observacoes);

                sb.AppendFormat(" where {0} = '{1}'", "IdCliente", IdCliente);

                return sb.ToString();
            }
        }

        public string SelectQuery {
            get {
                var sb = new StringBuilder();
                sb.AppendFormat("SELECT * from {0} c INNER JOIN Venda v ON v.IdCliente = c.IdCliente", TableName);
                return sb.ToString();
            }
        }

        public void ConvertDr(DataRow dr)
        {
            int idCliente; int.TryParse(dr["IdCliente"].ToString(), out idCliente);
            IdCliente = idCliente;

            Nome = dr["Nome"].ToString();
            Email = dr["Email"].ToString();
            Telefone = dr["Telefone"].ToString();
            DataNascimento = dr["DataNascimento"].ToString();
            Observacoes = dr["Observacoes"].ToString();
            Endereco = dr["Endereco"].ToString();
        }
    }
}