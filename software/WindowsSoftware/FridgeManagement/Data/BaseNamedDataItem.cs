using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagement.Data {
  public class BaseNamedDataItem : BaseDataItem {
    #region Internal Variables
    private string _name;
    #endregion

    #region Properties

    /// <summary>
    /// Name of the location
    /// </summary>
    public String name {
      get => _name;
      set {
        if (_name != value)
        {
          _name = value;
          UpdateDatabaseAndNotifyPropertyChanged();
        }
      }
    }
    #endregion

    protected BaseNamedDataItem(MySqlConnection connection, string tableName, UInt32 id, string name)
      : base(connection, tableName, id)
    {
      this._name = name;
    }

    protected BaseNamedDataItem(MySqlConnection connection, string tableName, UInt32 id, string name, UInt32 lastChanged)
      : base(connection, tableName, id, lastChanged)
    {
      this._name = name;
    }

    internal override byte[] getHashData()
    {
      byte[] parentData = base.getHashData();
      byte[] retData = new byte[parentData.Length + _name.Length];

      parentData.CopyTo(retData, 0);
      Encoding.ASCII.GetBytes(_name).CopyTo(retData, parentData.Length);

      return retData;
    }
  }
}

