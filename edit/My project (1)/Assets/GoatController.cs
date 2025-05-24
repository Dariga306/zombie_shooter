using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoatController : MonoBehaviour
{
    [Header("References")]
    public FixedJoystick joystick;
    public Animator animator;
    public TextMeshProUGUI cabbageCounterText;
    public AudioSource audioSource;
    public AudioClip cabbageSound;
    public AudioClip bombSound;
    public AudioClip hoofSound;
    public GameObject winPanel;
    public TextMeshProUGUI winText;
    public string nextSceneName = "NextLevel";

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private int cabbageCount = 0;
    private int totalCabbages = 9; 
    private int totalBombs = 11; 
    private bool isRunning = false;
    private float hoofSoundCooldown = 0.4f;
    private float nextHoofSoundTime = 0f;
    private float volume = 0.2f;

    private int totalObjectsToCollect;  // Total objects (cabbages + bombs)

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 40f;
        rb.linearDamping = 3f;
        rb.angularDamping = 4f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        totalObjectsToCollect = totalCabbages + totalBombs;  // Total number of objects to collect
        UpdateCabbageUI();
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (input.magnitude > 0.1f)
        {
            Vector3 moveDir = new Vector3(input.x, 0, input.y).normalized;
            Vector3 velocity = moveDir * moveSpeed;
            velocity.y = rb.linearVelocity.y;
            rb.linearVelocity = velocity;

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);

            animator.SetBool("isRunning", true);
            isRunning = true;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            animator.SetBool("isRunning", false);
            isRunning = false;
        }

        if (isRunning && Time.time >= nextHoofSoundTime)
        {
            if (audioSource != null && hoofSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(hoofSound, volume);
                nextHoofSoundTime = Time.time + hoofSoundCooldown;
            }
        }

        if (cabbageCount + totalBombs == totalObjectsToCollect)
        {
            WinGame();  // Win the game once all objects are collected
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cabbage"))
        {
            cabbageCount++;
            Destroy(other.gameObject);
            UpdateCabbageUI();

            if (audioSource != null && cabbageSound != null)
            {
                audioSource.PlayOneShot(cabbageSound);
            }
        }
        else if (other.CompareTag("Bomb"))
        {
            cabbageCount = Mathf.Max(0, cabbageCount - 1);  // Bombs decrease cabbage count
            Destroy(other.gameObject);
            UpdateCabbageUI();

            if (audioSource != null && bombSound != null)
            {
                audioSource.PlayOneShot(bombSound);
            }
        }
    }

    void UpdateCabbageUI()
    {
        if (cabbageCounterText != null)
        {
            cabbageCounterText.text = "Cabbages: " + cabbageCount;
        }
    }

    void WinGame()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            winText.text = "You Win!";
        }
    }

    public void OnQuitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");  
    }
}
