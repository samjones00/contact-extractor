@echo off
setlocal enabledelayedexpansion
set ROOT=%~dp0

echo ========================================
echo  ContactExtractor - Starting Application
echo ========================================

echo.
echo [1/3] Restoring API packages...
cd /d "%ROOT%src\API"
dotnet restore >nul 2>&1
if %ERRORLEVEL% neq 0 (
    dotnet restore
)

echo.
echo [2/3] Starting API...
start /B "ContactExtractor API" dotnet run --project ContactExtractor.Api --launch-profile http --no-restore

echo   Waiting for API to start...
for /f "tokens=*" %%a in ('powershell -NoProfile -Command "$json = Get-Content '%ROOT%src\API\ContactExtractor.Api\Properties\launchSettings.json' -Raw | ConvertFrom-Json; $urls = $json.profiles.http.applicationUrl -split ';'; Write-Output $urls[0]"') do set API_URL=%%a

:wait_for_api
timeout /t 1 /nobreak >nul
powershell -NoProfile -Command "try { $r=Invoke-WebRequest -Uri '%API_URL%/solicitors/locations' -TimeoutSec 2 -UseBasicParsing; exit 0 } catch { exit 1 }" >nul 2>&1
if %ERRORLEVEL% neq 0 goto wait_for_api

echo   %API_URL% is ready.

echo.
echo [3/3] Installing SPA packages...
cd /d "%ROOT%src\SPA"
call npm install
if %ERRORLEVEL% neq 0 (
    echo npm install failed. Check your Node.js installation.
    pause
    exit /b 1
)

echo.
echo Building SPA...
call npm run build
if %ERRORLEVEL% neq 0 (
    echo SPA build failed.
    pause
    exit /b 1
)

echo.
echo Starting SPA...
echo   Proxying API calls to %API_URL%
echo.
set "API_URL=%API_URL%"
call npm run dev

echo.
pause
