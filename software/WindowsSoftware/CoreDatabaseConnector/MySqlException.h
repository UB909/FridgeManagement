/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include <exception>
#include <mysql.h>
#include <string>

#if !defined(_GLIBCXX_TXN_SAFE_DYN)
  #define _GLIBCXX_TXN_SAFE_DYN
#endif
#if !defined(_GLIBCXX_USE_NOEXCEPT)
  #define _GLIBCXX_USE_NOEXCEPT
#endif

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {

/**
 * @brief Exception thrown if a mysql error is triggered
 */
class MySqlException : public std::exception {
protected:
  /**
   * @brief error message
   */
  std::string message;

public:
  /**
   * @brief Creates a new MySqlException based on the passed message
   * 
   * @param message reason for the exception
   */
  explicit MySqlException(const std::string& message);

public:
  /**
   * @brief Creates a new MySqlException by requesting the last SQL error message
   * 
   * @param sqlConnection from which the error message should be retrieved
   */
  explicit MySqlException(MYSQL* sqlConnection);

public:
  /**
   * @brief copy constructor
   */
  MySqlException(const MySqlException& other);

public:
  /**
   * @brief move constructor
   */
  MySqlException(MySqlException&& other) noexcept;

public:
  /**
   * @brief copy operator
   */
  MySqlException& operator=(const MySqlException& other);

public:
  /**
   * @brief move operator
   */
  MySqlException& operator=(MySqlException&& other) noexcept;

public:
  /**
   * Destructor
   */
  virtual ~MySqlException();

public:
  /**
   * @brief returns the string of the exception
   */
  virtual const char* what() const _GLIBCXX_TXN_SAFE_DYN _GLIBCXX_USE_NOEXCEPT;
};
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com