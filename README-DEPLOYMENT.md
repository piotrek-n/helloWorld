# BlazorApp1 - Azure Deployment

## GitHub Actions Configuration

### Opcja 1: Deploy z Azure Container Registry (Rekomendowane)

**Workflow**: `.github/workflows/azure-deploy.yml`

#### Wymagane GitHub Secrets:
1. `ACR_USERNAME` - Username do Azure Container Registry
2. `ACR_PASSWORD` - Password do Azure Container Registry  
3. `AZURE_WEBAPP_PUBLISH_PROFILE` - Publish profile z Azure Web App

#### Kroki konfiguracji:
1. Utwórz Azure Container Registry
2. W Azure Web App → Deployment Center → wybierz Container Registry
3. Dodaj secrets w GitHub repo → Settings → Secrets and variables → Actions

### Opcja 2: Bezpośredni deploy .NET

**Workflow**: `.github/workflows/azure-deploy-direct.yml`

#### Wymagane GitHub Secrets:
1. `AZURE_WEBAPP_PUBLISH_PROFILE` - Publish profile z Azure Web App

#### Kroki konfiguracji:
1. W Azure Web App → Deployment Center → GitHub Actions
2. Azure automatycznie wygeneruje publish profile
3. Dodaj secret w GitHub repo

### Jak pobrać Publish Profile:
1. Idź do swojego Azure Web App
2. Overview → Get publish profile (Download)
3. Skopiuj zawartość pliku
4. Dodaj jako secret `AZURE_WEBAPP_PUBLISH_PROFILE`

### Zmienne do zaktualizowania w workflows:
- `AZURE_WEBAPP_NAME`: web-afd-ag (twoja nazwa)
- `CONTAINER_REGISTRY`: twój ACR URL (tylko dla opcji 1)

## Uruchomienie
Workflows uruchamiają się automatycznie przy push na branch `main` lub `master`, lub ręcznie przez GitHub Actions tab.

## Dockerfile
Aplikacja używa .NET 9.0 i jest skonfigurowana dla Azure Web App Linux Container.
