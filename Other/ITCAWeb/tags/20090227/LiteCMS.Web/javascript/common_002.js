// 选项卡
function secBoard(elementID,listName,n,op) {
	var elem = document.getElementById(elementID);
	var elemlist = elem.getElementsByTagName("li");
	for (var i=0; i<elemlist.length; i++) {
		elemlist[i].className = "normal";
		var m = i+1;
		document.getElementById(listName+"_"+m).className = "normal";	
	}
		elemlist[n-1].className = "current";
		document.getElementById(listName+"_"+n).className = "current";
	if(op != null) {
		var x = new Ajax('XML', 'statusid');
		x.get(siteUrl+'/batch.secboard.php?action='+op, function(s){
			if(s!=null) {
				document.getElementById(listName+"_"+n).innerHTML = s;
			}
		})
	}
}

// 焦点轮换图片
function addLoadEvent(func){
	var oldonload = window.onload;
	if (typeof window.onload != 'function') {
		window.onload = func;
	} else {
		window.onload = function(){
			oldonload();
			func();
		}
	}
}

function moveElement(elementID,final_x,final_y,interval) {
  if (!document.getElementById) return false;
  if (!document.getElementById(elementID)) return false;
  var elem = document.getElementById(elementID);
  if (elem.movement) {
    clearTimeout(elem.movement);
  }
  if (!elem.style.left) {
    elem.style.left = "0px";
  }
  if (!elem.style.top) {
    elem.style.top = "0px";
  }
  var xpos = parseInt(elem.style.left);
  var ypos = parseInt(elem.style.top);
  if (xpos == final_x && ypos == final_y) {
		return true;
  }
  if (xpos < final_x) {
    var dist = Math.ceil((final_x - xpos)/10);
    xpos = xpos + dist;
  }
  if (xpos > final_x) {
    var dist = Math.ceil((xpos - final_x)/10);
    xpos = xpos - dist;
  }
  if (ypos < final_y) {
    var dist = Math.ceil((final_y - ypos)/10);
    ypos = ypos + dist;
  }
  if (ypos > final_y) {
    var dist = Math.ceil((ypos - final_y)/10);
    ypos = ypos - dist;
  }
  elem.style.left = xpos + "px";
  elem.style.top = ypos + "px";
  var repeat = "moveElement('"+elementID+"',"+final_x+","+final_y+","+interval+")";
  elem.movement = setTimeout(repeat,interval);
}

function classNormal(focus_turn_btn,focus_turn_tx){
	var focusBtnList = $(focus_turn_btn).getElementsByTagName('li');
	var focusTxList = $(focus_turn_tx).getElementsByTagName('li');
	for(var i=0; i<focusBtnList.length; i++) {
		focusBtnList[i].className='normal';
		focusTxList[i].className='normal';
	}
}

function classCurrent(focus_turn_btn,focus_turn_tx,n){
	var focusBtnList = $(focus_turn_btn).getElementsByTagName('li');
	var focusTxList = $(focus_turn_tx).getElementsByTagName('li');
	focusBtnList[n].className='current';
	focusTxList[n].className='current';
}

function newsfocusChange() {
	if(!$('news_focus_turn')||!$('news_focus_turn_btn')) return;
	$('news_focus_turn').onmouseover = function(){atuokey = true};
	$('news_focus_turn').onmouseout = function(){atuokey = false};
	var focusBtnList = $('news_focus_turn_btn').getElementsByTagName('li');
	if(!focusBtnList||focusBtnList.length==0) return;
	var listLength = focusBtnList.length;
		focusBtnList[0].onmouseover = function() {
			moveElement('news_focus_turn_picList',0,0,5);
			classNormal('news_focus_turn_btn','news_focus_turn_tx');
			classCurrent('news_focus_turn_btn','news_focus_turn_tx',0)
		}
	if (listLength>=2) {
		focusBtnList[1].onmouseover = function() {
			moveElement('news_focus_turn_picList',-400,0,5);
			classNormal('news_focus_turn_btn','news_focus_turn_tx');
			classCurrent('news_focus_turn_btn','news_focus_turn_tx',1)
		}
	}
	if (listLength>=3) {
		focusBtnList[2].onmouseover = function() {
			moveElement('news_focus_turn_picList',-800,0,5);
			classNormal('news_focus_turn_btn','news_focus_turn_tx');
			classCurrent('news_focus_turn_btn','news_focus_turn_tx',2)
		}
	}
	if (listLength>=4) {
		focusBtnList[3].onmouseover = function() {
			moveElement('news_focus_turn_picList',-1200,0,5);
			classNormal('news_focus_turn_btn','news_focus_turn_tx');
			classCurrent('news_focus_turn_btn','news_focus_turn_tx',3)
		}
	}
}

setInterval('newsautoFocusChange()', 5000);
var atuokey = false;
function newsautoFocusChange() {
	if(!$('news_focus_turn_btn')) return;
	if(atuokey) return;
	var focusBtnList = $('news_focus_turn_btn').getElementsByTagName('li');
	var listLength = focusBtnList.length;
	for(var i=0; i<focusBtnList.length; i++) {
		if (focusBtnList[i].className == 'current') var currentNum = i;
	}
	if (currentNum==0&&listLength!=1 ){
		moveElement('news_focus_turn_picList',-400,0,5);
		classNormal('news_focus_turn_btn','news_focus_turn_tx');
		classCurrent('news_focus_turn_btn','news_focus_turn_tx',1)
	}
	if (currentNum==1&&listLength!=2 ){
		moveElement('news_focus_turn_picList',-800,0,5);
		classNormal('news_focus_turn_btn','news_focus_turn_tx');
		classCurrent('news_focus_turn_btn','news_focus_turn_tx',2)
	}
	if (currentNum==2&&listLength!=3 ){
		moveElement('news_focus_turn_picList',-1200,0,5);
		classNormal('news_focus_turn_btn','news_focus_turn_tx');
		classCurrent('news_focus_turn_btn','news_focus_turn_tx',3)
	}
	if (currentNum==3){
		moveElement('news_focus_turn_picList',0,0,5);
		classNormal('news_focus_turn_btn','news_focus_turn_tx');
		classCurrent('news_focus_turn_btn','news_focus_turn_tx',0)
	}
	if (currentNum==1&&listLength==2 ){
		moveElement('news_focus_turn_picList',0,0,5);
		classNormal('news_focus_turn_btn','news_focus_turn_tx');
		classCurrent('news_focus_turn_btn','news_focus_turn_tx',0)
	}
	if (currentNum==2&&listLength==3 ){
		moveElement('news_focus_turn_picList',0,0,5);
		classNormal('news_focus_turn_btn','news_focus_turn_tx');
		classCurrent('news_focus_turn_btn','news_focus_turn_tx',0)
	}
}

function indexfocusChange() {
	if(!$('index_focus_turn')||!$('index_focus_turn_btn')) return;
	$('index_focus_turn').onmouseover = function(){atuokey = true};
	$('index_focus_turn').onmouseout = function(){atuokey = false};
	var focusBtnList = $('index_focus_turn_btn').getElementsByTagName('li');
	if(!focusBtnList||focusBtnList.length==0) return;
	var listLength = focusBtnList.length;
		focusBtnList[0].onmouseover = function() {
			moveElement('index_focus_turn_picList',0,0,5);
			classNormal('index_focus_turn_btn','index_focus_turn_tx');
			classCurrent('index_focus_turn_btn','index_focus_turn_tx',0);
		}
	if (listLength>=2) {
		focusBtnList[1].onmouseover = function() {
			moveElement('index_focus_turn_picList',0,-225,5);
			classNormal('index_focus_turn_btn','index_focus_turn_tx');
			classCurrent('index_focus_turn_btn','index_focus_turn_tx',1);
		}
	}
	if (listLength>=3) {
		focusBtnList[2].onmouseover = function() {
			moveElement('index_focus_turn_picList',0,-450,5);
			classNormal('index_focus_turn_btn','index_focus_turn_tx');
			classCurrent('index_focus_turn_btn','index_focus_turn_tx',2);
		}
	}
	if (listLength>=4) {
		focusBtnList[3].onmouseover = function() {
			moveElement('index_focus_turn_picList',0,-675,5);
			classNormal('index_focus_turn_btn','index_focus_turn_tx');
			classCurrent('index_focus_turn_btn','index_focus_turn_tx',3);
		}
	}
}

setInterval('indexautoFocusChange()',5000);
var atuokey = false;
function indexautoFocusChange() {
	if(!$('index_focus_turn')||!$('index_focus_turn_btn')) return;
	if(atuokey) return;
	var focusBtnList = $('index_focus_turn_btn').getElementsByTagName('li');
	var listLength = focusBtnList.length;
	for(var i=0; i<listLength; i++) {
		if (focusBtnList[i].className == 'current') var currentNum = i;
	}
	if (currentNum==0&&listLength!=1 ){
		moveElement('index_focus_turn_picList',0,-225,5);
		classNormal('index_focus_turn_btn','index_focus_turn_tx');
		classCurrent('index_focus_turn_btn','index_focus_turn_tx',1);
	}
	if (currentNum==1&&listLength!=2 ){
		moveElement('index_focus_turn_picList',0,-450,5);
		classNormal('index_focus_turn_btn','index_focus_turn_tx');
		classCurrent('index_focus_turn_btn','index_focus_turn_tx',2);
	}
	if (currentNum==2&&listLength!=3 ){
		moveElement('index_focus_turn_picList',0,-675,5);
		classNormal('index_focus_turn_btn','index_focus_turn_tx');
		classCurrent('index_focus_turn_btn','index_focus_turn_tx',3);
	}
	if (currentNum==3 ){
		moveElement('index_focus_turn_picList',0,0,5);
		classNormal('index_focus_turn_btn','index_focus_turn_tx');
		classCurrent('index_focus_turn_btn','index_focus_turn_tx',0);
	}
	if (currentNum==1&&listLength==2 ){
		moveElement('index_focus_turn_picList',0,0,5);
		classNormal('index_focus_turn_btn','index_focus_turn_tx');
		classCurrent('index_focus_turn_btn','index_focus_turn_tx',0);
	}
	if (currentNum==2&&listLength==3 ){
		moveElement('index_focus_turn_picList',0,0,5);
		classNormal('index_focus_turn_btn','index_focus_turn_tx');
		classCurrent('index_focus_turn_btn','index_focus_turn_tx',0);
	}
}
addLoadEvent(newsfocusChange);
addLoadEvent(indexfocusChange);
addLoadEvent(nav);

// 导航效果改进
function nav(){
	var mainNav = $('nav').getElementsByTagName('li');
	var currentNum = -1;
	for(var i=0; i<mainNav.length; i++){
		if (mainNav[i].className == 'current') var currentNum = i;

	}
	if(currentNum == -1){
		mainNav[0].getElementsByTagName('a')[0].style.background='none';
	}
	if(mainNav[currentNum+1] != null) mainNav[currentNum+1].getElementsByTagName('a')[0].style.background='none';
	if(!currentNum==0){
		mainNav[0].getElementsByTagName('a')[0].style.background='none';
	}	
}