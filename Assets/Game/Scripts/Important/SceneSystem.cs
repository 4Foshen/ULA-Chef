using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class SceneSystem : MonoBehaviour
{

    #region BASIC FIELDS

        [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("BASIC VARIABLES")]
            
            [Space(10)] [Header("Basic Variables")]
            public PlayableDirector levelOutroDirector;

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
            //levelOutroDirector.playOnAwake = false;
            levelOutroDirector.enabled = false;
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
        public void ChangeScene(string sceneName)
        {
            StartCoroutine(StartChangingScene(levelOutroDirector, sceneName));
        }

        /// <summary>
        /// An example coroutine that waits for 2 seconds.
        /// </summary>
        IEnumerator StartChangingScene(PlayableDirector levelOutroDir, string sceneName)
        {
        if (levelOutroDir != null)  levelOutroDirector.enabled = true;
            if(levelOutroDir != null) levelOutroDir.Play();

            yield return new WaitForSeconds(levelOutroDir != null ? (float)levelOutroDir.duration : 0);

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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