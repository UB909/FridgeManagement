/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include "../sql_parameters/ParameterBase.h"
#include "NamedDatabaseItemBase.h"

#include <list>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace items {
class Category;
}
namespace history_items {

class Category : public NamedDatabaseItemBase {
  friend class DatabaseItemBase;
  friend class NamedDatabaseItemBase;

#pragma region static stuff
public:
  /**
   * @brief loads all unhandled history events from the database
   * 
   * @param core reference to the core connector
   */
  static std::list<Category*> loadUnhandledHistoryEntries(CoreDatabaseConnector& core);

protected:
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
   * @brief Constructor which is called if a parameter should be changed
   */
  Category(CoreDatabaseConnector& core, const int& id, const sql_parameters::ParameterBase& updatedParameter);

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

public:
  /**
   * @brief Performs the action described by this history item
   * 
   * @param lstCategories categories list which is currently updated
   */
  void handle(std::list<items::Category*>& lstCategories);
};

} // namespace history_items
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com