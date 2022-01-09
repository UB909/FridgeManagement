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
  /// Container for one category
  /// </summary>
  [DebuggerDisplay("Category {name}")]
  public class Category : BaseNamedDataItem {
    static string tableName = "categories";

    #region Object Construction
    /// <summary>
    /// load all categories from the SQL connection to the target list
    /// </summary>
    public static void loadDataFromSql(MySqlConnection connection, BindingList<Category> targetList)
    {
      targetList.Clear();
      var cmd = connection.CreateCommand();
      cmd.CommandText = @"SELECT id, name, UNIX_TIMESTAMP(last_changed) as last_changed FROM " + tableName + " ORDER BY name";
      var reader = cmd.ExecuteReader();

      while (reader.Read())
      {
        targetList.Add(new Category(connection, reader.GetUInt32("id"), reader.GetString("name"), reader.GetUInt32("last_changed")));
      }
      reader.Close();
    }

    internal static Category createAndAddItem(MySqlConnection connection, string name)
    {
      var cmd = connection.CreateCommand();
      cmd.CommandText = "INSERT INTO " + tableName + "(name) VALUES (@name)";
      cmd.Parameters.AddWithValue("@name", name);

      cmd.ExecuteNonQuery();

      return new Category(connection, (UInt32)cmd.LastInsertedId, name);
    }
    #endregion


    internal Category(MySqlConnection connection, UInt32 id, string name)
      : base(connection, tableName, id, name)
    {
    }

    protected Category(MySqlConnection connection, UInt32 id, string name, UInt32 lastChanged)
      : base(connection, tableName, id, name, lastChanged)
    {
    }
  }
}
