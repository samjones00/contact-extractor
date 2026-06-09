# contact-extractor

## Objective
There is a requirement to create a single page app with a location select list, which submits a query to an API which in turn scrapes an external conveyancy site.

The results are then formatted and displayed for the user to view.

## Getting Started

### Pre-requisites
* .NET 8 SDK - `winget install -e --id Microsoft.DotNet.SDK.8`
* NodeJs 18 - `winget install -e --id OpenJS.NodeJS`

### Starting the application

#### API
Swagger page available at https://localhost:7101/swagger/index.html, where 7101 is the port number of the API, which may be different in your case.

To call the API directly, you can launch swagger and use the following request body:
```json
{
  "location": "London"
}
````

#### Frontend
```bash
npm run run-app
```

## Features implemented

### Functional
* API Search endpoint, which accepts a location and returns a list of contact details for conveyancers in that location
* The available locations are read from appsettings and available from /solicitors/locations, making the SPA and API available locations match
* If the location is empty or unsupported, a bad request is returned.

### TODO:
* Update code to return details for the bottom contacts
* Add C4 diagram L2, L3?

### Copilot use
* XML Doc comments, used when displaying the API documentation in swagger

## Features Not implemented
* Polly to retry transient errors when calling the external site
* Pagination
* Logging
* Caching - the http response could be cached to prevent unnecessary calls to solicitors.com

## Decisions
* I'm using XPath to find the html elements, it's very fragile as a minor change can break the extraction. However, I couldn't think of a better way.

## Assumptions
* https://www.solicitors.com does not have a rate limitter, downtime or any other way of making the site unavailable.
* The query format and the layout of the html on https://www.solicitors.com does not change
* The correct value has been entered on https://www.solicitors.com, e.g. there is an address in the address html element
* The location name is sent in the same format as received in the /locations endpoint, matching the casing.