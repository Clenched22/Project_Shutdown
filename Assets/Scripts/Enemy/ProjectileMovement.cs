using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] GameObject RefProjectile;
    [SerializeField] GameObject RefPrefab;
    [SerializeField] float ShotForce;
    [SerializeField] float DestroyTime;
    [SerializeField] string EnemyTag;
    public float Damage;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 shotForce = RefProjectile.transform.up * ShotForce;
        GetComponent<Rigidbody2D>().AddForce(shotForce, ForceMode2D.Impulse);
        StartCoroutine(DestroyDelay());

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                FindObjectOfType<PlayerScript>().DecreaseHealth(Damage);
                Destroy(RefPrefab);
            }
        }
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(RefPrefab);
    }
}
