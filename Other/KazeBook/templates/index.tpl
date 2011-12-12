{include file="header.tpl"}
        {include file="sidebar.tpl"}

	<div id="post_list">
            <!---留言框开始--->
                <form action="post.php" method="post">
                    <ul>
                        <li>留言内容：<textarea id="content" name="content"></textarea></li>
                        <li>
                        {if $uid<1}
                            昵&nbsp;&nbsp;&nbsp;称&nbsp;：<input type="text" id="username" name="username" value="游客" />
                        {/if}
                            <input type="submit" value="提交留言" />
                        </li>
                    </ul>
                </form>
            <!---留言框结束--->

            <!---留言列表开始--->
		{section name=post loop=$postlist}
                    {if $smarty.section.post.first}
                        <hr />
                    <!---留言列表UL开始--->
                        <ul class="post">
                    {/if}

                    <!---单个留言开始--->
                    {if $smarty.section.post.index%2===0}
                            <li class="silver">
                    {else}
                            <li class="gray">
                    {/if}
                                <span class="postuser">{$postlist[post].postuser}</span>：<span class="content">{$postlist[post].content}</span>
                                <div class="postdate">留言于{$postlist[post].postdate}
                            {if $postlist[post].replyuid==0}
                                [<a href="reply.php?pid={$postlist[post].pid}">回复</a>]
                            {/if}
                                </div>

                            {if $postlist[post].replyuid!=0}
                            <!---留言回复开始--->
                                <ul class="re">
                                {if $smarty.section.post.index%2===0}
                                        <li class="gray">
                                {else}
                                        <li class="silver">
                                {/if}
                                        <span class="postuser">{$postlist[post].replyuser}</span>：<span class="content">{$postlist[post].replycontent}</span>
                                        <div class="postdate">回复于{$postlist[post].replydate}[<a href="reply.php?pid={$postlist[post].pid}">编辑</a>]</div>
                                    </li>
                                </ul>
                            <!---留言回复结束--->
                            {/if}

                            </li>
                    <!---单个留言结束--->
                    {if $smarty.section.post.last}
                    <!---留言列表UL结束--->
                        </ul>
                        <hr />
                        <!---总共有{$smarty.section.post.total}条留言.--->
                        <div class="pager">
                            <a href="index.php?page=1">首页</a>
                            |<a href="index.php?page={$pre}">上一页</a>
                            |<a href="index.php?page={$next}">下一页</a>
                            |<a href="index.php?page={$pagecount}">末页</a>
                            |共{$pagecount}页
                        </div>
                    {/if}
		{/section}
            <!---留言框结束--->

	</div>
{include file="footer.tpl"}