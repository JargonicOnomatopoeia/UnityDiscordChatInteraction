using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class activityList : MonoBehaviour
{
    private static List<player> players = new List<player>();
    private static Dictionary<action, int> actions = new Dictionary<action, int>();
    public static List<messageSender> messages = new List<messageSender>();
    public static string channelId;
    public static int timer = 30;

    public static bool playerExist(string id) {
        return players.Any(n => (n.id == id));
    }

    public static void addPlayer(player p)
    {
        if (!playerExist(p.id))
        {
            players.Add(p);
        }
    }

    public static void removePlayer(string userId)
    {
        if (playerExist(userId))
        {
            player temp = players.Find(item => (item.id.Equals(userId)));
            players.Remove(temp);
        }
    }

    public static int playerCount()
    {
        return players.Count;
    }

    public static void addAction(player p, string action, int order)
    {
        actions.Add(new action(p.id, action), order);
    }

    public static void removeAction()
    {

    }

    public static bool messageCheck(string messageId)
    {
        return messages.Any(n => (n.messageId.Equals(messageId)));
    }

    public static void getMessageIndex(string messageId)
    {
        if (messageCheck(messageId))
        {
            int index = messages.FindIndex(n => (n.messageId.Equals(messageId)));
        }
    }
}

public class player {
    public string id;
    public string name;

    public player(string id, string name)
    {
        this.id = id;
        this.name = name;
    }
}

public class action
{
    public string id;
    public int act;
    
    
    public action(string id, string emoji)
    {
        this.id = id;
        act = emojiList.findEmoji(emoji);
    }
}

