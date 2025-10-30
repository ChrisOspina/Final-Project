using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 20f;
    public float launchDelay = 1f;
    public float projectileLifetime = 5f;

    AudioSource src;
    public AudioClip launchsound;

    private void Start()
    {
        src = GetComponent<AudioSource>();
        StartCoroutine(Launch());
    }

    IEnumerator Launch()
    {
        while (true)
        {
            GameObject ball = Instantiate(projectile, transform.position, transform.rotation);

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null) 
            {
                rb.AddForce(transform.forward * launchVelocity, ForceMode.VelocityChange);
            }

            //ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));
            src.PlayOneShot(launchsound);

            StartCoroutine(DestroyAfterDelay(ball, projectileLifetime));

            yield return new WaitForSeconds(launchDelay);

        }

    }
    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if(obj!= null)
            Destroy(obj);
    }
}
