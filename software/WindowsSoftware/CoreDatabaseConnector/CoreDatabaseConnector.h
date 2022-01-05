/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#pragma once

#include <mysql.h>
#include <string>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {

/**
 * @brief Class handling the database for the fridge management
 */
class CoreDatabaseConnector {
protected:
  /**
   * @brief connection object to the SQL server
   */
  MYSQL* sqlConnection;

public:
  /**
   * @brief Create a new instance of the database connector
   * 
   * @param url Address of the database server
   * @param username Username to login into the database server
   * @param password Password to login into the database server
   * @param databaseName Name of database which contains the data for the fridge management
   * 
   * @throws MySqlException in case of an error
   */
  CoreDatabaseConnector(const std::string& url,
                        const std::string& username,
                        const std::string& password,
                        const std::string databaseName);

public:
  /**
   * @brief closes the database connection
   */
  virtual ~CoreDatabaseConnector();
};
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com