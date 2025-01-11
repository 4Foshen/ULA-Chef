using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{

    #region BASIC FIELDS

        [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("BASIC VARIABLES")]
            
            [Space(10)] [Header("Basic Variables")]
            [Tooltip("The first chosen card")] [ShowOnly] public GameObject firstCardObj;
            [Tooltip("The second chosen card")] [ShowOnly] public GameObject secondCardObj;
            public float matchedTimer;
            public float notMatchedTimer;
            public Card firstCard;
            public Card secondCard;
            public static InteractionManager instance;

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
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DisableCards();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        void Start()
        {
            // Perform initial setup that occurs when the game starts
            // Example: Initialize game state, start coroutines, load resources, etc.
            
            // Example of adding a component    
            // SpriteRenderer spriteRenderer;
            // MyGame.Utils.AddComponent<SpriteRenderer>(out spriteRenderer, gameObject, this.GetType().Name);
            
            // Example of starting a coroutine
            // StartCoroutine(BasicCoroutine());
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        void Update()
        {
            // Add your per-frame logic here
            // Example: Move objects, check user input, update animations, etc.
        }

        /// <summary>
        /// Called at fixed intervals, ideal for physics updates.
        /// Use this for physics-related updates like applying forces or handling Rigidbody physics.
        /// </summary>
        void FixedUpdate()
        {
            // Add physics-related logic here
            // Example: Rigidbody movement, applying forces, or collision detection
        }

    #endregion

    #region CUSTOM METHODS

        /// <summary>
        /// Card selection.
        /// </summary>
        public void SelectCard(Card card)
        {
            if (firstCard == null)
            {
                firstCard = card;
                firstCardObj = firstCard.cardInstance;
                MyGame.Utils.GameComment("The First Card: " + firstCard.cardInstance.name);
            }
            else if (firstCard != null)
            {
                if (firstCard.cardInstance != card.cardInstance)
                {
                    secondCard = card;   
                    secondCardObj = secondCard.cardInstance;
                    MyGame.Utils.GameComment("The Second Card: " + secondCard.cardInstance.name);
                }
                else
                {
                    StartCoroutine(NotMatched(firstCard, firstCard, 0.1f));
                    DisableCards();
                    return;
                }
            }

            if (secondCard != null)
            {
                if (firstCard.originalCardInstance == secondCard.originalCardInstance)
                {
                    StartCoroutine(Matched(firstCard, secondCard));
                    MyGame.Utils.GameComment("Matched!");
                }
                else
                {
                    StartCoroutine(NotMatched(firstCard, secondCard, notMatchedTimer));
                    MyGame.Utils.GameComment("Not Matched!");
                }

                DisableCards();
            }
        }

        /// <summary>
        /// A slight delay for showcasing a pair of cards that matched.
        /// </summary>
        IEnumerator Matched(Card firstCardInstance, Card secondCardInstance)
        {
            yield return new WaitForSeconds(matchedTimer);
            
            firstCardInstance.Matched();
            secondCardInstance.Matched();
        }

        /// <summary>
        /// A slight delay for showcasing a pair of cards that didn’t match.
        /// </summary>
        IEnumerator NotMatched(Card firstCardInstance, Card secondCardInstance, float timer)
        {
            yield return new WaitForSeconds(timer);

            firstCardInstance.NotMatched();
            secondCardInstance.NotMatched();
        }

        /// <summary>
        /// Disabling the current cards.
        /// </summary>
        public void DisableCards()
        {
            firstCard = null;
            secondCard = null;
            firstCardObj = null;
            secondCardObj = null;
        }

    #endregion

}