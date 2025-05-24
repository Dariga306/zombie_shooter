using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ZombieWalker : MonoBehaviour
{
    public float speed = 0.7f;
    private Rigidbody rb;
    private Animator animator;
    private ZombieHealth zombieHealth;

    private Transform player; // ссылка на игрока

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        zombieHealth = GetComponent<ZombieHealth>();

        player = GameObject.FindGameObjectWithTag("Player").transform; // ищем игрока по тегу
        animator.Play("Z_Walk");
    }

    void FixedUpdate()
    {
        if (zombieHealth != null && zombieHealth.IsDead()) return;

        if (player != null)
        {
            // Движение вперёд
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // чтобы не тянулся вверх/вниз

            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

            // Поворот к игроку
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.fixedDeltaTime));
        }
    }
}
