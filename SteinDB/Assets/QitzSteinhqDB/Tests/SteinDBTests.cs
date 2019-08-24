using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Qitz.SteinhqDB;
using System;

namespace Tests
{
    [Serializable]
    public class TestClassObejct
    {
        [SerializeField]
        int id;
        public int Id { get { return id; } }

        [SerializeField]
        string name;
        public string Name { get { return name; } }

        [SerializeField]
        string discription;
        public string Discription { get { return discription; } }
    }

    public class SteinDBTests
    {

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SteinhqDBからデータを取得するテスト()
        {
            var db = SteinhqDBFactory.Create("https://api.steinhq.com/v1/storages/5d6093ecbb4eaf04c5eaa2b5", "test_data");
            yield return  db.GetData<TestClassObejct>((data)=> {
                foreach (var d in data)
                {
                    Debug.Log(d.Name);
                }
            });

            yield return null;
        }
    }
}
