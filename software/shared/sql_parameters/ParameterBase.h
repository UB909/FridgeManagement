/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include "../CoreDatabaseConnector.h"

#include <functional>
#include <string>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace sql_parameters {

/**
 * @brief Base class for the SQL parameters
 */
class ParameterBase {
#pragma region Members
public:
  /**
   * @brief Name of the parameter
   */
  const std::string name;

protected:
  /**
   * @brief reference to the core connector
   */
  CoreDatabaseConnector& core;
#pragma endregion

#pragma region Events
protected:
  /**
   * @brief Event triggered as soon as the value is changed
   */
  std::function<void(const ParameterBase&)> valueChanged;
#pragma endregion

#pragma region Object Construction / Destruction
public:
  /**
   * @brief Initializes a new base parmeter
   *
   * @param core reference to the core connector
   * @param name name of the parameter
   * @param valueChanged Event triggered as soon as the value is changed
   */
  ParameterBase(CoreDatabaseConnector& core, const std::string& name, std::function<void(const ParameterBase&)> valueChanged);

public:
  /**
   * @brief copy constructor
   */
  ParameterBase(const ParameterBase& other);

public:
  /**
   * @brief move constructor
   */
  ParameterBase(ParameterBase&& other) noexcept;

public:
  /**
   * @brief Create a copy of this object
   */
  virtual const ParameterBase* newCopy() const = 0;

public:
  /**
   * @brief Destructor
   */
  virtual ~ParameterBase();
#pragma endregion

#pragma region SQL Interface
public:
  /**
   * @brief returns a string of the value which can directly be SQL statements.
   * It must be escaped and quotes are included if necessary.
   *
   * @return The SQL string for the value
   */
  virtual const std::string getAsSqlString() const = 0;

public:
  /**
   * @brief Loads the value from the result of an SQL query
   *
   * @param s the loaded column
   */
  virtual void setAsSqlString(const char* s) = 0;
#pragma endregion
};

} // namespace sql_parameters
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com
