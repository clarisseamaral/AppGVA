using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using GVA.DataLocal;
using GVA.Util;
using System;
using System.Data;

namespace GVA
{
    [Activity(Label = "Cadastrar Cliente", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CadastrarClienteActivity : Activity
    {
        EditText DataNascimento;
        EditText Email;
        EditText Endereco;
        EditText Nome;
        EditText Observacoes;
        EditText Telefone;

        public int IdCliente { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.cadastrar_cliente);

            CarregarElementos();

            if (!String.IsNullOrEmpty(Intent.GetStringExtra("FluxoEdicaoCliente")))
            {
                IdCliente = int.Parse(Intent.GetStringExtra("FluxoEdicaoCliente"));

                var where = string.Format(" IdCliente = {0}", IdCliente);
                var dtClientes = UtilDataBase.GetItems(ClienteDB.TableName, where, null);

                ExibirCliente(dtClientes.Rows[0]);
            }

        }

        private void CarregarElementos()
        {
            FindViewById<Button>(Resource.Id.btnSalvarCliente).Click += Salvar_Click;

            FindViewById<Button>(Resource.Id.btnCancelarCliente).Click += Cancelar_Click;

            DataNascimento = FindViewById<EditText>(Resource.Id.txtDataNascimento);
            Email = FindViewById<EditText>(Resource.Id.txtEmail);
            Endereco = FindViewById<EditText>(Resource.Id.txtEndereco);
            Nome = FindViewById<EditText>(Resource.Id.txtNome);
            Observacoes = FindViewById<EditText>(Resource.Id.txtObservacoes);
            Telefone = FindViewById<EditText>(Resource.Id.txtTelefone);
        }

        private void ExibirCliente(DataRow dataRow)
        {
            DataNascimento.Text = dataRow["DataNascimento"].ToString();
            Email.Text = dataRow["Email"].ToString();
            Endereco.Text = dataRow["Endereco"].ToString();
            Nome.Text = dataRow["Nome"].ToString();
            Observacoes.Text = dataRow["Observacoes"].ToString();
            Telefone.Text = dataRow["Telefone"].ToString();
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void Salvar_Click(object sender, EventArgs e)
        {
            if (VerificaNomePreenchido())
            {
                AtualizarBancoLocal();
                Toast.MakeText(this, "Dados salvos com sucesso.", ToastLength.Long).Show();
                Finish();
                var intentLista = new Intent(this, typeof(ListarClienteActivity));
                StartActivity(intentLista);
            }
            else
            {
                Toast.MakeText(this, "Favor preencher o nome do cliente.", ToastLength.Long).Show();
            }
        }

        private bool VerificaNomePreenchido()
        {
            return !string.IsNullOrEmpty((FindViewById<EditText>(Resource.Id.txtNome)).Text);
        }


        private void AtualizarBancoLocal()
        {
            var tbCliente = new ClienteDB()
            {
                DataNascimento = DataNascimento.Text,
                Email = Email.Text.Trim(),
                Endereco = Endereco.Text.Trim(),
                Nome = Nome.Text.Trim(),
                Observacoes = Observacoes.Text.Trim(),
                Telefone = Telefone.Text
            };

            var stringBuilder = new System.Text.StringBuilder();

            if (IdCliente > 0)
            {
                tbCliente.IdCliente = IdCliente;
                stringBuilder.AppendFormat("{0};", tbCliente.UpdateQuery);
            }
            else
            {
                stringBuilder.AppendFormat("{0};", tbCliente.InsertQuery);
            }

            UtilDataBase.Save(stringBuilder.ToString());
        }
    }
}