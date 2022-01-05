/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */

#include "CoreDatabaseConnector.h"
#include "MySqlException.h"

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {

CoreDatabaseConnector::CoreDatabaseConnector(const std::string& url,
                                             const std::string& username,
                                             const std::string& password,
                                             const std::string databaseName)
  : sqlConnection(nullptr) {

  sqlConnection = mysql_init(NULL);

  if (sqlConnection == NULL) {
    throw MySqlException(sqlConnection);
  }

  if (mysql_real_connect(sqlConnection, url.c_str(), username.c_str(), password.c_str(), databaseName.c_str(), 0, NULL, 0) == NULL) {
    throw MySqlException(sqlConnection);
  }
}

CoreDatabaseConnector::~CoreDatabaseConnector() {
  mysql_close(sqlConnection);
}


} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com