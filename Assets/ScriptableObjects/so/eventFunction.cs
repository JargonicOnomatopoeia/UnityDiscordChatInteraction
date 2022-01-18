using UnityEngine;

public abstract class eventFunction: ScriptableObject
{
    public abstract void OnEmojiClick(player p, string emoji);

    public abstract void OnEmojiRemove(string userId);

    public abstract void GetResult();
}
