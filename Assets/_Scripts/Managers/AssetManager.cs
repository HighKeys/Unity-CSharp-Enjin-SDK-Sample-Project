using UnityEngine;
using Enjin.SDK;
using Enjin.SDK.Models;
using Enjin.SDK.ProjectSchema;

[System.Serializable]

public class AssetManager : MonoBehaviour
{   
    public CreateAssetRequest CreateReq;
    public MintRequest MintReq;
    public AdvancedSendRequest AdvSendReq;
    public CreateTradeRequest CreateTradeReq;
    EnjinManager Enjin;
    Player Player;

    // Start is called before the first frame update
    void Awake()
    {
        EnjinManager.OnProjectAuthentication += (_enjin)=> {Enjin = _enjin;};
        PlayerManager.OnGetPlayerClient += (_manager) => {Player = _manager.player;};
    }

    [ContextMenu("Create Asset")]   
    public void CreateAsset()
    {
        var response = Enjin.projectClient.CreateAsset(new CreateAsset().Name(CreateReq.Name)
                                                          .TotalSupply(CreateReq.TotalSuppy)
                                                          .InitialReserve(CreateReq.InitialReserve)
                                                          .SupplyModel(CreateReq.SupplyModel)
                                                          .MeltValue(CreateReq.MeltValue)
                                                          .MeltFeeRatio(CreateReq.MeltFeeRatio)
                                                          .Transferable(CreateReq.TransferMode)
                                                          .TransferFeeSettings(new AssetTransferFeeSettingsInput()
                                                                                .Type(CreateReq.TransferSettings.FeeType)
                                                                                .AssetId(CreateReq.TransferSettings.AssetId)
                                                                                .Value(CreateReq.TransferSettings.Value))
                                                          .NonFungible(CreateReq.NonFungible)
                                                          .EthAddress(CreateReq.EthAddress)).Result;
    }
    [ContextMenu("Mint Item")]
    public void TestMint()
    {
        MintReq.toWallet = Player.Wallet.EthAddress;
        MintItem(MintReq.projectWallet, MintReq.assetID, MintReq.numOfItems, MintReq.toWallet);
    }

    public void MintItem(string projectWallet, string assetID, string numOfItems, string toWallet)
    {
        var response = Enjin.projectClient.MintAsset(new MintAsset()
                            .EthAddress(projectWallet)
                            .AssetId(assetID)
                            .Mints(new MintInput()
                            .To(toWallet)
                            .Value(numOfItems))).Result;
        if(!response.IsSuccess)
        {
            Enjin.LogErrors(response.Errors);             
        }else
        {
            
            print("Item Sent! Transaction ID: " + response.Result.Id);
        }                                                                                                       
    }

    public void AdvancedSendAsset()
    {
        Transfer[] transfers = new Transfer[AdvSendReq.Transfers.Length];
        for (int i = 0; i < transfers.Length; i++)
        {
            transfers[i].From(AdvSendReq.Transfers[i].FromAddress)
                        .To(AdvSendReq.Transfers[i].ToAddress)
                        .AssetId(AdvSendReq.Transfers[i].AssetId)
                        .Value(AdvSendReq.Transfers[i].Value);
        }
        var response = Enjin.projectClient.AdvancedSendAsset(new AdvancedSendAsset()
                                                .Transfers(transfers)
                                                .EthAddress(AdvSendReq.WalletAddress))
                                                .Result;

        if(!response.IsSuccess)
        {
            Enjin.LogErrors(response.Errors);             
        }else
        {
            print("Items Sent! Transaction ID: " + response.Result.Id);
        }                            
    }

    public void InitiateTrade()
    {
        Trade[] askingAssets = new Trade[CreateTradeReq.AskingAssets.Length];
        for (int i = 0; i < askingAssets.Length; i++)
        {
            askingAssets[i].AssetId(CreateTradeReq.AskingAssets[i].AssetID)
                           .AssetIndex(CreateTradeReq.AskingAssets[i].AssetIndex)
                           .Value(CreateTradeReq.AskingAssets[i].Value);
        }
        Trade[] offeringAssets = new Trade[CreateTradeReq.OfferingAssets.Length];
        for (int i = 0; i < offeringAssets.Length; i++)
        {
            offeringAssets[i].AssetId(CreateTradeReq.OfferingAssets[i].AssetID)
                             .AssetIndex(CreateTradeReq.OfferingAssets[i].AssetIndex)
                             .Value(CreateTradeReq.OfferingAssets[i].Value);
        }

        var response = Enjin.projectClient.CreateTrade(new CreateTrade().AskingAssets(askingAssets)
                                                        .OfferingAssets(offeringAssets)
                                                        .RecipientAddress(CreateTradeReq.SecondPartyAddress)
                                                        .EthAddress(CreateTradeReq.WalletAddress))
                                                        .Result;
        if(!response.IsSuccess)
        {
            Enjin.LogErrors(response.Errors);             
        }else
        {
            print("Trade Started! Transaction ID: " + response.Result.TransactionId);
        }             
        
    }

    public void CompleteTrade(string TransactionId)
    {
        var response = Enjin.projectClient.CompleteTrade(new CompleteTrade().TradeId(TransactionId)).Result;
    }

}
