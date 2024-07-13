using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseManager : MonoBehaviour
{
    private string databaseURL = "https://travelwithus-2d942-default-rtdb.firebaseio.com/";
    private string apiKey = "AIzaSyBZiPzBveuteXahGTnnHgD62ypj97RYt_s";
    public QuestionData questiondata;
    public UserData userData;

    void Start()
    {
        // WriteUserData("user123", "password123", 1, 0);
        WriteQuestionData("question_1_level_2", "Hi mario! Do you have any plans for today? ", "yes im planing to learn english and talk with the pepople i will meet later", 2);
        // WriteQuestionData("question_4", "Hi Mario, what do you want to report?", "My wallet was stolen, I need help", 1);
        // WriteQuestionData("question_5", "Hi mario what would you like to order?", "I would love a fat-free coffee, please!", 1);

        // ReadUserData("user123");
        // ReadQuestionData("Mario");
    }

  public void WriteUserData(string username, string password, int level, int scoreEn, int scoreEs)
    {
        userData = new UserData
        {
            username = username,
            password = password,
            level = level,
            scoreEn = scoreEn,
            scoreEs = scoreEs
        };
        string jsonData = JsonUtility.ToJson(userData);
        StartCoroutine(PostRequest("user_data/" + username, jsonData));
    }

    public void WriteQuestionData(string characterName, string question, string answer, int level)
    {
        questiondata = new QuestionData
        {
            character_name = characterName,
            question = question,
            answer = answer,
            level = level,
        };
        string jsonData = JsonUtility.ToJson(questiondata);
        StartCoroutine(PostRequest("questions/" + characterName, jsonData));
    }

     public void ReadUserData(string username, System.Action<UserData> callback)
    {
        StartCoroutine(GetRequest("user_data/" + username, jsonData =>
        {
            UserData userData = JsonUtility.FromJson<UserData>(jsonData);
            callback?.Invoke(userData);
        }));
    }

    public void ReadQuestionData(string characterName)
    {
        StartCoroutine(GetRequest("questions/" + characterName, OnQuestionDataReceived));
    }

    public IEnumerator GetQuestionData(string characterName, System.Action<QuestionData> callback)
    {
        string path = $"questions/{characterName}";
        string url = $"{databaseURL}{path}.json?auth={apiKey}";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error retrieving data from Firebase: " + request.error);
        }
        else
        {
            Debug.Log("Data successfully retrieved from Firebase.");
            string jsonData = request.downloadHandler.text;
            questiondata = JsonUtility.FromJson<QuestionData>(jsonData);
            callback?.Invoke(questiondata);
        }
    }
    public IEnumerator GetUserData(string userName, System.Action<UserData> callback)
    {
        string path = $"questions/{userName}";
        string url = $"{databaseURL}{path}.json?auth={apiKey}";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error retrieving data from Firebase: " + request.error);
        }
        else
        {
            Debug.Log("Data successfully retrieved from Firebase.");
            string jsonData = request.downloadHandler.text;
            this.userData = JsonUtility.FromJson<UserData>(jsonData);
            callback?.Invoke(userData);
        }
    }
    private IEnumerator PostRequest(string path, string jsonData)
    {
        string url = $"{databaseURL}{path}.json?auth={apiKey}";
        UnityWebRequest request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error posting data to Firebase: " + request.error);
        }
        else
        {
            Debug.Log("Data successfully sent to Firebase.");
        }
    }

    private IEnumerator GetRequest(string path, System.Action<string> callback)
    {
        string url = $"{databaseURL}{path}.json?auth={apiKey}";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error retrieving data from Firebase: " + request.error);
        }
        else
        {
            Debug.Log("Data successfully retrieved from Firebase.");
            callback?.Invoke(request.downloadHandler.text);
        }


        
    }

    private void OnUserDataReceived(string jsonData)
    {
        userData = JsonUtility.FromJson<UserData>(jsonData);
        Debug.Log("Username: " + userData.username);
        Debug.Log("ScoreEs: " + userData.scoreEs);
         Debug.Log("ScoreEn: " + userData.scoreEn);
    }

    private void OnQuestionDataReceived(string jsonData)
    {
        questiondata = JsonUtility.FromJson<QuestionData>(jsonData);
        Debug.Log("Question: " + questiondata.question);
        Debug.Log("Answer: " + questiondata.answer);
    }
}


[System.Serializable]
public class UserData
{
    public string username;
    public string password;
    public int level;
    public int scoreEn;
    public int scoreEs;
    
  
}

[System.Serializable]
public class QuestionData
{
    public string character_name;
    public string question;
    public string answer;
    public int level;
}