@echo off
setlocal EnableDelayedExpansion

set extends_jpg=*.jpg
set extends_jpeg=*.jpeg

set dir_path=%HOMEPATH%\Desktop\jpg_files

if exist "%dir_path%" (
    echo "error: target directory already exists."
    pause
    exit /b
) else (
    echo "Directory does not exist. Creating now..."
    mkdir "%dir_path%"
)

call :cp_file_func "%cd%" "%extends_jpg%" "%dir_path%"
call :cp_file_func "%cd%" "%extends_jpeg%" "%dir_path%"

echo;
echo "Process is complete!! You can confirm the directory."
echo "%dir_path%"
pause

rem Open the directory where copied files are stored.
start "" "%dir_path%"

rem -------------------------------------------
rem Copy target files to the specified directory, preserving directory structure.
rem arg1: source directory.
rem arg2: file extension pattern.
rem arg3: destination directory.
rem -------------------------------------------
:cp_file_func
set "source_dir=%~1"
set "dest_dir=%~3"
set "pattern=%~2"

for /r "%source_dir%" %%f in (%pattern%) do (
    set "relative_path=%%f"
    set "relative_path=!relative_path:%source_dir%=!"
    
    rem Extract the directory path without the file name
    set "relative_dir=!relative_path!\"
    set "relative_dir=!relative_dir:%%~nxf=!"

    set "dest_path=%dest_dir%!relative_dir!"
    
    rem Create the destination directory if it doesn't exist
    if not exist "!dest_path!" (
        mkdir "!dest_path!"
    )
    
    set "same_file_name=!relative_dir!%%~nf"
    echo "!same_file_name!"
    rem for %%sf in (dir "!relative_dir!" ) do (
    	rem Copy the file to the destination directory
    	copy "%%f" "!dest_path!"
    rem )
)

exit /b
