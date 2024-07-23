using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField] float StartWidth;
    [SerializeField] float Height;
    [SerializeField] float HowMuchToAdd;
    [SerializeField] float EndWidth;
    [SerializeField] float Speed;
    [SerializeField] float DeathTime;
    private float CurrentWidth;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentWidth = StartWidth;
        transform.localScale = new Vector3(CurrentWidth, Height, 1);
        Speed *= Time.deltaTime;
        StartCoroutine(IncreaseWidth());
        StartCoroutine(DelayDeath());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator IncreaseWidth()
    {
        if (CurrentWidth < EndWidth)
        {
            yield return new WaitForSeconds(Speed);
            CurrentWidth += HowMuchToAdd;
            transform.localScale = new Vector3(CurrentWidth, Height, 1);
            StartCoroutine(IncreaseWidth());
        }
    }

    IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(DeathTime);
        Destroy(gameObject);
    }    
}
