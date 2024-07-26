using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInformation 
{
    public int LevelIndex;
    public int SpawnIndex;
    public GameObject EnemyPrefab;
    public Vector3 SpawnPosition;
    public Quaternion SpawnRotation;
    public bool Death;
    public float Health;
    public float DamageDealt;
    public float MaxHealth;
}
