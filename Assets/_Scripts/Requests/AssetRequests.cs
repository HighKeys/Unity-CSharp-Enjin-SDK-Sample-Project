using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enjin.SDK.Models;

public class AssetRequests : MonoBehaviour
{
    
}

[Serializable]
public struct CreateAssetRequest
{
    public string Name;
    public string TotalSuppy;
    public string InitialReserve;
    public AssetSupplyModel SupplyModel;
    public string MeltValue;
    [Range(0,5000)]
    public int MeltFeeRatio; //The ratio of the melt value returned to the developer in the range 0-5000 to allow fractional ratios, e,g, 1 = 0.01%, 5000 = 50%, 250 = 2.5% and so on.
    public AssetTransferable TransferMode;
    public TransferFeeSettings TransferSettings;
    public bool NonFungible;
    public string EthAddress;
}
[Serializable]
public struct TransferFeeSettings
{
    public AssetTransferFeeType FeeType;
    public string AssetId;
    public string Value; //The transfer fee value (in Wei). This also gets set as the max transfer fee.
}

/*Transfer input data, represents transfer of a 
single token type with a single to/from pair.*/
[Serializable]
public struct TransferInput 
{
    public string FromAddress; //Source of the funds
    public string ToAddress; //Destination of the funds
    public string AssetId; //Asset to send (pass null/omit to send ENJ).
    public string AssetIndex; //Index of asset to send (for NFTs)
    public string Value; //The number of items to transfer / amount of ENJ to send in Wei (10^18 value, e.g. 1 ENJ = 1000000000000000000).
}

[Serializable]
public struct AdvancedSendRequest
{
    public TransferInput[] Transfers; //The different transfers to perform.
    public string data; //The data to forward with the transaction.
    public string WalletAddress; //Project Wallet for use with Project Schema
}
[Serializable]
public struct MintRequest
{
    public string projectWallet; 
    public string assetID; 
    public string numOfItems; 
    public string toWallet;
}

[Serializable]
public struct TradeInput
{
    public string AssetID;
    public string AssetIndex;
    public string Value;
}

[Serializable]
public struct CreateTradeRequest
{
    public TradeInput[] AskingAssets;
    public TradeInput[] OfferingAssets;
    public string SecondPartyAddress;
    public string WalletAddress;
}