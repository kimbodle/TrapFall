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
            Debug.Log("IceTileEffect �ߵ�");

            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.ModifySpeed(0.5f, 3f); // ���� ����, ���ӽð�
            }

            StartCoroutine(ResetAfterDuration(3f)); // 3�� �� �ٽ� ��� ����
            StartCoroutine(tile.RevertTile());      // Ÿ�� ���µ� ����
        }
    }

    private IEnumerator ResetAfterDuration(float delay)
    {
        yield return new WaitForSeconds(delay);
        triggered = false;
    }

    public void ResetTile()
    {
        triggered = false;
        StopCoroutine(nameof(ResetAfterDuration));
    }
}
