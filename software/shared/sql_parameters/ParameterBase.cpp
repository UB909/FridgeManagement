/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#include "ParameterBase.h"

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace sql_parameters {

#pragma region Object Construction / Destruction
ParameterBase::ParameterBase(CoreDatabaseConnector& core,
                             const std::string& name,
                             std::function<void(const ParameterBase&)> valueChanged)
  : name(name)
  , core(core)
  , valueChanged(valueChanged) {}

ParameterBase::ParameterBase(const ParameterBase& other)
  : name(other.name)
  , core(other.core)
  , valueChanged(other.valueChanged) {}

ParameterBase::ParameterBase(ParameterBase&& other) noexcept
  : name(std::move(other.name))
  , core(other.core)
  , valueChanged(std::move(other.valueChanged)) {}

ParameterBase::~ParameterBase() {};
#pragma endregion

} // namespace sql_parameters
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com
