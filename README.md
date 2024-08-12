# 画像データ削除用プログラム
## 概要
以下の条件に合致するファイルのみ抽出する。
1. 拡張子が「.jpg」または「.jpeg」のファイル(サブディレクトリも含む)
2. 1.と同じディレクトリにjpg(jpeg)ファイルと同名のファイル(拡張子は「.jpg」または「.jpeg」ではない)

抽出したファイルは、指定したディレクトリと同じディレクトリにコピーされる。

## 使いかた
1. `DuplicationFilesExstractorApp\\bin\\Debug\\net8.0-windows\\DuplicationFilesExstractorApp.exe` を実行する。
**※.Netのダウンロードが促されることがありますので、指示にしたがってインストールしてください。**
2. 「ファイル選択ボタン」を押下し、削除したいデータを含むディレクトリを選択
3. 「重複ファイル名以外を削除する」ボタンを押す。
