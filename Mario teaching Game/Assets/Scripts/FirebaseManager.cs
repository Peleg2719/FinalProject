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
 //WriteQuestionData("question_5_level_2_es", "Hola Mario, ¿te gustaría personalizar tu pedido de café hoy?", "Sí, me gustaría un café sin grasa con un shot de espresso y canela, por favor", 2);


        // ReadUserData("user123");
        // ReadQuestionData("Mario");
    }

    public void WriteUserData(string username, string password, int levelEn,int levelEs, int scoreEn, int scoreEs)
    {
        userData = new UserData
        {
            username = username,
            password = password,
            levelEn = levelEn,
            levelEs=levelEs,
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
    Debug.Log("Reading user data for username: " + username); // Debug log to see what username is being passed
    StartCoroutine(GetRequest("user_data/" + username, jsonData =>
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("No data received from Firebase for username: " + username); // Log if no data is received
            callback?.Invoke(null);
            return;
        }

        Debug.Log("Received JSON data: " + jsonData); // Log the received JSON data for debugging

        UserData userData = JsonUtility.FromJson<UserData>(jsonData);
        if (userData != null)
        {
            Debug.Log("Parsed UserData: " + userData.username + ", " + userData.password); // Log parsed user data
            if (userData.username == username)
            {
                UserManager.Instance.SetCurrentUser(userData);
                Debug.Log($"User data set for {userData.username}, LevelEn: {userData.levelEn}, LevelEs: {userData.levelEs}");
                callback?.Invoke(userData);
                this.userData=userData;
                
            }
            else
            {
                Debug.LogError($"Username mismatch. Requested: {username}, Received: {userData.username}");
                callback?.Invoke(null);
            }
        }
        else
        {
            Debug.LogError($"Failed to parse user data for {username}");
            callback?.Invoke(null);
        }
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
        string path = $"user_data/{userName}";  // Changed from "questions" to "user_data"
        string url = $"{databaseURL}{path}.json?auth={apiKey}";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error retrieving data from Firebase: " + request.error);
            callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Data successfully retrieved from Firebase.");
            string jsonData = request.downloadHandler.text;
            this.userData = JsonUtility.FromJson<UserData>(jsonData);
            if (this.userData != null)
            {
                UserManager.Instance.SetCurrentUser(this.userData);
                // Debug.Log($"User data set for {this.userData.username}, Level: {this.userData.level}");
              //  Debug.Log($"User data set for {UserManager.Instance.CurrentUser.level}, Level: {UserManager.Instance.CurrentUser.level}");
            }
            else
            {
                Debug.LogError($"Failed to parse user data for {userName}");
            }
            callback?.Invoke(this.userData);
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
    public int levelEn;
    public int levelEs;
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