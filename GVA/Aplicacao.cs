﻿using System;
using Android.App;
using Android.Runtime;
using GVA.DataLocal;
using GVA.Util;

namespace GVA
{
    [Application]
    public class Aplicacao : Application
    {
        public Aplicacao(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            UtilDataBase.CriarBanco();

            //Carrega Dados Iniciais
            if (!UtilDataBase.VerificaTabela(ClienteDB.TableName))
                UtilDataBase.CriarTabela(ClienteDB.TableName, ClienteDB.TableColumns);

            if (!UtilDataBase.VerificaTabela(VendaDB.TableName))
                UtilDataBase.CriarTabela(VendaDB.TableName, VendaDB.TableColumns);

        }

        public static void ResetarAplicacao()
        {
            UtilDataBase.Delete(VendaDB.TableName, string.Empty);
            UtilDataBase.Delete(ClienteDB.TableName, string.Empty);
        }
    }
  
}