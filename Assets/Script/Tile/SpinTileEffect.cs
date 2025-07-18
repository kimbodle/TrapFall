using UnityEngine;

public class SpinTileEffect : MonoBehaviour, ISpecialTile
{
    public void Activate(GameObject player)
    {
        Debug.Log("SpinTileEffect 발동");

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
            controller.InvertInput(2f); // 2초간 좌우 반전
    }


    public void ResetTile() { }
}
