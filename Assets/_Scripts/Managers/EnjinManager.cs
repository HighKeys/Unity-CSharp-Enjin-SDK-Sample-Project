using System;
using System.Collections.Generic;
using UnityEngine;
using Enjin.SDK;
using Enjin.SDK.Graphql;
using Enjin.SDK.Models;
using Enjin.SDK.ProjectSchema;
using Enjin.SDK.Shared;
using Enjin.SDK.Events;

public enum Host_Platform
{
    Main,
    Jump,
    Kovan,
    Goerli
}
[System.Serializable]
public struct API_Credentials
{   
    public Uri Platform_URL;    
    public string APP_ID;
    public string APP_SECRET;        
}

public class EnjinManager : MonoBehaviour
{
    public static EnjinManager Instance;
    
    public ProjectClient projectClient;
    
    public Host_Platform host;

    [SerializeField]
    private API_Credentials main, kovan, jump, goerli;
    public PusherEventService EventService;
    [HideInInspector]
    public API_Credentials _api;
    
    void Awake()
    {
        _api = host switch 
        {
            Host_Platform.Main => new API_Credentials
            {
                Platform_URL = EnjinHosts.MAIN_NET,
                APP_ID = main.APP_ID,
                APP_SECRET = main.APP_SECRET
            },
            Host_Platform.Jump => new API_Credentials
            {
                Platform_URL = EnjinHosts.JUMP_NET,
                APP_ID = jump.APP_ID,
                APP_SECRET = jump.APP_SECRET
            },
            Host_Platform.Kovan => new API_Credentials
            {
                Platform_URL = EnjinHosts.KOVAN,
                APP_ID = kovan.APP_ID,
                APP_SECRET = kovan.APP_SECRET
            },
            Host_Platform.Goerli => new API_Credentials
            {
                Platform_URL = EnjinHosts.KOVAN,
                APP_ID = kovan.APP_ID,
                APP_SECRET = kovan.APP_SECRET
            },
            _=> main            
        };

        if(Instance == null) Instance = this;
        else Destroy(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        projectClient = new ProjectClient(_api.Platform_URL);
       
        AuthProject request = new AuthProject().Uuid(_api.APP_ID)
                                               .Secret(_api.APP_SECRET);
        
        var response = projectClient.AuthProject(request).Result;
        if(!response.IsSuccess) LogErrors(response.Errors);        

        projectClient.Auth(response.Result.Token);

        if(projectClient.IsAuthenticated) print("Project is now authenticated");
        else print("Project was not authenticated");

        var AccessToken = response.Result.Token;               

        StartEventService();
    }

    async void StartEventService()
    {
        var platform = projectClient.GetPlatform(new GetPlatform()
                                .WithNotificationDrivers())
                                .Result
                                .Result;

        EventService = new PusherEventService(platform);
        await EventService.Start();

        var reg = EventService.RegisterListener(new PlayerListener());

        if(EventService.IsConnected()) print("Event Service is Listening...");
    }
    public void LogErrors(List<GraphqlError> Errors)
    {
         print($"GraphQL request failed");
            foreach (var error in Errors)
            {
                print("Error Code :" + error.Code);
                print("Error Messgae :" + error.Message);
                print("Error Details :" + error.Details);                
            }            
    }
    void OnDisable()
    {        
        EventService.Shutdown();
        projectClient.Dispose();
    }
}
