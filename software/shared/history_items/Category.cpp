/**
 * @author    Sebastian Winter
 * @copyright 2-clause BSD license
 */
#include "Category.h"

#include "../items/Category.h"

#include <stdexcept>

namespace com {
namespace sebastianwinter {
namespace fridge_management {
namespace core_data_base_connector {
namespace history_items {

#pragma region static stuff

std::list<Category*> Category::loadUnhandledHistoryEntries(CoreDatabaseConnector& core) {
  return NamedDatabaseItemBase::loadUnhandledHistoryEntries<Category>(core);
}

const std::string Category::getTableName() {
  return "categories_history";
}
#pragma endregion

#pragma region Object Construction / Destruction

Category::Category(CoreDatabaseConnector& core, const int& id, const sql_parameters::ParameterBase& updatedParameter)
  : NamedDatabaseItemBase(core, id, updatedParameter) {}

Category::Category(CoreDatabaseConnector& core, MYSQL_ROW& row)
  : NamedDatabaseItemBase(core, row) {}
#pragma endregion

void Category::handle(std::list<items::Category*>& lstCategories) {
  if (handled) {
    return;
  }

  if (type == Type::added) {
    items::Category* newCategory = new items::Category(core, id, name);
    id = newCategory->insertToDatabase();
    lstCategories.push_back(newCategory);

    // mark as handled
    setHandled(getTableName());
  }
  else {
    items::Category* modifiedCategory = nullptr;
    for (auto cat : lstCategories) {
      if (cat->getId() == id) {
        modifiedCategory = cat;
        break;
      }
    }
    if (!modifiedCategory) {
      throw std::runtime_error("Category not found for id = " + std::to_string(id));
    }

    if (type == Type::removed) {
      throw std::runtime_error("Not implemented  Category::handle");
    }
    else if (type == Type::modified) {
      modifiedCategory->modifyParameter(*updatedParameter);
    }
  }
}
} // namespace history_items
} // namespace core_data_base_connector
} // namespace fridge_management
} // namespace sebastianwinter
} // namespace com