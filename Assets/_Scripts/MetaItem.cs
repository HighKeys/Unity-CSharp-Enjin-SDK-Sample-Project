using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Enjin.SDK.Models;

[Serializable]
public class Metadata
{
    public string name;
    public string description;
    public string image;

}
public class MetaItem 
{
    public Asset asset;
    public Metadata data;
    public Sprite image;

    public IEnumerator LoadMetadata(Action<Metadata> callback)
    {
        var url = asset.ConfigData.MetadataUri.Replace("{id}", asset.Id);
        var metadataRequest = UnityWebRequest.Get(url);
        yield return metadataRequest.SendWebRequest();
        
        data = JsonConvert.DeserializeObject<Metadata>(metadataRequest.downloadHandler.text);
        callback(data);        
    }

    public IEnumerator LoadImage(Action<Sprite> callback)
    {
        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(data.image);
        yield return imageRequest.SendWebRequest();

        if(imageRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
            image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0, 0));
            callback(image);
        }                
    }    
}
