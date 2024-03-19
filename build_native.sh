#!/bin/bash
cd $(dirname "$0")
./Quik.Common/sh/quik_build_native.sh .
dotnet build
