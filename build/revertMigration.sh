#!/bin/bash

ProjectPath="../src/backend/CardReader.Infrastructure.Persistence/CardReader.Infrastructure.Persistence.csproj"
StartupProjectPath="../src/backend/CardReader.WebApi/CardReader.WebApi.csproj"

execute_command() {
    command=$1
    echo "Executing: $command"
    eval $command
    status=$?
    if [ $status -ne 0 ]; then
        echo "Error: Command exited with code $status"
        exit $status
    fi
}

echo "Reverting the last migration..."

execute_command "dotnet ef migrations remove --project $ProjectPath --startup-project $StartupProjectPath"

echo "Last migration successfully reverted."