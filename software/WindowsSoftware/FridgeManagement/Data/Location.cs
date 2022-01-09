using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FridgeManagement.Data {
  /// <summary>
  /// Container for one location
  /// </summary>
  [DebuggerDisplay("Location {name}")]
  public class Location : BaseNamedDataItem {
    static string tableName = "locations";
    
    #region Object Construction
    /// <summary>
    /// load all locations from the SQL connection to the target list
    /// </summary>
    public static void loadDataFromSql(MySqlConnection connection, BindingList<Location> targetList)
    {
      targetList.Clear();
      var cmd = connection.CreateCommand();
      cmd.CommandText = @"SELECT id, name, UNIX_TIMESTAMP(last_changed) as last_changed FROM " + tableName + " ORDER BY name";
      var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        targetList.Add(new Location(connection, reader.GetUInt32("id"), reader.GetString("name"), reader.GetUInt32("last_changed")));
      }
      reader.Close();
    }

    internal static Location createAndAddItem(MySqlConnection connection, string name)
    {
      var cmd = connection.CreateCommand();
      cmd.CommandText = "INSERT INTO " + tableName + "(name) VALUES (@name)";
      cmd.Parameters.AddWithValue("@name", name);
      cmd.ExecuteNonQuery();

      return new Location(connection, (UInt32)cmd.LastInsertedId, name);
    }
    #endregion


    protected Location(MySqlConnection connection, UInt32 id, string name)
      : base(connection, tableName, id, name)
    {
    }

    protected Location(MySqlConnection connection, UInt32 id, string name, UInt32 lastChanged)
      : base(connection, tableName, id, name, lastChanged)
    {
    }
  }
}
