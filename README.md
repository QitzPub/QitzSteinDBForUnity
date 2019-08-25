# QitzSteinDBForUnity とは？

**GoogleSpreadSheetを**<br>
データベースがわりに使用できる<br>
「Stein」<br>
https://steinhq.com/
<br>のUnity向けのラッパーです。<br>
パフォーマンスは出なさげですが、個人開発でちょっとしたDBを簡単に入れたいときや、<br>
アプリのモックやアルファ版などでStubのDBを入れておきたい時などに役立つかなと思います！<br>
（ちゃんとFactoryパターンをかませてあるので本番DBにも容易に差し替えられるような構造で組んでありますよ〜〜〜〜）<br>

# 導入方法

## まずはGoogleSpreadSheetを作成！

とにもかくにもシートを作成します！
カラムはこんな感じにしときます！
![カラム図](https://i.gyazo.com/69a7bcb0cb98c605b296db81fa24b72e.png "カラム")<br>
<br>
**idのカラム必須です。**

## Steinのアカウントを作ります。

![アカウント](https://i.gyazo.com/e4d6a95b15cc31b1abb7d39616684b48.png "アカウント")<br>
アカウント作成を選択<br>
アカウント作成が完了するとこのようにapiを取得できるようになります<br>
<br>
![アカウント](https://i.gyazo.com/e714f41414e63fa55bbb0df893e99a5f.png "アカウント")<br>

これで利用準備ができました！！！超簡単！<br>

## パッケージのインスコ

Archives中の以下パッケージから導入が可能です！<br>
https://github.com/QitzPub/QitzSteinDB/raw/master/Archives/QitzSteinDBPlugin.unitypackage

パッケージからUnityへインスコします〜〜！<br>
![インスコ図](https://i.gyazo.com/33c9b746a8ee226278a1b6d4a43cffce.png "インスコ")<br>


##  使い方

```C#
using Qitz.SteinDB;
```
を入れます。

### DBからの受け口用のクラスを作成しておきます

受け口用のクラスはAEntityを継承しておきます。<br>

#### AEntity構造

```C#
    [Serializable]
    public abstract class AEntity
    {
        [SerializeField]
        protected string id;
        public string ID => id;
    }
```


また、　DBのカラム名と対応するフィールドに<br>
[SerializeField]かpublicをつけておきます。<br>
(idフィールドはAEntity中にあります)

```C#
    public class StubData: AEntity
    {
        [SerializeField]
        string discription;
        public string Discription => discription;

        [SerializeField]
        string name;
        public string Name => name;


        public StubData(string id, string name, string discription)
        {
            this.id = id;
            this.discription = discription;
            this.name = name;
        }

    }
```

##  実際の読み込み方法例


### インスタンス作成

#### 構造
```C#
IDB<T> Create<T>(string steinApiUrl, string googleSpreadSheetName) where T : AEntity
```

string steinhqApiUrlにはapiのurlをいれます。<br>
![インスコ図](https://i.gyazo.com/58e004f245abf906250bf1bb52b28404.png "インスコ")<br>
<br>
string targetSheetNameにはGoogleシートのシート名をいれます。<br>
![インスコ図](https://i.gyazo.com/ab2a1d70a9f2b7c49068dc229f13bbf6.png "インスコ")<br>
<br>

#### 使い方例
```C#
IDB<StubData> db = SteinDBFactory.Create<StubData>("https://api.steinhq.com/v1/storages/5d6093ecbb4eaf04c5eaa2b5", "test_data");
```

### GoogleSpreadSheetからデータを取得する

#### 構造
```C#
IEnumerator GetData(Action<List<T>> callBack);
```
ジェネリックで指定した型で値が取得できます！

#### 使い方例
```C#
        public IEnumerator GoogleSpreadSheetからデータを取得するテスト()
        {
            var db = SteinDBFactory.Create<StubData>("https://api.steinhq.com/v1/storages/5d6093ecbb4eaf04c5eaa2b5", "test_data");
            yield return  db.GetData((data)=> {
                foreach (var d in data)
                {
                    Debug.Log(d.ID);
                    Debug.Log(d.Name);
                }
            });

        }
```

### GoogleSpreadSheetにデータをAddする

#### 構造
```C#
IEnumerator AddData(T addData,Action<string> response);
```
ジェネリックで指定した型でDBにレコードを追加できます。

#### 使い方例
```C#
        public IEnumerator GoogleSpreadSheetにダミーデータをPostするテスト()
        {
            var db = SteinDBFactory.Create<StubData>("https://api.steinhq.com/v1/storages/5d6093ecbb4eaf04c5eaa2b5", "test_data");
            var testData = new StubData("1","zzzzsss","gggsasa");
            yield return db.AddData(testData,(response)=> {
                Debug.Log(response);
            });

        }
```

### GoogleSpreadSheetのデータを上書き更新するテスト

#### 構造
```C#
IEnumerator UpdateData(string id,T upDateData, Action<string> response);
```
idで指定したレコードを書き換えます。

#### 使い方例
```C#
        public IEnumerator GoogleSpreadSheetのデータを上書き更新するテスト()
        {
            var db = SteinDBFactory.Create<StubData>("https://api.steinhq.com/v1/storages/5d6093ecbb4eaf04c5eaa2b5", "test_data");
            var testData = new StubData("1", "dsgsss", "ssfddddd");
            yield return db.UpdateData("1",testData, (response) => {
                Debug.Log(response);
            });

        }
```


### GoogleSpreadSheetの指定データを消去する

#### 構造
```C#
IEnumerator DeleatData(string id, Action<string> response);
```
idで指定したレコードを消去できます。


#### 使い方例
```C#
        public IEnumerator GoogleSpreadSheetの指定データを消去する()
        {
            var db = SteinDBFactory.Create<StubData>("https://api.steinhq.com/v1/storages/5d6093ecbb4eaf04c5eaa2b5", "test_data");
            yield return db.DeleatData("1", (response) => {
                Debug.Log(response);
            });

        }
```






