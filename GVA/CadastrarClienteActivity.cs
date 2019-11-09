using Android.App;
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.cadastrar_cliente);

            FindViewById<Button>(Resource.Id.btnSalvarCliente).Click += Salvar_Click;
        }

        private void Salvar_Click(object sender, EventArgs e)
        {
            if (VerificaNomePreenchido())
            {
                InserirBancoLocal();
            }
            else
            {
                ///todo: alerta informando que o nome é obrigatório!
            }
        }

        private bool VerificaNomePreenchido()
        {
            return !string.IsNullOrEmpty((FindViewById<EditText>(Resource.Id.txtDataNascimento)).Text);
        }


        private void InserirBancoLocal()
        {
            var tbCliente = new ClienteDB()
            {
                DataNascimento = (FindViewById<EditText>(Resource.Id.txtDataNascimento)).Text,
                Email = (FindViewById<EditText>(Resource.Id.txtEmail)).Text,
                Endereco = (FindViewById<EditText>(Resource.Id.txtEndereco)).Text,
                Nome = (FindViewById<EditText>(Resource.Id.txtNome)).Text,
                Observacoes = (FindViewById<EditText>(Resource.Id.txtObservacoes)).Text,
                Telefone = (FindViewById<EditText>(Resource.Id.txtTelefone)).Text
            };

            var stringBuilder = new System.Text.StringBuilder();

            stringBuilder.AppendFormat("{0};", tbCliente.InsertQuery);

            UtilDataBase.Save(stringBuilder.ToString());
        }
    }
}