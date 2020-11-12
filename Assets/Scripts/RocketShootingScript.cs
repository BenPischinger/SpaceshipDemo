using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketShootingScript : MonoBehaviour
{
    [Header("Game Objects")]
    public Camera mainCamera;
    public GameObject rocketProjectileObject;
    public Transform firePointRight;
    public Transform firePointLeft;
   
    [Header("Rocket Settings")]
    public float rocketSpeed;
    public float rocketCD;
    private float nextShot = 0;
    private Vector3 projectileDestination;

    [Header("UI Settings")]
    public Image rocketCDImage;

    private bool canShoot = true;

    // Update is called once per frame
    void Update()
    {
        if (!canShoot)
        {
            rocketCDImage.fillAmount += 1 / rocketCD * Time.deltaTime;
        }

        ShootLaser();
    }

    void ShootLaser()
    {
        if (nextShot < 0)
        {
            nextShot += Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R) && canShoot)
            {

                StartCoroutine(RocketShootingCD());

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

                var projectile1 = Instantiate(rocketProjectileObject, firePointRight.position, transform.rotation) as GameObject;
                projectile1.GetComponent<Rigidbody>().velocity = (projectileDestination - firePointRight.position).normalized * rocketSpeed;

                var projectile2 = Instantiate(rocketProjectileObject, firePointLeft.position, transform.rotation) as GameObject;
                projectile2.GetComponent<Rigidbody>().velocity = (projectileDestination - firePointLeft.position).normalized * rocketSpeed;

            }
        }
    }

    IEnumerator RocketShootingCD()
    {
        canShoot = false;

        nextShot -= rocketCD;

        rocketCDImage.fillAmount = 0;

        yield return new WaitForSeconds(rocketCD);

        canShoot = true;
    }
}
