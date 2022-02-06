using UnityEngine;
using Enjin.SDK.Models;
using Enjin.SDK.ProjectSchema;

[System.Serializable]
public struct MintRequest
{
    public string projectWallet; 
    public string assetID; 
    public string numOfItems; 
    public string toWallet;
}
public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance;
    public MintRequest mintReq;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }
    [ContextMenu("Mint Item")]
     public void TestMint()
    {
        mintReq.toWallet = WalletManager.Instance.wallet.EthAddress;
        MintItem(mintReq.projectWallet, mintReq.assetID, mintReq.numOfItems, mintReq.toWallet);
    }

    void MintItem(string projectWallet, string assetID, string numOfItems, string toWallet)
    {
        var response = EnjinManager.Instance.projectClient.MintAsset(new MintAsset()
                                    .EthAddress(projectWallet)
                                    .AssetId(assetID)
                                    .Mints(new MintInput()
                                    .To(toWallet)
                                    .Value(numOfItems))).Result;
        if(!response.IsSuccess)
        {
            EnjinManager.Instance.LogErrors(response.Errors);             
        }else
        {
            print("Item Sent!");
        }                                                                                                       
    }
}
