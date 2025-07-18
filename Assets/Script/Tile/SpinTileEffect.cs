using System.Collections;
using UnityEngine;

public class SpinTileEffect : MonoBehaviour, ISpecialTile
{
    private bool triggered = false;

    public void Activate(GameObject player)
    {
        if (triggered) return;
        triggered = true;

        Debug.Log("SpinTileEffect 발동");

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
            controller.InvertInput(2f); // 2초간 좌우 반전

        StartCoroutine(RevertTile());
    }

    private IEnumerator RevertTile()
    {
        yield return new WaitForSeconds(1f);

        TileComp tile = GetComponent<TileComp>();
        if (tile != null)
            tile.SetTileType(TileType.Normal);
    }

    public void ResetTile()
    {
        triggered = false;
    }
}
