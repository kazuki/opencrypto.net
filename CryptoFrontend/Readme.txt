CryptoFrontend (Frontend of openCrypto.NET)
            Copyright(C) 2008 Kazuki Oikawa

* 概要 *
========
 openCrypto.NETという暗号ライブラリのフロントエンドです。
 手軽に楕円曲線暗号を試すことが出来ます。

 現在、楕円曲線暗号を用いた暗号方式(ECIES)を利用する
 ことが出来ます。公開鍵暗号なので、公開鍵でテキストを
 暗号化し、秘密鍵で暗号化されたテキストを復号します。
 ファイルの暗号化はサポートしていないため、テキスト
 ボックスに収まる程度のテキストのみの暗号化に対応して
 います。

 また、SECGのSEC1という規格では排他的論理和を用いた
 暗号しか規格化されていませんが、このソフトでは独自に
 CamelliaやRijndael(AES)の選択が可能となっています。
 この場合、ECIES内部で利用するKDFを利用して対称鍵を
 生成し、IVは全てゼロ、CBCモード、PKCS#1パディングで
 暗号化を行います。
 CamelliaやRijndaelを用いた暗号化は独自規格であるため
 将来的に復号できなくなる可能性がありますので、その点
 ご注意ください。なお、本プログラムでも、IVの扱いに
 ついて将来的に変更する予定で、その際には暗号文の
 互換性が失われる予定です。

* 鍵ストアの位置 *
==================
 秘密鍵や公開鍵はユーザプロファイル内のアプリケーション
 データディレクトリに保存されています。
 Windows 2000やXPでは
   C:\Documents and Settings\[UserName]\ApplicationData\openCrypto.NET
 Windows Vistaでは
   C:\Users\[UserName]\AppData\Roaming\openCrypto.NET
 ディレクトリです。アンインストールする際は
 このディレクトリを削除してください。

* ライセンス *
==============
 openCrypto.NETと同じく修正BSDライセンスに基づきます
