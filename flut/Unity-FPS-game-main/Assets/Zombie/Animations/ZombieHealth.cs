using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int health = 5;
    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        int r = Random.Range(0, 2);
        if (r == 0)
            animator.SetTrigger("DieBack");
        else
            animator.SetTrigger("DieFront");

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        KillCounter.Instance?.AddKill();
    }

    public bool IsDead()
    {
        return isDead;
    }
}
