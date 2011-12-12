<?php
    include 'include/db_class.php';   

    if(!isset ($_POST["username"])){
        echo 'please input nickname';
        exit;
    }

    if(!isset ($_POST["content"])){
        echo 'please input content';
        exit;
    }

    $dbhelper->connect();

    $postuser = mysql_real_escape_string(htmlspecialchars($_POST["username"]));
    $content = mysql_real_escape_string(htmlspecialchars($_POST["content"]));
    $postdate = date("Y-m-d");

    $sql = "INSERT INTO posts(uid,postuser,content,postdate) VALUES(1,'$postuser','$content','$postdate')";
    $result = $dbhelper->execute($sql);
    if($result){
        echo "<meta http-equiv=refresh content='0; url=index.php'>";
    }
    else{
        echo mysql_errno();
        exit;
    }
?>
