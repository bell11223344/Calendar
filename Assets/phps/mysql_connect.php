<?php

function connectDB() {

    $dsn = 'mysql:host=mysql330.db.sakura.ne.jp;dbname=nakap_calendar;charset=utf8';
    $username = 'nakap';
    $password = 'nakamura0020';
    
    try {
        $pdo = new PDO($dsn, $username, $password);
    } catch (PDOException $e) {
        exit('' . $e->getMessage());
    }
    
    return $pdo;

}    