using System.Collections.Generic;
using UnityEngine;
using Enjin.SDK.Models;

public class WalletManager : MonoBehaviour
{
    private PlayerManager playerManager;

    public Wallet wallet;

    public MetaCard cardPrefab;
    public Transform WalletWindow;
    public List<MetaCard> cardList = new List<MetaCard>();

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.OnPlayerAuthentication += (_manager) => {playerManager = _manager;};
    }

    public void GetWallet()
    {
        playerManager.GetPlayer();         

        wallet = playerManager.player.Wallet;

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
