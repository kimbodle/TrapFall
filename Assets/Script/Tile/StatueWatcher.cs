using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueWatcher : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firePoint;
    private Vector2 shootDir = Vector2.down;

    public void SetDirection(Vector2 dir)
    {
        shootDir = dir.normalized;

        // 🔄 firePoint의 로컬 회전만 조정 (부모인 석상은 회전 안 함)
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        firePoint.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnEnable()
    {
        StartCoroutine(FireLaserAfterDelay(3f));
    }

    IEnumerator FireLaserAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        FireLaser();
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void FireLaser()
    {
        GameObject laser = Instantiate(
            laserPrefab,
            firePoint.position,
            Quaternion.identity
        );
        laser.GetComponent<Laser>().Init(shootDir);
    }
}
