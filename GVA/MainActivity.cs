using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content.PM;
using GVA.Util;
using GVA.DataLocal;

namespace GVA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : ListActivity
    {
        static readonly string[] countries = new String[] {
                "Afghanistan","Albania","Algeria","American Samoa","Andorra",
                "Angola","Anguilla","Antarctica","Antigua and Barbuda","Argentina",
                "Armenia","Aruba","Australia","Austria","Azerbaijan"
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //SetContentView(Resource.Layout.activity_main);

            base.OnCreate(savedInstanceState);

            var dtVendas = UtilDataBase.GetItems(VendaDB.TableName);

            if (dtVendas.Rows.Count > 0)
            {
                ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.venda_item, countries);

                ListView.TextFilterEnabled = true;

                ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();
                };

            }
            else
            {
                SetContentView(Resource.Layout.activity_main);
            }
        }

        private void AtribuirEventos()
        {
            //FindViewById<Button>(Resource.Id.btnLogin).Click += BtnLogin_Click;
        }
    }
}