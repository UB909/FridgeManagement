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
  /// Container for one entry
  /// </summary>
  [DebuggerDisplay("Entry {id}")]
  public class Entry : BaseDataItem {
    static string tableName = "entries";

    #region Internal Variables
    private Item _item;
    private Location _location;
    private int _numberOfItems;
    #endregion

    #region Properties
    /// <summary>
    /// The item
    /// </summary>
    public Item item { get => _item; }

    /// <summary>
    /// The location
    /// </summary>
    public Location location { get => _location; }

    /// <summary>
    /// Number of Items at the given location
    /// </summary>
    public int numberOfItems {
      get => _numberOfItems;
      set {
        if (_numberOfItems != value)
        {
          _numberOfItems = value;
          if (id == 0)
          {
            // check if a new line has to be added to the database
            var cmd = _connection.CreateCommand();

            cmd.CommandText = "INSERT INTO " + tableName + "(item_id, location_id, number_of_items) VALUES (@item_id, @location_id, @number_of_items)";
            cmd.Parameters.AddWithValue("@item_id", item.id);
            cmd.Parameters.AddWithValue("@location_id", location.id);
            cmd.Parameters.AddWithValue("@number_of_items", numberOfItems);
            cmd.ExecuteNonQuery();
            _id = (UInt32)cmd.LastInsertedId;
            _lastChanged = loadLastChanged();
            NotifyPropertyChanged();
          }
          else
          {
            UpdateDatabaseAndNotifyPropertyChanged("numberOfItems", "number_of_items");
          }
        }
      }
    }
    #endregion

    #region Object Construction
    /// <summary>
    /// load all categories from the SQL connection to the target list
    /// </summary>
    public static void loadDataFromSql(MySqlConnection connection, BindingList<Entry> targetList, BindingList<Item> items, BindingList<Location> locations)
    {
      targetList.Clear();
      var cmd = connection.CreateCommand();
      cmd.CommandText = @"SELECT id, item_id, location_id, number_of_items, UNIX_TIMESTAMP(last_changed) as last_changed FROM " + tableName + "";
      var reader = cmd.ExecuteReader();

      while (reader.Read())
      {
        var newEntry = new Entry(
          connection, 
		      reader.GetUInt32("id"),
          DataContainer.GetItem(reader.GetUInt32("item_id")),
          DataContainer.GetLocation(reader.GetUInt32("location_id")),
          reader.GetInt32("number_of_items"),
          reader.GetUInt32("last_changed"));

        newEntry.item.entriesOfItem.Add(newEntry);
        targetList.Add(newEntry);
      }
      reader.Close();
    }

    /// <summary>
    /// Creates an object without adding it to the database. It will be added as soon as the number of items is changed.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    public static Entry createDelayed(MySqlConnection connection, Item item, Location location)
    {
      return new Entry(connection, 0, item, location, 0, 0);
    }
    #endregion


    private Entry(MySqlConnection connection, UInt32 id, Item item, Location location, int numberOfItems, UInt32 lastChanged)
      : base(connection, tableName, id, lastChanged)
    {
      this._item = item;
      this._location = location;
      this._numberOfItems = numberOfItems;
    }

    internal override byte[] getHashData()
    {
      byte[] parentData = base.getHashData();
      byte[] retData = new byte[parentData.Length + 2*sizeof(UInt32) + sizeof(int)];

      int targetByte = 0;
      parentData.CopyTo(retData, targetByte);
      targetByte += parentData.Length;
      BitConverter.GetBytes(_item.id).CopyTo(retData, targetByte);
      targetByte += sizeof(UInt32);
      BitConverter.GetBytes(_location.id).CopyTo(retData, targetByte);
      targetByte += sizeof(UInt32);
      BitConverter.GetBytes(_numberOfItems).CopyTo(retData, targetByte);

      return retData;
    }
  }
}
