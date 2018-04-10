## Introduction
Infinity works technical test submission. 

Created by Andy Thurgood 11/05/2017

## Instructions

#### Client Application

To run the client application the following steps should be followed:

1. Ensure that [NodeJS](http://nodejs.org/) is installed. This provides the platform on which the build tooling runs.

2. From the command line, navigate to the project directory (/client) and install the dependencies

```
npm install
```

3. Build and launch Application

```
npm start
```

4. Browse to http://localhost:8080 to launch the application.

#### Client-Server Application
To run the associated server application (to enable the client-server UI) the following steps should be followed:

1. Ensure that [.NET Core](https://www.microsoft.com/net/download/core) is installed. This is the runtime for the server application.

2. From the command line, navigate to the project directory (/server/src/server) and restore dependencies

```
dotnet restore
```

3. Build the application

```
dotnet build
```

4. The application can then be launched from the server directory.

```
dotnet run
```

5. Browse to http://localhost:8080/#/client-server to leverage the client-server version of the application. (Run client\npm start again if needed)

#### IDE's

Both components can be built and launched from associated IDE's Task support is available from VSCODE (F1 -> Run task) and building and launching from within VS2015 is supported as standard.

## Tests

#### Client Application (Unit Tests)
1. From the command line navigate to the project directory (/client) and issue the following command:

```
npm test
```

#### Client Application (End 2 End Tests)
E2E tests use Chrome webdriver, to run these test ensure the chrome brower is installed and up to date.
1. From the command line navigate to the project directory (/client) and issue the following command:

```
npm start -- e2e
```

#### Client-Server
1. From the command line, navigate to the project directory (/server/src/unittests) and issue the following command:

```
dotnet test
```

Alternatively use test discovery within VS2017 to discover and run the server unit tests

## Assumptions

* The service should display all ratings, this ensures results display accurate percentage’s (e.g. results that total 100%).
* Service has a max limit of 5000 results regardless of the API route (Authorities/{id}/Establishments vs Establishment?localAuthorityId=123), this is ok.
* FHIS authorities do not use the FHRS 0-5 rating systems, assumption is that these ratings should be shown in the same way as the FHRS ratings.
* Unit tests somewhat happy path, would typically require more alt path/destructive tests.
* The Front end component has some repetition (not DRY), this is intentional to show the difference in implementation and potential gains from a middleware component (it could have just been an endpoint swap in theory).
* Server cache not invalidated for the sake of this task, would typically use memory or Redis Cache.
* Error logging is light touch, handled and written to the console for now.

## Design notes

Created using Visual Studio Code and Visual Studio 2017. Consists of a stand alone Typescript SPA application, and an optional DotNET WebAPI component.

### Client component
SPA front end component that directly queries the FHRS API and optionally - a middleware server instance.

* TypeScript --> JavaScript
* Aurelia.JS
* Webpack
* Jest /  Jasmine
* Npm

### Server component
Middleware component (webapi), that provides orchestration, caching and handles logic to shape the response data that ensures the SPA frontend receives just the information needed.

* .NET Core
* C#
* XUnit / Moq
* Nuget