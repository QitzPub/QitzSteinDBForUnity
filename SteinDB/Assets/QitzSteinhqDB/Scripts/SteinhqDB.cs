using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Networking;

namespace Qitz.SteinhqDB
{
    public interface IDB<T> where T: AEntity
    {
        IEnumerator GetData(Action<List<T>> callBack);
        IEnumerator AddData(T addData,Action<string> response);
        IEnumerator UpdateData(string id,T upDateData, Action<string> response);
        IEnumerator DeleatData(string id, Action<string> response);
    }

    [Serializable]
    public abstract class AEntity
    {
        [SerializeField]
        protected string id;
        public string ID => id;
    }

    public static class SteinhqDBFactory
    {
        class SteinhqDB<T> : IDB<T> where T : AEntity
        {
            string steinhqApiUrl = "";
            string targetSheetName = "";
            string targetAPIUrl => $"{steinhqApiUrl}/{targetSheetName}";

            public SteinhqDB(string steinhqApiUrl, string targetSheetName)
            {
                this.steinhqApiUrl = steinhqApiUrl;
                this.targetSheetName = targetSheetName;
            }

            public IEnumerator GetData(Action<List<T>> callBack)
            {
                UnityWebRequest request = UnityWebRequest.Get(targetAPIUrl);
                yield return request.SendWebRequest();
                if (request.isNetworkError)
                {
                    Debug.LogError(request.error);
                    yield break;
                }
                string json = request.downloadHandler.text;
                //先頭行と最終行の文字列を削除
                json = json.TrimStart('[');
                json = json.TrimEnd(']');
                //jsonの配列取得
                string[] jsonArray = GetJsonArray(json);
                callBack(jsonArray.Select(j => JsonUtility.FromJson<T>(j)).ToList());
            }

            string[] GetJsonArray(string json)
            {
                string[] del = { "}," };
                string[] jsonArray = json.Split(del, StringSplitOptions.None);
                for (int i = 0; i < jsonArray.Length; i++)
                {
                    string rightString = jsonArray[i].Substring(jsonArray[i].Length - 1, 1);
                    if (rightString != "}")
                    {
                        jsonArray[i] = jsonArray[i] + "}";
                    }
                }
                return jsonArray;
            }

            public IEnumerator AddData(T addData, Action<string> response)
            {
                string jsonMapper = $"[{JsonUtility.ToJson(addData)}]";
                byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonMapper);
                var request = new UnityWebRequest(targetAPIUrl, "POST");
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    response.Invoke(request.error);
                    Debug.LogError(request.error);
                    yield break;
                }
                response.Invoke(request.downloadHandler.text);
                Debug.Log("Complete!");

            }

            public IEnumerator UpdateData(string id, T upDateData, Action<string> response)
            {
                string targetMapper = "\"condition\""+ ":{\"id\":"+ "\""+ id + "\"}";
                string setJson = JsonUtility.ToJson(upDateData);
                string setMapper = "\"set\":"+ setJson;
                string jsonData = "{"+ targetMapper+","+ setMapper+ "}";
                byte[] putData = System.Text.Encoding.UTF8.GetBytes(jsonData);

                var request = new UnityWebRequest(targetAPIUrl, "PUT");
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(putData);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                if (request.isNetworkError)
                {
                    response.Invoke(request.error);
                    Debug.LogError(request.error);
                    yield break;
                }
                response.Invoke(request.downloadHandler.text);
                Debug.Log("Complete!");
            }

            public IEnumerator DeleatData(string id, Action<string> response)
            {
                string targetMapper = "\"condition\"" + ":{\"id\":" + "\"" + id + "\"}";
                string jsonData = "{" + targetMapper +  "}";
                byte[] putData = System.Text.Encoding.UTF8.GetBytes(jsonData);
                var request = new UnityWebRequest(targetAPIUrl, "DELETE");
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(putData);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                if (request.isNetworkError)
                {
                    response.Invoke(request.error);
                    Debug.LogError(request.error);
                    yield break;
                }
                response.Invoke(request.downloadHandler.text);
                Debug.Log("Complete!");
            }
        }

        public static IDB<T> Create<T>(string steinhqApiUrl, string targetSheetName) where T : AEntity
        {
            return new SteinhqDB<T>(steinhqApiUrl, targetSheetName);
        }
    }

}
