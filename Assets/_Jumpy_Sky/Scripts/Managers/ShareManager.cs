using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CBGames
{
    public class ShareManager : MonoBehaviour
    {
        [SerializeField] private string Gizlilik = "https://higherprivacypolicy.blogspot.com/2021/06/privacy-policy-developercompany-name.html?m=1";

        [Header("Native Sharing Config")]
        [SerializeField] private string shareText = "Can you beat my score!!!";
        [SerializeField] private string shareSubject = "Share With";
        [SerializeField] private string appUrl = "";


        [Header("Twitter Sharing Config")]
        [SerializeField] private string titterAddress = "http://twitter.com/intent/tweet";
        [SerializeField] private string textToDisplay = "Hey Guys! Check out my score: ";
        [SerializeField] private string tweetLanguage = "en";

        [Header("Facebook Sharing Config")]
        [SerializeField] private string fbAppID = "1013093142200006";
        [SerializeField] private string caption = "Check out My New Score: ";
        [Tooltip("The URL of a picture attached to this post.The Size must be atleat 200px by 200px.If you dont want to share picture, leave this field empty.")]
        [SerializeField] private string pictureUrl = "http://i-cdn.phonearena.com/images/article/85835-thumb/Google-Pixel-3-codenamed-Bison-to-be-powered-by-Andromeda-OS.jpg";
        [SerializeField] private string description = "Enjoy Fun, free games! Challenge yourself or share with friends. Fun and easy to use games.";

        public string gizlilik { get { return Gizlilik; } }

        public string AppUrl { get { return appUrl; } }
        private Texture2D screenshot2D = null;

        /// <summary>
        /// Create the screenshot
        /// </summary>
        public void CreateScreenshot()
        {
            StartCoroutine(CRTakeScreenshot());
        }
        private IEnumerator CRTakeScreenshot()
        {
            if (screenshot2D != null)
            {
                Destroy(screenshot2D);
            }
            yield return new WaitForEndOfFrame();

            screenshot2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenshot2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot2D.Apply();
        }


        /// <summary>
        /// Share screenshot with text
        /// </summary>
        public void NativeShare()
        {
            new NativeShare().AddFile(screenshot2D).SetSubject(shareSubject).SetText(shareText + " " + AppUrl).Share();
        }


        /// <summary>
        /// Share on titter page
        /// </summary>
        public void TwitterShare()
        {
            Application.OpenURL(titterAddress + "?text=" + UnityWebRequest.EscapeURL(textToDisplay) + "&amp;lang=" + UnityWebRequest.EscapeURL(tweetLanguage));
        }


        /// <summary>
        /// Share on facbook page
        /// </summary>
        public void FacebookShare()
        {
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                Application.OpenURL("https://www.facebook.com/dialog/feed?" + "app_id=" + fbAppID + "&link=" + appUrl + "&picture=" + pictureUrl
                             + "&caption=" + caption + "&description=" + description);
            }
            else
            {
                Application.OpenURL("https://www.facebook.com/dialog/feed?" + "app_id=" + fbAppID + "&link=" + appUrl + "&caption=" + caption + "&description=" + description);
            }
        }
    }

}