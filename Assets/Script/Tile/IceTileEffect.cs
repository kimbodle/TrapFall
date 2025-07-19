using System.Collections;
using UnityEngine;

public class IceTileEffect : MonoBehaviour, ISpecialTile
{
    private bool triggered = false;
    private TileComp tile;

    private void Awake()
    {
        tile = GetComponent<TileComp>();
    }
    public void Activate(GameObject player)
    {
        if (!triggered)
        {
            triggered = true;
            Debug.Log("IceTileEffect ¹ßµ¿");

            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                //multi,time
                controller.ModifySpeed(0.5f, 3f);
        }
        StartCoroutine(tile.RevertTile());
    }
    public void ResetTile()
    {
        triggered = false;
        StopAllCoroutines();
    }
}
