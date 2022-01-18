using UnityEngine;
using DiscordUnity;


[CreateAssetMenu(fileName = "EventPlayerJoin",menuName = "Discord Interaction/Event Player Join",order = 0)]
public class eventPlayerJoin :  eventFunction
{
    public message nextMessage;

    public override void OnEmojiClick(player p, string emoji)
    {
        if (emojiList.findEmoji(emoji) == 20)
        {
            activityList.addPlayer(p);
        }

        //if (activityList.playerCount() == 0)
        //{
            DiscordManager.Singleton.countdownStart();
        //}
        //throw new System.NotImplementedException();
    }

    public override void OnEmojiRemove(string userId)
    {
        activityList.removePlayer(userId);

        //f (activityList.playerCount() <= 1)
        //{
            DiscordManager.Singleton.countdownStop();
        //}
        //throw new System.NotImplementedException();
    }

    public override void GetResult()
    {
        DiscordManager.Singleton.countdownStop();
        if (activityList.playerCount() >= 2)
        {
            DiscordAPI.DeleteMessage(activityList.channelId, activityList.messages[0].messageId);
            activityList.messages.Add(new messageSender(activityList.channelId, nextMessage));
            activityList.messages[1].sendIt(activityList.timer);
            DiscordManager.Singleton.countdownStart();
            activityList.messages.RemoveAt(0);
        }
        //throw new System.NotImplementedException();
    }
}
