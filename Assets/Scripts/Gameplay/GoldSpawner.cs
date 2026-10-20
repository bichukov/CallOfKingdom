using UnityEngine;

public class GoldSpawner : MonoBehaviour
{
    [Header("Настройки генерации")]
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private int goldCount = 10;

    [Header("Границы игровой области")]
    [SerializeField] private Vector2 minPosition = new Vector2(-7f, -3.5f);
    [SerializeField] private Vector2 maxPosition = new Vector2(7f, 3.5f);

    private void Start()
    {
        SpawnGold();
    }

    private void SpawnGold()
    {
        if (goldPrefab == null)
        {
            Debug.LogError("Gold Prefab не назначен!");
            return;
        }

        for (int i = 0; i < goldCount; i++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y)
            );

            Instantiate(
                goldPrefab,
                randomPosition,
                Quaternion.identity
            );
        }
    }
}
