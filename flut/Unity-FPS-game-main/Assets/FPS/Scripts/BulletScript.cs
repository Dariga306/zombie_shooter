using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Tooltip("Furthest distance bullet will look for target")]
    public float maxDistance = 1000000;

    public GameObject decalHitWall;
    public GameObject bloodEffect;
    public GameObject woodEffect;
    public GameObject barrelEffect;
    public GameObject stoneEffect;
    public GameObject metalEffect;
    public LayerMask ignoreLayer;

    private RaycastHit hit;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            // ✅ 1. Урон зомби
            ZombieHealth zombie = hit.collider.GetComponent<ZombieHealth>();
            if (zombie != null)
            {
                zombie.TakeDamage(1);
            }

            // ✅ 2. Проверка на тип материала (если ObjectHealth есть)
            ObjectHealth objectHealth = hit.collider.GetComponent<ObjectHealth>();
            if (objectHealth != null)
            {
                switch (objectHealth.materialType)
                {
                    case MaterialType.Wood:
                        SpawnDecal(hit, woodEffect);
                        break;
                    case MaterialType.Metal:
                        SpawnDecal(hit, metalEffect);
                        break;
                    case MaterialType.Barrel:
                        if (barrelEffect != null)
                            SpawnDecal(hit, barrelEffect);
                        else
                            Debug.LogWarning("❗ BarrelEffect not assigned in BulletScript.");
                        break;
                    case MaterialType.Skin:
                        SpawnDecal(hit, bloodEffect, true); // для зомби – маленький и быстрый
                        break;
                    case MaterialType.Stone:
                        SpawnDecal(hit, stoneEffect);
                        break;
                    case MaterialType.Wall:
                        SpawnDecal(hit, decalHitWall);
                        break;
                    default:
                        SpawnDecal(hit, decalHitWall);
                        break;
                }
            }
            else
            {
                
                SpawnDecal(hit, decalHitWall);
            }

            Destroy(gameObject);
        }

        Destroy(gameObject, 2f);
    }

    void SpawnDecal(RaycastHit hit, GameObject prefab, bool isZombie = false)
    {
        if (prefab == null) return;

        GameObject spawnedDecal = Instantiate(
            prefab,
            hit.point + hit.normal * 0.01f,
            Quaternion.LookRotation(hit.normal)
        );

        spawnedDecal.transform.SetParent(hit.collider.transform);

        if (isZombie)
        {
            spawnedDecal.transform.localScale *= 0.3f;      // меньше дырка
            Destroy(spawnedDecal, 0.3f);                     // исчезает быстро
        }
        else
        {
            Destroy(spawnedDecal, 2f);                       // обычная длительность
        }
    }
}
