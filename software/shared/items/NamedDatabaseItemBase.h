/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include "../Tools.h"
#include "../sql_parameters/StringParameter.h"
#include "DatabaseItemBase.h"

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace items {

/**
 * @brief Base class for a named database item
 */
class NamedDatabaseItemBase : public DatabaseItemBase {
  friend class DatabaseItemBase;

#pragma region static stuff
  /**
   * @brief if passed, adds all select parameters to the targetList, if not only
   * the number of parameters which would have been added is returned
   */
  static size_t getSelectFieldNames(std::list<std::string>* targetList = nullptr) {
    size_t res = DatabaseItemBase::getSelectFieldNames(targetList);

    if (targetList) {
      targetList->push_back("name");
    }
    return res + 1;
  }
#pragma endregion

#pragma region Object Construction / Destruction
protected:
  /**
   * @brief Creates a new named base item
   * 
   * @param core reference to the core connector
   * @param id id in the database
   * @param name name of the item
   * @param callback which is triggered as soon as a parameter value has changed
   */
  NamedDatabaseItemBase(CoreDatabaseConnector& core,
                        const int& id,
                        const std::string& name,
                        const std::function<void(const sql_parameters::ParameterBase&)>& valueChanged)
    : DatabaseItemBase(core, id, valueChanged) {
    registerSqlParameter<sql_parameters::StringParameter>("name", name);
  }

protected:
  /**
   * @brief Load from SQL row constuctor
   * 
   * The data is provided as specified by getSelectFieldNames()
   * 
   * @param core reference to the core connector
   * @param row row data row to load the data from
   * @param callback which is triggered as soon as a parameter value has changed
   * @param callback which is triggered as soon as a parameter value has changed
   */
  NamedDatabaseItemBase(CoreDatabaseConnector& core,
                        MYSQL_ROW& row,
                        const std::function<void(const sql_parameters::ParameterBase&)>& valueChanged)
    : DatabaseItemBase(core, row, valueChanged) {
    registerSqlParameter<sql_parameters::StringParameter>("name", "")
      ->setAsSqlString(row[DatabaseItemBase::getSelectFieldNames()]);
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
   * @brief Move of object disabled due to references due to references
   */
  NamedDatabaseItemBase& operator=(NamedDatabaseItemBase&& other) noexcept = delete;
#pragma endregion

#pragma region Setter
#pragma region Name
public:
  /**
   * @brief Gets the name of the item
   */
  const std::string& getName() const {
    return getParameter<sql_parameters::StringParameter>("name")->get();
  }
#pragma endregion
#pragma endregion
};

} // namespace items
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com