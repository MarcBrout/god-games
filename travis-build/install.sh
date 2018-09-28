#! /bin/sh

# URL of unity
BASE_URL=https://netstorage.unity3d.com/unity

# download hash
HASH=88d00a7498cd

#project verion
VERSION=2018.2.10f1

# Download Unity3D installer into the container
download() {
  file=$1
  url="$BASE_URL/$HASH/$package"

  echo "Downloading from $url: "
  curl --retry 5 -o `basename "$package"` "$url"
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

}

install() {
  package=$1
  download "$package"

  echo "Installing "`basename "$package"`
  sudo installer -dumplog -package `basename "$package"` -target /
}

# Run installer(s)
install "MacEditorInstaller/Unity-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-$VERSION.pkg"
# install "MacEditorTargetInstaller/UnitySetup-WebGL-Support-for-Editor-$VERSION.pkg"
