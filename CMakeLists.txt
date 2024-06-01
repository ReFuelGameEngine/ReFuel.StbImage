cmake_minimum_required(VERSION 3.0)

project(quik_stbi LANGUAGES C VERSION 1.0)

if (NOT ("${CMAKE_SYSTEM_NAME}" STREQUAL "Darwin"))
    add_compile_options(-static-libgcc -static-libstdc++ -fno-exceptions)
endif()

add_library(stbi SHARED "quik_stbi.c")
install(
    TARGETS stbi
    RUNTIME DESTINATION .
    LIBRARY DESTINATION .)
