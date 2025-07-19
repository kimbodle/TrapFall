using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Game/Dialog")]
public class DialogData : ScriptableObject
{
    public Sprite[] characterImages;
    public Sprite[] backgroundImages;
    public string[] dialogLines; 
}
