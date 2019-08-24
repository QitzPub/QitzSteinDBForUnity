using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Qitz.SteinhqDB;
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
        IDB<StubData> db = SteinhqDBFactory.Create<StubData>("https://api.steinhq.com/v1/storages/5d6093ecbb4eaf04c5eaa2b5", "test_data");
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SteinhqDBからデータを取得するテスト()
        {

            yield return  db.GetData((data)=> {
                foreach (var d in data)
                {
                    Debug.Log(d.ID);
                    Debug.Log(d.Name);
                }
            });

            yield return null;
        }

        [UnityTest]
        public IEnumerator SteinhqDBにダミーデータをPostするテスト()
        {
            var testData = new StubData("1","zzzzsss","gggsasa");
            yield return db.AddData(testData,(response)=> {
                Debug.Log(response);
            });

            yield return null;
        }

        [UnityTest]
        public IEnumerator SteinhqDBにのデータを上書き更新するテスト()
        {
            var testData = new StubData("1", "dsgsss", "ssfddddd");
            yield return db.UpdateData("1",testData, (response) => {
                Debug.Log(response);
            });

            yield return null;
        }
        [UnityTest]
        public IEnumerator SteinhqDBの指定データを消去する()
        {
            yield return db.DeleatData("1", (response) => {
                Debug.Log(response);
            });

            yield return null;
        }
    }
}
