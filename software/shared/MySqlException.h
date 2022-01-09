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
  explicit MySqlException(const std::string& message = "");

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

private:
  template<typename T>
  using isCharPointer = std::is_same<typename std::remove_const<typename std::remove_pointer<T>::type>::type, char>;

private:
  template<typename T>
  using isCharArray = std::is_same<typename std::remove_extent<typename std::remove_pointer<T>::type>::type, char>;

private:
  /**
   * @brief helper template testing if T is a std::string, char* or const char*
   */
  template<typename T>
  using testIfString = std::enable_if<std::is_same<T, std::string>::value || isCharPointer<T>::value || isCharArray<T>::value,
                                      int>;
private:
  /**
   * @brief helper template testing if T is a arithmetic type
   */
  template<typename T>
  using testIfArithmetic = std::enable_if<std::is_arithmetic<T>::value, int>;

private:
  /**
   * @brief helper template testing if T is none of the above
   */
  template<typename T>
  using testIfStrFunction = std::enable_if<!std::is_arithmetic<T>::value && !std::is_same<T, std::string>::value &&
                                             !isCharPointer<T>::value && !isCharArray<T>::value,
                                           int>;

public:
  /**
   * @brief operator to append a string to the message
   */
  template<typename T, typename testIfString<T>::type = 0>
  inline MySqlException& operator<<(const T& add) {
    message += add;
    return *this;
  }

public:
  /**
   * @brief operator to append a arithmetic type to the message
   */
  template<typename T, typename testIfArithmetic<T>::type = 0>
  inline MySqlException& operator<<(const T& add) {
    message += std::to_string(add);
    return *this;
  }

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