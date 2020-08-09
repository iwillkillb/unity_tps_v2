using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public string bulletTag = "Bullet";
    public float ShooterSlerp = 5f;

    public float attackSpeed = 1f;
    public float attackDelay = 0.5f;
    private float attackCooldown = 0f;

    public event System.Action onAttack;

    void FixedUpdate()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        // Shot
        if (Input.GetMouseButtonDown(0))
        {
            Shot();
        }

        // Rotation lerp
        Aim();
    }


    void Aim()
    {
        Quaternion newRot;

        //Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray rayMouse = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f, 0f));
        RaycastHit hitMouse;

        if (Physics.Raycast(rayMouse, out hitMouse))
        {
            newRot = Quaternion.LookRotation(hitMouse.point - bulletSpawnPoint.position);
        }
        else
        {
            newRot = Quaternion.LookRotation(rayMouse.direction);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * ShooterSlerp);
    }


    void Shot()
    {
        // Delay reset
        if (attackCooldown > 0f)
        {
            return;
        }
        attackCooldown = 1f / attackSpeed;

        // CallBullet
        ObjectPool.instance.Call(bulletTag, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Event
        if (onAttack != null)
        {
            onAttack();
        }
    }


    /*
    public void Attack(CharacterStats targetStats)
    {
        if (attackCooldown > 0f)
        {
            return;
        }
        StartCoroutine(DoDamage(targetStats, attackDelay));
        
        if (onAttack != null)
        {
            onAttack();
        }

        attackCooldown = 1f / attackSpeed;
    }

    IEnumerator DoDamage(CharacterStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);

        stats.TakeDamage(myStats.damage.GetValue());
    }*/
}
