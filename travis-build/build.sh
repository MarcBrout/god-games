#! /bin/sh

project="gods-games"
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

# Zip the build
echo 'Build success ! Attempting to zip build'
zip -r $(pwd)/Build/windows.zip $(pwd)/Build/windows/