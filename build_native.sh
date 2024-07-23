#!/bin/bash
cd $(dirname "$0")
./docker-cross-compiler/sh/build_native.sh .
dotnet build -c Release
