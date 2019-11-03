
using System.Data;

namespace GVA.Util
{
    public interface IDataLocal
    {
        string InsertQuery { get; }

        string UpdateQuery { get; }

        void ConvertDr(DataRow dr);
    }
}