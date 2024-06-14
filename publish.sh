#!/bin/bash

cd $(dirname "$0")

if [ -z "$REFUEL_API_KEY" ]
then
    echo "Please define REFUEL_API_KEY"
    exit 1
fi

if [ -z "$NUGET_USER_NAME" ]
then
    echo "Please define NUGET_USER_NAME"
    exit 1
fi

if [ -z "$NUGET_INDEX" ]
then
    echo "Please define NUGET_INDEX"
    exit 1
fi

dotnet nuget add source \
    -n ReFuel -u "$NUGET_USER_NAME" -p "$REFUEL_API_KEY" \
    --store-password-in-clear-text \
    "$NUGET_INDEX"
dotnet nuget push -s ReFeul bin/*/*.nupkg
