using System.Collections;
using UnityEngine;

public class IceTileEffect : MonoBehaviour, ISpecialTile
{
    private bool triggered = false;

    public void Activate(GameObject player)
    {
        if (!triggered)
        {
            triggered = true;
            Debug.Log("IceTileEffect 발동");

            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                //multi,time
                controller.ModifySpeed(0.5f, 3f);
        }
        StartCoroutine(RevertTile());
    }
    private IEnumerator RevertTile()
    {
        yield return new WaitForSeconds(1f);

        TileComp tile = GetComponent<TileComp>();
        if (tile != null)
            tile.SetTileType(TileType.Normal); // 타일 복구
    }
    public void ResetTile()
    {
        triggered = false;
    }
}
