@echo off
dotnet pwsh -NoProfile -ExecutionPolicy RemoteSigned -File "%~dpn0.ps1"
