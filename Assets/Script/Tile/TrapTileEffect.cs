using UnityEngine;
using System.Collections;

public class TrapTileEffect : MonoBehaviour, ISpecialTile
{
    private bool triggered = false;

    public void Activate(GameObject player)
    {
        if (!triggered)
        {
            Debug.Log("TrapTileEffect 발동");
            triggered = true;
            TrapRoutine();
        }
    }

    void TrapRoutine()
    {
        Debug.Log("TrapRoutine 발동");

        TileComp tile = GetComponent<TileComp>();
        if (tile != null)
        {
            tile.StartCoroutine(tile.DestroyTile());
            Debug.Log("tile != null 발동");
        }
    }

    public void ResetTile()
    {
        triggered = false;
        StopAllCoroutines();
    }
}

