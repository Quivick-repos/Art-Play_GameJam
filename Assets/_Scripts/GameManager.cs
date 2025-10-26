using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public enum DebuffType { None, SpeedReduction, Slippery, FlipScreen };
    private enum NoteHitState { Correct, Fail_Bar, Fail_Miss }

    [Header("Game Links")]
    public GameObject team1Player;
    public GameObject team2Player;
    [SerializeField] private Sprite noteSprite;

    public Sprite NoteSprite => noteSprite;

    [Header("Debuff Links")]
    [SerializeField] private GameObject debuffMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Camera mainCamera;

    [Header("Debuff Settings")]
    [SerializeField] private float speedDebuffMultiplier = 0.5f;

    private DebuffType _nextDebuff = DebuffType.None;

    [Header("Guitar Setup")]
    [SerializeField] private GameObject guitarNeckObject;

    [Header("Game Settings")]
    [SerializeField] private float timePerRound = 5f; // 5 seconds per finger
    [SerializeField] private int numRounds = 3;
    [SerializeField] private int maxGames = 3;

    [Header("Visuals")]
    [SerializeField] private Color targetNoteColor = new Color(0.5f, 1f, 0.5f, 0.7f); // Transparent soft green

    [Header("Game State")]
    private int _currentRound = 0;
    private int _team1Score = 0;
    private int _team2Score = 0;
    private float _roundTimer;
    private bool _isRoundActive = false;
    private int _currentGame = 0;

    // [Header("UI Links")]
    // public Text team1ScoreText;
    // public Text team2ScoreText;
    // public Text team1TargetText;
    // public Text team2TargetText;
    // public Text timerText;
    // public GameObject gameOverScreen;

    private GuitarNote _team1TargetNote;
    private GuitarNote _team2TargetNote;

    private List<GuitarNote> _allNotes = new List<GuitarNote>();

    void Awake()
    {
        // Set up Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Find all GuitarNote components in the guitar neck object
        _allNotes.AddRange(guitarNeckObject.GetComponentsInChildren<GuitarNote>());
        Debug.Log($"Found {_allNotes.Count} notes on the guitar neck.");
    }

    void Start()
    {
        debuffMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        // Wait one frame so all GuitarNote.Start() methods can finish
        StartCoroutine(DelayedGameStart());
    }

    private IEnumerator DelayedGameStart()
    {
        // Wait for one frame
        yield return null;

        // Now it's safe to start the game
        StartGame();
    }

    void StartGame()
    {
        _currentGame++;
        _currentRound = 0;
        _team1Score = 0;
        _team2Score = 0;

        debuffMenuPanel.SetActive(false);
        ApplyDebuffs();
        StartNewRound();
    }

    void StartNewRound()
    {
        // 1. Reset the old note colors
        if (_team1TargetNote != null)
        {
            _team1TargetNote.ResetColor();
        }
        if (_team2TargetNote != null)
        {
            _team2TargetNote.ResetColor();
        }

        // 2. Tell the HandControllers to switch to the current finger
        //team1Hand.SwitchToFinger(_currentRound);
        //team2Hand.SwitchToFinger(_currentRound);

        // 3. Pick new notes
        _team1TargetNote = PickRandomNote();
        _team2TargetNote = PickRandomNote(_team1TargetNote); // This version avoids the first note

        // 4. Highlight new notes (using the new color)
        if (_team1TargetNote != null)
        {
            _team1TargetNote.Highlight(targetNoteColor);
        }
        if (_team2TargetNote != null)
        {
            _team2TargetNote.Highlight(targetNoteColor);
        }

        // 5. Update UI
        Debug.Log($"--- ROUND {_currentRound + 1} START! ---");
        Debug.Log($"Team 1 Target: S{_team1TargetNote.StringIndex} F{_team1TargetNote.FretIndex}");
        Debug.Log($"Team 2 Target: S{_team2TargetNote.StringIndex} F{_team2TargetNote.FretIndex}");
        // team1TargetText.text = $"S{_team1TargetNote.stringIndex} F{_team1TargetNote.fretIndex}";
        // team2TargetText.text = $"S{_team2TargetNote.stringIndex} F{_team2TargetNote.fretIndex}";

        _roundTimer = timePerRound;
        _isRoundActive = true;
    }

    void Update()
    {
        if (!_isRoundActive) return;

        _roundTimer -= Time.deltaTime;
        // timerText.text = _roundTimer.ToString("F1");

        if (_roundTimer <= 0)
        {
            EndRound();
        }
    }

    void EndRound()
    {
        _isRoundActive = false;
        Debug.Log("--- ROUND END ---");
        team1Player.GetComponent<Player1Controller>().SelectNextFinger();
        team2Player.GetComponent<Player2Controller>().SelectNextFinger();



        // --- Check Team 1's state and play sound ---
        NoteHitState team1State = CheckWinCondition(_team1TargetNote /* , team1FingerTip */);
        if (team1State == NoteHitState.Correct)
        {
            _team1Score++;
            Debug.Log("Team 1 Scored!");
            AudioManager.Instance.PlayCorrectNote(_team1TargetNote.StringIndex);
        }
        else if (team1State == NoteHitState.Fail_Bar)
        {
            Debug.Log("Team 1 hit a bar!");
            AudioManager.Instance.PlayFailBarSound();
        }
        else // Must be Fail_Miss
        {
            Debug.Log("Team 1 missed!");
            AudioManager.Instance.PlayFailMissSound();
        }

        // --- Check Team 2's state and play sound ---
        NoteHitState team2State = CheckWinCondition(_team2TargetNote /* , team2FingerTip */);
        if (team2State == NoteHitState.Correct)
        {
            _team2Score++;
            Debug.Log("Team 2 Scored!");
            AudioManager.Instance.PlayCorrectNote(_team2TargetNote.StringIndex);
        }
        else if (team2State == NoteHitState.Fail_Bar)
        {
            Debug.Log("Team 2 hit a bar!");
            AudioManager.Instance.PlayFailBarSound();
        }
        else // Must be Fail_Miss
        {
            Debug.Log("Team 2 missed!");
            AudioManager.Instance.PlayFailMissSound();
        }

        // Update score UI
        // team1ScoreText.text = _team1Score.ToString();
        // team2ScoreText.text = _team2Score.ToString();

        // Go to next round
        _currentRound++;
        if (_currentRound >= numRounds)
        {
            EndGame();
        }
        else
        {
            StartNewRound();
        }
    }

    // --- TEMPORARY CheckWinCondition (until you have FingerTip) ---
    NoteHitState CheckWinCondition(GuitarNote target /*, FingerTip tip */)
    {
        // Remove comments when you have the real FingerTip script
        /*
        // Priority #1: Hitting a metal bar is an instant fail
        if (tip.isTouchingFailZone)
        {
            return NoteHitState.Fail_Bar;
        }

        // Priority #2: Hitting the correct note
        if (tip.currentNote == target)
        {
            return NoteHitState.Correct;
        }
        */

        // Priority #3: Anything else is a miss (Always returns this for now)
        return NoteHitState.Fail_Miss;
    }

    void EndGame()
    {
        // --- Reset the final notes ---
        if (_team1TargetNote != null)
        {
            _team1TargetNote.ResetColor();
        }
        if (_team2TargetNote != null)
        {
            _team2TargetNote.ResetColor();
        }

        Debug.Log("--- GAME OVER ---");
        if (_team1Score > _team2Score)
        {
            Debug.Log("Team 1 Wins!");
        }
        else if (_team2Score > _team1Score)
        {
            Debug.Log("Team 2 Wins!");
        }
        else
        {
            Debug.Log("It's a Draw!");
        }

        if (_currentGame < maxGames)
        {
            // We have more games to play, show debuff menu
            _nextDebuff = DebuffType.None;
            debuffMenuPanel.SetActive(true);
            Debug.Log($"Audience, please pick a debuff for Game #{_currentGame + 1}!");
        }
        else
        {
            // This was the final game, show game over screen
            Debug.Log("GAME OVER! Thanks for playing!");
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        debuffMenuPanel.SetActive(false); // Just in case
        gameOverPanel.SetActive(true);
    }

    GuitarNote PickRandomNote()
    {
        if (_allNotes.Count == 0)
        {
            Debug.LogError("No notes found! Did you link the guitarNeckObject?");
            return null;
        }

        // Just pick a random note from the list. Simple!
        int randomIndex = Random.Range(0, _allNotes.Count);
        return _allNotes[randomIndex];
    }

    GuitarNote PickRandomNote(GuitarNote noteToAvoid)
    {
        // If we can't avoid, just return a random one
        if (_allNotes.Count <= 1)
        {
            return PickRandomNote();
        }

        GuitarNote newNote;
        int safetyBreak = 100; // Prevents infinite loops

        do
        {
            newNote = PickRandomNote();
            safetyBreak--;
        }
        // Keep picking a new note *until* it's not the one we want to avoid
        while (newNote == noteToAvoid && safetyBreak > 0);

        return newNote;
    }

    private void ApplyDebuffs()
    {
        // --- COMMENTED OUT until you have the scripts ---

        // --- Reset all debuffs from last game ---
        if (team1Player != null)
        {
            team1Player.GetComponent<Player1Controller>().frogHand.resetMoveSpeed();
        }
        if (team2Player != null)
        {
            team2Player.GetComponent<Player2Controller>().frogHand.resetMoveSpeed();
        }
        //team1Player.GetComponent<Player1Controller>().frogHand.SetSlippery(false);
        //team2Player.GetComponent<Player2Controller>().frogHand.SetSlippery(false);
        mainCamera.transform.rotation = Quaternion.identity;

        // --- Apply the selected debuff ---
        switch (_nextDebuff)
        {
            case DebuffType.SpeedReduction:
                team1Player.GetComponent<Player1Controller>().frogHand.setMoveSpeed(speedDebuffMultiplier);
                team2Player.GetComponent<Player2Controller>().frogHand.setMoveSpeed(speedDebuffMultiplier);
                break;
            //case DebuffType.Slippery:
            //    team1Player.GetComponent<Player1Controller>().frogHand.SetSlippery(true);
            //    team2Player.GetComponent<Player2Controller>().frogHand.SetSlippery(true);
            //    break;
            case DebuffType.FlipScreen:
                mainCamera.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }
    }

    /// <summary>
    /// This is called by the "Quit" or "Main Menu" button on the game over screen.
    /// It loads the Main Menu scene.
    /// </summary>
    public void GoToMainMenu()
    {
        // Make sure "MainMenuScene" exactly matches your scene file name
        SceneManager.LoadScene("MainMenuScene");
    }

    // --- NEW PUBLIC UI FUNCTIONS ---

    /// <summary>
    /// Call this from your debuff buttons (e.g., "Slippery", "Slow").
    /// </summary>
    public void SelectDebuff(int debuffIndex)
    {
        _nextDebuff = (DebuffType)debuffIndex;
        Debug.Log($"Audience selected debuff: {(_nextDebuff)}");
        // You could add a UI element to show "Slippery selected!"
    }

    /// <summary>
    /// Call this from your "Start Next Game" button.
    /// </summary>
    public void StartNextGame()
    {
        // This will hide the panel and call StartGame(),
        // which calls ApplyDebuffs().
        StartGame();
    }
}
