using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OGTGlobalChatManager : MonoBehaviour
{

    [SerializeField]
    private Text txtChat = null;


    public class MyMsgType
    {
        public static short ChatMessageID = MsgType.Highest + 1;
    }

    public class ChatMessage : MessageBase
    {
        public int PlayerID = 0;
        public string NewMesssage = "";
    }


    public void SendNewChatMessage(string str)
    {
        ChatMessage msg = new ChatMessage
        {
            NewMesssage = str,
            PlayerID = OGTPlayer.Client.PlayerID
        };

        NetworkServer.SendToAll(MyMsgType.ChatMessageID, msg);
    }
}
