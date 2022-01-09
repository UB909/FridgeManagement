/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include "ParameterBase.h"
#include "../CoreDatabaseConnector.h"

#include <functional>
#include <string>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace sql_parameters {

/**
 * @brief Integer Parameter
 */
class IntegerParameter : public ParameterBase {
public:
  /**
   * @brief the type of this parameter
   */
  using valueType = int;

#pragma region Members
protected:
  /**
   * @brief Value of the parameter
   */
  valueType value;
#pragma endregion

#pragma region Object Construction / Destruction
public:
  /**
   * @brief Initializes a new strign parmeter
   *
   * @param core reference to the core connector
   * @param name name of the parameter
   * @param valueChanged Event triggered as soon as the value is changed
   * @param value value of the parameter
   */
  IntegerParameter(CoreDatabaseConnector& core,
                   const std::string& name,
                   std::function<void(const ParameterBase&)> valueChanged,
                   const valueType& value = 0);

public:
  /**
   * @brief copy constructor
   */
  IntegerParameter(const IntegerParameter& other);

public:
  /**
   * @brief move constructor
   */
  IntegerParameter(IntegerParameter&& other) noexcept;

public:
  /**
   * @brief Create a copy of this object
   */
  virtual const ParameterBase* newCopy() const;
#pragma endregion

#pragma region Getter / Setter
public:
  /**
   * @brief Gets the value of the item
   */
  const valueType& get() const;

public:
  /**
   * @brief Changes the value and triggers the modification event
   */
  void set(valueType newValue);

public:
  /**
   * @brief Changes the value
   */
  void setWithoutEvent(valueType newValue);
#pragma endregion

#pragma region SQL Interface
public:
  /**
   * @brief returns a string of the value which can directly be SQL statements.
   * It must be escaped and quotes are included if necessary.
   *
   * @return The SQL string for the value
   */
  virtual const std::string getAsSqlString() const;

public:
  /**
   * @brief Loads the value from the result of an SQL query
   *
   * @param s the loaded column
   */
  virtual void setAsSqlString(const char* s);
#pragma endregion
};

} // namespace sql_parameters
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com