#!/bin/bash

ProjectPath="../src"

execute_command() {
    local command=$1

    echo "Executing: $command"
    eval $command
    if [ $? -ne 0 ]; then
        echo "Error: Command exited with code $?"
        exit $?
    fi
}

clear_publish_folder() {
    local publish_folder=$1

    if [ -d "$publish_folder" ]; then
        echo "Clearing publish folder: $publish_folder"
        rm -rf "$publish_folder/*"
    else
        echo "Publish folder does not exist. Creating: $publish_folder"
        mkdir -p "$publish_folder"
    fi
}

PublishFolder=$(realpath "$ProjectPath/publish")

echo "Clearing contents of publish folder..."
clear_publish_folder "$PublishFolder"

publish_command="dotnet publish /p:DebugType=None /p:DebugSymbols=false --no-self-contained --configuration Release -o publish -r linux-x64"
execute_command "$publish_command"

echo "Project published successfully to $PublishFolder."
