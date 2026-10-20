using UnityEngine;

public class Gold : MonoBehaviour
{
    private bool _isCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCollected)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (GameplayManager.Instance == null)
        {
            Debug.LogError(
                "GameplayManager не найден на сцене Gameplay."
            );

            return;
        }

        _isCollected = true;

        GameplayManager.Instance.CollectGold();

        Destroy(gameObject);
    }
}