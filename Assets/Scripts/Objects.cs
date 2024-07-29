using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public EnemySpawnInformation ESI;
    public void Death()
    {
        FindObjectOfType<PlayerScript>().IncreaseHealth(ESI.DamageDealt);
        FindObjectOfType<LevelController>().EnemyDeathIndexReset(ESI.LevelIndex, ESI.SpawnIndex);
        FindObjectOfType<AudioManager>().Play("ItemPickup");
        Destroy(gameObject);
    }
}
