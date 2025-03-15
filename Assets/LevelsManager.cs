using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{

    #region BASIC FIELDS

        [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("BASIC VARIABLES")]
            
            [Space(10)] [Header("Basic Variables")]
            public GameObject[] levels;

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
        /// 
        void Start()
        {
            StartCoroutine(GameManager.instance.apiClient.GetLevels(GameManager.instance.UserTelegramID, ProcessLevels));
            
        }

        private void ProcessLevels(int[] numbersOfFinishedLevels)
        {
            if (numbersOfFinishedLevels != null && numbersOfFinishedLevels.Length > 0)
            {
                
                foreach (var i in numbersOfFinishedLevels)
                {
                    if (i-1 <= this.levels.Length + 1)
                    {
                        CanvasGroup canvasGroup = this.levels[i-1].GetComponent<CanvasGroup>();
                        canvasGroup.alpha = 0.5f;
                        canvasGroup.blocksRaycasts = false;
                    }
                }
                
            }
            else
            {
                Debug.LogWarning("Нет данных о пройденных уровнях.");
            }
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
        void MethodName()
        {
            // Implement custom functionality here.
            // Example: Execute game-specific behavior or helper logic.
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