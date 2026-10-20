using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    [Header("Настройки испытания")]
    [SerializeField] private int targetGold = 10;
    [SerializeField] private float timeLimit = 30f;

    [Header("Интерфейс")]
    [SerializeField] private TMP_Text goldCounterText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private GameObject resultPanel;

    private int _collectedGold;
    private float _timeRemaining;
    private bool _isFinished;

    public int CollectedGold => _collectedGold;
    public bool IsFinished => _isFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _collectedGold = 0;
        _timeRemaining = timeLimit;
        _isFinished = false;

        if (statusText != null)
        {
            statusText.text = string.Empty;
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        UpdateGoldCounter();
        UpdateTimer();
    }

    private void Update()
    {
        if (_isFinished)
        {
            return;
        }

        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0f)
        {
            _timeRemaining = 0f;
            UpdateTimer();
            FinishChallenge(false);
            return;
        }

        UpdateTimer();
    }

    public void CollectGold()
    {
        if (_isFinished)
        {
            return;
        }

        _collectedGold++;

        UpdateGoldCounter();

        if (_collectedGold >= targetGold)
        {
            FinishChallenge(true);
        }
    }

    private void UpdateGoldCounter()
    {
        if (goldCounterText != null)
        {
            goldCounterText.text =
                $"Золото: {_collectedGold} / {targetGold}";
        }
    }

    private void UpdateTimer()
    {
        if (timerText == null)
        {
            return;
        }

        int seconds = Mathf.CeilToInt(_timeRemaining);
        timerText.text = $"Время: 00:{seconds:00}";
    }

    private void FinishChallenge(bool success)
    {
        if (_isFinished)
        {
            return;
        }

        _isFinished = true;

        if (statusText != null)
        {
            statusText.text = success
                ? "ИСПЫТАНИЕ ПРОЙДЕНО!"
                : "ВРЕМЯ ВЫШЛО!";
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        PlayerMovement playerMovement =
            FindFirstObjectByType<PlayerMovement>();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        Debug.Log(
            success
                ? $"Испытание пройдено! Собрано золота: {_collectedGold}"
                : $"Время вышло. Собрано золота: {_collectedGold}"
        );
    }

    public void RetryChallenge()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
