/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#include "StringParameter.h"

#include "../Tools.h"

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace sql_parameters {

#pragma region Object Construction / Destruction
StringParameter::StringParameter(CoreDatabaseConnector& core,
                                 const std::string& name,
                                 std::function<void(const ParameterBase&)> valueChanged,
                                 const valueType& value)
  : ParameterBase(core, name, valueChanged)
  , value(value) {}

StringParameter::StringParameter(const StringParameter& other)
  : ParameterBase(other)
  , value(other.value) {}

StringParameter::StringParameter(StringParameter&& other) noexcept
  : ParameterBase(std::move(other))
  , value(std::move(other.value)) {}

const ParameterBase* StringParameter::newCopy() const {
  return new StringParameter(*this);
}
#pragma endregion

#pragma region Getter / Setter
const StringParameter::valueType& StringParameter::get() const {
  return value;
}

void StringParameter::set(valueType newValue) {
  if (value != newValue) {
    value = newValue;
    valueChanged(*this);
  }
}

void StringParameter::setWithoutEvent(valueType newValue) {
  value = newValue;
}
#pragma endregion

#pragma region SQL Interface
const std::string StringParameter::getAsSqlString() const {
  return "\"" + Tools::escapeString(core.getSqlConnection(), value) + "\"";
}

void StringParameter::setAsSqlString(const char* s) {
  value = s;
};
#pragma endregion

} // namespace sql_parameters
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com
