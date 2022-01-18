using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Message", menuName = "Message", order = 0)]
public class message : ScriptableObject
{
    [TextArea]
    public string msg;
    public emoji[] emojis;
    public eventFunction eventsActivator;

    public bool isEmojiTheSame(string emoji)
    {
        int x;
        for(x = 0; x < emojis.Length && emoji.Equals(emojis[x].getEmoji()); x++){}

        bool checker = x != emojis.Length;

        return checker;
    }
}

[System.Serializable]
public class emoji {
    public int emojiIndex;

    public string getEmoji()
    {
        return emojiList.emojis[emojiIndex];
    }
}


