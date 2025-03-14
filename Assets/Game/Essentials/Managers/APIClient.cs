using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour
{
    private string baseUrl = "https://untitled-devs.ru/api/v1";
    
    public IEnumerator GetUser(int telegramId)
    {
        string url = $"{baseUrl}/user/{telegramId}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error GetUser: " + request.error);
            }
        }
    }
    
    public IEnumerator FinishLevel(long telegramId, int levelId, string prize)
    {
        // Для безопасного кодирования строки prize используем UnityWebRequest.EscapeURL
        string url =
            $"{baseUrl}/levels/finish?level_id={levelId}&telegram_id={telegramId}&prize={UnityWebRequest.EscapeURL(prize)}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("FinishLevel: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error FinishLevel: " + request.error);
            }
        }
    }

    // Получение списка завершённых уровней по telegram_id
    public IEnumerator GetLevels(int telegramId, System.Action<int[]> callback)
    {
        string url = $"{baseUrl}/levels/{telegramId}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.certificateHandler = new AcceptAllCertificates(); // Для HTTP-соединения
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // Десериализация массива чисел
                    int[] levelIds = JsonConvert.DeserializeObject<int[]>(request.downloadHandler.text);

                    callback?.Invoke(levelIds);

                    Debug.Log("Levels: " + string.Join(", ", levelIds));
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON Parsing Error: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Error GetLevels: " + request.error);
            }
        }
    }
}


[System.Serializable]
public class LevelData
{
    public int id;
    public string name;
}

public class AcceptAllCertificates : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData) => true;
}