#!/bin/bash

set -e
export ASPNETCORE_URLS=http://*:5002
run_cmd="dotnet watch run"

until dotnet ef database update; do
>&2 echo "postgres Server is starting up"
sleep 1
done

>&2 echo "postgres Server is up - executing command"
exec $run_cmd