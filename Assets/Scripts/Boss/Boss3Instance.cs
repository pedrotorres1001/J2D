using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Instance : MonoBehaviour
{
    private Boss3 controller;
    public GameObject projectileSpawnPoint;
    public GameObject vital;
    public bool isActive;
    public bool isPlayerInArea;

    private GameObject fireBall;
    private int fireBallDamage;
    private float fireBallSpeed;

    private GameObject projectile;
    private int projectileDamage;
    private float projectileSpeed;

    [SerializeField] private bool isTeleporting = false;

    public void Active(bool active, bool teleport)
    {
        switch (teleport)
        {
            case true:
                switch (active)
                {
                    case true:
                        isActive = true;
                        isTeleporting = true;
                        gameObject.GetComponent<Animator>().Play("FinalBossTeleportStart", -1, 0f);
                        gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        gameObject.GetComponent<Animator>().enabled = true;
                        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                        vital.SetActive(active);
                        break;
                    case false:
                        isActive = false;
                        isTeleporting = true;
                        gameObject.GetComponent<Animator>().Play("FinalBossTeleportEnd", -1, 0f);
                        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                        vital.SetActive(active);
                        break;

                }
                break;
            case false:
                switch (active)
                {
                    case true:
                        isActive = true;
                        gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        gameObject.GetComponent<Animator>().enabled = true;
                        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                        vital.SetActive(active);
                        break;
                    case false:
                        isActive = false;
                        gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        gameObject.GetComponent<Animator>().enabled = false;
                        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                        vital.SetActive(active);
                        break;

                }
                break;
        }
    }

    public void SetValues(Boss3 cont,GameObject fr, int frdmg, float frspd, GameObject pr, int prdmg, float prspd)
    {
        controller = cont;

        fireBall = fr;
        fireBallDamage = frdmg;
        fireBallSpeed = frspd;

        projectile = pr;
        projectileDamage = prdmg;
        projectileSpeed = prspd;
    }

    public void CreateFireBall()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = (player.transform.position - projectileSpawnPoint.transform.position).normalized;

        GameObject proj = Instantiate(fireBall);
        proj.transform.position = projectileSpawnPoint.transform.position;
        proj.GetComponent<Projectile>().SetValues(dir, fireBallDamage, fireBallSpeed);
    }

    public void CreateProjectile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = (player.transform.position - projectileSpawnPoint.transform.position).normalized;

        GameObject proj = Instantiate(projectile);
        proj.transform.position = projectileSpawnPoint.transform.position;
        proj.GetComponent<Projectile>().SetValues(dir, fireBallDamage, fireBallSpeed);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        proj.transform.Rotate(0, 0, 180);
    }

    public void AnimationEnd()
    {
        if (isTeleporting)
        {
            if (!isActive)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<Animator>().enabled = false;
            }
            isTeleporting = false;
            
        }
        controller.AnimationEnd();
    }
}
