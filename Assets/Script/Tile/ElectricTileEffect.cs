using UnityEngine;

public class ElectricTileEffect : MonoBehaviour, ISpecialTile
{
    private bool triggered = false;

    public void Activate(GameObject player)
    {
        if (triggered) return;
        triggered = true;

        Debug.Log("ElectricTileEffect 발동");

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
            controller.DisableJump(3f); // 5초간 점프 금지

        StartCoroutine(RevertTile());
    }

    private System.Collections.IEnumerator RevertTile()
    {
        yield return new WaitForSeconds(1f); // 타일 1초 후 복구

        TileComp tile = GetComponent<TileComp>();
        if (tile != null)
            tile.SetTileType(TileType.Normal);
    }

    public void ResetTile()
    {
        triggered = false;
    }
}
