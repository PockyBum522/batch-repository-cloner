@echo off

cacls "%systemroot%\system32\config\system" 1>nul 2>&1

if "%errorlevel%" equ "0" (
    echo -------------------------------------------------------------
    echo ERROR: YOU ARE RUNNING THIS WITH ADMINISTRATOR PRIVILEGES
    echo -------------------------------------------------------------
    echo. 
    echo If you're seeing this, it means you are running this as admin!
    echo.
    echo You will need to restart this program without admin 
    echo privileges by just double clicking this .bat file
    echo. 
    echo Make sure to Run NOT As Administrator next time!
    echo. 
    echo Press any key to exit . . .

    pause> nul

    exit /B 1 
) 

Powershell -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "dotnet build"

Powershell -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command  ".\InitializeRepos\Project\bin\Debug\net6.0-windows\InitializeRepos.exe"

echo -------------------------------------------------------
echo APPLICATION HAS FINISHED CONFIGURATION OF THIS MACHINE
echo -------------------------------------------------------
echo.
echo If there are no errors above, your
echo workstation is now configured with all repos you chose.
echo.
echo Press any key to exit . . .

pause> nul

EXIT /B 1   

:: -------------------------------------------------------
:: BELOW HERE IS ALL CODE FROM CHOCOLATEY'S RefreshEnv.cmd
:: -------------------------------------------------------

:: Set one environment variable from registry key
:SetFromReg
    "%WinDir%\System32\Reg" QUERY "%~1" /v "%~2" > "%TEMP%\_envset.tmp" 2>NUL
    for /f "usebackq skip=2 tokens=2,*" %%A IN ("%TEMP%\_envset.tmp") do (
        echo/set "%~3=%%B"
    )
    goto :EOF

:: Get a list of environment variables from registry
:GetRegEnv
    "%WinDir%\System32\Reg" QUERY "%~1" > "%TEMP%\_envget.tmp"
    for /f "usebackq skip=2" %%A IN ("%TEMP%\_envget.tmp") do (
        if /I not "%%~A"=="Path" (
            call :SetFromReg "%~1" "%%~A" "%%~A"
        )
    )
    goto :EOF

:RefreshEnvironmentVariables
    echo/@echo off >"%TEMP%\_env.cmd"

    :: Slowly generating final file
    call :GetRegEnv "HKLM\System\CurrentControlSet\Control\Session Manager\Environment" >> "%TEMP%\_env.cmd"
    call :GetRegEnv "HKCU\Environment">>"%TEMP%\_env.cmd" >> "%TEMP%\_env.cmd"

    :: Special handling for PATH - mix both User and System
    call :SetFromReg "HKLM\System\CurrentControlSet\Control\Session Manager\Environment" Path Path_HKLM >> "%TEMP%\_env.cmd"
    call :SetFromReg "HKCU\Environment" Path Path_HKCU >> "%TEMP%\_env.cmd"

    :: Caution: do not insert space-chars before >> redirection sign
    echo/set "Path=%%Path_HKLM%%;%%Path_HKCU%%" >> "%TEMP%\_env.cmd"

    :: Cleanup
    del /f /q "%TEMP%\_envset.tmp" 2>nul
    del /f /q "%TEMP%\_envget.tmp" 2>nul

    :: capture user / architecture
    SET "OriginalUserName=%USERNAME%"
    SET "OriginalArchitecture=%PROCESSOR_ARCHITECTURE%"

    :: Set these variables
    call "%TEMP%\_env.cmd"

    :: Cleanup
    del /f /q "%TEMP%\_env.cmd" 2>nul

    :: reset user / architecture
    SET "USERNAME=%OriginalUserName%"
    SET "PROCESSOR_ARCHITECTURE=%OriginalArchitecture%"

    echo | set /p dummy="Finished refreshing environtment variables."
    echo.
    