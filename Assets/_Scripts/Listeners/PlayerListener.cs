using System;
using Enjin.SDK.Events;
using Enjin.SDK.Models;


public class PlayerListener : IEventListener
{
    public static event Action PlayerLinked, PlayerCreated, PlayerDeleted, PlayerUnlinked, PlayerUpdated;
    public void NotificationReceived(NotificationEvent notificationEvent)
    {
        var action = notificationEvent.Type switch
        {
            EventType.PLAYER_LINKED => PlayerLinked,
            EventType.PLAYER_CREATED => PlayerCreated,
            EventType.PLAYER_DELETED => PlayerDeleted,
            EventType.PLAYER_UNLINKED => PlayerUnlinked,
            EventType.PLAYER_UPDATED => PlayerUpdated,
            _ => null   
        };
        action?.Invoke();
    }

    
}
