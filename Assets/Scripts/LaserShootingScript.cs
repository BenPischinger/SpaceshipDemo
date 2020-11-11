using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShootingScript : MonoBehaviour
{
    [Header ("Game Objects")]
    public Camera mainCamera;
    public GameObject spaceship;
    public GameObject laserProjectileObject;
    public Transform firePointOuterRight;
    public Transform firePointOuterLeft;
    public Transform firePointInnerRight;
    public Transform firePointInnerLeft;

    [Header("Laser Settings")]
    public float laserSpeed;
    public float fireRate;
    private float nextShot = 0;
    private Vector3 projectileDestination;

    private bool canShoot = true;

    // Update is called once per frame
    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        if (nextShot > 0)
        {
            nextShot -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0) && canShoot)
            {
                nextShot += fireRate;

                // Raycast to detect hits

                Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                RaycastHit raycastHit;

                if (Physics.Raycast(ray, out raycastHit))
                {
                    projectileDestination = raycastHit.point;
                }
                else
                {
                    projectileDestination = ray.GetPoint(1000);
                }

                var projectile1 = Instantiate(laserProjectileObject, firePointInnerRight.position, transform.rotation) as GameObject;
                projectile1.GetComponent<Rigidbody>().velocity = (projectileDestination - firePointInnerRight.position).normalized * laserSpeed;

                var projectile2 = Instantiate(laserProjectileObject, firePointInnerLeft.position, transform.rotation) as GameObject;
                projectile2.GetComponent<Rigidbody>().velocity = (projectileDestination - firePointInnerLeft.position).normalized * laserSpeed;

            }
        }
    }
}
