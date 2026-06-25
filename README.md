# 🏇 Gallop IQ

> AI-powered horse racing predictions — built with Angular, ASP.NET Core, and Google Gemini.

![Tech Stack](https://img.shields.io/badge/Frontend-Angular%2018-red?style=flat-square&logo=angular)
![Tech Stack](https://img.shields.io/badge/Backend-.NET%209-purple?style=flat-square&logo=dotnet)
![Tech Stack](https://img.shields.io/badge/AI-Google%20Gemini-blue?style=flat-square&logo=google)
![Tech Stack](https://img.shields.io/badge/Data-The%20Racing%20API-green?style=flat-square)

---

## Overview

Gallop IQ fetches live UK/IRE horse racing data from The Racing API, builds a feature set for each runner, and sends it to Google Gemini to generate structured AI predictions — including win probability, confidence ratings, and value scores for every horse in a race.

---

## Project Structure

```
gallop-iq/
├── backend/
│   └── GallopIQ.Api/          # ASP.NET Core Web API (.NET 9)
│       ├── Controllers/        # RacesController, PredictionsController
│       ├── DTOs/               # Data Transfer Objects
│       ├── Services/           # Business logic (Racing API, Gemini, Predictions)
│       ├── Stores/             # In-memory prediction cache
│       ├── appsettings.json    # ⚠️ Contains API keys — do NOT commit
│       └── appsettings.example.json  # Safe template for version control
├── frontend/                   # Angular 18 standalone app
│   ├── src/app/
│   │   ├── core/               # Models + Services
│   │   └── features/           # Dashboard + Race Detail pages
│   └── proxy.conf.json         # Dev proxy → backend :5000
└── README.md
```

---

## Prerequisites

| Tool | Version | Download |
|------|---------|----------|
| .NET SDK | 9.0+ | https://dotnet.microsoft.com/download |
| Node.js | 18+ | https://nodejs.org |
| Angular CLI | 18+ | `npm install -g @angular/cli` |

---

## API Keys Required

| Service | Purpose | Get yours at |
|---------|---------|--------------|
| [The Racing API](https://www.theracingapi.com) | Live racecards & results | theracingapi.com (Free plan available) |
| [Google Gemini](https://aistudio.google.com) | AI win probability predictions | aistudio.google.com |

---

## Backend Setup

### 1. Configure API Keys

Edit `backend/GallopIQ.Api/appsettings.json`:

```json
{
  "RacingApi": {
    "BaseUrl": "https://api.theracingapi.com",
    "Username": "YOUR_RACING_API_USERNAME",
    "Password": "YOUR_RACING_API_PASSWORD"
  },
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY",
    "Model": "gemini-2.0-flash"
  }
}
```

### 2. Run the API

```bash
cd backend/GallopIQ.Api
dotnet restore
dotnet run
```

The API will be available at **http://localhost:5000**

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/races/today` | Fetch today's racecards |
| `GET` | `/races/{id}` | Fetch a specific race with runners |
| `POST` | `/predict/{raceId}` | Generate AI predictions for a race |
| `GET` | `/predictions/{raceId}` | Retrieve stored predictions |

Swagger UI: **http://localhost:5000/swagger**

---

## Frontend Setup

### 1. Install dependencies

```bash
cd frontend
npm install
```

### 2. Start dev server

```bash
npm start
# or
ng serve
```

The app will be available at **http://localhost:4200**

The Angular dev proxy automatically forwards API calls to `http://localhost:5000`.

---

## How It Works

```
User clicks race
      ↓
Angular → POST /predict/{raceId}
      ↓
Backend fetches racecard from The Racing API
      ↓
FeatureBuilderService extracts runner features
(name, age, form, jockey, trainer, weight, odds)
      ↓
GeminiService sends structured prompt to gemini-2.0-flash
with response_mime_type: application/json
      ↓
Gemini returns JSON array of predictions:
[{ horseName, winProbability, confidence, valueScore, reasoning }]
      ↓
PredictionService ranks horses, flags the top pick
      ↓
Predictions stored in-memory + returned to frontend
      ↓
Angular renders animated probability bars, confidence badges,
gold-highlighted top pick
```

---

## Development Notes

- **Predictions are in-memory only** — they reset when the backend restarts. Add a database (PostgreSQL + EF Core) to persist them.
- **Free Racing API tier** — uses `/v1/racecards/free` which covers basic daily racecards for GB/IRE.
- **Gemini JSON mode** — `response_mime_type: application/json` ensures Gemini always returns parseable JSON.
- **CORS** — backend allows `http://localhost:4200` for local dev. Update for production.

---

## Production Deployment

1. Build frontend: `cd frontend && ng build --configuration=production`
2. Publish backend: `cd backend/GallopIQ.Api && dotnet publish -c Release`
3. Serve Angular `dist/` via a static host or the .NET app's `wwwroot`
4. Set API keys via environment variables (not appsettings.json) in production

---

## Security

> ⚠️ `appsettings.json` contains live API credentials and is listed in `.gitignore`.  
> Never commit it to version control. Use `appsettings.example.json` as the template.

---

## License

MIT — built for educational and personal use.
