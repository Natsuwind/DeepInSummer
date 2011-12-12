<?php
        include 'include/pageauth.php';

        if($isadmin != 1){
            session_destroy();
            echo '请登录管理员帐号进行回复';
            echo "<meta http-equiv=refresh content='2; url=login.php'>";
            exit;
        }

	include 'include/db_class.php';
        include 'include/pagebase.php';
        $pagename = "回复留言";
        $templatefile="reply.tpl";

        $pid = intval($_GET["pid"]);
        if(!$pid){
            echo '参数不能为空!';
            exit ;
        }

	$dbhelper->connect();
        if(!isset ($_POST["replycontent"])){
            $postinfo = $dbhelper->ExecuteArray("SELECT * FROM posts WHERE pid=".$pid);

            $smarty->assign("postinfo", $postinfo);
        }
        else{
            $replycontent = mysql_real_escape_string(htmlspecialchars($_POST["replycontent"]));
            $replyuser = mysql_real_escape_string($_SESSION["login"]);
            $replydate = date("Y-m-d");
            $sql="UPDATE posts SET replyuid=2,replyuser='$replyuser',replycontent='$replycontent',replydate='$replydate' WHERE pid=$pid";
            $result = $dbhelper->execute($sql);
            if($result){
                echo "<meta http-equiv=refresh content='0; url=index.php'>";
            }
            else{
                echo mysql_errno();
                exit;
            }
        }
        include 'include/pagefooter.php';

?>
