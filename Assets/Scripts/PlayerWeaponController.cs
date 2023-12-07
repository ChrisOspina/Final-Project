using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    AudioSource src;
    public float attackDist = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    public AudioClip swingEffect;
    public AudioClip hitSound;
    public GameObject Sword;

    bool attacking = false;
    bool readytoAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!readytoAttack || attacking) return;

        readytoAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        StartCoroutine(SwordSwing());
        src.PlayOneShot(swingEffect);
    }

    void ResetAttack()
    {
        attacking = false;
        readytoAttack = true;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attackDist, attackLayer))
        {
            HitTarget(hit.point);
        }
    }
    void HitTarget(Vector3 pos)
    {
        src.PlayOneShot(hitSound);

        GameObject hitObject = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(hitObject, 20);

        if (hitObject.CompareTag("AI"))
        {
            Destroy(hitObject);
        }
    }

    IEnumerator SwordSwing()
    {
        Sword.GetComponent<Animator>().Play("SwordSwing");
        yield return new WaitForSeconds(1.0f);
        Sword.GetComponent<Animator>().Play("Idle");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            Destroy(other);
        }
    }
}
