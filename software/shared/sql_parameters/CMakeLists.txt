#add_subdirectory(ctrl_native_core)

set(SOURCE
   ${SOURCE}
   ${CMAKE_CURRENT_SOURCE_DIR}/ParameterBase.cpp
   ${CMAKE_CURRENT_SOURCE_DIR}/IntegerParameter.cpp
   ${CMAKE_CURRENT_SOURCE_DIR}/StringParameter.cpp
   PARENT_SCOPE
)