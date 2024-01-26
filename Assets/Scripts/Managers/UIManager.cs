using UnityEngine;
using TMPro;

namespace B2510.Managers
{
    /// <summary>
    /// Class <c>UIManager</c> contains the logic for the UI.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static UIManager Instance;

        /// <value>Property <c>timerText</c> represents the timer text.</value>
        [SerializeField]
        private TextMeshProUGUI timerText;
        
        /// <value>Property <c>noticeText</c> represents the notice text.</value>
        [SerializeField]
        private TextMeshProUGUI noticeText;
        
        /// <summary>
        /// Method <c>_pauseMenu</c> represents the pause menu.
        /// </summary>
        [SerializeField]
        private GameObject pauseMenu;

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
        /// Method <c>UpdateTimerText</c> updates the timer text.
        /// </summary>
        /// <param name="time">The time to display.</param>
        public void UpdateTimerText(float time)
        {
            timerText.text = Mathf.Max(0, time).ToString("00");
        }
        
        /// <summary>
        /// Method <c>UpdateNoticeText</c> updates the notice text.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public void UpdateNoticeText(string text)
        {
            noticeText.text = text;
        }
        
        /// <summary>
        /// Method <c>EnablePauseMenu</c> enables or disables the pause menu.
        /// </summary>
        public void EnablePauseMenu(bool enable)
        {
            pauseMenu.SetActive(enable);
        }
    }
}
