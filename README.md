

| Builds  | Branch | Status 
| ------------- | -----  |--------
| Circle CI  | main   | [![CircleCI](https://circleci.com/gh/dof-dss/de-exceptional-closures/tree/main.svg?style=svg&circle-token=3e0ce0f5b4ec766d5d209c0cc88e4385201a0b83)](https://circleci.com/gh/dof-dss/de-exceptional-closures/tree/main)
| SonarCloud  | main   | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=dof-dss_de-exceptional-closures&metric=alert_status)](https://sonarcloud.io/dashboard?id=dof-dss_de-exceptional-closures)

# de-exceptional-closures

## Description

Application to record exceptional closures of schools in Northern Ireland.

## Contents of this file

- [Contributing](#contributing)
- [Licensing](#licensing)
- [Project Documentation](#project-documentation)
    - [Why did we build this project](#why-did-we-build-this-project)
    - [What problem was it solving](#what-problem-was-it-solving)
    - [How did we do it](#how-did-we-do-it)
    - [Future Plans](#future-plans)
    - [Deployment Guide](#deployment-guide)

## Contributing

Contributions are welcomed! Read the [Contributing Guide](./docs/contributing/Index.md) for more information.

## Licensing

Unless stated otherwise, the codebase is released under the MIT License. This covers both the codebase and any sample code in the documentation. The documentation is © Crown copyright and available under the terms of the Open Government 3.0 licence.

## Project Documentation

### Why did we build this project?

We built this project to replace a legacy application and also update this project with cloud technology and modern techniques.

### What problem was it solving?

The old site is outdated and has some outstanding issues. This site will address all of these and add extra functionality.

### How did we do it?

This is a dotnet core application which uses Mysql, Entity Framework, Dotnet Core Identity and Notify to send various emails. We are using the mediator pattern and CQRS to be more SOLID. We use various nuget packages.

The main ones are below:

- Dotnet Core
- Dotnet Core Identity
- Gov.UK Notify services for text and email notifications
- MediaR
- Pomelo
- Mysql
- EntityFramework Core
- AutoMapper
- Fluent Validation
- UXG css
- GDS Design system

### Future plans

None at present.

### Deployment guide

To build run "dotnet build" in command line then dotnet run to run the site. 
To update the database run update-database on the infrastructure project.
