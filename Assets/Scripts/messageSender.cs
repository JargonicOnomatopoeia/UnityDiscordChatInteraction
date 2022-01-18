using System.Threading.Tasks;
using DiscordUnity.State;
using DiscordUnity;
using System.Collections;
using UnityEngine;

public class messageSender
{
    public string channelId { private set; get; }
    public string messageId { private set; get; }
    public message content { private set; get; }
    private Coroutine capsule;
    

    public messageSender(string channelId, message content)
    {
        this.channelId = channelId;
        this.content = content;
    }

    public async void sendIt(int timer)
    {
        RestResult<DiscordMessage> botmsg = await DiscordAPI.CreateMessage(channelId, content.msg+" "+timer, null, false, null, null, null, null);
        messageId = botmsg.Data.Id;
        for (int y = 0; y < content.emojis.Length; y++)
        {
            await DiscordAPI.CreateReaction(channelId, botmsg.Data.Id, content.emojis[y].getEmoji());
            await Task.Delay(100);
        }
    }

    public async void editIt(int timer)
    {
        RestResult<DiscordMessage> botmsg  = await DiscordAPI.EditMessage(channelId, messageId, content.msg+" "+timer, null, 0);
        //messageId = botmsg.Data.Id;
    }
}
