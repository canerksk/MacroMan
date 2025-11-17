#!/bin/bash

project="./MacroMan.csproj"
output_directory="bin/dist"
log_file="build.log"

{
dotnet publish "$project" \
  -c Release \
  -o "$output_directory" \
  -f net9.0-windows \
  --self-contained false \
  -p:PublishSingleFile=true

} || {
    echo ""
    echo "❌ Hata oluştu! Log: $log_file"
}

echo ""
echo "Bitti. Kapatmak için bir tuşa bas..."
read -n 1
