#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e  

# Define paths relative to the solution root
solutionRoot=$(dirname "$(realpath "$0")")/..
migrationsFolderPath="$solutionRoot/src/Fend.Data/Migrations"
databaseDirectoryPath="$solutionRoot/src/Fend.Api"
dbFiles=("Fend.db" "Fend.db-shm" "Fend.db-wal")

# Paths for EF commands
dataProjectCsproj="$solutionRoot/src/Fend.Data/Fend.Data.csproj"
apiProjectCsproj="$solutionRoot/src/Fend.Api/Fend.Api.csproj"
contextName="Fend.Data.ApplicationDbContext"
configuration="Debug"

# Delete the target folder if it exists
if [ -d "$migrationsFolderPath" ]; then
  rm -rf "$migrationsFolderPath"
fi

# Delete the database files if they exist
for file in "${dbFiles[@]}"; do
  if [ -f "$databaseDirectoryPath/$file" ]; then
    rm "$databaseDirectoryPath/$file"
  fi
done

# Run Entity Framework migration to add a new migration
dotnet ef migrations add --project "$dataProjectCsproj" \
  --startup-project "$apiProjectCsproj" \
  --context "$contextName" --configuration "$configuration" --verbose Initial --output-dir Migrations

# Apply the latest migration
dotnet ef database update --project "$dataProjectCsproj" \
  --startup-project "$apiProjectCsproj" \
  --context "$contextName" --configuration "$configuration" --verbose