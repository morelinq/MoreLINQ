# Dev Container

[![Open in Dev Containers](https://img.shields.io/static/v1?label=Dev%20Containers&message=Open&color=blue)](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/morelinq/MoreLINQ)

For a consistent development experience, you can use [Visual Studio Code] with
the [Dev Containers extension]. A [dev container] provides a ready-to-use
development environment with all the necessary dependencies.

To use [the dev container]:

1. Install [Visual Studio Code]
2. Install the [Dev Containers extension]
3. Clone the repository and open it in VS Code
4. When prompted, click **Reopen in Container** or run the **Dev Containers:
   Reopen in Container** command

Alternatively, you can use [GitHub Codespaces] to develop in the cloud without
installing anything locally. Simply click the **Code** button in [the GitHub
repository] and select **Open with Codespaces** to get started with the same
development environment.

The dev container uses an Ubuntu-based image and automatically installs:

- .NET SDK based on the version in [`global.json`]
- Supported .NET runtimes targets for testing
- Git and GitHub CLI
- All necessary VS Code extensions for .NET development

Once the container environment is up and running, build and test the solution by
executing:

```sh
./test.sh
```

[`global.json`]: ../global.json
[dev container]: https://containers.dev/
[the dev container]: devcontainer.json
[Visual Studio Code]: https://code.visualstudio.com/
[Dev Containers extension]: https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers
[GitHub Codespaces]: https://github.com/features/codespaces
[the GitHub repository]: https://github.com/morelinq/MoreLINQ
