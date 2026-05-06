using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private RoomGate exitGate;
    [SerializeField] private bool offerBoonOnClear = true;

    private int aliveCount;
    private bool activated;
    private bool cleared;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        if (other.GetComponent<Player>() == null) return;

        ActivateRoom();
    }

    private void ActivateRoom()
    {
        activated = true;
        aliveCount = 0;

        if (exitGate != null)
            exitGate.Close();

        foreach (GameObject enemyObj in enemies)
        {
            if (enemyObj == null) continue;

            enemyObj.SetActive(true);

            EnemyStats stats = enemyObj.GetComponent<EnemyStats>();
            if (stats != null)
            {
                stats.SetRoom(this);
                aliveCount++;
            }
        }

        // Room has no enemies — clear immediately
        if (aliveCount == 0)
            ClearRoom();
    }

    public void OnEnemyDied()
    {
        aliveCount--;

        if (aliveCount <= 0)
            ClearRoom();
    }

    private void ClearRoom()
    {
        if (cleared) return;
        cleared = true;

        if (exitGate != null)
            exitGate.Open();

        if (offerBoonOnClear && BoonManager.instance != null)
            BoonManager.instance.OfferBoons();

        if (RunManager.instance != null)
            RunManager.instance.roomsCleared++;
    }
}
