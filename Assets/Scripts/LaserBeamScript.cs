using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserBeamScript : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform outerRight;
    public Transform outerLeft;
    public ParticleSystem laserBeamOuterRight;
    public ParticleSystem laserBeamOuterLeft;

    [Header("Laser Beam Settings")]
    public float maxEnergy;
    public float energyCost;
    public float currentEnergy;

    [Header("Energy Bar Settings")]
    public Image energyBar;
    private float fillRatio;

    private bool canShoot = true;
    private bool isShooting = false;
    private bool isRecharging = false;

    // Start is called before the first frame update
    void Start()
    {
        fillRatio = maxEnergy / energyCost;

        currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaserBeam();

        if(currentEnergy <= 0)
        {
            canShoot = false;
            isShooting = false;
        }

        if (isRecharging)
        {
            energyBar.fillAmount += 1 / fillRatio * Time.deltaTime * 5;


            currentEnergy += maxEnergy / fillRatio * Time.deltaTime * 5;


            if (currentEnergy >= maxEnergy)
            {
                isRecharging = false;
                currentEnergy = maxEnergy;
            }
        }
    }

    void ShootLaserBeam()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && canShoot && currentEnergy > 0)
        {
            isShooting = true;
            isRecharging = false;

            laserBeamOuterRight.Play();
            laserBeamOuterLeft.Play();
        }

        if (Input.GetKey(KeyCode.Mouse1) && canShoot && currentEnergy > 0)
        {
            currentEnergy -= maxEnergy / fillRatio * Time.deltaTime;

            energyBar.fillAmount -= 1 / fillRatio * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) || !canShoot)
        {
            isShooting = false;

            StartCoroutine(RechargeEnergy());

            laserBeamOuterRight.Stop();
            laserBeamOuterRight.Clear();
            laserBeamOuterLeft.Stop();
            laserBeamOuterLeft.Clear();
        }
    }

    IEnumerator RechargeEnergy()
    {
        yield return new WaitForSeconds(2.0f);

        if (!isShooting)
        {
            isRecharging = true;

            if(currentEnergy < 0)
            {
                currentEnergy = 0;
            }
        }
    }
}
