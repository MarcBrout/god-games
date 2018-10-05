#! /bin/sh

project="gods-games"
winname="${project}_win.tar.gz"
webname="${project}_web.tar.gz"

#commit= ${TRAVIS_COMMIT}

## Run the editor unit tests and linter
# echo "Running editor unit tests for ${UNITY_PROJECT_NAME}"
# /Applications/Unity/Unity.app/Contents/MacOS/Unity \
#	-batchmode \
#	-nographics \
#	-silent-crashes \
#	-logFile $(pwd)/unity.log \
#	-projectPath "$(pwd)/${UNITY_PROJECT_NAME}" \
#	-runEditorTests \
#	-editorTestsResultFile $(pwd)/test.xml \
#	-quit

#rc0=$?
#echo "Unit test logs"
# cat $(pwd)/test.xml
# exit if tests failed
#if [ $rc0 -ne 0 ]; then { echo "Failed unit tests"; exit $rc0; } fi

# Build Windows
echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" \
  -quit

rc1=$?
echo "Build logs (Windows)"
cat $(pwd)/unity.log

# Build WebGL
echo "Attempting to build $project for WebGL"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -buildTarget WebGL\
  -quit

rc1=$?
echo "Build logs (WebGL)"
cat $(pwd)/unity.log

# set file permissions so builds can run out of the box
for infile in `find $(pwd)/Build | grep -E '\.exe$|\.dll$'`; do
	chmod 755 $infile
done

# tar and zip the build folders
echo 'Build success ! Attempting to tar build'
sudo tar -czvf $winname "$(pwd)/Build/windows/$project.exe" -C "$(pwd)/Build"
sudo tar -czvf $webname "$(pwd)/Build/WebGL/$project.exe" -C "$(pwd)/Build"