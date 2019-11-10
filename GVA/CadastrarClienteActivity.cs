using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using GVA.DataLocal;
using GVA.Util;
using System;

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

                ExibirCliente(new ClienteDB(dtClientes.Rows[0]));
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

        private void ExibirCliente(ClienteDB cliente)
        {
            FindViewById<LinearLayout>(Resource.Id.linearLayout4).Visibility = Android.Views.ViewStates.Visible;

            FindViewById<Button>(Resource.Id.btnApagarCliente).Click += Apagar_Click;

            DataNascimento.Text = cliente.DataNascimento;
            Email.Text = cliente.Email;
            Endereco.Text = cliente.Endereco;
            Nome.Text = cliente.Nome;
            Observacoes.Text = cliente.Observacoes;
            Telefone.Text = cliente.Telefone;
        }

        

        private void RedirecionarListagem()
        {
            Finish();
            var intentLista = new Intent(this, typeof(ListarClienteActivity));
            StartActivity(intentLista);
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

        #region Eventos
        private void Apagar_Click(object sender, System.EventArgs e)
        {
            var dtVendas = UtilDataBase.GetItems(VendaDB.TableName, string.Format(" IdCliente = {0}", IdCliente));
            if (dtVendas.Rows.Count > 0)
            {
                Toast.MakeText(this, "Não é possível apagar clientes que possuem vendas vinculadas!", ToastLength.Long).Show();
            }
            else
            {
                UtilDataBase.Delete(ClienteDB.TableName, string.Format(" IdCliente = {0}", IdCliente));
                Toast.MakeText(this, "Cliente apagado com sucesso!", ToastLength.Long).Show();
                RedirecionarListagem();
            }
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
                RedirecionarListagem();
            }
            else
            {
                Toast.MakeText(this, "Favor preencher o nome do cliente.", ToastLength.Long).Show();
            }
        }
        #endregion

    }
}