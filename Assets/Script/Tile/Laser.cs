using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 15f;
    private Vector2 direction;
    public void Init(Vector2 dir)
    {
        Debug.Log("laser위치" + dir.ToString());
        direction = dir;
        Destroy(gameObject, 2f); // 일정 시간 후 제거
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 knockbackDir = direction;
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(knockbackDir * 3000f, ForceMode2D.Force); // 넉백
            }

            Destroy(gameObject);
        }
    }
}
