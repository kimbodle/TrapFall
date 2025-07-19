using UnityEngine;
using System.Collections;

public class TrapTileEffect : MonoBehaviour, ISpecialTile
{
    private bool triggered = false;

    public void Activate(GameObject player)
    {
        if (!triggered)
        {
            Debug.Log("TrapTileEffect ¹ßµ¿");
            triggered = true;
            StartCoroutine(TrapRoutine());
        }
    }

    IEnumerator TrapRoutine()
    {
        yield return new WaitForSeconds(1f);

        TileComp tile = GetComponent<TileComp>();
        if (tile != null)
        {
            tile.SetTileType(TileType.Destroyed);

            yield return new WaitForSeconds(tile.recoveryTileTime);

            tile.SetTileType(TileType.Normal);
        }

        triggered = false;
    }

    public void ResetTile()
    {
        triggered = false;
        StopAllCoroutines();
    }
}

