using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Net;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class File {
    public string filename;
    public string raw_url;
}

[Serializable]
public class JSONData {
    public string url;
    public Dictionary<string, File> files;
}

[Serializable]
public class Leaders
{
    public Dictionary<string, int> leaders;
}

[Serializable]
public class Content
{
    public string content;
}

public class Web : MonoBehaviour {
    private const string token = "0f0dbf09063d73d12f1d24353d88ed7a5bd3dbec";
    private const string gistId = "430e763ecc4a0956463ddaf40418b51b";
    private const string fileName = "Leaderboard.json";
    private const string url = "https://api.github.com/gists";
    private List<KeyValuePair<string, int>> leaders;
    private bool noConnection = false;

    [SerializeField] private Image slotPrefab;
    [SerializeField] private GameObject content;

    private void Start() {
        var www = UnityWebRequest.Get(url + '/' + gistId);
        StartCoroutine(GetLeaders(www));
    }

    public void PatchAndQuit()
    {
        if (!noConnection)
        {
            var request = FormPatchReq();
            StartCoroutine(Patch(request));
        } else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    private UnityWebRequest FormPatchReq()
    {
        Content content = new Content();
        content.content = MakeFileContent();
        Debug.Log(content.content);
        Dictionary<System.Object, System.Object> requestBody = new Dictionary<System.Object, System.Object>()
            {
                { "files", new Dictionary<string, Content>() { { fileName, content} } }
            };
        string requestBodyString = JsonConvert.SerializeObject(requestBody);
        byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(requestBodyString);
        return UnityWebRequest.Put(url + '/' + gistId, requestBodyData);
    }

    private string MakeFileContent()
    {
        string ret = "{\n" +
            "\t \"leaders\": {\n";
        for(var i = 0; i < leaders.Count; ++i)
        {
            if (i != leaders.Count - 1) {
                ret += "\t\t \"" + leaders[i].Key + "\": " + leaders[i].Value.ToString() + ",\n";
            } else
            {
                ret += "\t\t \"" + leaders[i].Key + "\": " + leaders[i].Value.ToString() + "\n\t}\n}";
            }
        }
        return ret;
    }

    IEnumerator Patch(UnityWebRequest www)
    {
        www.method = "PATCH";
        www.SetRequestHeader("Authorization", $"Token {token}");
        www.SetRequestHeader("accept", "application/vnd.github.v3+json");
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");     
        }
        SceneManager.LoadScene("Main Menu");
    }

    IEnumerator GetLeaders(UnityWebRequest www) {
        www.SetRequestHeader("Authorization", $"Token {token}");
        www.SetRequestHeader("accept", "application/vnd.github.v3+json");
        yield return www.SendWebRequest();
        if (!www.isNetworkError) {
            Debug.Log("WWW success: " + www.downloadHandler.text);
            JSONData data = JsonConvert.DeserializeObject<JSONData>(www.downloadHandler.text);
            Debug.Log(data.files[fileName].raw_url);    
            var raw_url = data.files[fileName].raw_url;
            using (var webClient = new WebClient())
            {
                var responce = webClient.DownloadString(raw_url);
                var dictLeaders = JsonConvert.DeserializeObject<Leaders>(responce).leaders;
                dictLeaders[GameManager.Instance.GetUsername()] = GameManager.Instance.GetHighscore();
                var unsortedLeaders = new List<KeyValuePair<string,int>>();             
                foreach (var elem in dictLeaders)
                {
                    unsortedLeaders.Add(new KeyValuePair<string, int>(elem.Key, elem.Value));
                }
                unsortedLeaders.Sort(
                    delegate (KeyValuePair<string, int> firstPair,
    KeyValuePair<string, int> nextPair)
                    {
                        return firstPair.Value.CompareTo(nextPair.Value);
                    });


                unsortedLeaders.Reverse();
                leaders = unsortedLeaders;

                foreach(var pair in leaders)
                {
                    var instance = Instantiate(slotPrefab);
                    var texts = instance.GetComponentsInChildren<TextMeshProUGUI>();
                    texts[0].text = pair.Key;
                    if(pair.Key == GameManager.Instance.GetUsername())
                    {
                        instance.color = Color.green;
                    }
                    texts[1].text = pair.Value.ToString();
                    instance.transform.SetParent(content.transform);
                }
            }

        } else {
            Debug.Log("WWW error: " + www.error);
            noConnection = true;

            var instance = Instantiate(slotPrefab);
            var texts = instance.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = GameManager.Instance.GetUsername();
            instance.color = Color.green;
            texts[1].text = GameManager.Instance.GetHighscore().ToString();
            instance.transform.SetParent(content.transform);
        }
    }
}