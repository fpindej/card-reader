param(
    [string]$ProjectPath = "..\src"
)

function ExecuteCommand {
    param(
        [string]$Command
    )

    try {
        Write-Host "Executing: $Command"
        Invoke-Expression $Command
        if ($LASTEXITCODE -ne 0) {
            throw "Command exited with code $LASTEXITCODE"
        }
    } catch {
        Write-Host "Error: $_"
        exit $LASTEXITCODE
    }
}

function ClearPublishFolder {
    param(
        [string]$PublishFolder
    )

    if (Test-Path -Path $PublishFolder) {
        Write-Host "Clearing publish folder: $PublishFolder"
        Remove-Item -Path "$PublishFolder\*" -Recurse -Force
    } else {
        Write-Host "Publish folder does not exist. Creating: $PublishFolder"
        New-Item -ItemType Directory -Path $PublishFolder
    }
}

$PublishFolder = Join-Path -Path (Resolve-Path $ProjectPath) -ChildPath "publish"

Set-Location -Path $ProjectPath

Write-Host "Clearing contents of publish folder..."
ClearPublishFolder -PublishFolder $PublishFolder

$publishCommand = "dotnet publish /p:DebugType=None /p:DebugSymbols=false --no-self-contained --configuration Release -o publish -r linux-x64"
ExecuteCommand $publishCommand

Write-Host "Project published successfully to $PublishFolder."