#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
VERSION_SUFFIX=
if [ ! -z "$1" ]; then VERSION_SUFFIX="--version-suffix $1"; fi
./build.sh
if [ ! -d dist ]; then mkdir dist; fi
./msbuild.sh /v:m /t:Pack                      \
                  /p:Configuration=Release     \
                  /p:IncludeSymbols=false      \
                  /p:IncludeSource=false       \
                  /p:PackageOutputPath=../dist \
                  $VERSION_SUFFIX              \
                  MoreLinq/MoreLinq.csproj
