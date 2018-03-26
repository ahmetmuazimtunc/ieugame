using System;
using System.Collections.Generic;
using UnityEngine;

public class OGTPlayer : MonoBehaviour
{
    public static Dictionary<short, string> OnlinePlayers = new Dictionary<short, string>();
    private static OGTPlayer _client;

    public static OGTPlayer Client
    {
        get { return _client; }
    }


    public int PlayerID;
    public string PlayerName = "";

    public void LoadPlayerValues()
    {
        /* Ileride Firebase'in Databse'inden veri okuyup yazma için kullanılacak*/
        //TO DO...
    }

    public static string IdentifyAPlayer(Int16 id)
    {
        return OnlinePlayers[id];
    }

    private void Start()
    {
        //singleton pattern is only for initializing. Values of other players can only be loaded from our server
        if (_client == null)
        {
            _client = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
