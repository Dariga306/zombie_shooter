using UnityEngine;

public class GoatMovement : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;   //Объявление переменных

    void Awake()  //инициализация переменных
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        if (audioSource != null)
            audioSource.enabled = true;  
    }

    public void StartWalking()  //белый козел
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.enabled = true;
            audioSource.Play();
        }
    }

    public void StartRunning()  //черный козел
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.enabled = true; 
            audioSource.Play();
        }
    }

    public void StopMoving()  //стоп
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        if (audioSource != null)
            audioSource.Stop();
    }
}
