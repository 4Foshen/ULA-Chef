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
    
    #endregion

    #region CUSTOM METHODS
        public void OnVictoryCardPressed()
        {
            if (LevelManager.instance.isClicked || LevelManager.instance.isWaiting) return;


            LevelManager.instance.isClicked = true;
            LevelManager.instance.StartLerpUIPosition(transform.parent.GetComponent<RectTransform>(), Vector3.zero, 1f);     
            LevelManager.instance.StartLerpUIScale(transform.parent.GetComponent<RectTransform>(), new Vector3 (1.5f, 1.5f, 1.5f), 1f);  
            LevelManager.instance.StartFrontImage(transform.parent.gameObject, false, 1f);

            LevelManager.instance.StartLerpUIPosition(LevelManager.instance.levelsObject.GetComponent<RectTransform>(), new Vector3 (-40f, -360f, 0f), 2.5f);
            
            transform.parent.transform.SetAsLastSibling();
            
            string imageName = "";
            foreach (Transform child in transform.parent)
            {
                if (child.gameObject.name == "Icon - Button")
                {
                    Image image = child.GetComponent<Image>();
                    if (image != null && image.sprite != null)
                    {
                        imageName = image.sprite.name;
                        break;
                    }
                }
            }
            Debug.Log("Selected card image name: " + imageName);

            int telegramID = GameManager.instance.UserTelegramID;
            StartCoroutine(GameManager.instance.apiClient.AddPrize(telegramID, imageName));

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