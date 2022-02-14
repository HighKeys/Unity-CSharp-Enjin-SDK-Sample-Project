using System;
using UnityEngine;
using Enjin.SDK;
using Enjin.SDK.Models;
using Enjin.SDK.ProjectSchema;
using Enjin.SDK.Shared;

public class PlayerManager : MonoBehaviour
{
    public PlayerClient playerClient;
    public Player player;
    public string PlayerName;
    EnjinManager Enjin;
    public static event Action<PlayerManager> OnGetPlayerClient;

    void OnEnable()
    {
        PlayerListener.PlayerCreated += ()=> print("Player Created");
        PlayerListener.PlayerLinked += GetPlayer;         
        PlayerListener.PlayerUpdated += ()=> print("Player Updated");
        PlayerListener.PlayerUnlinked += ()=> print("Player Unlinked");
        PlayerListener.PlayerDeleted += ()=> print("Player Deleted");
    }
    void Awake()
    {
        EnjinManager.OnProjectAuthentication += (_enjin)=> 
        {
            Enjin = _enjin;
            playerClient = new PlayerClient(Enjin._api.Platform_URL);
            OnGetPlayerClient?.Invoke(this);
        };
                        
    }
    public void SetPlayerName(string _name)
    {
        PlayerName = _name;
    }

    public void GetPlayer()
    {
        var response = Enjin.projectClient.GetPlayer(new GetPlayer()
                                    .Id(PlayerName)
                                    .WithLinkingInfo()
                                    .WithWallet())
                                    .Result;
        if (response.IsSuccess)
        {
            player = response.Result;
            if(!Enjin.EventService.IsSubscribedToPlayer(Enjin._api.APP_ID, player.Id))
                Enjin.EventService.SubscribeToPlayer(Enjin._api.APP_ID, player.Id);
            
            if (player.Wallet == null)
            {
                print($"Please Link Account \n" +
                      $"Link Info: Code: {player.LinkingInfo.Code} \n" +
                      $"and QR: {player.LinkingInfo.Qr}");
                return;
            }

            //PlayerWallet = player.Wallet.EthAddress;
            if (!playerClient.IsAuthenticated)
            {
                playerClient.Auth(Enjin.projectClient
                            .AuthPlayer(new AuthPlayer()
                            .Id(PlayerName))
                            .Result
                            .Result
                            .Token);

                if(playerClient.IsAuthenticated)
                {
                    print("Player is Authenticated!");
                }
                
            }
        }
        else
        {
            Enjin.LogErrors(response.Errors);
            CreatePlayer();
        }
    }

    void CreatePlayer()
    {
        var response = Enjin.projectClient.CreatePlayer(new CreatePlayer()
                                    .Id(PlayerName))
                                    .Result;
        if (response.IsSuccess)
        {
            playerClient.Auth(response.Result.Token);
            GetPlayer();
        }
        else
        {
            Enjin.LogErrors(response.Errors);
        }
    }

    public void DeletePlayer()
    {
        var response = Enjin.projectClient.DeletePlayer(new DeletePlayer()
                                          .Id(PlayerName)).Result.Result;
    }

    void OnDisable()
    {
        PlayerListener.PlayerCreated -= ()=> print("Player Created");
        PlayerListener.PlayerLinked -= GetPlayer;         
        PlayerListener.PlayerUpdated -= ()=> print("Player Updated");
        PlayerListener.PlayerUnlinked -= ()=> print("Player Unlinked");
        PlayerListener.PlayerDeleted -= ()=> print("Player Deleted");
        Enjin.EventService.UnsubscribeToPlayer(Enjin._api.APP_ID, player.Id);        
    }
}
