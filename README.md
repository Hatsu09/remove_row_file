# 画像データ削除用プログラム
## 概要
以下の条件に合致するファイルのみ抽出する。
1. 拡張子が「.jpg」、「.jpeg」、「.JPG」、「.JPEG」のファイル(サブディレクトリも含む)
2. 1.と同じディレクトリにjpg(jpeg)ファイルと同名のファイル(拡張子は「.jpg」または「.jpeg」ではない)

## 動作環境
Windows 11

※Windows 10以前のバージョンでは動作確認をしていないため、動作確認を行っていません。
※MacやLinuxは対応していません。

## ダウンロード方法
以下のURLを参照
https://docs.github.com/ja/repositories/working-with-files/using-files/downloading-source-code-archives

## 使いかた
1. `DuplicationFilesExstractorApp\\bin\\Debug\\net8.0-windows\\DuplicationFilesExstractorApp.exe` を実行する。
**※MicroSoft Windows Desktop Runtime - 8.x.xのダウンロードが促されることがありますので、指示にしたがってインストールしてください。**
![スクリーンショット 2024-08-12 180340](https://github.com/user-attachments/assets/021ef926-9a38-4762-8d2a-0649101f4478)

3. 「ファイル選択ボタン」を押下し、削除したいデータを含むディレクトリを選択
4. 「重複ファイル名以外を削除する」ボタンを押す。
