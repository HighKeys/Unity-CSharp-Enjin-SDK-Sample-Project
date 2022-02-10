using System;
using Enjin.SDK.Events;
using Enjin.SDK.Models;

public class AssetListener : IEventListener
{
    public static event Action ASSET_CREATED,ASSET_MELTED,ASSET_MINTED,ASSET_TRANSFERRED,ASSET_UPDATED,TRADE_ASSET_COMPLETED,TRADE_ASSET_CREATED;
        
    public void NotificationReceived(NotificationEvent notificationEvent)
    {
        var action = notificationEvent.Type switch
        {
            EventType.ASSET_CREATED => ASSET_CREATED,
            EventType.ASSET_MELTED => ASSET_MELTED,
            EventType.ASSET_MINTED => ASSET_MINTED,
            EventType.ASSET_TRANSFERRED => ASSET_TRANSFERRED,
            EventType.ASSET_UPDATED => ASSET_UPDATED,
            EventType.TRADE_ASSET_COMPLETED => TRADE_ASSET_COMPLETED,
            EventType.TRADE_ASSET_CREATED => TRADE_ASSET_CREATED,
            _ => null
        };
        action?.Invoke();
    }

    
}
