# RestoMap

**RestoMap** - це веб-додаток для пошуку і управління ресторанами, побудований на основі Clean Architecture з використанням .NET 9 та Angular.

## 🏗️ Архітектура

Проект використовує **Clean Architecture** підхід і складається з наступних шарів:

- **Domain** - доменні сутності, value objects та бізнес-правила
- **Application** - команди, запити, обробники та інтерфейси
- **Infrastructure** - реалізація інтерфейсів, доступ до даних, зовнішні сервіси
- **Web** - API контролери, ендпоінти та Angular клієнт

## 🛠️ Технології

- **.NET 9** - Backend API
- **Angular 18** - Frontend SPA
- **PostgreSQL** - Основна база даних
- **Redis** - Кешування
- **Entity Framework Core** - ORM
- **MediatR** - CQRS паттерн
- **Docker** - Контейнеризація

## 🚀 Швидкий старт

### Опція 1: Локальна розробка (рекомендовано)

1. **Запустити тільки сервіси в Docker:**
```bash
make dev-services  # або docker-compose up -d postgres redis
```

2. **Встановити залежності:**
```bash
make dev-setup  # або dotnet restore + npm install
```

3. **Запустити додаток:**
```bash
make run  # або cd src/Web && dotnet watch run
```

Додаток буде доступний за адресою: https://localhost:5001

### Опція 2: Повний Docker (для тестування продакшену)

```bash
make docker-up-build  # або docker-compose up -d --build
```

Додаток буде доступний за адресою: http://localhost:8080

## 🐳 Docker для розробки

### Чи потрібно використовувати Docker для розробки?

**Рекомендація:** Використовуйте **гібридний підхід**:
- ✅ **PostgreSQL та Redis в Docker** - для швидкого налаштування без встановлення локально
- ✅ **.NET додаток локально** - для hot reload, дебагінгу та швидшої розробки
- ✅ **Angular локально** - для live reload та кращого dev experience

### Команди для роботи з Docker:

```bash
# Розробка (тільки сервіси)
make dev-services        # Запустити PostgreSQL + Redis
make dev-stop-services   # Зупинити сервіси

# Повний Docker (продакшн-подібний)
make docker-up-build     # Побудувати і запустити все
make docker-logs         # Переглянути логи
make docker-down         # Зупинити все
```

## 👨‍💻 Флоу розробника: Додавання нового ендпоінту

### 1. Створити структуру в Application шарі

```bash
# Використати генератор коду
make generate-usecase NAME=CreateRestaurant FEATURE=Restaurants TYPE=command RETURN=int

# Або створити вручну в src/Application/Restaurants/Commands/CreateRestaurant/
```

### 2. Реалізувати Command/Query

```csharp
// CreateRestaurant.cs
public record CreateRestaurantCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Address { get; init; } = null!;
}

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
{
    // Реалізація...
}
```

### 3. Додати валідатор (опціонально)

```csharp
// CreateRestaurantCommandValidator.cs
public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
    }
}
```

### 4. Створити ендпоінт групу

```csharp
// src/Web/Endpoints/Restaurants.cs
public class Restaurants : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateRestaurant);
    }

    public async Task<Created<int>> CreateRestaurant(ISender sender, CreateRestaurantCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Restaurants)}/{id}", id);
    }
}
```

### 5. Перевірити результат

```bash
make run                 # Запустити додаток
# Перейти на https://localhost:5001/api для перегляду Swagger
```

## 🧪 Тестування

```bash
make test              # Всі тести крім acceptance
make test-unit         # Юніт тести
make test-integration  # Інтеграційні тести
make test-acceptance   # Acceptance тести (потребує запущений додаток)
```

## 🗄️ База даних

```bash
make db-migration NAME=AddRestaurantTable  # Створити міграцію
make db-update                            # Застосувати міграції
make db-drop                              # Видалити базу данних
```

## 🔧 Корисні команди

```bash
make help          # Показати всі доступні команди
make build         # Побудувати рішення
make clean         # Очистити артефакти збірки
make format        # Форматувати код
make restore       # Відновити пакети
```

## 📝 Code Scaffolding

Проект підтримує автоматичну генерацію коду:

```bash
# Створити команду
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int

# Створити запит
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

## 🌐 API документація

Swagger UI доступний за адресою: `/api` в режимі розробки.

## 🏃‍♂️ Швидкий розвиток

1. **Старт:** `make dev-start` - налаштує все необхідне
2. **Розробка:** `make run` - запустить додаток з hot reload
3. **Тестування:** `make test` - запустить тести
4. **Продакшн тест:** `make prod-test` - протестує Docker збірку

## 🤝 Contributing

1. Створіть feature branch від `main`
2. Реалізуйте зміни, дотримуючись Clean Architecture принципів
3. Додайте тести для нового функціоналу
4. Переконайтеся, що `make test` проходить успішно
5. Створіть Pull Request

---

**Примітка:** Цей проект базується на [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/CleanArchitecture) версії 9.0.11.