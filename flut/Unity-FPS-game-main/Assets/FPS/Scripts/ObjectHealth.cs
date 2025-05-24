using UnityEngine;

public enum MaterialType
{
    Wood,
    Metal,
    Barrel,
    Skin,
    Stone,
    Wall,
    Door
}

public class ObjectHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public MaterialType materialType;
    public GameObject smallExplosionEffect;

    public AudioSource woodHitSound;
    public AudioSource metalHitSound;
    public AudioSource characterHitSound;
    public AudioSource destructionSound;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        switch (materialType)
        {
            case MaterialType.Wood:
                if (woodHitSound) woodHitSound.Play();
                break;
            case MaterialType.Metal:
                if (metalHitSound) metalHitSound.Play();
                break;
            case MaterialType.Skin:
                if (characterHitSound) characterHitSound.Play();
                break;
        }

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        if (destructionSound) destructionSound.Play();
        if (smallExplosionEffect) Instantiate(smallExplosionEffect, transform.position, transform.rotation);

        if (materialType == MaterialType.Door)
        {
            Destroy(gameObject);
        }
    }
}
