using UnityEngine;

public class ElectricTileEffect : MonoBehaviour, ISpecialTile
{
    private bool triggered = false;
    private TileComp tile;

    private void Awake()
    {
        tile = GetComponent<TileComp>();
    }
    public void Activate(GameObject player)
    {
        if (triggered) return;
        triggered = true;

        Debug.Log("ElectricTileEffect ¹ßµ¿");

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
            controller.DisableJump(3f);

        StartCoroutine(tile.RevertTile());
    }

    public void ResetTile()
    {
        triggered = false;
        StopAllCoroutines();
    }
}
