using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("Game Links")]
    public HandController team1Hand;
    public HandController team2Hand;

    //public FingerTip team1FingerTip;
    //public FingerTip team2FingerTip;

    [Header("Guitar Setup")]
    [SerializeField] private GameObject guitarNeckObject;

    [Header("Game Settings")]
    [SerializeField] private float timePerRound = 5f; // 5 seconds per finger
    [SerializeField] private int numRounds = 3;

    [Header("Visuals")]
    [SerializeField] private Color targetNoteColor = new Color(0.5f, 1f, 0.5f, 0.7f); // Transparent soft green
    public Sprite noteSprite; // Assign in Inspector

    [Header("Game State")]
    private int _currentRound = 0;
    private int _team1Score = 0;
    private int _team2Score = 0;
    private float _roundTimer;
    private bool _isRoundActive = false;

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
        StartGame();
    }

    void StartGame()
    {
        _currentRound = 0;
        _team1Score = 0;
        _team2Score = 0;
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
        _team2TargetNote = PickRandomNote();

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

        // Check Team 1's "locked" position
        //if (CheckWinCondition(team1FingerTip, _team1TargetNote))
        //{
        //    _team1Score++;
        //    Debug.Log("Team 1 Scored!");
        //}

        // Check Team 2's "locked" position
        //if (CheckWinCondition(team2FingerTip, _team2TargetNote))
        //{
        //    _team2Score++;
        //    Debug.Log("Team 2 Scored!");
        //}

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

    /*bool CheckWinCondition(FingerTip tip, GuitarNote target)
    {
        // Must NOT be touching a fail zone
        if (tip.isTouchingFailZone)
        {
            return false;
        }
        // Must BE touching the correct note
        if (tip.currentNote == target)
        {
            return true;
        }

        return false;
    }*/

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
        // Pause here for debuffs
        // "Restart" button that calls StartGame()
        // gameOverScreen.SetActive(true); check if game is over, change some of these names
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
}
