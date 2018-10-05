# Workflow

In order to keep the project organised, and so that everyone understands where and how to find the material, here's a detailed description of the folder structure to support the design workflow and a summarize to contribute to the project

## Contributing


## Folder structure

Folders roughly represent different development stages:

- `travis-build`: Conf and script for our CI and deployment system.
- `Assets`: Photos, vectors, icons, scripts, Scene all the code and assets/ressources that made gods-game real :)
- `Package`: Manifest json corresponding to unity packages that we use.
- `ProjectSettings`: All final files corresponding to Unity settings;

We will talk about Assets folder later ! :)

### Versioning

Poor versioning typically looks like this:

- `commitNb_gods-games.exe`

### Organising filling folders

Projects are typically composed of several subjects/sections, which then need to be organised. In order to avoid scalability issues, these folders follow a few conventions.

#### Generic conventions

- **Allowed characters**: Whenever naming something, unless instructed otherwise, always use lowercase alpha-numeric characters and hyphens, **NEVER** spaces and underscores. Reference regex: `[a-z0-9][a-z0-9\-]*`
- **Subject folders:** If you need to group multiple subjects or sections that heavily relate to each other, name the sections and create folders for each.

#### `assets` conventions

- **Versioning:** Always version your assets. If you need to store a `banner.png`, store it as `banner_1.png`.
- **Updating:** Do not update assets. Let's say you need to update the `banner_1.png` file. Instead of replacing it, create a `banner_2.png` file and use that. This guarantees that any design pointing to that `banner_1.png` will still work as expected.

#### `design` conventions

- **File naming:** Files must be in CamelCase;