#!/usr/bin/env bash

# Drop all unstaged files to avoid running tests with dirty repo
git stash -q --keep-index
# Run the tests
# need script for test coding style warning ...
# Get the result of the test
RESULT=$?
# Apply previous stash
git stash pop -q
# if test don't pass, exit 1
[[ $RESULT -ne 0 ]] && exit 1
exit 0
