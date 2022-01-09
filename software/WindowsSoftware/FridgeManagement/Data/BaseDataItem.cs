using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagement.Data {
  public class BaseDataItem : INotifyPropertyChanged {
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    protected void UpdateDatabaseAndNotifyPropertyChanged([CallerMemberName] string propertyName = "", [CallerMemberName] string sqlName = "", Object sqlValue = null)
    {
      if (_connection != null)
      {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = "UPDATE " + _tableName + " SET " + sqlName + " = @value, last_changed = CURRENT_TIMESTAMP WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        if (sqlValue != null)
        {
          cmd.Parameters.AddWithValue("@value", sqlValue);
        }
        else
        {
          cmd.Parameters.AddWithValue("@value", GetType().GetProperty(propertyName).GetValue(this));
        }
        cmd.ExecuteNonQuery();
        this._lastChanged = loadLastChanged();
      }

      NotifyPropertyChanged(propertyName);
    }
    #endregion

    #region Internal Variables
    protected string _tableName;
    protected UInt32 _id;
    protected UInt32 _lastChanged;
    protected MySqlConnection _connection;
    #endregion

    #region Properties
    /// <summary>
    /// ID of the location
    /// </summary>
    public UInt32 id {
      get => _id;
    }
    #endregion

    protected BaseDataItem(MySqlConnection connection, string tableName, UInt32 id)
    {
      this._connection = connection;
      this._tableName = tableName;
      this._id = id;
      if (id != 0)
      {
        this._lastChanged = loadLastChanged();
      }
    }

    protected BaseDataItem(MySqlConnection connection, string tableName, UInt32 id, UInt32 lastChanged) 
    {
      this._connection = connection;
      this._tableName = tableName;
      this._id = id;
      this._lastChanged = lastChanged;
    }

    internal virtual byte[] getHashData()
    {
      byte[] retData = new byte[sizeof(UInt32) + sizeof(UInt32)];
      BitConverter.GetBytes(_id).CopyTo(retData, 0);
      BitConverter.GetBytes(_lastChanged).CopyTo(retData, sizeof(UInt32));

      return retData;
    }
    public byte[] getHash()
    {
      return MD5.Create().ComputeHash(getHashData());
    }

    protected UInt32 loadLastChanged(string fieldname = "last_changed")
    {
      var cmd = _connection.CreateCommand();
      cmd.CommandText = @"SELECT UNIX_TIMESTAMP("+ fieldname+") as last_changed FROM " + _tableName + " WHERE id = @id";
      cmd.Parameters.AddWithValue("@id", id);
      var reader = cmd.ExecuteReader();
      if (!reader.Read())
      {
        throw new Exception("cannot load last_changed timestamp from " + _tableName);
      }
      UInt32 t = reader.GetUInt32("last_changed");
      reader.Close();
      return t;
    }
  }
}
