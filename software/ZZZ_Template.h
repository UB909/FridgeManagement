/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {

/**
 * @brief 
 */
class TemplateClass {
#pragma region static stuff
#pragma endregion

#pragma region Members
#pragma endregion

#pragma region Events
#pragma endregion

#pragma region Object Construction / Destruction
protected:
  /**
   * @brief Copy of object disabled
   */
  TemplateClass(const TemplateClass& other) = delete;

protected:
  /**
   * @brief Move of object disabled
   */
  TemplateClass(TemplateClass&& other) noexcept = delete;

protected:
  /**
   * @brief Copy of object disabled
   */
  TemplateClass& operator=(const TemplateClass& other) = delete;

protected:
  /**
   * @brief Move of object disabled
   */
  TemplateClass& operator=(TemplateClass&& other) noexcept = delete;
#pragma endregion

#pragma region Getter / Setter
#pragma endregion

#pragma region SQL Interface
#pragma endregion
};
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com