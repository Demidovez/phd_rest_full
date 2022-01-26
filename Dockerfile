FROM ubuntu:latest
WORKDIR /app
EXPOSE 5104
COPY /Release/ .
