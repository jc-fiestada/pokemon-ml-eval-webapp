# pokemon-ml-eval-webapp

## Overview

**pokemon-ml-eval-webapp** is a web application that uses:
- a **TypeScript frontend** for the browser,
- a **C# backend** for authentication, data handling, and API logic,
- and a **Python service** for machine learning processing.

The project focuses on connecting these parts into one working system.

---

## Tech Stack

### Frontend
- TypeScript
- HTML
- CSS

### Backend (C#)
- C# Minimal API
- JWT authentication
- MySQL
- Environment variables using `.env`

### Machine Learning Service
- Python
- FastAPI
- Uvicorn
- scikit-learn

---

## Packages Used

### C# (.NET)
- BCrypt.Net-Next
- Microsoft.AspNetCore.Authentication.JwtBearer
- System.IdentityModel.Tokens.Jwt
- MySqlConnector

### Python
- pandas
- scikit-learn
- fastapi
- uvicorn
- mysql-connector-python
- python-dotenv

---

## How It Works

- Pokémon data is fetched from the public **PokeAPI**:  
  https://pokeapi.co/api/v2/pokemon/
- The C# backend processes the raw JSON using `System.Text.Json`
- Processed data is stored in MySQL and the original JSON is deleted
- The C# backend sends the data to a Python FastAPI service
- The Python service trains and evaluates multiple ML models  
  (KNN, Logistic Regression, Decision Tree)
- The results are sent back to C# and shown in the browser

---

## Notes

The machine learning results (metrics) may be lower than expected, but this project shows that a Python ML service can be integrated into a C# backend and used in a real web app. I can use what I learned from this project to improve and scale future projects that require similar machine learning integration.

---

