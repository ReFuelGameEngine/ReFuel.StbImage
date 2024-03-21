#!/bin/bash

cd $(dirname "$0")
if [ -z "$QUIK_API_KEY" ]
then
    echo "Please define QUIK_API_KEY"
    exit 1
fi

dotnet nuget add source -n QUIK -u themixedupstuff -p "$QUIK_API_KEY" https://git.mixedup.dev/api/packages/QUIK/nuget/index.json
dotnet nuget push -s QUIK bin/*/*.nupkg
