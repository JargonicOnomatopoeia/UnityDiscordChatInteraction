using UnityEngine;

[CreateAssetMenu(fileName = "Command List", menuName = "Command List", order = 0)]
public class commandList : ScriptableObject {
    public cmd[] commands;
}

[System.Serializable]
public class cmd
{
    public string name;
    public message messageProp;
}

