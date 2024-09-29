<img src="https://github.com/savchenkoaddev/TraffiLearn.WebApi/blob/main/logo.png">

# TraffiLearn Web API

![Build Status](https://github.com/savchenkoaddev/TraffiLearn.WebApi/actions/workflows/main.yml/badge.svg)
![License](https://img.shields.io/badge/license-MIT-brightgreen.svg)

## Overview

The **TraffiLearn Web API** is the backbone of the TraffiLearn Web Application, dedicated for improving traffic rules education in Ukraine. This API serves as an intermediary between the client-side application and the underlying data sources. Note that the API is not directly accessible to users and is designed exclusively for frontend consumption.

## Full Documentation

At present, there is no official documentation available for the TraffiLearn Web API. Efforts are being made to develop comprehensive documentation to support users in effectively utilizing the API.

## Installation Guide

To run the TraffiLearn Web API locally, you need to have **Docker** installed on your local machine.

You can download Docker from the [official Docker website](https://www.docker.com/products/docker-desktop).

### **Note:**

> **Important:** To utilize the **TraffiLearn Web API**, you'll need an **API key** from Groq API. This key is essential for the API to function correctly.

### Needed environment variables:
> - GroqApiSettings__ApiKey
<br>

After installing Docker, follow these steps to get started with the API:
1. **Download the source files** for the TraffiLearn Web API.
2. **Navigate to the `src` folder of the API** in your terminal.
3. **Set the needed environment variables via CLI**:
   
   - **For Windows:**
     
     ```bash
     $env:GroqApiSettings__ApiKey="your-secret-key"
     ```
   - **For Linux:**
     
     ```bash
     export GroqApiSettings__ApiKey="your-secret-key"
     ```

**Make sure**, the mentioned environment variables are set correctly, otherwise, you are going to encounter the problem during the further steps.

4. **Run the application** using Docker Compose (the directory should already be set to the ~/src):
   
   ```bash
     docker-compose up -d
   ```

### **Note:**

> Ensure the following ports are not occupied, as they will be used by the app:
> - **5000** (API)
> - **10000** (Azurite)
> - **5432** (Postgres)

<br>
After some time, you should be able to **access the API** at http://localhost:5000/swagger

## Tests

To run the tests, you need to have **.NET** installed on your machine.

You can download .NET from the [official .NET website](https://dotnet.microsoft.com/en-us/download).

If you are eager to run the API tests, please follow the next steps:
1. **Make sure** you have repository files installed.
2. **Navigate to the `src` folder of the API** in your terminal.
3. **Set the needed environment variables via CLI** (see previous sections).
4. Run **dotnet test**:
   
   ```bash
     dotnet test TraffiLearn.sln
   ```

**If everything done right, you will see the test results.**


## Communication

- TraffiLearn Team Discord Server: [Join Us](https://discord.gg/WjwtMsHeva)
- Owner's Telegram: [@masfrte](https://t.me/masfrte)
- [GitHub Issues](https://github.com/savchenkoaddev/TraffiLearn.WebApi/issues)

## Contributing

We welcome contributions from the community! If you would like to contribute to this repository, please contact us.

## Bugs and Feedback

For bugs, questions and discussions you can use the [GitHub Issues](https://github.com/savchenkoaddev/TraffiLearn.WebApi/issues).

## LICENSE

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

