using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace B2510.Managers
{
    /// <summary>
    /// Class <c>OpeningManager</c> contains the methods and properties needed for the opening sequence.
    /// </summary>
    public class OpeningManager : MonoBehaviour
    {
        /// <value>Property <c>companyLogo</c> represents the UI element containing the company logo.</value>
        public TextMeshProUGUI companyLogo;
        
        /// <value>Property <c>companyMotto</c> represents the UI element containing the company motto.</value>
        public TextMeshProUGUI companyMotto;

        /// <summary>
        /// Method <c>Start</c> is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private IEnumerator Start()
        {
            companyLogo.canvasRenderer.SetAlpha(0.0f);
            companyMotto.canvasRenderer.SetAlpha(0.0f);
            companyLogo.CrossFadeAlpha(1.0f, 1.5f, false);
            companyMotto.CrossFadeAlpha(1.0f, 1.5f, false);
            yield return new WaitForSeconds(2.5f);
            companyLogo.CrossFadeAlpha(0.0f, 1.5f, false);
            companyMotto.CrossFadeAlpha(0.0f, 1.5f, false);
            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene("MainMenu");
        }
    }
}