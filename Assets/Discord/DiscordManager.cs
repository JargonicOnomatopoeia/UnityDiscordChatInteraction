using UnityEngine;
using System;
using System.Linq;
using DiscordUnity;
using DiscordUnity.API;
using DiscordUnity.State;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

public class DiscordManager : MonoBehaviour, IDiscordServerEvents, IDiscordMessageEvents, IDiscordStatusEvents
{
    [SerializeField] private commandList cmdList;
    [SerializeField] private string botToken;
    [SerializeField] private string channelid;
    public DiscordLogLevel logLevel = DiscordLogLevel.None;
    private Coroutine corTimer;
    
    #region Singleton
    public static DiscordManager Singleton { get; private set; }

    protected virtual void Awake()
    {
        activityList.channelId = channelid;
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Singleton == this)
        {
            DiscordAPI.UnregisterEventsHandler(this);
            DiscordAPI.Stop();
            Singleton = null;
        }
    }
    #endregion

    protected virtual async void Start()
    {
        Debug.Log("Starting DiscordUnity ...");
        DiscordAPI.Logger = new DiscordLogger(logLevel);
        DiscordAPI.RegisterEventsHandler(this);
        await DiscordAPI.StartWithBot(botToken);
        Debug.Log("DiscordUnity started.");
    }

    private void Update()
    {
        DiscordAPI.Update();
    }

    private IEnumerator coundownTimer()
    {
        int sec = 0;

        while(sec <= activityList.timer)
        {
            yield return new WaitForSeconds(1);
            sec++;

            messageSender temp = activityList.messages[0];
            int timeLeft = activityList.timer - sec;
            DiscordAPI.EditMessage(activityList.channelId, temp.messageId, temp.content + " " + timeLeft, null, 0);
        }
    }

    public void countdownStart()
    {
        corTimer = StartCoroutine("countdownTimer");
    }

    public void countdownStop()
    {
        StopCoroutine(corTimer);
    }

    public async void OnServerJoined(DiscordServer server)
    {
        //server.Channels.Values.FirstOrDefault(x => x.Type == DiscordUnity.Models.ChannelType.GUILD_TEXT)?.CreateMessage("Hello World!", null, null, null, null, null, null);
        //Debug.Log("hello world");
        //await DiscordAPI.CreateMessage(channelid, "Hello World", null, false, null, null, null, null);
    }

    public void OnServerUpdated(DiscordServer server)
    {

    }

    public void OnServerLeft(DiscordServer server)
    {

    }

    public void OnServerBan(DiscordServer server, DiscordUser user)
    {

    }

    public void OnServerUnban(DiscordServer server, DiscordUser user)
    {

    }

    public void OnServerEmojisUpdated(DiscordServer server, DiscordEmoji[] emojis)
    {

    }

    public void OnServerMemberJoined(DiscordServer server, DiscordServerMember member)
    {

    }

    public void OnServerMemberUpdated(DiscordServer server, DiscordServerMember member)
    {

    }

    public void OnServerMemberLeft(DiscordServer server, DiscordServerMember member)
    {

    }

    public void OnServerMembersChunk(DiscordServer server, DiscordServerMember[] members, string[] notFound, DiscordPresence[] presences)
    {

    }

    public void OnServerRoleCreated(DiscordServer server, DiscordRole role)
    {
        //Debug.Log("role added: " + role.Name);
    }

    public void OnServerRoleUpdated(DiscordServer server, DiscordRole role)
    {

    }

    public void OnServerRoleRemove(DiscordServer server, DiscordRole role)
    {

    }

    //message events
    public async void OnMessageCreated(DiscordMessage message)
    {  
        if((message.Author.Bot == null || message.Author.Bot == false) && 
            message.ChannelId.Equals(activityList.channelId) && message.Content.StartsWith("!")) // check if the author is not a bot
        {
            #region Debug
            Debug.Log("Message send: " + message.Content + ", from: " + message.Author.Username + ", messageID: " + message.Id);
            //Debug.Log("Server name: " + message.Channel.Name);
            #endregion Debug
            #region Message Filter
            string cmd = string.Empty;
            int cmdLength = -1;
            if(message.Content.Contains(" ")){
                cmdLength = message.Content.IndexOf(" ");
            }else{
                cmdLength = message.Content.Length-1;
            }
            cmd = message.Content.Substring(1, cmdLength);
            #endregion Message Filter

            int x = 0;
            for(;x < cmdList.commands.Length && !cmd.Equals(cmdList.commands[x].name);x++){}

            if(x != cmdList.commands.Length){
                messageSender curMes = new messageSender(channelid, cmdList.commands[x].messageProp);
                curMes.sendIt(activityList.timer);
                activityList.messages.Add(curMes);
            }
        }/*else if (message.Author.Bot == true){
            Debug.Log("Message send: " + message.Content + ", from: " + message.Author.Username + ", messageID: " + message.Id);

            //await Task.WhenAll(emoji);
            /*
            await AddEmoji(message.ChannelId, message.Id, "💩");

            //await Task.Delay(100);

            await AddEmoji(message.ChannelId, message.Id, "👺");

            //await Task.Delay(100);

            await AddEmoji(message.ChannelId, message.Id, "👻");

            //await Task.Delay(100);

            await AddEmoji(message.ChannelId, message.Id, "👍");

            Debug.Log("poop Sent");
            
        }*/


            /*
            if (message.Author.Bot == null )
            {
                Debug.Log("sender is not a bot, null");

            }

            if (message.Author.Bot == false)
            {
                Debug.Log("sender is not a bot, false");

            }

            if (message.Author.Bot == true)
            {
                Debug.Log("sender is a bot, true");

            }

            if (!message.Author.Bot == true)
            {
                Debug.Log("sender is not a bot, not true");

            }

            if (message.Author.Bot == null || message.Author.Bot == false)
            {
                Debug.Log("sender is not a bot, or");

        }*/
    }

    public void OnMessageUpdated(DiscordMessage message)
    {

    }

    public void OnMessageDeleted(DiscordMessage message)
    {
        Debug.Log("Message Deleted...");
    }

    public void OnMessageDeletedBulk(string[] messageIds)
    {

    }

    public async void OnMessageReactionAdded(DiscordMessageReaction messageReaction)
    {
        if ((messageReaction.Member.User.Bot == null || messageReaction.Member.User.Bot == false) && activityList.messageCheck(messageReaction.MessageId))
        {
            Debug.Log("reaction added to: " + messageReaction.MessageId + ", from: " + messageReaction.Member.Nick + ", reaction: " + messageReaction.Emoji.User + ", " + messageReaction.Emoji.User);

            foreach (messageSender item in activityList.messages)
            {
                item.content.eventsActivator.OnEmojiClick(new player(messageReaction.UserId, messageReaction.Member.Nick), messageReaction.Emoji.Name);
            }
            /*
            await DiscordAPI.CreateMessage(messageReaction.ChannelId, "User: " + messageReaction.Member.User.Username + " sent Emoji: " + messageReaction.Emoji.Name, null, false, null, null, null, null);

            RestResult<DiscordUser[]> reactionResult;
            reactionResult = await DiscordAPI.GetReactions(messageReaction.ChannelId, messageReaction.MessageId, "👍");

            DiscordUser[] array;

            array = reactionResult.Data.ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log(array[i].Username + ", has entered thumbs up");
            }
            
            Debug.Log(messageReaction.MessageId);
            */
        }
    }

    public void OnMessageReactionRemoved(DiscordMessageReaction messageReaction)
    {
        if (messageReaction.Member.User.Bot == false || messageReaction.Member.User.Bot == null)
        {
            foreach(messageSender item in activityList.messages)
            {
                item.content.eventsActivator.OnEmojiRemove(messageReaction.UserId);
            }
        }
        
    }

    public void OnMessageAllReactionsRemoved(DiscordMessageReaction messageReaction)
    {

    }

    public void OnMessageEmojiReactionRemoved(DiscordMessageReaction messageReaction)
    {
        Debug.Log("emoji removed");
    }


    // discord status events
    public void OnPresenceUpdated(DiscordPresence presence)
    {

    }

    public void OnTypingStarted(DiscordChannel channel, DiscordUser user, DateTime timestamp)
    {
        Debug.Log("started typing");
    }

    public void OnServerTypingStarted(DiscordChannel channel, DiscordServerMember member, DateTime timestamp)
    {
        Debug.Log("started typing");
    }

    public void OnUserUpdated(DiscordUser user)
    {

    }

    private async Task AddEmoji(string ChannelId,string messageId, string emoji)
    {
        await DiscordAPI.CreateReaction(ChannelId, messageId, emoji);
    }

    #region Logger
    public enum DiscordLogLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Debug = 3
    }

    private class DiscordLogger : DiscordUnity.ILogger
    {
        private readonly DiscordLogLevel level;

        public DiscordLogger(DiscordLogLevel level)
        {
            this.level = level;
        }

        public void Log(string log)
        {
            if (level >= DiscordLogLevel.Debug)
                Debug.Log(log);
        }

        public void LogWarning(string log)
        {
            if (level >= DiscordLogLevel.Warning)
            {
                Debug.LogWarning(log);
            }
        }

        public void LogError(string log, Exception exception = null)
        {
            if (level >= DiscordLogLevel.Error)
            {
                Debug.LogError(log);
                Debug.LogError(exception);
            }
        }
    }
    #endregion
}
