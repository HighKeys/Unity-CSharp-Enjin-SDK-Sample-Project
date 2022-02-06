using UnityEngine;
using Enjin.SDK;
using Enjin.SDK.Models;
using Enjin.SDK.ProjectSchema;
using Enjin.SDK.Shared;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public PlayerClient playerClient;
    public Player player;
    public string PlayerName;

    EnjinManager Enjin;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        Enjin = EnjinManager.Instance;

        playerClient = new PlayerClient(Enjin._api.Platform_URL);      
        PlayerListener.PlayerLinked += GetPlayer;   
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
            if(!Enjin.EventService.IsSubscribedToPlayer(EnjinManager.Instance._api.APP_ID, player.Id))
                Enjin.EventService.SubscribeToPlayer(EnjinManager.Instance._api.APP_ID, player.Id);
            
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

                print("Player is Authenticated!");
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
        Enjin.EventService.UnsubscribeToPlayer(Enjin._api.APP_ID, player.Id);
    }
}
