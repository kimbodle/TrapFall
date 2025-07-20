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
            Debug.Log("IceTileEffect 발동");

            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.ModifySpeed(0.5f, 3f); // 감속 배율, 지속시간
            }

            StartCoroutine(ResetAfterDuration(3f)); // 3초 후 다시 사용 가능
            StartCoroutine(tile.RevertTile());      // 타일 상태도 복구
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
