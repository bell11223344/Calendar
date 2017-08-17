<?php

require_once('h_ini.php');
require_once('mysql_connect.php');
$pdo = connectDB();

$id = $_POST["id"];
$password = $_POST["password"];
    
//エラーメッセージ、登録完了メッセージの初期化
$errorMessage = "";
$signUpMessage = "";


try {
    $stmt = $pdo->prepare('INSERT INTO `AccountsTable` (account_id,password) VALUES (?,?)');
    $stmt->execute(array($id, password_hash($password, PASSWORD_DEFAULT)));   // パスワードのハッシュ化を行う（今回は文字列のみなのでbindValue(変数の内容が変わらない)を使用せず、直接excuteに渡しても問題ない）
    $userid = $pdo->lastinsertid();  // 登録した(DB側でauto_incrementした)IDを$useridに入れる
    $signUpMassage = '登録が完了しました。あなたの登録IDは '. $id. 'です。パスワードは '.$password. ' です。';  // ログイン時に使用するIDとパスワード
        
} catch (PDOException $e) {
    $errorMessage = 'データベースエラー';
    // $e->getMassage() でエラー内容を参照可能（デバッグ時のみ表示）
    // echo $e->getMessage();
}

?>

<!DOCTYPE html>
<html lang="ja">
    <head>
        <title>アカウント作成</title>

        <meta charset="UTF-8">
        <meta name="robots" content="noindex,nofollow">

    </head>
<body>
    <h1>新規登録画面</h1>
    <form id="loginForm" name="loginForm" action="" method="POST">
        <fieldset>
            <legend>新規登録フォーム</legend>
            <div><font color="#ff0000"><?=h($errorMessage)?></font></div>
            <div><font color="#0000ff"><?=h($signUpMessage)?></font></div>
            <label for="id">ユーザー名</label><input type="text" id="id" name="id" placeholder="アカウント名を入力" value="<?php if (!empty($_POST["id_name"])) {echo h($_POST["id_name"]);} ?>">
            <br>
            <label for="password">パスワード</label><input type="password" id="password" name="password" value="" placeholder="パスワードを入力">
            <br>
            
            <input type="submit" id="signUp" name="signUp" value="新規登録">
        </fieldset>
    </form>
    <br>
    
    <h1>登録済みアカウント</h1>
    <?php if (isset($stm) && $stm->rowCount()): ?>
        <table border="1">
            <tr>
                <th>アカウント名</th>

            </tr>
        <?php foreach ($stm as $rowall): ?>
        <tr>
    
            <td><?=h($rowall['id'])?></td>

        </tr>
        <?php endforeach; ?>
        </table>
    <?php endif; ?>
</body>
</html>
            