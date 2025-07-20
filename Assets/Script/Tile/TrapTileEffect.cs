using UnityEngine;
using System.Collections;

public class TrapTileEffect : MonoBehaviour, ISpecialTile
{
    private bool triggered = false;

    public void Activate(GameObject player)
    {
        if (!triggered)
        {
            Debug.Log("TrapTileEffect �ߵ�");
            triggered = true;
            TrapRoutine();
        }
    }

    void TrapRoutine()
    {
        Debug.Log("TrapRoutine �ߵ�");

        TileComp tile = GetComponent<TileComp>();
        if (tile != null)
        {
            tile.StartCoroutine(tile.DestroyTile());
            Debug.Log("tile != null �ߵ�");
        }
    }

    public void ResetTile()
    {
        triggered = false;
        StopAllCoroutines();
    }
}

