<?php
	include 'include/db_class.php';
        include 'include/pagebase.php';
        include 'include/pageauth.php';
        $pagename = "首页";
        $templatefile = "index.tpl";
	
	$dbhelper->connect();

        $pagesize = 10;  //定义每页显示多少条记录
        $page = isset($_GET["page"])?intval($_GET["page"]):1;   //定义page的初始值,如果get 传过来的page为空,则page=1,
        $total = mysql_result($dbhelper->Execute("SELECT count(pid) as total from posts"),0,"total");  //执行查询获取总记录数
        $pagecount = ceil($total/$pagesize);  //计算出总页数
        if ($page > $pagecount){
            $page = $pagecount;  // 对提交过来的page做一些检查
        }
        if ($page <= 0){
            $page = 1;                   // 对提交过来的page做一些检查
        }
        $offset = ($page-1)*$pagesize;   //偏移量
        $pre = $page>1?$page-1:1;           //上一页
        $next = $page<$pagecount?$page+1:$pagecount;         //下一页



	$postlist = $dbhelper->ExecuteArray("SELECT * FROM posts ORDER BY pid DESC limit $offset,$pagesize");
	
	$smarty->assign("postlist", $postlist);
        $smarty->assign("pagecount", $pagecount);
        $smarty->assign("uid", $uid);
        $smarty->assign("username", $username);
        $smarty->assign("pre", $pre);
        $smarty->assign("next", $next);
        include 'include/pagefooter.php';
?>