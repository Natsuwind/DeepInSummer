<?php
	include 'include/db_class.php';
        include 'include/pagebase.php';
        include 'include/pageauth.php';
        $pagename = "会员登录";
        $templatefile = "login.tpl";

        if(isset ($_GET['logout'])){
            session_destroy();
            echo 'Logout~';
            echo "<meta http-equiv=refresh content='2; url=index.php'>";
            exit;
        }

        
        if($uid>0){
            echo '<html>';
            echo '<head>';
            echo '<meta http-equiv="Content-Type" content="text/html; charset=utf-8">';
            echo "<meta http-equiv=refresh content='2; url=index.php'>";
            echo '</head>';
            echo '<body>';
            echo '你已经登录了';
            echo '</body>';
            echo '</html>';
            exit;
        }
        else {
            if(isset($_POST["username"]) && isset ($_POST["password"])){

                list($uid, $username, $password, $email) = uc_user_login($_POST['username'], $_POST['password']);
                
                
                if($uid > 0) {
                        //用户登陆成功，设置 Cookie，加密直接用 uc_authcode 函数，用户使用自己的函数
                        $auth = uc_authcode($uid."\t".$username, 'ENCODE');
                        $_SESSION['login'] = $auth;
                        //setcookie('auth', $auth);
                        //生成同步登录的代码
                        //$ucsynlogin = uc_user_synlogin($uid);
                        echo 'Login Success';
                        echo "<meta http-equiv=refresh content='2; url=index.php'>";
                        //echo $ucsynlogin;
                        //echo '<br><a href="'.$_SERVER['PHP_SELF'].'">继续</a>';
                        exit;
                } elseif($uid == -1) {
                        echo '用户不存在,或者被删除';
                } elseif($uid == -2) {
                        echo '密码错';
                } else {
                        echo '未定义';
                }
            }
        }

        include 'include/pagefooter.php';
?>
