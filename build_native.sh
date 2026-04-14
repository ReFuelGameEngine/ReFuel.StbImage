#!/bin/bash
cd $(dirname "$0")
# Add the local directory as a .NET package source.
dotnet nuget add source -n ReFuel.StbImage bin

# Build each architecture in its own subfolder.
DST=ReFuel.StbImage.redis.linux.arm     ./docker-cross-compiler/sh/build_native.sh $PWD linux-arm
DST=ReFuel.StbImage.redis.linux.arm64   ./docker-cross-compiler/sh/build_native.sh $PWD linux-arm64
DST=ReFuel.StbImage.redis.linux.x64     ./docker-cross-compiler/sh/build_native.sh $PWD linux-x64
DST=ReFuel.StbImage.redis.osx.arm64     ./docker-cross-compiler/sh/build_native.sh $PWD osx-arm64
DST=ReFuel.StbImage.redis.osx.x64       ./docker-cross-compiler/sh/build_native.sh $PWD osx-x64
DST=ReFuel.StbImage.redis.win.x64       ./docker-cross-compiler/sh/build_native.sh $PWD win.x64
DST=ReFuel.StbImage.redis.win.x86       ./docker-cross-compiler/sh/build_native.sh $PWD win.x86

dotnet build -c Release
