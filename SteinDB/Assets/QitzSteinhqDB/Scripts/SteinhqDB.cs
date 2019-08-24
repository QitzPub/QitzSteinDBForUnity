using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Networking;

namespace Qitz.SteinhqDB
{
    public interface ISteinhqDB
    {
        IEnumerator GetData<T>(Action<List<T>> callBack);
    }

    public static class SteinhqDBFactory
    {
        class SteinhqDB : ISteinhqDB
        {
            string steinhqApiUrl = "";
            string targetSheetName = "";
            string targetAPIUrl => $"{steinhqApiUrl}/{targetSheetName}";

            public SteinhqDB(string steinhqApiUrl, string targetSheetName)
            {
                this.steinhqApiUrl = steinhqApiUrl;
                this.targetSheetName = targetSheetName;
            }

            public IEnumerator GetData<T>(Action<List<T>> callBack)
            {
                UnityWebRequest request = UnityWebRequest.Get(targetAPIUrl);
                yield return request.Send();
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

        }

        public static ISteinhqDB Create(string steinhqApiUrl, string targetSheetName)
        {
            return new SteinhqDB(steinhqApiUrl, targetSheetName);
        }
    }

}
