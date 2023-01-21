namespace EventBus.Messages.Events;

public class IntegrationBaseEvent
{
    public IntegrationBaseEvent()
    {
        MessageId = Guid.NewGuid();
        MessageCreationDate = DateTime.UtcNow;
    }

    public IntegrationBaseEvent(Guid id, DateTime createDate)
    {
        MessageId = id;
        MessageCreationDate = createDate;
    }

    public Guid MessageId { get; private set; }

    public DateTime MessageCreationDate { get; private set; }
}
