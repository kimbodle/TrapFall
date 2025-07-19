using UnityEngine;

public class Laser : MonoBehaviour
{
    public float length = 20f;
    public float duration = 0.2f;
    public LayerMask playerLayer;
    public LineRenderer lineRenderer;

    private void Start()
    {
        Vector3 dir = transform.up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, length, playerLayer);
        Vector3 endPos = transform.position + dir * length;

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            PlayerController player = hit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                Vector3 knockDir = -dir;
                Vector3 targetPos = GameManager.Instance.tileManager.GetLastTilePositionInDirection(player.transform.position, knockDir);
                player.KnockbackTo(targetPos);
            }

            endPos = hit.point;
        }

        DrawLaser(endPos);
        Destroy(gameObject, duration);
    }

    private void DrawLaser(Vector3 end)
    {
        if (lineRenderer == null) return;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, end);
    }
}
