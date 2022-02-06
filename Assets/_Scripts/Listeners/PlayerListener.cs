using System.Collections;
using System.Collections.Generic;
using Enjin.SDK.Events;
using Enjin.SDK.Models;
using System;
using print = UnityEngine.Debug; 

public class PlayerListener : IEventListener
{
    public static Action PlayerLinked, PlayerCreated, PlayerDeleted;
    public void NotificationReceived(NotificationEvent notificationEvent)
    {
        var action = notificationEvent.Type switch
        {
            EventType.PLAYER_LINKED => PlayerListener.PlayerLinked,
            EventType.PLAYER_CREATED => PlayerListener.PlayerCreated,
            EventType.PLAYER_DELETED => PlayerListener.PlayerDeleted,
            _ => null   
        };
        action.Invoke();
    }

    
}
