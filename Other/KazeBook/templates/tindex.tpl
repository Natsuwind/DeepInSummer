<html>
<head>
	<title>{$appname}</title>
</head>
{* Smarty *}

hello, {$name}!<br />

{$smarty.server.SERVER_NAME}<br />

{$smarty.now|date_format:"%Y-%m-%d %H:%M:%S"}<br /><br />

{foreach name=outer item=contact from=$contacts}
	{foreach key=key item=item from=$contact}
		{$key}: {$item}<br>
	{/foreach}
	<hr />
{/foreach}
