using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    private GameObject currentEnemy;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null) return;

        if (currentEnemy != null)
            Destroy(currentEnemy);

        currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // 🔥 เรียกตอนเวลา Quest หมด
    public void DestroyEnemy()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }
}
