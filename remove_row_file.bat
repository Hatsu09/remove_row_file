@echo off

set extends_jpg=*.jpg
set extends_jpeg=*.jpeg


rem set dir_path=%cd%\jpg_files
set dir_path=%HOMEPATH%\Desktop\jpg_files

if  exist %dir_path% (
	echo [31m[1m"target directory already exists."[0m[0m
	pause
	exit /b
)else (
	mkdir %dir_path%
)
call:cp_file_func %extends_jpg% %dir_path%
call:cp_file_func %extends_jpeg% %dir_path%

echo;
echo [32mprocess is complete!! you confirm directory.[0m
echo 	[32m %dir_path%[0m
pause

start %dir_path%

rem -------------------------------------------
rem copy target files to specified directory.
rem arg1: extends of copy file.
rem arg2: diretctory path where copy files are stored.
rem -------------------------------------------
:cp_file_func
echo ---------------------------------------------
echo %0
echo "%%1:"%1
echo "%%2:"%2

for /f %%f in ('dir "%cd%"\"%1" /b/s/a-d') do (
	copy %%f %2\
)
echo ---------------------------------------------
exit /b
