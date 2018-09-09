# Contributing

When contributing to this repository, please first discuss the change you wish to make via issue,
email, or any other method with the owners of this repository before making a change.

Please note we have a code of conduct, please follow it in all your interactions with the project.

## Pull Request Process

1. Respect the pull request template.
2. Update unit tests.
3. Increase the version numbers if is necessary.
4. Don't forget to rebase your branch.
5. Waiting a review.
6. Updating your wrike's task.

## Code of Conduct

### Our Pledge

In the interest of fostering an open and welcoming environment, we as
contributors and maintainers pledge to making participation in our project and
our community a harassment-free experience for everyone, regardless of age, body
size, disability, ethnicity, gender identity and expression, level of experience,
nationality, personal appearance, race, religion, or sexual identity and
orientation.

### Our Standards

Examples of behavior that contributes to creating a positive environment
include:

* Using welcoming and inclusive language
* Being respectful of differing viewpoints and experiences
* Gracefully accepting constructive criticism
* Focusing on what is best for the community
* Showing empathy towards other community members

Examples of unacceptable behavior by participants include:

* The use of sexualized language or imagery and unwelcome sexual attention or
advancesre
* Trolling, insulting/derogatory comments, and personal or political attacks
* Public or private harassment
* Publishing others' private information, such as a physical or electronic
  address, without explicit permission
* Other conduct which could reasonably be considered inappropriate in a
  professional setting

### Our Responsibilities

Project maintainers are responsible for clarifying the standards of acceptable
behavior and are expected to take appropriate and fair corrective action in
response to any instances of unacceptable behavior.

Project maintainers have the right and responsibility to remove, edit, or
reject comments, commits, code, wiki edits, issues, and other contributions
that are not aligned to this Code of Conduct, or to ban temporarily or
permanently any contributor for other behaviors that they deem inappropriate,
threatening, offensive, or harmful.

### Scope

This Code of Conduct applies both within project spaces and in public spaces
when an individual is representing the project or its community. Examples of
representing a project or community include using an official project e-mail
address, posting via an official social media account, or acting as an appointed
representative at an online or offline event. Representation of a project may be
further defined and clarified by project maintainers.

### Enforcement

Instances of abusive, harassing, or otherwise unacceptable behavior may be
reported by contacting the project team at [INSERT EMAIL ADDRESS]. All
complaints will be reviewed and investigated and will result in a response that
is deemed necessary and appropriate to the circumstances. The project team is
obligated to maintain confidentiality with regard to the reporter of an incident.
Further details of specific enforcement policies may be posted separately.

Project maintainers who do not follow or enforce the Code of Conduct in good
faith may face temporary or permanent repercussions as determined by other
members of the project's leadership.

# How to contribute

First of all, thank you for your help,
The workflow is straightforward :

## Basics

First, checkout on dev and get the latest updates :
```
$ git checkout dev && git pull origin dev
```

Now you're all set up and you can create your very own branch ! 🚀
```
$ git checkout -b "awesome-feature"
```

Yay, you've updated and added many files, you should commit those changes !
```
$ git add $(UNTRACKED_FILES) # You can see your untracked files with `git status`
$ git commit $(FILES_TO_COMMIT) -m "relevant commit message"
```

You can now, if you want, push those on your branch
```
$ git push -u origin awesome-feature
```

After many commits, and possibly some pushes, you want to merge this awesome feature !
First of all, get the latest `dev` version ! ⚠️
```
$ git fetch origin dev
```

Now you're ready for rebasing ! 🙌

## Rebasing

You should always rebase before submitting a PR, if you don't, git will warn you that your local `dev` branch is late (compared to the remote)

To do so :
```
$ git rebase dev
```
You could also provide the `-i` flag (shorthand for `--interactive`) to adapt the rebase and have a clean history (this isn't necessary if your commits makes sense)
Interactive mode allows you to create a clean branch, and possibly merge / delete commits. (google is your friend)

If, during your rebase, you have some conflicts (That's normal, don't worry), you should fix those merge conflicts. Once you're done, you can do :

```
$ git add $(FILES_WITH_RESOLVED_CONFLICTS)
```

Once you resolved all your conflicts, you can :
```
$ git rebase --continue
```

Do those two steps as many times as needed, you can have multiple conflicts per rebase (If you're unlucky)

When git told you your rebase is complete, you can finnaly push (houra !)
```
$ git push -f
```

The `-f` flag forces the remote (github) to rewrite your history, this is necessary after a rebase.

Now, you can submit your PR ! 🎉

### Author

* Thank's to [Junique Virgile](https://github.com/weay).