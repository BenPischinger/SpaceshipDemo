
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionScript : MonoBehaviour
{
    public GameObject projectile;
    public GameObject impactVFX;
    public ParticleSystem trail;

    private bool wasDestroyed;

    private bool collided;

    private void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("SpaceshipPivot").GetComponentInChildren<Collider>());
    }
    private void FixedUpdate()
    {
        if (!wasDestroyed)
        {
            StartCoroutine(DestroyMissedProjectile());
        }
    }

    // Checks if the projectile collided with anything other than the player or a projectile/itself
    // Instantiates the impact VFX if it hit something
    // Trail stopped manually and VFX destroyed after 2 seconds to give the trail time to catch up with the projectile
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }

        if (collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Player" && !collided)
        {
            collided = true;

            if (impactVFX)
            {
                var impact = Instantiate(impactVFX, collision.contacts[0].point, Quaternion.identity.normalized) as GameObject;
                
                Destroy(impact, 2);
            }

            Destroy(projectile);

            if (trail)
            {
                trail.Stop();
            }

            Destroy(gameObject, 2);

            wasDestroyed = true;

        }
    }

    IEnumerator DestroyMissedProjectile()
    {
        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
    }
}