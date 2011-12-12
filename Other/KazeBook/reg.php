<?php
	include 'include/db_class.php';
        include 'include/pagebase.php';
        $pagename = "会员注册";
        $templatefile = "reg.tpl";
	
	$isreg = $_POST["isreg"];
	if($isreg==1)
	{
		if(!isset ($_POST["username"])||!isset ($_POST["password"]))
		{
			echo "用户名或密码为空";
			exit;
		}
                
		$dbhelper->connect();

		$username = mysql_real_escape_string(htmlspecialchars($_POST["username"]));
		$password = $_POST["password"];

		$exists = $dbhelper->ExecuteArray("SELECT uid FROM members WHERE username='$username'");
		
		if(isset ($exists))
		{
			$success = $username."用户已存在,请换一个用户名注册";
		}
		else
		{
			$result = $dbhelper->Execute("INSERT INTO members(username,password) VALUES('$username','".md5($password)."')");
			if($result)
			{
				$success = "success!";
			}
			else
			{
				$success = mysql_error();
			}
		}
	}	
	$smarty->assign("isreg", $isreg);
	$smarty->assign("success", $success);
        
        include 'include/pagefooter.php';
?>