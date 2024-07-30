using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public EnemySpawnInformation ESI;
    public void Death()
    {
        if (ESI.DamageDealt > 0)
        {
        FindObjectOfType<PlayerScript>().IncreaseHealth(ESI.DamageDealt);
        }
        FindObjectOfType<LevelController>().EnemyDeathIndexReset(ESI.LevelIndex, ESI.SpawnIndex);
        Destroy(gameObject);
    }
}
