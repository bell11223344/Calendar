<?php

require_once('mysql_connect.php');
$pdo = connectDB();

$id = $_POST["Id"];
$password = $_POST["Password"];

try{
    //$stmt = $pdo->query("SELECT * FROM `AccountsTable` WHERE `account_id` = '".$id."' AND `password` = '".$password."'" );
    $stmt = $pdo->prepare('SELECT * FROM `AccountsTable` WHERE `account_id` = ?');
    $stmt->execute(array($id));
    
    foreach ($stmt as $row) {
        //if($row["account_id"] == $id && password_verify($password, $row['password'])){
        if(password_verify($password, $row['password'])){
            $res = "Success";
        }
    }
    /*
    if($stmt != null){
        $res = "Success";
    }
    */
    /*
    foreach ($stmt as $row) {
        $res .= $row['naiyou'];
        $res .= "(担当者：" . $row['tantou'] . ")\n";
    }*/
} catch (PDOException $e) {
    var_dump($e->getMessage());
}
$pdo = null;

echo ($res);

?>