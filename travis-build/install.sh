#! /bin/sh

# URL of unity
BASE_URL=https://netstorage.unity3d.com/unity/674aa5a67ed5/MacEditorInstaller/Unity-2018.2.10f1.pkg?_ga=2.244022213.1557949389.1538395704-358393280.1535462062

# Download Unity3D installer into the container
download() {
  file=$1
  url="$BASE_URL"

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

#downloadButler() {
 # url="https://broth.itch.ovh/butler/darwin-amd64"

  #echo "Downloading from $url: "
  #curl --retry 5 -o "$url"
#if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi
#}

#install "~/butler-darwin-amd.pkg"
#export PATH="$PATH:~/Library/Application Support/itch/apps/butler"
