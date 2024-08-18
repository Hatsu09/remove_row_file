# 画像データ削除用プログラム

## 概要

以下の条件に合致するファイルのみ抽出する。詳細は、以下の「重複ファイル名以外を削除するボタン押下時の動作について」の項目を参照。

1. 選択したディレクトリ配下のファイル拡張子が以下のもの(サブディレクトリのファイル対象)

   - jpg ファイル(.jpeg .jpg .JPG .JPEG)

2. 1.で対象になったファイルと同じディレクトリに存在する同名のファイル(拡張子以外)

## 動作環境

Windows 11

- Windows 10 以前のバージョンでは動作確認をしていないため、動作確認を行っていません。
- Mac や Linux は対応していません。

## ダウンロード方法

以下の URL を参照

https://docs.github.com/ja/repositories/working-with-files/using-files/downloading-source-code-archives

## 使いかた

1. `DuplicationFilesExstractorApp\\bin\\Debug\\net8.0-windows\\DuplicationFilesExstractorApp.exe` を実行する。

   **※MicroSoft Windows Desktop Runtime - 8.x.x のダウンロードが促されることがありますので、指示にしたがってインストールしてください。**

   ![スクリーンショット 2024-08-12 180340](https://github.com/user-attachments/assets/021ef926-9a38-4762-8d2a-0649101f4478)

2. 「ファイル選択ボタン」を押下し、削除したいデータを含むディレクトリを選択
3. 「重複ファイル名以外を削除する」ボタンを押す。

## 重複ファイル名以外を削除するボタン押下時の動作について

1. ユーザが選択したディレクトリと同じ階層に「work」ディレクトリを作成する。<br>
   すでに 「work」 ディレクトリが存在する場合、「work_xx」のように重複しないようにディレクトリを作成<br>
   ※`xx` は数字

2. ユーザが選択したディレクトリとそのサブディレクトリに対して、対応する拡張子のファイルとファイル名が重複するファイルを 「work」 ディレクトリにコピーする。
3. ユーザが選択したディレクトリを削除(ゴミ箱へ移動)する。<br>
   ※この時、抽出対象のファイルが存在しない(「work」 ディレクトリにファイルもディレクトリも入っていない)場合、「work」ディレクトリを削除して、終了する。
4. work ディレクトリを選択したディレクトリと同じフォルダー名になるようにリネームする。
