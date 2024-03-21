#!/bin/bash

cd $(dirname "$0")
if [ -z "$QUIK_API_KEY" ]
then
    echo "Please define QUIK_API_KEY"
    exit 1
fi

docker-compose run build && docker-compose run publish
