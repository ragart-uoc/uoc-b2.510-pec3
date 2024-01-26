using UnityEngine;

namespace B2510.Managers
{
    /// <summary>
    /// Class <c>UIManager</c> contains the logic for the UI.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        public static UIManager Instance;
        
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
        /// Method <c>EnablePauseMenu</c> enables or disables the pause menu.
        /// </summary>
        public void EnablePauseMenu(bool enable)
        {
            pauseMenu.SetActive(enable);
        }
    }
}
