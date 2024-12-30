# HotelUp - Cleaning service
![dockerhub_badge](https://github.com/Wiaz24/HotelUp.Cleaning/actions/workflows/dockerhub.yml/badge.svg)

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
