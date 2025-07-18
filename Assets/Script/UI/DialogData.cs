using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Game/Dialog")]
public class DialogData : ScriptableObject
{
    public Sprite characterSprite;
    [TextArea] public string[] dialogLines;
}
