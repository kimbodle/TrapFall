using UnityEngine;

public class SpinTileEffect : MonoBehaviour, ISpecialTile
{
    public void Activate(GameObject player)
    {
        Debug.Log("SpinTileEffect 발동");
        // 방향 반전
        var move = player.GetComponent <PlayerController >();
        if (move != null)
        {
            //move.InvertInput(); // 좌우 반전 구현 해야함
        }
    }

    public void ResetTile() { }
}
