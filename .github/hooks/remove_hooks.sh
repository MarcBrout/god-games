#!/usr/bin/env bash

## Script variables
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

## Environment variables needed
REV_HOOKS="${DIR}/../../.github/hooks"
RUN_HOOKS="${DIR}/../../.git/hooks"

for hook in `ls -1 ${REV_HOOKS}`
do
    # Check if not .sh
    echo ${hook} | grep "\.sh" > /dev/null 2>&1
    if [[ $? -ne 0 ]]
    then
        unlink ${RUN_HOOKS}/${hook}
    fi
done
