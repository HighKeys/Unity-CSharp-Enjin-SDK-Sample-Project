using System;
using Enjin.SDK.Events;
using Enjin.SDK.Models;

public class WalletListener : IEventListener
{
    public static event Action<NotificationEvent> TRANSACTION_BROADCAST,TRANSACTION_CANCELED,TRANSACTION_DROPPED, TRANSACTION_EXECUTED, TRANSACTION_FAILED, TRANSACTION_PENDING, TRANSACTION_PROCESSING, TRANSACTION_UPDATED, MESSAGE_PROCESSED;
    public void NotificationReceived(NotificationEvent notificationEvent)
    {
         var action = notificationEvent.Type switch
        {
            EventType.TRANSACTION_BROADCAST => TRANSACTION_BROADCAST,
            EventType.TRANSACTION_CANCELED => TRANSACTION_CANCELED,
            EventType.TRANSACTION_DROPPED => TRANSACTION_DROPPED,
            EventType.TRANSACTION_EXECUTED => TRANSACTION_EXECUTED,
            EventType.TRANSACTION_FAILED => TRANSACTION_FAILED,
            EventType.TRANSACTION_PENDING => TRANSACTION_PENDING,
            EventType.TRANSACTION_PROCESSING => TRANSACTION_PROCESSING,
            EventType.TRANSACTION_UPDATED => TRANSACTION_UPDATED,
            EventType.MESSAGE_PROCESSED => MESSAGE_PROCESSED,
            _ => null
        };
        action?.Invoke(notificationEvent);
    }
}
