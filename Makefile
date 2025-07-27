# RestoMap Makefile
# Development commands for .NET + Angular project with Docker

.PHONY: help build run test clean docker-build docker-up docker-down docker-logs dev-setup

# Default target
help: ## Show this help message
	@echo "RestoMap Development Commands:"
	@echo ""
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

# Development setup
dev-setup: ## Install dependencies and setup development environment
	dotnet restore
	cd src/Web/ClientApp && npm install

# Build commands
build: ## Build the entire solution
	dotnet build -tl

build-release: ## Build solution in Release mode
	dotnet build -c Release -tl

publish: ## Publish the application
	dotnet publish src/Web/Web.csproj -c Release -o ./publish

# Run commands
run: ## Run the web application in development mode
	cd src/Web && dotnet watch run

run-api: ## Run only the API (without Angular dev server)
	cd src/Web && dotnet run

run-client: ## Run Angular client in development mode
	cd src/Web/ClientApp && npm start

# Test commands
test: ## Run all tests except acceptance tests
	dotnet test --filter "FullyQualifiedName!~AcceptanceTests"

test-unit: ## Run only unit tests
	dotnet test tests/Application.UnitTests tests/Domain.UnitTests

test-integration: ## Run integration tests
	dotnet test tests/Application.FunctionalTests tests/Infrastructure.IntegrationTests

test-acceptance: ## Run acceptance tests (requires app to be running)
	dotnet test tests/Web.AcceptanceTests

test-all: ## Run all tests including acceptance tests
	dotnet test

# Code generation
generate-usecase: ## Generate new usecase (command or query). Usage: make generate-usecase NAME=CreateRestaurant FEATURE=Restaurants TYPE=command RETURN=int
	cd src/Application && dotnet new ca-usecase --name $(NAME) --feature-name $(FEATURE) --usecase-type $(TYPE) --return-type $(RETURN)

# Docker commands
docker-build: ## Build Docker image
	docker build -t restomap .

docker-up: ## Start services with docker-compose
	docker-compose up -d

docker-up-build: ## Build and start services with docker-compose
	docker-compose up -d --build

docker-down: ## Stop and remove docker-compose services
	docker-compose down

docker-logs: ## Show docker-compose logs
	docker-compose logs -f

docker-logs-web: ## Show logs for web service only
	docker-compose logs -f web

docker-clean: ## Remove docker containers, networks, and volumes
	docker-compose down -v --remove-orphans
	docker system prune -f

# Database commands
db-update: ## Apply database migrations
	cd src/Web && dotnet ef database update

db-migration: ## Create new migration. Usage: make db-migration NAME=AddRestaurantTable
	cd src/Web && dotnet ef migrations add $(NAME)

db-drop: ## Drop database
	cd src/Web && dotnet ef database drop --force

# Development utilities
clean: ## Clean build artifacts
	dotnet clean
	cd src/Web/ClientApp && npm run clean || true
	rm -rf src/*/bin src/*/obj tests/*/bin tests/*/obj

format: ## Format code
	dotnet format

restore: ## Restore NuGet and npm packages
	dotnet restore
	cd src/Web/ClientApp && npm install

# Local development with Docker services only (recommended)
dev-services: ## Start only PostgreSQL and Redis in Docker for local development
	docker-compose up -d postgres redis

dev-stop-services: ## Stop PostgreSQL and Redis Docker services
	docker-compose stop postgres redis

# Full development workflow
dev-start: dev-services dev-setup ## Setup and start development environment
	@echo "✅ Development environment ready!"
	@echo "Run 'make run' to start the application"

# Production-like testing
prod-test: docker-up-build ## Test production build with Docker
	@echo "🚀 Production test environment started"
	@echo "Application available at http://localhost:8080" 