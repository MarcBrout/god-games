# God Games

## Description

Welcome !
God Games is battle arena game, built with `unity` and `C#`.

Using your computer with controller or not and enter into the god's arena !!

## Getting started

### Prerequisites

#### IDE

You can choose the IDE you whant, but you need to have VSCode or visual Studio IDE.

#### Unity / C#

Since it's a C#-Based project, due to unity you need to install some stuff on your computer:

- macOS:
  Install [Unity](https://store.unity.com/)
  you also need to install a special compiler with you want to test your c# out of the box.
---

- Windows:
  Note that you can't run iOS app on windows.
  Install [Unity](https://store.unity.com/) as package manager and install the following dependencies:

---

- Linux:
  Follow the [install instructions for your linux distribution](https://www.youtube.com/watch?v=HKdepfTqTPQ) to install Unity.
---

### Install the project

First you need to clone the repository:
Do not forget to upload your SSH Key into github and having the right access.

```bash
git@github.com:werayn/god-games.git
```

And switch to implementation branch

```bash
git checkout implementation
```
Remember to always update your local repo.

### Run it

When everything is installed, if you want to run it directly into Unity Editor
Or install the binary file that you can find into our Amazon S3 solution

### Test it

Travis will automatically test your code. But you always need to add unit test
and respecting our coding style.
We can check our configuration through travis or directly into configuration files.

## Deployement

When all test are good Travis build and upload the last build into the right bucket in Amazon S3

## Documentation

On the way, our team work hard to host and describe the best our games.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

Every success build is a new iteration of the version. But Master and Dev branch will stay stable.

## Authors

* **Junique Virgile** - *Initial contributor* - [Junique Virgile](https://github.com/werayn)

See also the list of [contributors](https://github.com/werayn/god-games/graphs/contributors) who participated in this project.

## License
This project is no licensed - it's a completely private project.
