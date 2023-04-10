#!/bin/bash
npx openapi-generator-cli generate -i ./Design/design.yaml -g typescript-axios -o ./client