using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Qitz.SteinDB;
using System;

namespace Tests
{

    public class StubData: AEntity
    {
        [SerializeField]
        string discription;
        public string Discription { get { return discription; } }

        [SerializeField]
        string name;
        public string Name { get { return name; } }


        public StubData(string id, string name, string discription)
        {
            this.id = id;
            this.discription = discription;
            this.name = name;
        }

    }

    public class SteinDBTests
    {
        IDB<StubData> db = SteinDBFactory.Create<StubData>("https://api.steinhq.com/v1/storages/5d6093ecbb4eaf04c5eaa2b5", "test_data");
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator GoogleSpreadSheetからデータを取得するテスト()
        {

            yield return  db.GetData((data)=> {
                foreach (var d in data)
                {
                    Debug.Log(d.ID);
                    Debug.Log(d.Name);
                }
            });

        }

        [UnityTest]
        public IEnumerator GoogleSpreadSheetにダミーデータをPostするテスト()
        {
            var testData = new StubData("1","zzzzsss","gggsasa");
            yield return db.AddData(testData,(response)=> {
                Debug.Log(response);
            });

        }

        [UnityTest]
        public IEnumerator GoogleSpreadSheetデータ上書き更新テスト()
        {
            var testData = new StubData("1", "dsgsss", "ssfddddd");
            yield return db.UpdateData("1",testData, (response) => {
                Debug.Log(response);
            });

        }
        [UnityTest]
        public IEnumerator GoogleSpreadSheetの指定データを消去する()
        {
            yield return db.DeleatData("1", (response) => {
                Debug.Log(response);
            });

        }
    }
}
