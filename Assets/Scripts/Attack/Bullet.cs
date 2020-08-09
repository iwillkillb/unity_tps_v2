using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    public GameObject shooter;
    public float damage = 10f;
    public float speed = 10f;
    public float lifeTime = 10f;
    float curLifeTime = 0f;
    public LayerMask blockLayers;

    public void OnObjectSpawn()
    {

    }

    void EndOfLifeTime()
    {
        // Lifetime reset
        curLifeTime = 0f;
        gameObject.SetActive(false);
        //magazine.ReturnToMagazine(this);
    }

    void FixedUpdate()
    {
        // Life time
        if (curLifeTime < lifeTime)
            curLifeTime += Time.deltaTime;
        else
            EndOfLifeTime();

        // Moving
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Delete myself
        if (((1 << collision.gameObject.layer) & blockLayers) != 0)
        {
            EndOfLifeTime();
        }
    } 

    private void OnTriggerEnter(Collider other)
    {
        // Damage
        CharacterStats characterStats = other.GetComponent<CharacterStats>();
        if (characterStats != null && other.gameObject != shooter)
        {
            characterStats.TakeDamage(damage);
            // Delete myself
            EndOfLifeTime();
        }

        // Delete myself
        if (((1 << other.gameObject.layer) & blockLayers) != 0)
        {
            EndOfLifeTime();
        }
    }
}
