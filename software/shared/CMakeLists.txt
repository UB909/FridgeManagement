cmake_minimum_required(VERSION 3.5)
project(CoreDatabaseConnector)

# Use C++ 11
set (CMAKE_CXX_STANDARD 11)

##############################################################################
# Set-up the include directories
##############################################################################
if (WIN32)
  include_directories(
    "C:/Program Files/MariaDB/MariaDB Connector C 64-bit/include"
  )
else(WIN32)
endif(WIN32)

##############################################################################
# set up the library directories
##############################################################################
if (WIN32)
  link_directories(
    "C:/Program Files/MariaDB/MariaDB Connector C 64-bit/lib"
  )
else(WIN32)
endif(WIN32)


##############################################################################
# Define sources
##############################################################################
#add_subdirectory(whatever)
set(SOURCE
  ${SOURCE}
  ${CMAKE_CURRENT_SOURCE_DIR}/CoreDatabaseConnector.cpp
  ${CMAKE_CURRENT_SOURCE_DIR}/MySqlException.cpp
)

add_subdirectory(sql_parameters)
add_subdirectory(history_items)
add_subdirectory(items)


##############################################################################
# Define libraries
##############################################################################
set(LIB
  libmariadb
)

##############################################################################
# Combine all sources to the binary
##############################################################################
SET(LIBRARY_OUTPUT_PATH ${CMAKE_SOURCE_DIR}/lib/${CMAKE_BUILD_TYPE})

##############################################################################
# Link
##############################################################################
#add_library(CoreDatabaseConnector STATIC ${SOURCE})
#target_link_libraries(CoreDatabaseConnector ${LIB})


# TESTING!!!!!!!!!!!!
set(SOURCE
  ${SOURCE}
  ${CMAKE_CURRENT_SOURCE_DIR}/test.cpp)
add_executable (CoreDatabaseConnector ${SOURCE} )

##############################################################################
# Link
##############################################################################
target_link_libraries(CoreDatabaseConnector ${LIB})


