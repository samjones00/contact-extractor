# contact-extractor

## Objective
There is a requirement to create a single page app with a location select list, which submits a query to an API which in turn scrapes an external conveyancy site, solicitors.com

The results are then formatted and displayed for the user to view.

## Specifications

### Implemented

- **API Search endpoint** accepts a location and returns a list of contact details for conveyancers in that location.
- **Supported locations** are read from `appsettings`, exposed via `/Solicitors/Locations`, and retrieved by the SPA, keeping both apps in sync.
- **Invalid location returns 400** — empty or unsupported locations return a bad request response.
- **Parser extracts contacts from full result items** — `HtmlContactParser` extracts name, telephone, and address from standard `div.result-item` elements.
- **Parser extracts contacts from small result items** — contact details are also extracted from `div.result-item.item-small` elements.

### Future

- **Resilience** — Polly retry policy for transient errors when calling the external site.
- **Pagination** — support for paged result sets.
- **Logging** — structured request/error logging.
- **Caching** — cache upstream HTTP responses to avoid repeated calls to solicitors.com for the same request.
- **Middleware** — global exception handling, request validation, etc, over-engineering for a single endpoint.

## Getting Started

### Pre-requisites
* .NET 8 SDK - `winget install -e --id Microsoft.DotNet.SDK.8`
* NodeJs 18 - `winget install -e --id OpenJS.NodeJS`

### Starting the application

#### Quick start (recommended)
From the repository root:
```bash
run.bat
```
This restores API packages, starts the API on `http://localhost:5005`, builds the SPA, and launches the Vite dev server on `http://localhost:5173`.

#### Manual start

##### API
```bash
cd src\API\ContactExtractor.Api
dotnet run --launch-profile http
```
Swagger: `http://localhost:5005/swagger`

Example request:
```json
{
  "location": "London"
}
```

##### Frontend
```bash
cd src\SPA
npm run run-app
```

## Technical Decisions

- ~~XPath is used to find HTML elements. This is fragile — a minor markup change can break extraction — but no better approach was identified at the time.~~
- LINQ is used to find the HTML elements, this is still fragile but easier to handle different html layouts.
- XML Doc comments are used when displaying the API documentation in Swagger.

## Assumptions

- https://www.solicitors.com does not have a rate limiter, downtime, or any other mechanism that would make the site unavailable.
- The query format and HTML layout of https://www.solicitors.com do not change.
- The correct value has been entered on https://www.solicitors.com, e.g. there is an address in the address HTML element.
- The location name is sent in the same format as received from the `/Solicitors/Locations` endpoint, matching the casing.

## AI
- Created run.bat, I tend to do this for all projects where there are nultiple steps to start the application.
- Created the initial structure for the SPA.
- I used OpenCode (local LLM coding agent) to help with development, review changes, etc.