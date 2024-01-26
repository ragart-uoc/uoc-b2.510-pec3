using UnityEngine;
using UnityEngine.SceneManagement;

namespace B2510.Managers
{
    /// <summary>
    /// Class <c>MainMenuManager</c> contains the logic for the main menu.
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static MainMenuManager Instance;

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
        }

        /// <summary>
        /// Method <c>StartGame</c> starts the game.
        /// </summary>
        public void StartGame()
        {
            SceneManager.LoadScene("Level1");
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
