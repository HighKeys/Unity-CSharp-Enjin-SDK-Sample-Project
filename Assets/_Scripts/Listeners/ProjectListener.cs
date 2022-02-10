using System;
using Enjin.SDK.Events;
using Enjin.SDK.Models;

public class ProjectListener : IEventListener
{
    public static event Action PROJECT_CREATED, PROJECT_DELETED, PROJECT_LINKED, PROJECT_LOCKED, PROJECT_UNLINKED, PROJECT_UNLOCKED, PROJECT_UPDATED, BLOCKCHAIN_LOG_PROCESSED;
    public void NotificationReceived(NotificationEvent notificationEvent)
    {
        
        var action = notificationEvent.Type switch
        {
            EventType.PROJECT_CREATED => PROJECT_CREATED,
            EventType.PROJECT_DELETED => PROJECT_DELETED,
            EventType.PROJECT_LINKED => PROJECT_LINKED,
            EventType.PROJECT_LOCKED => PROJECT_LOCKED,
            EventType.PROJECT_UNLINKED => PROJECT_UNLINKED,
            EventType.PROJECT_UPDATED => PROJECT_UPDATED,
            EventType.BLOCKCHAIN_LOG_PROCESSED => BLOCKCHAIN_LOG_PROCESSED,
            _ => null
        };
        action?.Invoke();
    }

    
}
