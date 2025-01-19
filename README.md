# HotelUp - Cleaning service
![Application tests](https://github.com/Wiaz24/HotelUp.Cleaning/actions/workflows/tests.yml/badge.svg)
![Github issues](https://img.shields.io/github/issues/Wiaz24/HotelUp.Cleaning)
[![Docker Image Size](https://badgen.net/docker/size/wiaz/hotelup.cleaning?icon=docker&label=image%20size)](https://hub.docker.com/r/wiaz/hotelup.cleaning/)


This service should expose endpoints on port `5004` starting with:
```http
/api/cleaning/
```

## Healthchecks
Health status of the service should be available at:
```http
/api/cleaning/_health
```
and should return 200 OK if the service is running, otherwise 503 Service Unavailable.
