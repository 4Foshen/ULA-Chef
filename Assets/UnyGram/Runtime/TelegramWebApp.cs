using System.Runtime.InteropServices;
using UnityEngine;
using Newtonsoft.Json;

public class TelegramWebApp : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string Telegram_GetUserData();
    
    public TelegramUserData userData;

    public void RequestUserData()
    {
        string jsonUserData;
        jsonUserData = Telegram_GetUserData();
        Debug.Log(jsonUserData);
        if (!string.IsNullOrEmpty(jsonUserData))
        {
            userData = JsonConvert.DeserializeObject<TelegramUserData>(jsonUserData);
            if (userData == null)
            {
                Debug.LogError("Failed to parse user data with Newtonsoft.");
            }
        }
        else
        {
            Debug.LogError("Received empty JSON data.");
        }
    }
    
   
}

[System.Serializable]
public class TelegramUserData
{
    public int id;
    public string first_name;
    public string last_name;
    public string language_code;
    public bool allows_write_to_pm;
    public string photo_url;
}

[System.Serializable]
public class TelegramInitData
{
    public long id;
    public string first_name;
    public string last_name;
    public string username;
    public string language_code;
    public string auth_date;
    public string hash;
}