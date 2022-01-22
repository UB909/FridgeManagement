/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#include "Category.h"
#include "../history_items/Category.h"

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace items {

#pragma region static stuff
std::list<Category*> Category::loadFromDatabase(CoreDatabaseConnector& core) {
  return NamedDatabaseItemBase::loadFromDatabase<Category>(core);
}

const std::string Category::getTableName() {
  return "categories";
}

#pragma endregion

#pragma region Members
#pragma endregion

#pragma region Object Construction / Destruction

Category::Category(CoreDatabaseConnector& core, const int& id, const std::string& name)
  : NamedDatabaseItemBase(core, id, name, [this](const sql_parameters::ParameterBase& param) {
    onParameterValueChanged(param);
  }) {}

Category::Category(CoreDatabaseConnector& core, MYSQL_ROW& row)
  : NamedDatabaseItemBase(core, row, [this](const sql_parameters::ParameterBase& param) {
    onParameterValueChanged(param);
  }) {}
#pragma endregion

void Category::onParameterValueChanged(const sql_parameters::ParameterBase& param) {
  core.newCategoryHistoryItem(new history_items::Category(core, getId(), param), true);
}

int Category::insertToDatabase() {
  NamedDatabaseItemBase::insertToDatabase(getTableName());
  return getId();
}

void Category::modifyParameter(const sql_parameters::ParameterBase& param) 
{
  getParameter<sql_parameters::ParameterBase>(param.name)->setAsSqlString(param.getAsSqlString().c_str());
  update databse fehlt noch 
}


} // namespace items
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com