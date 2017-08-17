<?php

require_once('mysql_connect.php');
$pdo = connectDB();

$year = $_POST["Year"];
$month = $_POST["Month"];
$day = $_POST["Day"];

try{
    $stmt = $pdo->query("SELECT * FROM `yotei` WHERE `year` = '".$year."' AND `month` = '".$month."' AND `day` = '".$day."'" );
    foreach ($stmt as $row) {
        $res .= $row['naiyou'];
        $res .= "(担当者：" . $row['tantou'] . ")\n";
    }
} catch (PDOException $e) {
    var_dump($e->getMessage());
}
$pdo = null;
echo $res;

?>