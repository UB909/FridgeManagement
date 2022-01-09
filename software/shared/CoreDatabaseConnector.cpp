/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */

#include "CoreDatabaseConnector.h"

#include "MySqlException.h"
#include "history_items/Category.h"
#include "items/Category.h"

#include "sql_parameters/IntegerParameter.h"
#include "sql_parameters/StringParameter.h"


namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {

#pragma region static stuff
#pragma endregion

#pragma region Members
#pragma endregion

#pragma region Events
#pragma endregion

#pragma region Object Construction / Destruction
CoreDatabaseConnector::CoreDatabaseConnector(const std::string& url,
                                             const std::string& username,
                                             const std::string& password,
                                             const std::string databaseName)
  : sqlConnection(nullptr) {

  sqlConnection = mysql_init(NULL);

  if (sqlConnection == NULL) {
    throw MySqlException(sqlConnection);
  }

  if (mysql_real_connect(sqlConnection, url.c_str(), username.c_str(), password.c_str(), databaseName.c_str(), 0, NULL, 0) ==
      NULL) {
    throw MySqlException(sqlConnection);
  }

  // laod all categories
  categories = items::Category::loadFromDatabase(*this);

  categories.front()->setName("test123");
  // load and apply all modifications which are not applied to the categories
  for (auto x : history_items::Category::loadUnhandledHistoryEntries(*this)) {
    x->handle(categories);
    delete x;
  }


  //items::DatabaseItemBase::HistoryEntry::createHistoryDerivedEntry<items::Category::HistoryEntry>(
  //  sqlConnection,
  //  "categories",
  //  items::DatabaseItemBase::HistoryEntry::Type::added,
  //  nullptr,
  //  nullptr);
  //items::Entry e(sqlConnection, 0);

  handleScheduledActions();
}

CoreDatabaseConnector::~CoreDatabaseConnector() {
  mysql_close(sqlConnection);
}
#pragma endregion

#pragma region Getter / Setter
MYSQL*& CoreDatabaseConnector::getSqlConnection() {
  return sqlConnection;
}
#pragma endregion

#pragma region SQL Interface
#pragma endregion
void CoreDatabaseConnector::handleScheduledActions() {
  //if (mysql_query(con, "CREATE DATABASE testdb")) {
  //  fprintf(stderr, "%s\n", mysql_error(con));
  //  mysql_close(con);
  //  exit(1);
  //}
}

} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com