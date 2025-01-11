using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXStorage : MonoBehaviour
{    
    #region BASIC FIELDS

        [Space(20)] [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("SFXs")]
        public AudioClip sfxClip;
        public static SFXStorage instance;

        [Space(20)] [Header("N/A")]
        public string emptySpace;

    #endregion

    #region LIFE CYCLE METHODS
    
        /// <summary>
        /// Called When The Script Instance Is Being Loaded.
        /// </summary>
        /// <seealso cref="Start"/>
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject); // Prevent duplicate instances
            }
            
            DontDestroyOnLoad(gameObject); // Keep the manager across scenes if necessary
        }

        /// <summary>
        /// Called Before The First Frame Update.
        /// </summary>
        void Start()
        {
            // Use This For Any Initialization That Needs To Happen Once The Game Starts
            // E.g., Initializing Game State, Loading Resources, Or Starting Coroutines
        }

        /// <summary>
        /// Called Once Per Frame.
        /// </summary>
        /// <seealso cref="FixedUpdate"/>
        void Update()
        {
            // Use This For Any Logic That Needs To Happen Every Frame
            // E.g., Checking For User Input, Updating Animations, Or Moving Objects
        }

        /// <summary>
        /// Called At Fixed Intervals, Useful For Physics Updates.
        /// </summary>
        void FixedUpdate()
        {
            // Use This For Physics-Related Updates Or Calculations
            // E.g., Applying Forces Or Handling Rigidbody Physics Calculations
        }
    
    #endregion 

    #region CUSTOM METHODS
    
        /// <summary>
        /// Give Custom Method A Name.
        /// </summary>
        void MethodName()
        {
            // Create Your Own Logic
        }

    #endregion
}