/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include <mysql.h>
#include <string>
#include <list>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace history_items {
class Category;
class Location;
class Item;
class Entry;
} // namespace history_items
namespace items {
class Category;
class Location;
class Item;
class Entry;
} // namespace items

/**
 * @brief Class handling the database for the fridge management
 */
class CoreDatabaseConnector {
#pragma region static stuff
#pragma endregion

#pragma region Members
protected:
  /**
   * @brief connection object to the SQL server
   */
  MYSQL* sqlConnection;

protected:
  /**
   * @brief List with all categories
   */
  std::list<items::Category*> categories;
#pragma endregion

#pragma region Events
#pragma endregion

#pragma region Object Construction / Destruction

public:
  /**
   * @brief Create a new instance of the database connector
   * 
   * @param url Address of the database server
   * @param username Username to login into the database server
   * @param password Password to login into the database server
   * @param databaseName Name of database which contains the data for the fridge management
   * 
   * @throws MySqlException in case of an error
   */
  CoreDatabaseConnector(const std::string& url,
                        const std::string& username,
                        const std::string& password,
                        const std::string databaseName);

public:
  /**
   * @brief closes the database connection
   */
  virtual ~CoreDatabaseConnector();
#pragma endregion

#pragma region Getter / Setter
  /**
   * @brief Returns the SQL connection
   */
  MYSQL*& getSqlConnection();
#pragma endregion

#pragma region SQL Interface
#pragma endregion
  /**
   * @brief 
   * 
   * @param
   */
  void newCategoryHistoryItem(history_items::Category* catergory, const bool& forwardItem);

protected:
  /**
   * @brief Execute all actions in the *_history tables which are not handled
   */
  void handleScheduledActions();
};
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com