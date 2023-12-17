using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 700f;
    public float launchDelay = 1f;

    AudioSource src;
    public AudioClip launchsound;

    // Start is called before the first frame update
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
            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));
            src.PlayOneShot(launchsound);

            yield return new WaitForSeconds(launchDelay);

            StartCoroutine(DestroyAfterDelay(ball, 1f));
        }

    }
    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
}
