<?php
	require 'lib/smarty/Smarty.class.php';
        include './config.inc.php';
        include './uc_client/client.php';
	
	class _Smarty extends Smarty{
		
		function _Smarty(){
		
		$this->Smarty();
		
		//$this->template_dir = "";
		$this->compile_dir = "cache/template";
		//$this->config_dir = "";
		//$this->cache_dir = "";
		
		$this->caching = false;
		$this->assign("appname", "Guest Book");
		}
	}
?>