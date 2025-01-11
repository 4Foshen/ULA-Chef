using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryCardButton : MonoBehaviour
{

    #region BASIC FIELDS

        [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("BASIC VARIABLES")]
            
            [Space(10)] [Header("Basic Variables")]
            public int basicField;

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
            // Initialize variables or cache references here.
            // Example: Setting up components or data before start is called.
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
        /// An example custom method.
        /// Replace with your own custom logic.
        /// </summary>
        public void OnVictoryCardPressed()
        {
            LevelManager.instance.StartLerpUIPosition(transform.parent.GetComponent<RectTransform>(), Vector3.zero, 1f);     
            LevelManager.instance.StartLerpUIScale(transform.parent.GetComponent<RectTransform>(), new Vector3 (1.5f, 1.5f, 1.5f), 1f);  
            LevelManager.instance.StartFrontImage(transform.parent.gameObject, false, 1f);

            LevelManager.instance.StartLerpUIPosition(LevelManager.instance.levelsObject.GetComponent<RectTransform>(), new Vector3 (-40f, -460f, 0f), 2.5f);
            
            transform.parent.transform.SetAsLastSibling();

            /*foreach (Transform child in transform.parent.transform)
            {
                if (child.gameObject.name == "Front - Image")
                {
                    child.gameObject.SetActive(false);
                }
            }   */
        }

        /// <summary>
        /// An example coroutine that waits for 2 seconds.
        /// </summary>
        IEnumerator BasicCoroutine()
        {
            // Wait for 2 seconds before executing further code.
            yield return new WaitForSeconds(2f);

            Debug.Log("Action after 2 seconds.");
        }

    #endregion

}