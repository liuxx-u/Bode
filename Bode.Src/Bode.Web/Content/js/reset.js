	
	
$(function(){
	//音乐插件
	$('audio' ).audioPlayer();
	$('.audioplayer-playpause').click();
	
	
	var wh = $(window).height();
	$('.firstPage').height(wh);
	$('.rightCont').width($(document).width() - $('.menu').width());
	//思无限定位
	var tz = wh / 2 - 250;
	$('#sprcont1').css({'top':tz});
	
	//左侧导航
	$('.menuHover').hover(function(){$('.subNav').animate({'left':'0px'},800)});
	$('.menu .tc a:nth-child(3)').hover(function(){$('.subNav').animate({'left':'0px'},800);	});
	$('.subNav').mouseleave(function(){$('.subNav').animate({'left':'-25%'},800);	});
})

$(window).resize(function(){
	
	var wh = $(window).height();
	$('.firstPage').height(wh);	
	$('.rightCont').width($(document).width() - $('.menu').width())
	//思无限定位
	var tz = wh / 2 - 250;
	$('#sprcont1').css({'top':tz});
	$('#fs').click();

})