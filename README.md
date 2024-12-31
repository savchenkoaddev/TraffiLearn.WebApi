<img src="https://github.com/savchenkoaddev/TraffiLearn.WebApi/blob/main/logo.png">

# TraffiLearn Web API

![Build Status](https://github.com/savchenkoaddev/TraffiLearn.WebApi/actions/workflows/main.yml/badge.svg)
![License](https://img.shields.io/badge/license-MIT-brightgreen.svg)

## Overview

The **TraffiLearn Web API** is the backbone of the TraffiLearn Web Application, dedicated for improving traffic rules education in Ukraine. This API serves as an intermediary between the client-side application and the underlying data sources. Note that the API is not directly accessible to users and is designed exclusively for frontend consumption.

# Installation Guide

This guide will help you set up and run the **TraffiLearn Web API** locally using **Docker**.

## Prerequisites
To run the API, ensure the following tools are installed on your local machine:
1. **Docker**: Download and install Docker from the [official Docker website](https://www.docker.com/products/docker-desktop).
2. **Stripe CLI**: If using Stripe webhooks locally, download and install the Stripe CLI.
3. **API Keys and Environment Variables**: Gather all required API keys and environment variables as outlined below.

### Required API Keys and Configuration:
To fully utilize the TraffiLearn Web API, the following API keys and connection strings are required:

- **Groq API Key**: Obtain from [Groq API](https://groq.com/). *(If not using Groq AI-specific endpoints, set a dummy value.)*
- **Azure Service Bus Connection String**: Obtain from [Azure Service Bus](https://azure.microsoft.com/en-us/products/service-bus).
- **Stripe Keys**: If using payment-specific endpoints, obtain the following from [Stripe](https://stripe.com):
  - **Publishable Key**
  - **Secret Key**
  - **Webhook Secret**
- **Google Client ID**: Required for Google Authentication. Obtain from [Google Developers Console](https://developers.google.com/identity/oauth2/web/guides/get-google-api-clientid).

## Environment Variables
Set the following environment variables before running the application:

### **Core Environment Variables**

- `GroqApiSettings__ApiKey` *(Groq API Key)*
- `MessageBrokerSettings__ConnectionString` *(Azure Service Bus Connection String)*

### **Additional Environment Variables**
For specific features, set these variables:

- **Stripe Settings** *(For payment endpoints):*
  - `StripeSettings__PublishableKey`
  - `StripeSettings__SecretKey`
  - `StripeSettings__WebhookSecret`
- **Google Authentication**:
  - `GoogleAuthSettings__ClientId` *(Google Client ID)*

### **Optional Configuration**
You can also configure other settings specified in `appsettings.json` and `appsettings.Development.json` for advanced customization and environment-specific overrides.

---

## Installation Steps
Follow these steps to set up the TraffiLearn Web API:

### 1. Download the Source Code
Clone or download the TraffiLearn Web API source files to your local machine.

### 2. Navigate to the Source Directory
Open a terminal and navigate to the `src` folder of the downloaded project:

```bash
cd path/to/traffilearn/src
```

### 3. Set Environment Variables
Configure the necessary and additional environment variables in your terminal.

#### For Windows (PowerShell):
```bash
$env:GroqApiSettings__ApiKey="your-groq-api-key"
$env:MessageBrokerSettings__ConnectionString="your-connection-string"
$env:StripeSettings__PublishableKey="your-stripe-publishable-key"
$env:StripeSettings__SecretKey="your-stripe-secret-key"
$env:StripeSettings__WebhookSecret="your-stripe-webhook-secret"
$env:GoogleAuthSettings__ClientId="your-google-client-id"
```

#### For Linux/MacOS:
```bash
export GroqApiSettings__ApiKey="your-groq-api-key"
export MessageBrokerSettings__ConnectionString="your-connection-string"
export StripeSettings__PublishableKey="your-stripe-publishable-key"
export StripeSettings__SecretKey="your-stripe-secret-key"
export StripeSettings__WebhookSecret="your-stripe-webhook-secret"
export GoogleAuthSettings__ClientId="your-google-client-id"
```

### 4. Run the Application with Docker
Ensure you are in the `src` directory, then run the following command:

```bash
docker-compose up -d
```

### 5. Set Up Stripe CLI (Optional)
If using Stripe webhooks locally, start the Stripe CLI to listen for events and forward them to your application. Refer to the [official guide](https://stripe.com/docs/stripe-cli) for more details.

```bash
stripe listen --forward-to http://localhost:5000/api/webhooks/stripe --events checkout.session.completed
```

### **Note:**

> Ensure the following ports are not occupied, as they will be used by the app:
> - **5000** (API)
> - **10000** (Azurite)
> - **5432** (Postgres)
> - **8025** (MailHog)

### 7. Access the API
After some time, the application will be up and running. You can access the API documentation and test endpoints using Swagger:

[http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## Tests

To run the tests, you need to have **.NET** installed on your machine.

You can download .NET from the [official .NET website](https://dotnet.microsoft.com/en-us/download).

After installing .NET, please follow the next steps:
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

