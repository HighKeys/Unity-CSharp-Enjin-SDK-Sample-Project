using System.Collections.Generic;
using UnityEngine;
using Enjin.SDK.Models;

public class WalletManager : MonoBehaviour
{
    public static WalletManager Instance;
    public Wallet wallet;

    public MetaCard cardPrefab;
    public Transform WalletWindow;
    public List<MetaCard> cardList = new List<MetaCard>();

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    public void GetWallet()
    {
        PlayerManager.Instance.GetPlayer();         

        wallet = PlayerManager.Instance.player.Wallet;

        if(wallet != null)
        {
            foreach (var bal in wallet.Balances)
            { 
                var card = Instantiate(cardPrefab, WalletWindow);      
                card.item = new MetaItem();          
                card.item.asset = (Asset)bal.Asset;
                card.quantity = (int)bal.Value;                  
                StartCoroutine(card.item.LoadMetadata((response)=>
                {
                    card.Title.text =card.item.data.name;
                    card.Description.text = card.item.data.description; 
                    StartCoroutine(card.item.LoadImage((response)=>
                    {
                        card.Icon.sprite = card.item.image;
                    }));                    
                } )); 
                card.gameObject.SetActive(true);
                cardList.Add(card); 
            }
        }
    }    
}
