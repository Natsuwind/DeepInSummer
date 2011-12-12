<?php
/**
*数据库类
*/    
class DBHelper
{
  var $pConnect=FALSE;//是否使用长连接
  var $mHost;//数据库主机
  var $mDatabase;    
  var $db; //数据库
  var $charset; //数据库编码
  var $mUser;//数据库用户名
  var $mPwd;//数据库用户密码
  var $mConn;//连接标识
  var $result;// 执行query命令的结果资源标识
  var $num_rows;// 返回的条目数
  var $insert_id;// 传回最后一次使用 INSERT 指令的 ID
  var $affected_rows;// 传回query命令所影响的列数目
                             // INSERT、UPDATE 或 DELETE 所影响的列 (row) 数目。
                             // delete 如果不带where，那么则返回0
  //构造函数
  public function __construct($host,$user,$pwd,$db,$charset)
  {
    $this->mHost=$host;
    $this->mUser=$user;
    $this->mPwd=$pwd;
    $this->db=$db;
    $this->charset = $charset;
  }            
    
  //数据库连接                
  public function connect()
  {
    if($this->pConnect)
    $this->mConn=mysql_pconnect($this->mHost,$this->mUser,$this->mPwd);//长连接
    else
    $this->mConn=mysql_connect($this->mHost,$this->mUser,$this->mPwd);//short connect

    if(!$this->mConn) $this->dbhalt("不能连接数据库!");
    if($this->db=="") $this->db=$this->dbDatabase;
    
    if(!mysql_select_db($this->db,$this->mConn))
    $this->dbhalt("数据库不可用!");
  } // eof#dbconnect()

  //更改数据库
  public function dbChange($db){
    $this->db=$db;
    $this->connect();
  }

  //执行SQL语句,返回结果资源id
  public function execute($sql){
      mysql_query("SET NAMES $this->charset");
      $this->result=mysql_query($sql);
      return $this->result;
  }

  public function ExecuteArray($sql){
      $this->result = $result=$this->execute($sql,$dbbase);
      while ($row = mysql_fetch_array($this->result)){
		$data[]=$row;
	}
      return $data;
  }

  //获取数组-索引和关联
  public function fetchArray($resultType=MYSQL_BOTH)
  {
    return mysql_fetch_array($this->result,$resultType);
  }
    
  //获取关联数组
  public function fetchAssoc()
  {
    return mysql_fetch_assoc($this->result);
  }    
    
  //获取数字索引数组
  public function fetchIndexArray()
  {
    return mysql_fetch_row($this->result);
  }
    
  //获取对象数组
  public function fetchObject()
  {
    return mysql_fetch_object($this->result);
  }        
    
  //返回记录行数
  function numRows()
  {
    return mysql_num_rows($this->result);
  }

  //返回主机中所有数据库名
  public function dbNames()
  {
    $rsPtr=mysql_list_dbs($this->mConn);
    $i=0;
    $cnt=mysql_num_rows($rsPtr);
    while($i<$cnt)
    {
      $rs[]=mysql_db_name($rsPtr,$i);
      $i++;
    }
    return $rs;
  }

  function dbhalt($errmsg){
    $msg="数据库有问题!";
    $msg=$errmsg;
    echo"$msg";
    die();
  }

  //删
  function delete($sql){
    $result=$this->execute($sql,$dbbase);
    $this->affected_rows=mysql_affected_rows($this->dbLink);
    $this->free_result($result);
    return $this->affected_rows;
  }

  //增
  function insert($sql){
    $result=$this->execute($sql,$dbbase);
    $this->insert_id=mysql_insert_id($this->dbLink);
    $this->free_result($result);
    return $this->insert_id;
  }

  //改
  function update($sql){
    $result=$this->execute($sql,$dbbase);
    $this->affected_rows=mysql_affected_rows($this->dbLink);
    $this->free_result($result);
    return $this->affected_rows;
  }
    
  //关闭连接
  function dbclose(){
    mysql_close($this->dbLink);
  }
}// end class
?> 