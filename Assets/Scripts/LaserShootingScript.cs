using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class LaserShootingScript : MonoBehaviour
{
    [Header ("GameObjects")]
    public Camera mainCamera;
    public GameObject laserProjectileObject;
    public Transform firePointInnerRight;
    public Transform firePointInnerLeft;

    [Header("Laser Settings")]
    public float laserSpeed;
    public float fireRate;
    public float maxHeat;
    public float heatCost;
    public float currentHeat;
    private float nextShot = 0;
    private Vector3 projectileDestination;

    [Header("Heatbar Settings")]
    public Image heatBar;
    private float fillRatio;

    private bool canShoot = true;
    private bool isShooting = false;
    private bool isCoolingDown = false;

    void Start()
    {
        fillRatio = maxHeat / heatCost;
    }

    // Update is called once per frame
    void Update()
    {
        
        ShootLaser();

        if (isCoolingDown)
        { 
            heatBar.fillAmount -= 1 / fillRatio * Time.deltaTime * 5;

           
            currentHeat -= maxHeat / fillRatio * Time.deltaTime * 5;
            

            if(currentHeat <= 0)
            {
                isCoolingDown = false;
                currentHeat = 0;
            }
        }
    }


    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isShooting = true;

            isCoolingDown = false;
        }
       
        if (nextShot > 0)
        {
            nextShot -= Time.deltaTime;
        }
        else
        {
           
            if (Input.GetKey(KeyCode.Mouse0) && canShoot && currentHeat < maxHeat)
            {
 
                currentHeat += heatCost;

                heatBar.fillAmount += 1 / fillRatio;

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

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isShooting = false;

            StartCoroutine(HeatCooldown());
        }
    }

    IEnumerator HeatCooldown()
    {

        yield return new WaitForSeconds(2.0f);

        if (!isShooting)
        {
            isCoolingDown = true;

            if(currentHeat > maxHeat)
            {
                currentHeat = maxHeat;
            }
        }
             
    }
}
