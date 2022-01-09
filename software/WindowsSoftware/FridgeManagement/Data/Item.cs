using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FridgeManagement.Data {
  /// <summary>
  /// Container for one item
  /// </summary>
  [DebuggerDisplay("Item {name}")]
  public class Item : BaseNamedDataItem {
    static string tableName = "items";

    #region Internal Variables
    Category _category;

    byte[] _imageRawData;
    BitmapFrame _image;
    FormatConvertedBitmap _imageGray;
    UInt32 _lastChangedImage;

    BindingList<Entry> _entriesOfItem = new BindingList<Entry>();

    #endregion

    #region Properties
    /// <summary>
    /// Category of the item
    /// </summary>
    public Category category {
      get => _category;
      set {
        if (_category != value)
        {
          _category = value;
          UpdateDatabaseAndNotifyPropertyChanged("category", "category_id", value.id);
        }
      }
    }

    /// <summary>
    /// The image of the item if available
    /// </summary>
    public BitmapFrame image {
      get => _image;
      private set {
        if (_image != value)
        {
          _image = value;
          FormatConvertedBitmap grayBitmap = new FormatConvertedBitmap();
          grayBitmap.BeginInit();
          grayBitmap.Source = value;
          grayBitmap.DestinationFormat = PixelFormats.Gray8;
          grayBitmap.EndInit();
          grayBitmap.Freeze();
          imageGray = grayBitmap;
          NotifyPropertyChanged();
        }
      }
    }

    /// <summary>
    /// The grayscaled image of the item if available
    /// </summary>
    public FormatConvertedBitmap imageGray {
      get => _imageGray;
      private set {
        if (_imageGray != value)
        {
          _imageGray = value;
          NotifyPropertyChanged();
        }
      }
    }

    /// <summary>
    /// List of items of this item
    /// </summary>
    public BindingList<Entry> entriesOfItem { get => _entriesOfItem; }
    #endregion

    #region Object Construction
    /// <summary>
    /// load all items from the SQL connection to the target list
    /// </summary>
    public static void loadDataFromSql(MySqlConnection connection, BindingList<Item> targetList, BindingList<Category> categories)
    {
      targetList.Clear();
      var cmd = connection.CreateCommand();
      cmd.CommandText = @"SELECT id, name, category_id, image_size, image, UNIX_TIMESTAMP(last_changed) as last_changed, UNIX_TIMESTAMP(last_changed_image) as last_changed_image FROM " + tableName + " ORDER BY name";
      var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        Item newItem = new Item(
          connection, 
		      reader.GetUInt32("id"),
          reader.GetString("name"),
          DataContainer.GetCategory(reader.GetUInt32("category_id")),
          reader.GetUInt32("last_changed"),
          reader.GetUInt32("last_changed_image"));

        // load image if stored
        if (!reader.IsDBNull(reader.GetOrdinal("image_size")))
        {
          int imageSize = reader.GetInt32("image_size");
          newItem._imageRawData = new byte[imageSize];
          reader.GetBytes(reader.GetOrdinal("image"), 0, newItem._imageRawData, 0, (int)imageSize);
          MemoryStream strm = new MemoryStream(newItem._imageRawData);
          newItem.image = new PngBitmapDecoder(strm, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
        }

        targetList.Add(newItem);
      }
      reader.Close();
    }

    internal static Item createAndAddItem(MySqlConnection connection, string name, Category category, string pathToImage)
    {
      BitmapFrame image = null;
      var cmd = connection.CreateCommand();
      byte[] imageRawData = null;

      if (pathToImage == "")
      {
        cmd.CommandText = "INSERT INTO " + tableName + " (name, category_id, image_size, image) VALUES (@name, @categoryId, NULL, NULL)";
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@categoryId", category.id);

        cmd.ExecuteNonQuery();
      }
      else
      {
        imageRawData = File.ReadAllBytes(pathToImage);
        MemoryStream strm = new MemoryStream(imageRawData);
        image = new PngBitmapDecoder(strm, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];

        cmd.CommandText = "INSERT INTO " + tableName + " (name, category_id, image_size, image) VALUES (@name, @categoryId, @imageSize, @imageData)";
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@categoryId", category.id);
        cmd.Parameters.AddWithValue("@imageSize", imageRawData.Length);
        cmd.Parameters.AddWithValue("@imageData", imageRawData);

        cmd.ExecuteNonQuery();
      }

      Item newItem = new Item(connection, (UInt32)cmd.LastInsertedId, name, category);
      newItem.image = image;
      newItem._imageRawData = imageRawData;
      return newItem;
    }
    #endregion

    protected Item(MySqlConnection connection, UInt32 id, string name, Category category)
      : base(connection, tableName, id, name)
    {
      this._category = category;
      this._lastChangedImage = loadLastChanged("last_changed_image");
    }
    protected Item(MySqlConnection connection, UInt32 id, string name, Category category, UInt32 lastChanged, UInt32 lastChangedImage)
      : base(connection, tableName, id, name, lastChanged)
    {
      this._category = category;
      this._lastChangedImage = lastChangedImage;
    }

    internal override byte[] getHashData()
    {
      byte[] parentData = base.getHashData();
      byte[] retData = new byte[parentData.Length + sizeof(UInt32)];

      parentData.CopyTo(retData, 0);
      BitConverter.GetBytes(_category.id).CopyTo(retData, parentData.Length);

      return retData;
    }

    internal byte[] getHashDataWithImage()
    {
      if(_imageRawData == null)
      {
        return getHashData();
      }

      byte[] mainData = getHashData();
      byte[] retData = new byte[mainData.Length + _imageRawData.Length];

      mainData.CopyTo(retData, 0);
      mainData.CopyTo(_imageRawData, mainData.Length);

      return retData;
    }
    public byte[] getHashWithImage()
    {
      return MD5.Create().ComputeHash(getHashDataWithImage());
    }

    /// <summary>
    /// updates the picture from a file
    /// </summary>
    /// <param name="path"></param>
    public void setImage(String path)
    {
      var rawData = File.ReadAllBytes(path);
      MemoryStream strm = new MemoryStream(rawData);
      image = new PngBitmapDecoder(strm, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];

      var cmd = _connection.CreateCommand();
      cmd.CommandText = "UPDATE " + tableName + " SET image_size = @imageSize, image = @imageData, last_changed_image = CURRENT_TIMESTAMP WHERE id = @id";
      cmd.Parameters.AddWithValue("@id", id);
      cmd.Parameters.AddWithValue("@imageSize", rawData.Length);
      cmd.Parameters.AddWithValue("@imageData", rawData);

      cmd.ExecuteNonQuery();
      this._lastChangedImage = loadLastChanged("last_changed_image");
      this._imageRawData = rawData;
    }
  }
}
