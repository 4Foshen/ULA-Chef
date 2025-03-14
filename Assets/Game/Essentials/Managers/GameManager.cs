using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static MyGame.Utils;

public class GameManager : MonoBehaviour
{    

    #region BASIC FIELDS

        [Space(20)] [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("CUSTOM TITLE")]
        public bool isGamePlayed;

        public int[] numbersOfFinishedLevels;
        public int numberOfFinishedLevels;
        public static GameManager instance;

        [Space(20)] [Header("___")]
        public string emptySpace;

        public TextMeshProUGUI Tmp;
        private TelegramWebApp WebApp = new TelegramWebApp();
        
        public APIClient apiClient;

        public int UserTelegramID;

    #endregion

    #region LIFE CYCLE METHODS
    
        /// <summary>
        /// Called When The Script Instance Is Being Loaded.
        /// </summary>
        /// <seealso cref="Start"/>
        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            isGamePlayed = PlayerPrefs.GetInt("IsGamePlayed", 0) == 1; 
            
            PlayerPrefs.SetInt("IsGamePlayed", 1);
        }

        /// <summary>
        /// Called Before The First Frame Update.
        /// </summary>
        void Start()
        {
            // WebApp.RequestUserData();
            // Tmp.text = WebApp.userData.first_name;
            
            if (apiClient == null)
            {
                apiClient = GetComponent<APIClient>(); // Присваиваем APIClient
            }

#if UNITY_EDITOR

            UserTelegramID = 1234567;
#else
            WebApp.RequestUserData();
            UserTelegramID = WebApp.userData.id;
#endif
            StartCoroutine(apiClient.GetUser(UserTelegramID));
            StartCoroutine(apiClient.GetLevels(UserTelegramID, ProcessLevels));
        }
        
        // Метод для обработки полученного массива
        private void ProcessLevels(int[] levels)
        {
            if (levels != null && levels.Length > 0)
            {
                numbersOfFinishedLevels = levels;
            }
            else
            {
                Debug.LogWarning("Нет данных о пройденных уровнях.");
            }
        }
        

        void GetFinishedLevels()
        {
            
        }

        /// <summary>
        /// Called Once Per Frame.
        /// </summary>
        /// <seealso cref="FixedUpdate"/>
        void Update()
        {
            //PlayerPrefs.SetInt("Finished Levels", numberOfFinishedLevels);
        }

        /// <summary>
        /// Called At Fixed Intervals, Useful For Physics Updates.
        /// </summary>
        void FixedUpdate()
        {
            // Use This For Physics-Related Updates Or Calculations
        }
        
        #endregion 

        #region CUSTOM METHODS
        
        /// <summary>
        /// Restarts the game by reloading the current scene.
        /// </summary>
        [ContextMenu("RestartTheGame")]
        public void RestartTheGame()
        {
            PlayerPrefs.DeleteAll();
        }
        

    #endregion

}