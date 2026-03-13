using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerHide : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Spawn Points (วางในฉากได้กี่จุดก็ได้)")]
    public Transform[] spawnPoints;

    private GameObject currentEnemy;

    public GameObject SpawnEnemyNearPlayer(Transform player)
    {
        if (enemyPrefab == null)
        {
 
            return null;
        }

        if (player == null)
        {

            return null;
        }

        if (currentEnemy != null)
        {

            return currentEnemy;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {

            return null;
        }

        Transform nearestPoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform point in spawnPoints)
        {
            if (point == null) continue;

            float dist = Vector3.Distance(player.position, point.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearestPoint = point;
            }
        }

        if (nearestPoint == null)
        {

            return null;
        }


        currentEnemy = Instantiate(
            enemyPrefab,
            nearestPoint.position,
            nearestPoint.rotation
        );


        NavMeshAgent agent = currentEnemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(
                currentEnemy.transform.position,
                out hit,
                5f,
                NavMesh.AllAreas))
            {
                currentEnemy.transform.position = hit.position;
            }

            // กัน agent ดันตัวลอย
            agent.baseOffset = 0f;
        }

        Debug.Log("Enemy Spawned At: " + nearestPoint.name);

        return currentEnemy;
    }

    public void ClearEnemy()
    {
        currentEnemy = null;
    }
}
