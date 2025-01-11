using UnityEngine;

/// <summary>
/// Manages sound effects in the game.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    #region BASIC FIELDS
    
        [Space(20)] [Header("NOTES")] [TextArea(5, 10)]
        public string notes;

        [Space(20)] [Header("BASIC VARIABLES")]
        private AudioSource audioSource;
        public static SFXManager Instance { get; private set; }

        [Space(20)] [Header("___")]
        public string emptySpace;

    #endregion

    #region BASIC METHODS

        private void Awake()
        {
            // Implement Singleton pattern
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject); // Prevent duplicate instances
            }
            
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject); // Keep the manager across scenes if necessary
        }

        /// <summary>
        /// Plays a sound effect at a specified position with a given volume.
        /// For example: SFXManager.PlaySFX(SFXStorage.instance.sfxClip, transform, 1f);
        /// </summary>
        /// <param name="audioClip">The audio clip to play.</param>
        /// <param name="spawnTransform">The position to play the sound effect.</param>
        /// <param name="volume">The volume of the sound effect (0.0 - 1.0).</param>
        public static void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
        {
            if (audioClip == null)
            {
                MyGame.Utils.SystemComment("AudioClip is null! Cannot play sound effect!");
                return;
            }

            // Create a temporary audio source to play the sound
            AudioSource tempSource = new GameObject("Temporary Audio Source").AddComponent<AudioSource>();
            tempSource.clip = audioClip;
            tempSource.volume = Mathf.Clamp01(volume); // Ensure volume is between 0 and 1
            tempSource.transform.position = spawnTransform.position;

            // Play the sound and destroy the GameObject after the clip finishes
            tempSource.Play();
            Object.Destroy(tempSource.gameObject, audioClip.length);
        }

    #endregion
}