/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once


#include "MySqlException.h"

#include <mysql.h>
#include <string>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {

class Tools {
public:
  /**
   * @brief Escapes the passed string
   * 
   * @param sqlConnection reference to the SQL connection
   * @param s string to escape
   * @return the escaped string
   */
  static std::string escapeString(MYSQL*& sqlConnection, const std::string& s) {
    return escapeString(sqlConnection, s.c_str(), s.length());
  }

public:
  /**
   * @brief Escapes the passed data array
   * 
   * @param sqlConnection reference to the SQL connection
   * @param s string to escape
   * @param data data array
   * @param length length of the array
   * @return the escaped string
   */
  static std::string escapeString(MYSQL*& sqlConnection, const char* data, const size_t& length) {
    const size_t bufferSize = 2 * length + 1;
    char* escapedData = new char[bufferSize];
    try {
      if (mysql_real_escape_string(sqlConnection, escapedData, data, (unsigned long)length) == -1) {
        throw MySqlException(sqlConnection);
      }
    }
    catch (...) {
      delete[] escapedData;
      throw;
    }

    std::string retVal(escapedData);
    delete[] escapedData;
    return retVal;
  }
};
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com
