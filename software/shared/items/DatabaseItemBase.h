/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include "../CoreDatabaseConnector.h"
#include "../MySqlException.h"
#include "../sql_parameters/IntegerParameter.h"

#include <iostream>
#include <list>
#include <map>
#include <sstream>
#include <stdexcept>
#include <stdint.h>
#include <string>
#include <utility>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace items {

/**
 * @brief Base class for a database item
 */
class DatabaseItemBase {
#pragma region static stuff
protected:
  /**
   * @brief loads all unhandled history events from the database
   *
   * @param core reference to the core connector
   * @param tableName the name of the table from which the element should be loaded
   */
  template<typename tpDatabaseItemBase>
  static std::list<tpDatabaseItemBase*> loadFromDatabase(CoreDatabaseConnector& core) {
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
    ss << " FROM " << tpDatabaseItemBase::getTableName();

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
   */
  static size_t getSelectFieldNames(std::list<std::string>* targetList = nullptr) {
    if (targetList) {
      targetList->push_back("id");
    }
    return 1;
  }
#pragma endregion

#pragma region Members
protected:
  /**
   * @brief reference to the core connector
   */
  CoreDatabaseConnector& core;

private:
  /**
   * @brief All parameters which are stored in the database
   */
  std::map<std::string /*name*/, sql_parameters::ParameterBase*> parameters;
#pragma endregion

#pragma region Events
protected:
  /**
   * @brief Event triggered as soon as a parameter value has changed
   */
  std::function<void(const sql_parameters::ParameterBase&)> valueChanged;
#pragma endregion

#pragma region Object Construction / Destruction
protected:
  /**
   * @brief Creates a new base item
   * 
   * @param core reference to the core connector
   * @param id id in the database
   * @param callback which is triggered as soon as a parameter value has changed
   */
  DatabaseItemBase(CoreDatabaseConnector& core,
                   const int& id,
                   const std::function<void(const sql_parameters::ParameterBase&)>& valueChanged)
    : core(core)
    , valueChanged(valueChanged) {
    registerSqlParameter<sql_parameters::IntegerParameter>("id", id);
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
   */
  DatabaseItemBase(CoreDatabaseConnector& core,
                   MYSQL_ROW& row,
                   const std::function<void(const sql_parameters::ParameterBase&)>& valueChanged)
    : core(core)
    , valueChanged(valueChanged) {
    registerSqlParameter<sql_parameters::IntegerParameter>("id", 0)->setAsSqlString(row[0]);
  }

protected:
  /**
   * @brief Copy of object disabled due to references
   */
  DatabaseItemBase(const DatabaseItemBase& other) = delete;

protected:
  /**
   * @brief Move of object disabled due to references
   */
  DatabaseItemBase(DatabaseItemBase&& other) noexcept = delete;

public:
  /**
   * @brief Deletes all parameters
   */
  virtual ~DatabaseItemBase() noexcept {
    for (auto& param : parameters) {
      delete param.second;
    }
    parameters.clear();
  }

protected:
  /**
   * @brief Copy of object disabled due to references
   */
  DatabaseItemBase& operator=(const DatabaseItemBase& other) = delete;

protected:
  /**
   * @brief Move of object disabled due to references due to references
   */
  DatabaseItemBase& operator=(DatabaseItemBase&& other) noexcept = delete;
#pragma endregion

#pragma region Getter / Setter
protected:
  /**
   * @brief Requests the parameter with the given name
   */
  template<typename T>
  inline T* getParameter(const std::string& name) const {
    auto it = parameters.find(name);
    if (it == parameters.end()) {
      throw std::invalid_argument("Parameter " + name + " not found!");
    }
    T* p = dynamic_cast<T*>(it->second);
    if (p == nullptr) {
      throw std::invalid_argument("Parameter " + name + " wrong type assumed!");
    }
    return p;
  }

public:
  /**
   * @brief gets the id of the item
   */
  const int& getId() const {
    return getParameter<sql_parameters::IntegerParameter>("id")->get();
  }
#pragma endregion

protected:
  /**
   * @brief Registers a new SQL parameter 
   * 
   * @param parameter the new SQL parameter
   */
  template<typename parameterType>
  parameterType* registerSqlParameter(const std::string& name, const typename parameterType::valueType& initialValue) {
    if (parameters.find(name) != parameters.end()) {
      throw std::invalid_argument("Parameter " + name + " already registered!");
    }

    parameterType* newParameter = new parameterType(
      core,
      name,
      [this](const sql_parameters::ParameterBase& param) {
        valueChanged(param);
      },
      initialValue);

    parameters.emplace(std::make_pair(name, newParameter));

    return newParameter;
  }

protected:
  /**
   * @brief Inserts all parameters to a new row in the given table
   * 
   * @param tableName name of the table which should be used
   */
  void insertToDatabase(const std::string& tableName) {
    // get all parameters required to insert this instance to the database
    std::stringstream ss;
    ss << "INSERT INTO " << tableName << " (";
    bool first = true;
    auto id = getParameter<sql_parameters::IntegerParameter>("id");
    for (auto it = parameters.cbegin(); it != parameters.cend(); it++) {
      if ((it->first == "id") && (id->get() <= 0)) {
        continue;
      }

      if (first) {
        first = false;
      }
      else {
        ss << ", ";
      }
      ss << it->first;
    }
    ss << ") VALUES (";

    first = true;
    for (auto it = parameters.cbegin(); it != parameters.cend(); it++) {
      if ((it->first == "id") && (id->get() <= 0)) {
        continue;
      }

      if (first) {
        first = false;
      }
      else {
        ss << ", ";
      }
      ss << it->second->getAsSqlString();
    }
    ss << ");";

    // execute query
    if (mysql_query(core.getSqlConnection(), ss.str().c_str())) {
      throw MySqlException(core.getSqlConnection());
    }

    // get id if necessary
    if (id->get() <= 0) {
      id->setWithoutEvent((int)mysql_insert_id(core.getSqlConnection()));
    }
  }
};

} // namespace items
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com