<?php
        $uid = 0;
        $username = '';
        $isadmin = 0;
        
        session_start();
        if(isset ($_SESSION["login"])){
            list($uid, $username) = explode("\t", uc_authcode($_SESSION['login'], 'DECODE'));
        }

        if(isset($_SESSION["admin"])){
            $isadmin = 1;
        }
?>
