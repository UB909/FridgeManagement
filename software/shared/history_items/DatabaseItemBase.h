/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include "../MySqlException.h"
#include "../Tools.h"

#include <iostream>
#include <list>
#include <mysql.h>
#include <sstream>
#include <stdint.h>
#include <string>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace history_items {

/**
  * @brief represents one row in a history table
  */
class DatabaseItemBase {
public:
  /**
   * @brief Type of an action which is described by the history item
   */
  enum class Type { added = 0, modified = 1, removed = 2 };

#pragma region static stuff
protected:
  /**
   * @brief loads all unhandled history events from the database
   *
   * @tparam tpDatabaseItemBase the derived type which is currently
   * loaded
   * @param core reference to the core connector
   */
  template<typename tpDatabaseItemBase>
  static std::list<tpDatabaseItemBase*> loadUnhandledHistoryEntries(CoreDatabaseConnector& core) {
    std::list<tpDatabaseItemBase*> foundEntries;

    std::list<std::string> fieldnames;
    tpDatabaseItemBase::getSelectFieldNames(&fieldnames);
    std::stringstream ss;
    ss << "SELECT ";
    for (auto it = fieldnames.cbegin(); it != fieldnames.cend(); it++) {
      if (it != fieldnames.cbegin()) {
        ss << ", ";
      }
      ss << *it;
    }
    ss << " FROM " << tpDatabaseItemBase::getTableName() << " WHERE handled = 0;";

    // execute query
    if (mysql_query(core.getSqlConnection(), ss.str().c_str())) {
      throw MySqlException(core.getSqlConnection());
    }

    // load result
    MYSQL_RES* result = mysql_store_result(core.getSqlConnection());
    if (result == NULL) {
      throw MySqlException(core.getSqlConnection());
    }

    // provess found rows
    try {
      MYSQL_ROW row;

      while ((row = mysql_fetch_row(result))) {
        foundEntries.push_back(new tpDatabaseItemBase(core, row));
      }
    }
    catch (...) {
      mysql_free_result(result);
      throw;
    }
    mysql_free_result(result);

    return foundEntries;
  }

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
    if (targetList) {
      targetList->push_back("internal_id");
      targetList->push_back("date");
      targetList->push_back("type");
      targetList->push_back("handled");
      targetList->push_back("id");
    }
    return 5;
  }
#pragma endregion

#pragma region Members
protected:
  /**
   * @brief core reference to the core connector
   */
  CoreDatabaseConnector& core;

protected:
  /**
   * @brief Id of the row in the local database
   */
  int internalId;

protected:
  /**
   * @brief date string
   */
  std::string date;

protected:
  /**
   * @brief tyoe if the action
   */
  Type type;

protected:
  /**
   * @brief Flag if the entry was handeled
   */
  bool handled;

protected:
  /**
   * @brief The id of the item which should be modified
   */
  int id;

protected:
  /**
   * @brief if a value has been updated, the updated value
   */
  const sql_parameters::ParameterBase* updatedParameter;
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
  DatabaseItemBase(CoreDatabaseConnector& core, const int& id, const sql_parameters::ParameterBase& updatedParameter)
    : core(core)
    , internalId(0)
    , date("")
    , type(Type::modified)
    , handled(false)
    , id(id)
    , updatedParameter(updatedParameter.newCopy()) {}

protected:
  /**
   * @brief Load from SQL row constuctor
   * 
   * The data is provided as specified by getSelectFieldNames()
   * 
   * @param core reference to the core connector
   * @param row row data row to load the data from
   */
  DatabaseItemBase(CoreDatabaseConnector& core, MYSQL_ROW& row)
    : core(core)
    , internalId(atoi(row[0]))
    , date(row[1])
    , type((Type)atoi(row[2]))
    , handled(atoi(row[3]) != 0)
    , id(atoi(row[4]))
    , updatedParameter(nullptr) {}

protected:
  /**
   * @brief Copy of object disabled due to references
   */
  DatabaseItemBase(const DatabaseItemBase& other) = delete;

protected:
  /**
   * @brief Move of object disabled due to references
   */
  DatabaseItemBase(DatabaseItemBase&& other) = delete;

protected:
  /**
   * @brief Copy of object disabled due to references
   */
  DatabaseItemBase& operator=(const DatabaseItemBase& other) = delete;

protected:
  /**
   * @brief Move of object disabled due to references
   */
  DatabaseItemBase& operator=(DatabaseItemBase&& other) noexcept = delete;

public:
  virtual ~DatabaseItemBase() {
    if (updatedParameter) {
      delete updatedParameter;
    }
  }

#pragma endregion

protected:
  /**
   * @brief marks the history entry as handled
   */
  void setHandled(const std::string& tableName) {
    // mark as handled
    std::stringstream ss;
    ss << "UPDATE " << tableName << " SET handled = 1 WHERE internal_id = " << internalId;
    if (mysql_query(core.getSqlConnection(), ss.str().c_str())) {
      throw MySqlException(core.getSqlConnection());
    }

    handled = true;
  }
};

} // namespace history_items
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com