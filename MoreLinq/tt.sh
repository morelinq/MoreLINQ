#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
dotnet build -t:TransformTextTemplates "$@"
