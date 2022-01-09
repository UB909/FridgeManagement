/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#include "IntegerParameter.h"

#include "../Tools.h"

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace sql_parameters {

#pragma region Object Construction / Destruction
IntegerParameter::IntegerParameter(CoreDatabaseConnector& core,
                                   const std::string& name,
                                   std::function<void(const ParameterBase&)> valueChanged,
                                   const valueType& value)
  : ParameterBase(core, name, valueChanged)
  , value(value) {}

IntegerParameter::IntegerParameter(const IntegerParameter& other)
  : ParameterBase(other)
  , value(other.value) {}

IntegerParameter::IntegerParameter(IntegerParameter&& other) noexcept 
  : ParameterBase(std::move(other))
  , value(std::move(other.value)) {}


const ParameterBase* IntegerParameter::newCopy() const {
  return new IntegerParameter(*this);
}
#pragma endregion

#pragma region Getter / Setter
const IntegerParameter::valueType& IntegerParameter::get() const {
  return value;
}

void IntegerParameter::set(valueType newValue) {
  if (value != newValue) {
    value = newValue;
    valueChanged(*this);
  }
}

void IntegerParameter::setWithoutEvent(valueType newValue) {
  value = newValue;
}
#pragma endregion

#pragma region SQL Interface
const std::string IntegerParameter::getAsSqlString() const {
  return std::to_string(value);
}

void IntegerParameter::setAsSqlString(const char* s) {
  value = atoi(s);
};
#pragma endregion

} // namespace sql_parameters
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com
