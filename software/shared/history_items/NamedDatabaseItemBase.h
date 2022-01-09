/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include "DatabaseItemBase.h"

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace history_items {

class NamedDatabaseItemBase : public DatabaseItemBase {
  friend class DatabaseItemBase;

#pragma region static stuff
protected:
  /**
   * @brief if passed, adds all select parameters to the targetList, if not only
   * the number of parameters which would have been added is returned
   *
   * @param targetList the list to which the data should be added or nullptr if
   * only the return value should be generated
   * 
   * @return the number of select fields
   */
  static size_t getSelectFieldNames(std::list<std::string>* targetList = nullptr) {
    size_t res = DatabaseItemBase::getSelectFieldNames(targetList);

    if (targetList) {
      targetList->push_back("name");
    }
    return res + 1;
  }
#pragma endregion

#pragma region Members
protected:
  /**
   * @brief name of the item which should be modified
   */
  std::string name;
#pragma endregion

#pragma region Object Construction / Destruction
public:
  /**
   * @brief Constructor which is called if a parameter should be changed
   * 
   * @param core reference to the core connector
   * @param id id of the object which is modified
   * @param updatedParameter the parameter object which is changed
   */
  NamedDatabaseItemBase(CoreDatabaseConnector& core, const int& id, const sql_parameters::ParameterBase& updatedParameter)
    : DatabaseItemBase(core, id, updatedParameter) {}

protected:
  /**
   * @brief Load from SQL row constuctor
   * 
   * The data is provided as specified by getSelectFieldNames()
   * 
   * @param core reference to the core connector
   * @param row row data row to load the data from
   */
  NamedDatabaseItemBase(CoreDatabaseConnector& core, MYSQL_ROW& row)
    : DatabaseItemBase(core, row)
    , name(row[DatabaseItemBase::getSelectFieldNames()]) {
  }

protected:
  /**
   * @brief Copy of object disabled due to references
   */
  NamedDatabaseItemBase(const NamedDatabaseItemBase& other) = delete;

protected:
  /**
   * @brief Move of object disabled due to references
   */
  NamedDatabaseItemBase(NamedDatabaseItemBase&& other) noexcept = delete;

protected:
  /**
   * @brief Copy of object disabled due to references
   */
  NamedDatabaseItemBase& operator=(const NamedDatabaseItemBase& other) = delete;

protected:
  /**
   * @brief Move of object disabled due to references
   */
  NamedDatabaseItemBase& operator=(NamedDatabaseItemBase&& other) noexcept = delete;
#pragma endregion
};

} // namespace history_items
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com