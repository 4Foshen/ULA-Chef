using UnityEngine;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// A utility class that provides various helper methods for the game.
    /// </summary>
    public static class Utils
    {
        // Default color constants for logging
        private const string Yellow = "#FFFF00"; // System comments
        private const string Green = "#00B300";  // Game comments
        private const string Red = "#FF0000";    // Test comments

        /// <summary>
        /// Logs a message with a custom color to the Unity console.
        /// </summary>
        /// <param name="text"> The text to log. </param>
        /// <param name="color"> The color of the text (optional, defaults to yellow). </param>
        public static void LogCustom(string text, string color = Yellow)
        {
            Log(text, color);
        }

        /// <summary>
        /// Logs a system comment in yellow.
        /// </summary>
        /// <param name="text"> The text to log. </param>
        public static void SystemComment(string text)
        {
            Log(text, Yellow);
        }

        /// <summary>
        /// Logs a game-related comment in green.
        /// </summary>
        /// <param name="text"> The text to log. </param>
        public static void GameComment(string text)
        {
            Log(text, Green);
        }

        /// <summary>
        /// Logs a test comment in red.
        /// </summary>
        /// <param name="text"> The text to log. </param>
        public static void TestComment(string text)
        {
            Log(text, Red);
        }

        /// <summary>
        /// Private method to log a message with a specified color.
        /// </summary>
        /// <param name="text"> The text to log. </param>
        /// <param name="color"> The color of the text. </param>
        private static void Log(string text, string color)
        {
            Debug.Log($"<color={color}>{text}</color>");
        }

        /// <summary>
        /// Checks if a component exists, assigns it, and logs a message if not found.
        /// </summary>
        /// <typeparam name="T"> The type of component to check for. </typeparam>
        /// <param name="gameObject"> The gameobject to check on. </param>
        /// <param name="component"> The component to assign if found. </param>
        /// <param name="scriptName"> The name of the script where the check is being made. </param>
        /// <param name="color"> Optional color for the message if the component is not found. </param>
        /// <returns> Returns true if the component is found and assigned, false otherwise. </returns>
        public static bool AddComponent<T>(out T component, GameObject gameObject, string scriptName, string color = Yellow) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out component))
            {
                return true; // Component found
            }
            else
            {
                Log($"The component '{typeof(T).Name}' for '{gameObject.name}' in script '{scriptName}' not found!", color);
                component = null; // Ensure component is null if not found
                return false; // Component not found
            }
        }

        /// <summary>
        /// Attempts an action based on a given percentage chance.
        /// </summary>
        /// <param name="percentChance"> The percent of occurrence (0 - 1). </param>
        /// <returns> Returns true if it succeeds, false otherwise. </returns>
        public static bool TryPercentage(float percentangeChance)
        {
            float randomValue = Random.value; // Random float between 0 and 1
            
            return randomValue <= percentangeChance; // Return true if within chance
        }        
        
        /// <summary>
        /// The generic method of randomization of an array.
        /// </summary>
        /// <param name="array">The array that needs to be randomized.</param> 
        public static void ShuffleArray<T>(T[] array)
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
        /// The generic method of randomization of a two-dimensional array.
        /// </summary>
        /// <param name="array">The two-dimensional array that needs to be randomized.</param>
        public static void Shuffle2DArray<T>(T[,] array)
        {
            // Flatten the 2D array into a 1D list for easy shuffling.
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            List<T> flattenedList = new List<T>();

            // Add all elements to the list.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    flattenedList.Add(array[i, j]);
                }
            }

            // Shuffle the list using the original ShuffleArray method.
            ShuffleList(flattenedList);

            // Refill the 2D array with the shuffled elements.
            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = flattenedList[index++];
                }
            }
        }

        /// <summary>
        /// The generic method of randomization of a list.
        /// </summary>
        /// <param name="list">The list that needs to be randomized.</param>
        public static void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                // Get a random index
                int j = UnityEngine.Random.Range(0, i + 1);

                // Swap elements at indices i and j
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}