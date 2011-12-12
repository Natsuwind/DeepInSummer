<?php
    if(!isset ($_COOKIE["TestCookie"])){
        setcookie("TestCookie", "aaaa");
        echo 'cookie set';
    }
    else{
        echo $_COOKIE['TestCookie'];
    }
?>
