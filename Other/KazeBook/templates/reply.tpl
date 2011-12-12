{include file="header.tpl"}
        {include file="sidebar.tpl"}
        <div id="post_list">
            <ul class="post">
                <li class="gray">
                    <span class="postuser">{$postinfo[0].postuser}</span>：<span class="content">{$postinfo[0].content}</span>
                    <div class="postdate">留言于{$postinfo[0].postdate}</div>
                </li>
                <li class="silver">
                    <form action="" method="post">
                        内容：<textarea id="replycontent" name="replycontent">{$postinfo[0].replycontent}</textarea><br />
                        <input type="submit" value="回复/修改" />
                    </form>
                </li>
            </ul>
        </div>

{include file="footer.tpl"}