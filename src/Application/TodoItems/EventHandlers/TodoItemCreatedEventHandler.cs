using RestoMap.Domain.Events;
using Microsoft.Extensions.Logging;

namespace RestoMap.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedEventHandler> _logger;

    public TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RestoMap Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
