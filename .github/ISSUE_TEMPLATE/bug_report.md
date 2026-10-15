name: bug_report
about: Report a bug or issue
title: ""
labels: bug
assignees: ""
body:
  - type: markdown
    attributes:
      value: |
        Thanks for helping improve the project.
  - type: textarea
    id: description
    attributes:
      label: Description
      description: Describe the bug and the expected behavior.
    validations:
      required: true
  - type: textarea
    id: steps
    attributes:
      label: Steps to reproduce
      placeholder: 1. Open... 2. Click... 3. See error...
  - type: textarea
    id: environment
    attributes:
      label: Environment
      placeholder: Browser, OS, runtime, package version
