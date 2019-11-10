using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Threading.Tasks;

namespace GVA.Util
{
    public static class UtilDataBase
    {
        public static string CaminhoBanco {
            get {

                var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var pathToDatabase = System.IO.Path.Combine(docsFolder, "BancoLocal.db");
                return pathToDatabase;
            }
        }

        public static string ConnectionString {
            get {
                return string.Format("Data Source={0};Version=3;", CaminhoBanco);
            }
        }

        public static bool CriarBanco()
        {
            bool returnValue = false;
            try
            {
                if (!File.Exists(CaminhoBanco))
                    SqliteConnection.CreateFile(CaminhoBanco);

                returnValue = true;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Erro ao criar banco!");
            }

            return returnValue;
        }

        public static bool CriarTabela(string nameTable, string columns)
        {
            if (!VerificaTabela(nameTable))
            {
                using (var conn = new SqliteConnection((ConnectionString)))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = string.Format("CREATE TABLE {0} ({1})", nameTable, columns);
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool VerificaTabela(string nameTable)
        {
            bool returnValue = false;

            try
            {
                using (var conn = new SqliteConnection((ConnectionString)))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = string.Format("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = '{0}'", nameTable);
                        command.CommandType = CommandType.Text;
                        var obj = command.ExecuteScalar();

                        if (obj is long)
                        {
                            returnValue = ((long)obj) > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Erro to verify table!");
            }

            return returnValue;
        }

        public static void Save(string query)
        {

            using (var conn = new SqliteConnection((ConnectionString)))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Update(string query)
        {

            using (var conn = new SqliteConnection((ConnectionString)))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(string tableName, string filtro)
        {
            using (var conn = new SqliteConnection((ConnectionString)))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = string.Format("delete from {0} where {1}", tableName, filtro);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static DataTable GetItems(string nameTable, string filtro = null, string ordernacao = null)
        {
            DataTable dataTableResult = new DataTable();

            using (var conn = new SqliteConnection((ConnectionString)))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = string.Format("select * from {0}", nameTable);
                    if (!string.IsNullOrWhiteSpace(filtro))
                    {
                        query += string.Format(" where {0} ", filtro);
                    }

                    if (!string.IsNullOrWhiteSpace(ordernacao))
                    {
                        query += (" " + ordernacao);
                    }

                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    using (SqliteDataReader rdr = command.ExecuteReader())
                    {
                        dataTableResult.Load(rdr);
                    }
                }
            }

            return dataTableResult;
        }

        public static DataTable GetItemsQuery(string query)
        {
            DataTable dataTableResult = new DataTable();

            using (var conn = new SqliteConnection((ConnectionString)))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    using (SqliteDataReader rdr = command.ExecuteReader())
                    {
                        dataTableResult.Load(rdr);
                    }
                }
            }
            return dataTableResult;
        }
      
    }
}