#!/bin/bash
cd $(dirname "$0")
# Add the local directory as a .NET package source.
dotnet nuget add source -n ReFuel.StbImage bin

# Build each architecture in its own subfolder.
DST=$PWD    ./docker-cross-compiler/sh/build_native.sh $PWD linux-arm64
DST=$PWD    ./docker-cross-compiler/sh/build_native.sh $PWD linux-arm
DST=$PWD    ./docker-cross-compiler/sh/build_native.sh $PWD linux-x64
DST=$PWD    ./docker-cross-compiler/sh/build_native.sh $PWD osx-arm64
DST=$PWD    ./docker-cross-compiler/sh/build_native.sh $PWD osx-x64
DST=$PWD    ./docker-cross-compiler/sh/build_native.sh $PWD win-x64
DST=$PWD    ./docker-cross-compiler/sh/build_native.sh $PWD win-x86

dotnet build -c Release
