
localSearch();

//根据关键这搜索区域
function localSearch(){
var map = new BMap.Map("container"); // 创建地图实例
map.addControl(new BMap.NavigationControl());//地图控制插件，该插件可以控制地图的位置地图的显示比例
map.addControl(new BMap.ScaleControl());//显示在地图下方，告诉你地图上1cm对应的真实距离
//map.addControl(new BMap.OverviewMapControl());
map.addControl(new BMap.MapTypeControl());//地图的显示类型：包括地图和卫星
//确定搜索对象
var local=new BMap.LocalSearch(map,{
renderOptions:{map:map}
});
//根据关键字定义到相应的区域
local.search("成都,酒店");
}


             