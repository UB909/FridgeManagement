/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include "NamedDatabaseItemBase.h"
#include "../history_items/Category.h"

#include <list>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace items {

/**
 * @brief Class handling a category
 */
class Category : public NamedDatabaseItemBase {
  friend class DatabaseItemBase;
  friend class NamedDatabaseItemBase;

#pragma region static stuff
public:
  /**
   * @brief loads everything from the database
   */
  static std::list<Category*> loadFromDatabase(CoreDatabaseConnector& core);

public:
  /**
   * @brief returns the table name where the data is stored 
   */
  static const std::string getTableName();
#pragma endregion

#pragma region Members
#pragma endregion

#pragma region Object Construction / Destruction
public:
  /**
   * @brief Creates a category
   * 
   * @param core reference to the core connector
   * @param id id in the database
   * @param name name of the item
   */
  Category(CoreDatabaseConnector& core, const int& id, const std::string& name);

protected:
  /**
   * @brief Load from SQL row constuctor
   * 
   * The data is provided as specified by getSelectFieldNames()
   * 
   * @param core reference to the core connector
   * @param row row data row to load the data from
   */
  Category(CoreDatabaseConnector& core, MYSQL_ROW& row);

protected:
  /**
   * @brief Copy of object disabled due to references
   */
  Category(const Category& other) = delete;

protected:
  /**
   * @brief Move of object disabled due to references
   */
  Category(Category&& other) noexcept = delete;

protected:
  /**
   * @brief Copy of object disabled due to references
   */
  Category& operator=(const Category& other) = delete;

protected:
  /**
   * @brief Move of object disabled due to references due to references
   */
  Category& operator=(Category&& other) noexcept = delete;
#pragma endregion

protected:
  /**
   * @brief Callback handler which is called when a parameter is modified
   */
  void onParameterValueChanged(const sql_parameters::ParameterBase& param);

public:
  /**
   * @brief Inserts the category into the database. This functionshould only be
   * called by the history object
   * 
   * @returns the id of this object in the database
   */
  int insertToDatabase();
};
} // namespace items
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com