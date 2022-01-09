using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagement.Data {
  class DataContainer {
    #region Data Container
    private static DataContainer instance = new DataContainer();
    public static BindingList<Category> categories { get => instance._categories; }
    public static BindingList<Location> locations { get => instance._locations; }
    public static BindingList<Item> items { get => instance._items; }
    public static BindingList<Entry> entries { get => instance._entries; }

    public static MySqlConnection localConnection { get => instance.connection; }

    public static void loadLocalInitialData(MySqlConnection connection)
    {
      instance.loadInitialData(connection);
    }
    #endregion

    #region GetById
    protected static T GetById<T>(BindingList<T> lst, UInt32 id) where T : BaseDataItem
    {
      foreach (T val in lst)
      {
        if (val.id == id)
        {
          return val;
        }
      }
      return null;
    }
    public static Category GetCategory(UInt32 id) => GetById<Category>(categories, id);
    public static Location GetLocation(UInt32 id) => GetById<Location>(locations, id);
    public static Item GetItem(UInt32 id) => GetById<Item>(items, id);
    public static Entry GetEntry(UInt32 id) => GetById<Entry>(entries, id);
    #endregion

    #region Hashing
    protected static byte[] GetHash<T>(BindingList<T> lst) where T : BaseDataItem
    {
      byte[] buffer = new byte[16 * lst.Count];

      int offset = 0;
      foreach (T val in lst)
      {
        val.getHash().CopyTo(buffer, offset);
        offset += 16;
      }
      return MD5.Create().ComputeHash(buffer);
    }

    public static string BytesToStr(byte[] bytes)
    {
      StringBuilder str = new StringBuilder();
      foreach (var b in bytes)
      {
        str.AppendFormat("{0:X2}", b);
      }

      return str.ToString();
    }
    #endregion

    public static void synchronizeOverWifi()
    {
      //nicht sdtatisch getbyId

      //DataContainer remoteContainer = new DataContainer();

      //var builder = new MySqlConnectionStringBuilder
      //{
      //  Server = "192.168.xxx.xxx",
      //  UserID = "fridge",
      //  Password = "xxxx",
      //  Database = "fridge",
      //};

      //// open a connection and load everything from there
      //MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
      //connection.Open();
      //remoteContainer.loadInitialData(connection);

      //instance.synchronizeWifi(remoteContainer);
    }

    #region Instance
    private MySqlConnection connection = null;

    #region Containers
    private BindingList<Category> _categories = new BindingList<Category>();

    private BindingList<Location> _locations { get; } = new BindingList<Location>();
    private BindingList<Item> _items { get; } = new BindingList<Item>();
    private BindingList<Entry> _entries { get; } = new BindingList<Entry>();
    #endregion

    void loadInitialData(MySqlConnection connection)
    {
      this.connection = connection;
      Location.loadDataFromSql(connection, _locations);
      Category.loadDataFromSql(connection, _categories);
      Item.loadDataFromSql(connection, _items, _categories);
      Entry.loadDataFromSql(connection, _entries, _items, _locations);
    }

    #region synchronization
    internal void synchronizeWifi(DataContainer remote)
    {
      byte[] localHash;
      byte[] remoteHash;

      localHash = GetHash<Location>(_locations);
      remoteHash = GetHash<Location>(remote._locations);

      if(!localHash.SequenceEqual(remoteHash))
      {
        for(int i = 0; i < _locations.Count; i++)
        {

        }
      }
    }
    #endregion

    #endregion

  }
}
