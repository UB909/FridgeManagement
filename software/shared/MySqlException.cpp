/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */

#include "MySqlException.h"

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {

MySqlException::MySqlException(const std::string& message)
  : message(message) {}

MySqlException::MySqlException(MYSQL* sqlConnection)
  : message(mysql_error(sqlConnection)) {}

MySqlException::MySqlException(const MySqlException& other)
  : message(other.message) {}

MySqlException::MySqlException(MySqlException&& other) noexcept
  : message(std::move(other.message)) {}

MySqlException& MySqlException::operator=(const MySqlException& other) {
  // prevent self-copy
  if (this != &other) {
    message = other.message;
  }
  return *this;
}

MySqlException& MySqlException::operator=(MySqlException&& other) noexcept {
  // prevent self-copy
  if (this != &other) {
    message = std::move(other.message);
  }
  return *this;
}

MySqlException::~MySqlException() {}

const char* MySqlException::what() const {
  return message.c_str();
}

} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com