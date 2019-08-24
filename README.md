# QitzSteinDBForUnity とは？

GoogleSpreadSheetをデータベースがわりに使用できる
「Stein」
https://steinhq.com/
のUnity向けのラッパーです。
パフォーマンスは出なさげですが、個人開発でちょっとしたDBを簡単に入れたいときや、
アプリのモックやアルファ版などでStubのDBを入れておきたい時などに役立つかなと思います！
（ちゃんとFactoryパターンをかませてあるので本番DBにも容易に差し替えられるような構造で組んでありますよ〜〜〜〜）

# 導入方法

##まずはGoogleSpreadSheetを作成！

とにもかくにもシートを作成します！
カラムはこんな感じにしときます！
![カラム図](https://i.gyazo.com/69a7bcb0cb98c605b296db81fa24b72e.png "カラム")<br>
**idのカラム必須です。**

##Steinのアカウントを作ります。
![アカウント](https://i.gyazo.com/e4d6a95b15cc31b1abb7d39616684b48.png "アカウント")<br>
アカウント作成を選択
アカウント作成が完了するとこのようにapiを取得できるようになります
![アカウント](https://i.gyazo.com/e714f41414e63fa55bbb0df893e99a5f.png "アカウント")<br>

これで利用準備ができました！！！超簡単！

##パッケージのインスコ

Archives中の以下パッケージから導入が可能です！
https://github.com/QitzPub/QitzSteinDB/raw/master/Archives/QitzSteinDBPlugin.unitypackage

パッケージからUnityへインスコします〜〜！<br>
![インスコ図](https://i.gyazo.com/33c9b746a8ee226278a1b6d4a43cffce.png "インスコ")<br>



