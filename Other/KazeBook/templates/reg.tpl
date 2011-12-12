{include file="header.tpl"}
        {include file="sidebar.tpl"}
	<div>
		{$isreg}
		{if $isreg!=1}
		<form action="reg.php" method="post">
			<input type="hidden" name="isreg" id="isreg" value="1" />
			<ul>
				<li>User Name: <input type="text" name="username" id="username" /></li>
				<li>Password : <input type="password" name="password" id="password" /></li>
				<li><input type="submit" value="提交" /></li>
			</ul>
		</form>
		{else}
			{$success}
		{/if}
	</div>
{include file="footer.tpl"}