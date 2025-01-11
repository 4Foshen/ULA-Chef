using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card
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
        InteractionManager.instance.SelectCard(this);
        
        // Making a card half transparent.
        List<Image> images = new List<Image>(cardContainer.GetComponentsInChildren<Image>());

        foreach(Image image in images)
        {
            Color newColor = image.color;
            newColor.a = 0.5f;
            image.color = newColor;
        }
    }

    /// <summary>
    /// Called once two cards match.
    /// </summary>
    public void Matched()
    {
        // Disabling the button, so it is unpressable.
        Button button = cardInstance.GetComponent<Button>();
        button.interactable = false;

        // Making a card fully transparent.
        List<Image> images = new List<Image>(cardContainer.GetComponentsInChildren<Image>());

        foreach(Image image in images)
        {
            Color newColor = image.color;
            newColor.a = 0f;
            image.color = newColor;
        }
        
        // Incrementing number of matched cards.
        spawningManager.matchedNumber++;

        // Checking if all cards matched.
        if (spawningManager.matchedNumber == spawningManager.x * spawningManager.y)
        {
            LevelManager.instance.Victory();
        }
    }   

    /// <summary>
    /// Called once two cards don't match.
    /// </summary>    
    public void NotMatched()
    {           
        // Making a card fully visible.
        List<Image> images = new List<Image>(cardContainer.GetComponentsInChildren<Image>());

        foreach(Image image in images)
        {
            Color newColor = image.color;
            newColor.a = 1f;
            image.color = newColor;
        }
    }
}

public class SampleSpace
{
    public Sprite sprite;
    public Color color;
}

public class SpawningManager : MonoBehaviour
{

    #region BASIC FIELDS

        [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("BASIC VARIABLES")]
            
            [Space(10)] [Header("Basic Variables")]
            [Tooltip("Number of columns")] public int x;
            [Tooltip("Number of rows")] public int y;
            [Tooltip("Max legible number of columns")] public int maxX;
            [Tooltip("Max legible number of row")] public int maxY;
            [Tooltip("Allow random number of columns and rows?")] public bool random;
            [Tooltip("Number of matched cards")] [ShowOnly] public int matchedNumber;
            [Tooltip("Overall number of card pairs")] [ShowOnly] public float pairs;
            [Tooltip("The active set of cards")] public Card[,] cards;
            [Tooltip("The name of an icon object in card container")] [ShowOnly] public string iconSampleName;
            public static SpawningManager instance;

            [Space(10)] [Header("Samples")]
            [Tooltip("The icon samples")] public Sprite[] iconSamples;
            [Tooltip("The color samples")] public Color[] colorSamples;
            [Tooltip("The all available samples")] public SampleSpace[,] sampleSpace;
            [Tooltip("The all available samples")] public SampleSpace[] sampleSpace2;
            [Tooltip("The all using samples")] public GameObject[] usingSampleSpace;
            public GameObject cardContainer;
            public int counter;

            [Space(10)] [Header("Transform")]
            [Tooltip("The object where the cards will be located")] public GameObject parentObject;
            public float scale;

        [Space(20)] [Header("N/A")]
        public string emptySpace;

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
                instance = this; // Set the instance to this
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate GameManager instance
            }

            Randomize();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        void Start()
        {
            // Perform initial setup that occurs when the game starts.
            // Example: Initialize game state, start coroutines, load resources, etc.
            
            // Example of adding a component.    
            // SpriteRenderer spriteRenderer;
            // MyGame.Utils.AddComponent<SpriteRenderer>(out spriteRenderer, gameObject, this.GetType().Name);
            
            // Example of starting a coroutine.
            // StartCoroutine(BasicCoroutine());
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        void Update()
        {
            // Add your per-frame logic here.
            // Example: Move objects, check user input, update animations, etc.
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
        /// Initialization of a deck.
        /// </summary>
        public bool Initialize()
        {
            if ((x >= 2) && (y >= 2))
            {
                // Reseting the data.
                cards = null;
                matchedNumber = 0;

                foreach (Transform child in cardContainer.transform)
                {
                    if (child.TryGetComponent<Button>(out Button button))
                    {
                        iconSampleName = child.gameObject.name;
                        break;
                    }
                }

                foreach (Transform child in parentObject.transform)
                {
                    Destroy(child.gameObject);
                }
                
                // Checking if the initialization needs to be random or not.
                if (random)
                {
                    int threshold = iconSamples.Length;
                    bool suitablePair = false; 

                    while (!suitablePair)
                    {
                        x = UnityEngine.Random.Range(2, threshold);
                        y = UnityEngine.Random.Range(2, threshold);
                        
                        pairs = (float)(x * y) / 2f;

                        if (pairs % 2f == 0)
                        {   
                            if ((pairs <= iconSamples.Length) && (x <= maxX) && (y <= maxY))
                            {                         
                                suitablePair = true;   
                            }
                        }
                    }
                }
                else
                {
                    if ((x * y) % 2 == 0)
                    {
                        pairs = (x * y) / 2;

                        if (pairs > iconSamples.Length)
                        {
                            MyGame.Utils.SystemComment("Number Of Pairs Must Be Lower Than Or Equal To Number Of Card Samples!");
                            return false;
                        }
                    }
                    else
                    {
                        MyGame.Utils.SystemComment("The Multiplication Of X And Y Must Be Even!");
                        return false;   
                    }
                }

                // Initialization of a deck.
                cards = new Card[x, y];
                if (LevelManager.instance != null)
                {
                    LevelManager.instance.ResetTimer();
                }
                parentObject.transform.position = new Vector3 (0f, 0f, 0f);
                parentObject.transform.localScale = new Vector3 (scale, scale, scale);

                // Assigning positions.
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        cards[i, j] = new Card();
                        cards[i, j].isEmpty = true;

                        float xOffset = (x % 2 == 0) ? 0.5f : 1f;
                        float yOffset = (y % 2 == 0) ? 0.5f : 1f;
                        
                        cards[i, j].pos = new Vector3 ((i + xOffset) * scale, (j + yOffset) * scale, 0f);
                    }
                }

                return true;
            }
            else
            {
                MyGame.Utils.SystemComment("X And Y Must Be More Than 2!");
                return false;   
            }
        }

        /// <summary>
        /// Randomization of a deck.
        /// </summary>
        public void Randomize()
        {   
            // Checking if initialization was successful or not.
            bool isInitialized = Initialize();

            if (!isInitialized)
            {
                return;
            }

            // Checking if a loop was finished.
            bool isFinished;

            // Randomizing the samples.
            ShuffleArray(iconSamples);
            ShuffleArray(colorSamples);

            sampleSpace2 = new SampleSpace[iconSamples.Length];
            for (int i = 0; i < iconSamples.Length; i++)
            {
                sampleSpace2[i] = new SampleSpace
                {
                    sprite = iconSamples[i]
                };   
            }
            MyGame.Utils.ShuffleArray(sampleSpace2);

            // Initialization of a sample space that we will be using.
            usingSampleSpace = new GameObject[x * y];

            for (int i = 0; i < x * y; i++)
            {
                usingSampleSpace[i] = Instantiate(cardContainer);
            }

            // Gathering unique cards.
            counter = 0;

            for (int i = 0; i < iconSamples.Length; i++)
            {
                foreach (Transform child in usingSampleSpace[counter].transform)
                {
                    if (child.gameObject.name == iconSampleName)
                    {
                        Image image = child.gameObject.GetComponent<Image>();
                        image.sprite = sampleSpace2[i].sprite;
                    }
                }

                counter++;

                if (counter >= usingSampleSpace.Length)
                {
                    break;
                }
            }

            // Looping one time for one half of the deck, and then again for another.
            for (int k = 1; k <= 2; k++)
            {
                isFinished = false;

                for (int i = 0; i < pairs; i++)
                {
                    isFinished = false;

                    while (!isFinished)
                    {
                        // Random values.
                        int randX = UnityEngine.Random.Range(0, x);
                        int randY = UnityEngine.Random.Range(0, y);

                        if (cards[randX, randY].isEmpty)
                        {
                            // Instantiation of a card container.
                            GameObject cardContainer = Instantiate(usingSampleSpace[i], cards[randX, randY].pos, Quaternion.identity);

                            // Adjusting a card container's values.
                            cardContainer.name = "Card_Container_" + k.ToString() + "_" + i.ToString(); 
                            cardContainer.transform.SetParent(parentObject.transform);
                            RectTransform rectTransform = cardContainer.GetComponent<RectTransform>();
                            //rectTransform.localScale = new Vector3(scale, scale, scale);
                            rectTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                            // Finding needed object and getting values.
                            foreach (Transform child in cardContainer.transform)
                            {
                                if (child.gameObject.name == iconSampleName)
                                {
                                    cards[randX, randY].cardInstance = child.gameObject;
                                    cards[randX, randY].originalCardInstance = usingSampleSpace[i];
                                    cards[randX, randY].cardContainer = cardContainer;

                                    cards[randX, randY].cardInstance.name = cards[randX, randY].cardInstance.name;
                                    cards[randX, randY].spawningManager = this;
                                    cards[randX, randY].isEmpty = false;
                                    
                                    Button button = cards[randX, randY].cardInstance.GetComponent<Button>();
                                    button.onClick.AddListener(cards[randX, randY].Select);

                                    isFinished = true;

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // Destroying the instances. We used them just as a refference.
            for (int i = 0; i < x * y; i++)
            {
                Destroy(usingSampleSpace[i]);
            }
            
            AlterGlobalPosition();
            if (InteractionManager.instance != null)
            {
                InteractionManager.instance.DisableCards();   
            }        
        }

        /// <summary>
        /// The generic method of randomization of an array.
        /// </summary>
        /// <param name="array">The array that needs to be randomized.</param> 
        public void ShuffleArray<T>(T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                // Get a random index
                int j = UnityEngine.Random.Range(0, i + 1);

                // Swap elements at indices i and j
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        /// <summary>
        /// Changing the position of the parent object.
        /// </summary>
        public void AlterGlobalPosition()
        {
            float globalOffsetX = (float)(x) / 2f;
            float globalOffsetY = (float)(y) / 2f;

            if (x % 2 != 0)
            {
                globalOffsetX += 0.5f;
            }
            
            if (y % 2 != 0)
            {
                globalOffsetY += 0.5f;   
            }

            // The "parentObject.transform.localScale.x" and "xOffsetMultiplier" needs to be equal, otherwise the layout will be wrong.
            float xPos = -globalOffsetX * 100f * parentObject.transform.localScale.x;
            float yPos = -globalOffsetY * 100f * parentObject.transform.localScale.x;
            float zPos = 100f;
            parentObject.transform.localPosition = new Vector3 (xPos, yPos, zPos);
        }
        
        /// <summary>
        /// An example coroutine that waits for 2 seconds.
        /// </summary>
        IEnumerator BasicCoroutine()
        {
            // Wait for 2 seconds before executing further code.
            yield return new WaitForSeconds(2f);

            Debug.Log("Action after 2 seconds");
        }

    #endregion

}