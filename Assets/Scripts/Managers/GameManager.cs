using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using B2510.Entities;
using PolyverseSkiesAsset;

namespace B2510.Managers
{
    /// <summary>
    /// Class <c>GameManager</c> contains the logic for the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static GameManager Instance;
        
        /// <value>Property <c>_uiManager</c> represents the UI manager.</value>
        [SerializeField]
        private UIManager uiManager;
        
        /// <value>Property <c>_polyverseSkies</c> represents the Polyverse skies.</value>
        [SerializeField]
        private PolyverseSkies polyverseSkiesManager;

        /// <value>Property <c>_roundsToWin</c> represents the rounds to win.</value>
        [SerializeField]
        private int roundsToWin = 2;
        
        /// <value>Property <c>_roundNumber</c> represents the round number.</value>
        private int _roundNumber = 1;
        
        /// <value>Property <c>_roundsWon</c> represents the rounds won.</value>
        private readonly Dictionary<Character, int> _roundsWon = new Dictionary<Character, int>();
        
        /// <value>Property <c>_roundTime</c> represents the round time.</value>
        [SerializeField]
        private float roundTime = 60f;
        
        /// <value>Property <c>_roundTimer</c> represents the round timer.</value>
        private float _roundTimer;
        
        /// <value>Property <c>_characters</c> represents the characters.</value>
        private Character[] _characters;
        
        /// <value>Property <c>_roundWinner</c> represents the round winner.</value>
        private Character _roundWinner;
        
        /// <value>Property <c>_gameWinner</c> represents the game winner.</value>
        private Character _gameWinner;

        /// <value>Property <c>_roundActive</c> represents if the round is active.</value>
        private bool _roundActive;
        
        /// <value>Property <c>timeOfDay</c> represents the time of day.</value>
        [SerializeField, Range(0, 1)]
        private int timeOfDay = 1;
        
        /// <value>Property <c>_audioSource</c> represents the audio source.</value>
        private AudioSource _audioSource;
        
        /// <value>Property <c>AudioClips</c> represents a dictionary containing all sounds and music for this object.</value>
        public Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            // Get the components
            _audioSource = GetComponent<AudioSource>();
            
            // Get the other managers
            uiManager = (uiManager == null) ? FindObjectOfType<UIManager>() : uiManager;
            polyverseSkiesManager = (polyverseSkiesManager == null) ? FindObjectOfType<PolyverseSkies>() : polyverseSkiesManager;
            
            // Get the audio clips
            var audioClips = Resources.LoadAll<AudioClip>("Sounds/Game");
            foreach (var audioClip in audioClips)
            {
                AudioClips.Add(audioClip.name, audioClip);
            }
        }
        
        /// <summary>
        /// Method <c>Start</c> is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            // Get the characters on screen and add them to the rounds won dictionary
            _characters = FindObjectsOfType<Character>();
            foreach (var character in _characters)
            {
                _roundsWon.Add(character, 0);
            }
            
            // Ensure the timescale is set to 1
            Time.timeScale = 1;
            
            // Start the game loop
            StartCoroutine(GameLoop());
        }
        
        /// <summary>
        /// Method <c>FixedUpdate</c> is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void FixedUpdate()
        {
            // Update the timer
            if (!_roundActive)
                return;
            _roundTimer -= Time.deltaTime;
            uiManager.UpdateTimerText(_roundTimer);
            
            // Update the polyverse skies time of day (in the beginning of the round, the time of day is 1, at the end is 0)
            var timeRatio = _roundTimer / roundTime;
            polyverseSkiesManager.timeOfDay = (timeOfDay == 0) ? 1f - timeRatio : timeRatio;
        }
        
        /// <summary>
        /// Coroutine <c>GameLoop</c> is the main game loop.
        /// </summary>
        private IEnumerator GameLoop()
        {
            // Wait for the round to start
            yield return StartCoroutine(RoundStarting());
            
            // Wait for the round to end
            yield return StartCoroutine(RoundPlaying());
            
            // Wait for the round to end
            yield return StartCoroutine(RoundEnding());
        }
        
        /// <summary>
        /// Coroutine <c>RoundStarting</c> is called while the round is starting.
        /// </summary>
        private IEnumerator RoundStarting()
        {
            // Reset the round winner
            _roundWinner = null;
            
            // Reset the round timer
            _roundTimer = roundTime;
            
            // Reset the round timer text
            uiManager.UpdateTimerText(_roundTimer);
            
            // Reset the time of day
            polyverseSkiesManager.timeOfDay = timeOfDay;
            
            // Reset the characters
            foreach (var character in _characters)
            {
                character.Reset();
            }
            
            // Display the round number in the notice text
            uiManager.UpdateNoticeText($"Round {_roundNumber}");
            
            // Play the round start sound
            _audioSource.PlayOneShot(_roundNumber is >= 1 and <= 3
                ? AudioClips["round" + _roundNumber]
                : AudioClips["roundnext"]);

            // Wait
            yield return new WaitForSeconds(2f);
            
            // Display the fight text
            uiManager.UpdateNoticeText("Fight!");
            
            // Play the fight sound
            _audioSource.PlayOneShot(AudioClips["fight"]);
            
            // Wait
            yield return new WaitForSeconds(1f);
            
            // Reset the notice text
            uiManager.UpdateNoticeText("");

            // Activate the characters
            foreach (var character in _characters)
            {
                character.EnableMovement(true);
                character.ChangeState(CharacterProperties.States.IdleMove);
            }
        }
        
        /// <summary>
        /// Coroutine <c>RoundPlaying</c> is called while the round is playing.
        /// </summary>
        private IEnumerator RoundPlaying()
        {
            // Activate the round
            _roundActive = true;

            // While there is no winner
            while (!CheckRoundWinner() && !CheckRoundTime())
            {
                // Wait for the next frame
                yield return null;
            }
        }
        
        /// <summary>
        /// Method <c>RoundEnding</c> is called while the round is ending.
        /// </summary>
        private IEnumerator RoundEnding()
        {
            // Set the round as inactive
            _roundActive = false;

            // Prevent the characters from moving
            foreach (var character in _characters)
            {
                character.EnableMovement(false);
            }
            
            // Display the round winner (if null, it's a draw)
            uiManager.UpdateNoticeText(_roundWinner == null ? "Draw" : $"{_roundWinner.characterName} wins the round");
            
            // Play the round end sound
            _audioSource.PlayOneShot(_roundWinner == null ? AudioClips["draw"] : AudioClips["win"]);
            
            // Wait
            yield return new WaitForSeconds(2f);

            // Check if there is a game winner
            if (CheckGameWinner())
            {
                EndGame();
                yield break;
            }

            // Increment the round number
            _roundNumber++;
            
            // Start the next round
            StartCoroutine(GameLoop());
        }
        
        /// <summary>
        /// Method <c>CheckRoundTime</c> checks if the round time is over.
        /// </summary>
        private bool CheckRoundTime()
        {
            return _roundTimer <= 0f;
        }
        
        /// <summary>
        /// Method <c>CheckRoundWinner</c> checks if there is a round winner.
        /// </summary>
        private bool CheckRoundWinner()
        {
            // Filter the characters by the Dead state
            var aliveCharacters = System.Array.FindAll(_characters, character => character.characterState != CharacterProperties.States.Dead);
            
            // Check if there is only one character alive
            if (aliveCharacters.Length > 1)
                return false;
            _roundWinner = aliveCharacters[0];
            
            // Increment the rounds won
            _roundsWon[_roundWinner]++;
            
            // Update the UI
            _roundWinner.winsText.text = $"Wins: {_roundsWon[_roundWinner]}";

            return true;
        }
        
        /// <summary>
        /// Method <c>CheckGameWinner</c> checks if there is a game winner.
        /// </summary>
        private bool CheckGameWinner()
        {
            // Filter the characters by the rounds won
            var winners = _characters.Where(character => _roundsWon[character] >= roundsToWin).ToList();

            // Check if there is a game winner
            if (winners.Count == 0)
                return false;
            _gameWinner = winners[0];

            return true;
        }
        
        /// <summary>
        /// Method <c>EndGame</c> ends the game.
        /// </summary>
        private void EndGame()
        {
            // Display the game winner
            uiManager.UpdateNoticeText($"{_gameWinner.characterName} wins!");
            
            // Play the game end sound
            _audioSource.PlayOneShot(AudioClips["gameover"]);
            
            // Show the pause menu
            uiManager.EnablePauseMenu(true);
        }

        /// <summary>
        /// Method <c>OnPause</c> is called when the pause button is pressed.
        /// </summary>
        private void OnPause()
        {
            // Check if the game can be paused
            if (_roundActive)
                TogglePause();
        }
        
        /// <summary>
        /// Method <c>TooglePause</c> toggles the pause.
        /// </summary>
        private void TogglePause()
        {
            var gamePaused = Time.deltaTime == 0;

            // Toggle the pause
            Time.timeScale = gamePaused ? 1 : 0;
            
            // Toggle the movement
            foreach (var character in _characters)
            {
                character.EnableMovement(gamePaused);
            }
            
            // Toggle the pause menu
            uiManager.EnablePauseMenu(!gamePaused);
            
            // Show the notice text
            uiManager.UpdateNoticeText(gamePaused ? "" : "Pause");
        }

        /// <summary>
        /// Method <c>RestartGame</c> restarts the game.
        /// </summary>
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        /// <summary>
        /// Method <c>LoadMainMenu</c> loads the main menu.
        /// </summary>
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// Method <c>QuitGame</c> is used to quit the game.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
