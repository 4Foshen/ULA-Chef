using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LevelState
{
    Intro,
    Playing,
    Paused,
    Defeat, 
    Victory
}

public class VictoryCard
{
    [Tooltip("The refference to base script")] public SpawningManager spawningManager;
    [Tooltip("The original card that the object derived from")] public GameObject originalCardInstance;
    [Tooltip("The derived object")] public GameObject cardInstance;
    [Tooltip("The object where all of the card's elements are located")] public GameObject cardContainer;
    [Tooltip("The world position of a card")] public Vector3 pos;
    [Tooltip("Was a card already set?")] public bool isEmpty;

    /// <summary>
    /// Assigns a card as selected.
    /// </summary>
    public void Select()
    {
        // Method used to assign a card.
        //InteractionManager.instance.SelectCard(this);
        
        // Making a card half transparent.
        List<Image> images = new List<Image>(cardContainer.GetComponentsInChildren<Image>());

        foreach(Image image in images)
        {
            Color newColor = image.color;
            newColor.a = 0.5f;
            image.color = newColor;
        }
    }
}

public class LevelManager : MonoBehaviour
{

    #region BASIC FIELDS

        [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("BASIC VARIABLES")]
            
            [Space(10)] [Header("Basic Variables")]
            [Tooltip("The time before the timer starts")] public int introLength;
            [Tooltip("The additional time")] public int bufferTime;
            [Tooltip("The duration of frozen time after unpausing")] public int pausedLength;
            [Tooltip("The maximum allowed amount of time")] public int maxTime;
            [Tooltip("The current time")] public int currentTime;
            public bool isPaused;
            public int levelIndex;
            public Slider timeSlider;
            public LevelState currentLevelState;
            public static LevelManager instance;
            public GameObject pausedScreen;
            public GameObject victoryScreen;
            public GameObject defeatScreen;

            [Space(10)] [Header("Victory")]
            public GameObject[] victoryCards;
            public Vector3[] pos;
            public GameObject victoryCardContainer;
            public GameObject victoryCardParent;
            public Sprite[] victoryCardIcons;
            public GameObject victoryDarkBackground;
            public GameObject victoryWindow;
            public GameObject levelsObject;
            public GameObject timerObject;
            public GameObject buttonsObject;


    public bool isClicked;
    public bool isWaiting = true;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            LeanTween.init(800); 
            LeanTween.cancelAll();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        void Start()
        {
            currentLevelState = LevelState.Playing;

            pausedScreen.SetActive(false);
            victoryScreen.SetActive(false);
            defeatScreen.SetActive(false);
            
            ResetTimer();
            Invoke("StartTimer", introLength);
            
            victoryDarkBackground.SetActive(false);
            levelsObject.GetComponent<RectTransform>().localPosition = new Vector3 (-40f, -700f, 0f);
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        void Update()
        {
            if (currentTime <= 0) Defeat();
        }

        /// <summary>
        /// Called at fixed intervals, ideal for physics updates.
        /// Use this for physics-related updates like applying forces or handling Rigidbody physics.
        /// </summary>
        void FixedUpdate()
        {
            // Add physics-related logic here.
            // Example: Rigidbody movement, applying forces, or collision detection.
        }

    #endregion

    #region CUSTOM METHODS

        /// <summary>
        /// A method used to restart the timer.
        /// </summary>
        public void ResetTimer()
        {
            maxTime = SpawningManager.instance.x * SpawningManager.instance.y + bufferTime;
            currentTime = maxTime;
            timeSlider.value = 1f;
            isPaused = false;   
        }
    
        /// <summary>
        /// A method used to call the timer.
        /// </summary>
        void StartTimer()
        {
            StartCoroutine(Timer());   
        }

        /// <summary>
        /// Timer system.
        /// </summary>
        IEnumerator Timer()
        {
            while ((currentTime > 0) && (currentLevelState == LevelState.Playing))
            {
                currentTime--;

                timeSlider.value = (float)(currentTime) / (float)(maxTime);

                yield return new WaitForSeconds(1f);    
            }
        }        
        
        /// <summary>
        /// Method used to show victory screen.
        /// Triggered when the user matches all cards.
        /// </summary>
        [ContextMenu("Victory")]
        public void Victory()
        {
            StartCoroutine(VictoryCoroutine());
        }

        IEnumerator VictoryCoroutine()
        {
            currentLevelState = LevelState.Victory;      
            victoryCards = new GameObject[5];
            pos = new Vector3[5];
            
            yield return new WaitForSeconds(1f);
  
            victoryDarkBackground.SetActive(true);    
            StartCoroutine(LerpUIPosition(timerObject.GetComponent<RectTransform>(), new Vector3 (-200f, timerObject.GetComponent<RectTransform>().localPosition.y, 0f), 1.4f));
            StartCoroutine(LerpUIPosition(buttonsObject.GetComponent<RectTransform>(), new Vector3 (buttonsObject.GetComponent<RectTransform>().localPosition.x, -200f, 0f), 1.4f));
            
            MyGame.Utils.ShuffleArray(victoryCardIcons);

            for (int i = 0; i < victoryCards.Length; i++)
            {
                victoryCards[i] = Instantiate(victoryCardContainer);
                victoryCards[i].transform.SetParent(victoryCardParent.transform);
                victoryCards[i].name = victoryCardIcons[i].ToString();

                RectTransform rectTransform = victoryCards[i].GetComponent<RectTransform>();
                rectTransform.transform.position = new Vector3 ((i + 0.5f) * 4 - 10, 0f, 0f);
                rectTransform.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
            
                foreach (Transform child in victoryCards[i].transform)
                {
                    if (child.gameObject.name == "Front - Image")
                    {
                        child.gameObject.GetComponent<Button>().interactable = false;
                        child.gameObject.SetActive(false);
                    }
                    else if (child.gameObject.name == "Icon - Button")
                    {
                        Image imageA = child.gameObject.GetComponent<Image>();
                        imageA.sprite = victoryCardIcons[i];
                    }
                    else if (child.gameObject.name == "Link - TMP")
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                pos[i] = rectTransform.localPosition;
            }
            
            victoryWindow.GetComponent<RectTransform>().localPosition = new Vector3 (0f, -1080f, 0f);
            StartCoroutine(LerpUIPosition(victoryWindow.GetComponent<RectTransform>(), Vector3.zero, 1.2f));

            yield return new WaitForSeconds(1.3f);

            for (int i = 0; i < victoryCards.Length; i++)
            {
                StartFrontImage(victoryCards[i], true, 0.5f);
            }   

            yield return new WaitForSeconds(1.3f);
            
            Vector3[] posCopy = pos.ToArray(); // Creates a new array with the same values
            bool isDifferent = false;
            while (!isDifferent)
            {
                MyGame.Utils.ShuffleArray(pos);

                // Compare arrays element by element to detect differences
                int counter = 0;
                for(int i = 0; i < posCopy.Length; i++)
                {
                    if (posCopy[i] == pos[i])
                    {
                        counter++;
                    }
                }   

                if (counter == 0)
                {
                    isDifferent = true;
                }
            }

            for (int i = 0; i < victoryCards.Length; i++)
            {
                StartCoroutine(LerpUIPosition(victoryCards[i].GetComponent<RectTransform>(), pos[i], 1f));
            }   
        
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < victoryCards.Length; i++)
            {                
                foreach (Transform child in victoryCards[i].transform)
                {
                    
                    if (child.gameObject.name == "Front - Image")
                    {
                        child.gameObject.GetComponent<Button>().interactable = true;
                    }
                if (child.gameObject.name == "Icon - Button")
                {
                    child.transform.localScale = new Vector3(-0.8f, 0.8f, -0.8f);
                }
            }
            }

            isWaiting = false;


            GameManager.instance.numberOfFinishedLevels = levelIndex;
            StartCoroutine(GameManager.instance.apiClient.FinishLevel(GameManager.instance.UserTelegramID, levelIndex, "123"));
            
        }

        public void StartLerpUIPosition(RectTransform rectTransform, Vector3 endPosition, float lerpDuration)
        {
            StartCoroutine(LerpUIPosition(rectTransform, endPosition, lerpDuration));
        }

        IEnumerator LerpUIPosition(RectTransform rectTransform, Vector3 endPosition, float lerpDuration)
        {
            Vector3 startPosition = rectTransform.localPosition; // Get the current local position
            float elapsedTime = 0f;

            while (elapsedTime < lerpDuration)
            {
                // Calculate the new position
                rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / lerpDuration);

                elapsedTime += Time.deltaTime; // Increment elapsed time
                yield return null;            // Wait for the next frame
            }

            // Ensure the final position is set
            rectTransform.localPosition = endPosition;

            Debug.Log("UI Element has reached the target position.");
        }

        public void StartLerpUIScale(RectTransform rectTransform, Vector3 endScale, float lerpDuration)
        {
            StartCoroutine(LerpUIScale(rectTransform, endScale, lerpDuration));
        }

        IEnumerator LerpUIScale(RectTransform rectTransform, Vector3 endPosition, float lerpDuration)
        {
            Vector3 startPosition = rectTransform.localScale; // Get the current local position
            float elapsedTime = 0f;

            while (elapsedTime < lerpDuration)
            {
                // Calculate the new position
                rectTransform.localScale = Vector3.Lerp(startPosition, endPosition, elapsedTime / lerpDuration);

                elapsedTime += Time.deltaTime; // Increment elapsed time
                yield return null;            // Wait for the next frame
            }

            // Ensure the final position is set
            rectTransform.localScale = endPosition;

            Debug.Log("UI Element has reached the target scale.");
        }

        public void StartFrontImage(GameObject card, bool activate, float duration)
        {
            StartCoroutine(FrontImage(card, activate, duration));
        }

        IEnumerator FrontImage(GameObject card, bool activate, float duration)
        {
            yield return new WaitForSeconds(duration);

            foreach (Transform child in card.transform)
            {
                if (child.gameObject.name == "Front - Image")
                {
                    RectTransform rectTransform = card.GetComponent<RectTransform>();
                    Quaternion startRotation = rectTransform.localRotation; // Initial rotation
                    Quaternion halfwayRotation = Quaternion.Euler(0f, -90f, 0f); // Midway for flipping
                    Quaternion endRotation = Quaternion.Euler(0f, -180f, 0f); // Final rotation

                    float elapsedTime = 0f;
                    float halfDuration = duration / 2f; // Time for halfway rotation

                    // First half of rotation (to halfway point)
                    while (elapsedTime < halfDuration)
                    {
                        rectTransform.localRotation = Quaternion.Lerp(startRotation, halfwayRotation, elapsedTime / halfDuration);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                    // Ensure exact halfway rotation
                    rectTransform.localRotation = halfwayRotation;
                    
                    if (!activate)
                    {
                        ActivateLink(card, true);
                    }

                    // Activate/deactivate the front image
                    child.gameObject.SetActive(activate);

                    // Reset elapsed time for second half of rotation
                    elapsedTime = 0f;

                    // Second half of rotation (to end point)
                    while (elapsedTime < halfDuration)
                    {
                        rectTransform.localRotation = Quaternion.Lerp(halfwayRotation, endRotation, elapsedTime / halfDuration);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                    if (activate)
                    {
                        // Ensure exact final rotation
                        rectTransform.localRotation = startRotation;
                    }
                    else
                    {
                        rectTransform.localRotation = endRotation;
                    }

                    Debug.Log($"Front Image {(activate ? "activated" : "deactivated")} smoothly.");
                    break;
                }
            }
        }

        IEnumerator Win(GameObject card)
        {
            yield return new WaitForSeconds(1f);    

            foreach (Transform child in card.transform)
            {
                if (child.gameObject.name == "Front - Image")
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        void ActivateLink(GameObject card, bool activate = true)
        {
            foreach (Transform child in card.transform)
            {
                if (child.gameObject.name == "Link - TMP")
                {
                    child.gameObject.SetActive(activate);
                }
            }
        }

        /// <summary>
        /// Method used to show defeat screen.
        /// Triggered when the user runs out of time.
        /// </summary>
        public void Defeat()
        {
            currentLevelState = LevelState.Defeat;
            defeatScreen.SetActive(true);
            //StopAllCoroutines();

            //CanvasGroup canvasGroup = defeatScreen.GetComponent<CanvasGroup>();
            //canvasGroup.alpha = 0f; 
            //LeanTween.alphaCanvas(canvasGroup, 1f, 1f).setEase(LeanTweenType.easeOutQuad);
            
            //defeatScreen.transform.position = new Vector3 (0, 10, 0);
            //LeanTween.move(defeatScreen, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutQuad);
        }

        /// <summary>
        /// Pausing the game.
        /// </summary>
        [ContextMenu("Pause")]
        public void Pause()
        {
            currentLevelState = LevelState.Paused;
            pausedScreen.SetActive(true);
            
            CanvasGroup canvasGroup = pausedScreen.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f; 
            LeanTween.alphaCanvas(canvasGroup, 1f, 1f).setEase(LeanTweenType.easeOutQuad);
            
            pausedScreen.transform.position = new Vector3 (0, 10, 0);
            LeanTween.move(pausedScreen, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutQuad);

            isPaused = true;
        }
    
        /// <summary>
        /// Unpausing the game.
        /// </summary>
        public void Unpause()
        {
            currentLevelState = LevelState.Playing;
        
            CanvasGroup canvasGroup = pausedScreen.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f; 
            LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setEase(LeanTweenType.easeOutQuad);
            
            LeanTween.move(pausedScreen, new Vector3(0, 10, 0), 1f).setEase(LeanTweenType.easeOutQuad);

            Invoke("StartTimer", pausedLength);
            isPaused = false;
            StartCoroutine(Timer());   
        }

    #endregion

}