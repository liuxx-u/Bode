(function ($) {
    $.osharp = $.osharp || { version: 1.0 };
})(jQuery);

(function ($) {
    $.osharp.Datatable = function (selector, conf) {
        //数据加载参数
        this.tab = $(selector);

        this.conf = conf || "";
        this.isBatch = conf.isBatch || false;
        this.columnsHash = {};
        this.columns = conf.columns || [];
        this.originData = this.conf.data || [];
        this.queryParams = {
            pageIndex: conf.pageIndex || 1,
            pageSize: conf.pageSize || 15,
            sortField: conf.sortField || this.columns[0].data,
            sortOrder: conf.sortOrder || "desc"
        };
        this.searchOperators = {
            common: [{ val: "equal", text: "等于" }, { val: "notequal", text: "不等于" }],
            struct: [{ val: "less", text: "小于" }, { val: "lessorequal", text: "小于等于" }, { val: "greater", text: "大于" }, { val: "greaterorequal", text: "大于等于" }],
            text: [{ val: "contains", text: "包含" }, { val: "startswith", text: "开始于" }, { val: "endswith", text: "结束于" }]
        };


        //数据加载事件
        this.getCurrent = function () {
            return this.tab.find(".tr-selected");
        };
        this.beforeEdit = this.conf.beforeEdit || function (index, row) { };
        this.beforeInit = this.conf.beforeInit || function () { };
        this.initComplete = this.conf.initComplete || function () { };
        this.loadDataComplete = this.conf.loadDataComplete || function (data) { };

        //表格操作相关
        this.editingRow = null;
        this.addedRows = [];
        this.editedRows = [];

        //改行是否被缓存
        this.isRowCached = function (row) {
            for (var i = 0, aN = this.addedRows.length; i < aN; i++) {
                if (this.tool.isSameRow(this.addedRows[i], row)) return true;
            }

            for (var i = 0, eN = this.editedRows.length; i < eN; i++) {
                if (this.tool.isSameRow(this.editedRows[i]["row"], row)) return true;
            }
            return false;
        }

        //获取行的当前数据
        this.getRowData = function (row) {
            var data = {};
            this.tab.find("thead>tr:eq(0)>th").each(function (i, e) {
                if (this.isBatch && i == 0) return true;
                var dataKey = $(e).attr("data-key");
                var td = row.find("td:eq(" + i + ")");
                data[dataKey] = td.attr("data-ov") || td.text();
            });
            return data;
        }

        //添加新行
        this.addNewRow = function () {
            if (this.editingRow != null) {
                this.saveRow(this.editingRow);
            }
            //更新样式
            this.tab.find("tbody>.tr-selected").removeClass("tr-selected");

            var nRow = $("<tr></tr>");
            for (var i = 0, n = this.columns.length; i < n; i++) {
                var ovHtml;
                if (this.columns[i].data === "Id" || this.columns[i].type === "number") {
                    ovHtml = 'data-ov="0"';
                } else if (this.columns[i].type === "switch") {
                    ovHtml = 'data-ov="false"';
                } else {
                    ovHtml = 'data-ov=""';
                }
                var display = this.columns[i].type === "hide" ? 'style="display:none;"' : '';

                $('<td ' + display + ovHtml + '></td>').appendTo(nRow);
            }
            nRow.prependTo(this.tab);

            //新增的行加入缓存
            this.addedRows.push(nRow);
            //进入编辑
            this.editRow(nRow);
            //添加事件
            this.tool.addRowEvent(this, nRow);
        }

        //编辑行
        this.editRow = function (row) {
            var cRow = row || this.getCurrent();

            if (cRow == null || typeof (cRow) == "undefined" || this.tool.isSameRow(this.editingRow, cRow)) { return; }

            if (this.editingRow != null && !this.tool.isSameRow(this.editingRow, cRow)) {
                this.saveRow(this.editingRow);
            }

            if (!cRow.hasClass("tr-selected")) cRow.addClass("tr-selected");
            cRow.removeClass("tr-hover");

            var index = cRow.index();
            var data = this.getRowData(cRow);
            //执行编辑前事件
            this.beforeEdit(index, data);

            var jqTds = $('>td', cRow);

            for (var i = this.isBatch?1:0, n = jqTds.length; i < n; i++) {
                var dataKey = this.tab.find("thead>tr:eq(0)>th:eq(" + i + ")").attr("data-key");
                var editor = this.columnsHash[dataKey]["editor"];
                var colType = this.columnsHash[dataKey]["type"];

                if (dataKey === "Id" || typeof (editor) == "undefined") continue;

                var ef = editor["format"];
                if (typeof (ef) == "function") {
                    jqTds[i].innerHTML = ef(data[dataKey]);
                }
                else {
                    if (colType === "combobox" || colType === "switch") {
                        var source = this.columnsHash[dataKey].source;
                        var valueField = source.valueField || "val";
                        var textField = source.textField || "text";

                        var comboObj = $('<select class="input-sm" style="width:95%;margin-top: -6px;">');
                        for (var j = 0, m = source.data.length; j < m; j++) {
                            var selectHtml = source.data[j][valueField].toString() === data[dataKey].toString() ? 'selected="selected"' : '';
                            $('<option ' + selectHtml + ' value="' + source.data[j][valueField] + '">' + source.data[j][textField] + '</option>').appendTo(comboObj);
                        }
                        $(jqTds[i]).empty().append(comboObj);
                        comboObj.select2({ minimumResultsForSearch: -1 });
                    }
                    else if (colType === "img") {
                        jqTds[i].innerHTML = '<img src="' + data[dataKey] + '" style="width:120px;height:80px;"/><input id="uploadify_' + i + '" type="file" />';

                        var imgAjax = this.conf.imgAjax || "";

                        $(jqTds[i]).find("input:file").uploadify({
                            swf: '../../../../Content/js/uploadify/uploadify.swf',
                            uploader: imgAjax,
                            buttonText: "选择文件",
                            height: 32,
                            width: 75,
                            fileTypeDesc: 'Image Files',
                            fileTypeExts: '*.jpg; *.jpeg; *.png; *.bmp',
                            fileSizeLimit: '10MB',
                            auto: true,
                            multi: false,
                            onFallback: function () {
                                Notify("当前浏览器未安装flash,请安装或更换浏览器。", 'bottom-right', '5000', 'warning', 'fa-warning', true);
                            },
                            onUploadSuccess: function (file, data, response) {
                                var result = eval('(' + data + ')');
                                $("#" + this.settings.button_placeholder_id).prev("img").attr("src", result.ReturnData);
                            }
                        });

                    } else if (colType === "datepicker"||colType==="timepicker") {
                        jqTds[i].innerHTML = '<input class="input-sm date-picker" data-vv="' + data[dataKey] + '" type="text" style="width:90%;" value="' + data[dataKey] + '" readonly>';

                        var minView = colType === "datepicker" ? 2 : 1;
                        var format = colType === "datepicker"?"yyyy-MM-dd":"yyyy-MM-dd hh:ii";
                        $('.date-picker').datetimepicker({
                            minView: minView,
                            todayBtn: 1,
                            language: 'zh-CN',
                            format: format,
                            weekStart: 1,
                            autoclose: 1
                        });
                    } else if (colType === "number") {
                        jqTds[i].innerHTML = '<input type="text" data-vv="' + data[dataKey] + '" class="input-sm" style="width:95%;" value="' + data[dataKey] + '">';
                        $.osharp.tools.formatDiscount($(jqTds[i]).find("input:text"));
                    }
                    else {
                        jqTds[i].innerHTML = '<input type="text" data-vv="' + data[dataKey] + '" class="input-sm" style="width:95%;" value="' + data[dataKey] + '">';
                    }
                }
                //执行编辑列处理事件
                if (typeof (editor["eaction"]) == "function") {
                    editor["eaction"](jqTds[i], data[dataKey]);
                }
            }

            ////编辑的行加入缓存
            if (!this.isRowCached(cRow)) {
                this.editedRows.push({ row: cRow, originData: data });
            }

            //更新正在编辑的行
            this.editingRow = cRow;
        };

        //保存单行数据
        this.saveRow = function (row) {
            var i = 0, cN = this.columns.length;
            if (this.isBatch)
            {
                i++; cN++;
            }
            while (i< cN) {
                var colIndex = this.isBatch ? i - 1 : i;
                var editor = this.columns[colIndex].editor;
                if (typeof (editor) == "undefined" || this.columns[colIndex]["data"] === "Id") { i++; continue; }

                var ov;
                var f = this.columns[colIndex].format;
                var td = row.find("td:eq(" + i + ")");

                var colType = this.columns[colIndex]["type"];
                if (colType === "combobox" || colType === "switch") {
                    ov = td.find("select").select2("val");
                    td.html(this.tool.sourceFormat(ov, this.columns[colIndex]["source"], f));

                } else if (colType === "img") {
                    ov = td.find("img").attr("src");
                    td.html('<img src="' + ov + '" style="width:120px;height:80px;"/>');
                } else if (colType === "number") {
                    ov = td.find("input").val();
                    var reg = /^(-?\d+)(\.\d+)?$/;
                    if (!reg.test(ov)) {
                        var prev = td.find("input").attr("data-vv");
                        if (prev === "") prev = "0";
                        ov = prev;
                    }
                    td.html(this.tool.format(ov, null, f));
                } else {
                    ov = td.find("input").val();
                    var validation = this.columns[colIndex].validation;
                    if (validation) {
                        if (validation.required && ov === "") {
                            ov = this.columns[colIndex]["type"] === "datepicker" ? "2000-01-01 00:00" : " ";
                        }
                    }
                    td.html(this.tool.format(ov, null, f));
                }
                //存储原始值
                td.attr("data-ov", ov);
                i++;
            }
            this.editingRow = null;
        }

        //还原所有操作
        this.cancel = function () {
            //删除新增的数据
            for (var i = 0, aN = this.addedRows.length; i < aN; i++) {
                this.addedRows[i].remove();
            }
            //还原编辑的数据
            for (var i = 0, eN = this.editedRows.length; i < eN; i++) {
                var row = this.editedRows[i]["row"];

                for (var j = 0, cN = this.columns.length; j < cN; j++) {
                    var editor = this.columns[j].editor;
                    if (typeof (editor) == "undefined" || this.columns[j]["data"] === "Id") continue;

                    var dataKey = this.columns[j]["data"];
                    var d = this.editedRows[i]["originData"];
                    var ov = d[dataKey];
                    var f = this.columns[j]["format"];

                    row.find("td:eq(" + j + ")").attr("data-ov", ov);

                    var colType = this.columns[j]["type"];
                    if (colType === "combobox" || colType === "switch") {
                        row.find("td:eq(" + j + ")").html(this.tool.sourceFormat(ov, this.columns[j]["source"], f));
                    } else if (colType === "img") {
                        row.find("td:eq(" + j + ")").html('<img src="' + ov + '" style="width:120px;height:80px;"/>');
                    } else {
                        row.find("td:eq(" + j + ")").html(this.tool.format(ov,d, f));
                    }
                }
            }
            //清空缓存数组
            this.addedRows.length = 0;
            this.editedRows.length = 0;
            this.editingRow = null;
        }

        //获取新增与修改的数据
        this.getChanges = function () {
            if (this.editingRow != null) {
                this.saveRow(this.editingRow);
            }

            var adds = [];
            for (var i = 0, an = this.addedRows.length; i < an; i++) {
                adds.push(this.getRowData(this.addedRows[i]));
            }

            var edits = [];
            for (var i = 0, en = this.editedRows.length; i < en; i++) {
                var d = this.getRowData(this.editedRows[i]["row"]);
                var od = this.editedRows[i]["originData"];
                if (!this.tool.objCompare(d, od)) {
                    edits.push(d);
                }
            }
            return { adds: adds, edits: edits };
        }

        //工具方法
        this.tool = {
            addRowEvent: function (tab, row) {
                row.click(function () {
                    $("#dataTable tbody .tr-selected").removeClass("tr-selected");
                    $(this).removeClass("tr-hover");
                    $(this).addClass("tr-selected");
                }).mouseover(function () {
                    if ($(this).hasClass("tr-selected")) return;
                    $(this).addClass("tr-hover");
                }).mouseout(function () {
                    $(this).removeClass("tr-hover");
                }).dblclick(function () {
                    tab.editRow();
                });
            },

            //数据展示format
            format: function (v,d, f) {
                if (typeof (f) == 'function') return f(v,d);
                return v;
            },

            //判断两个行对象是否同一行
            isSameRow: function (row1, row2) {
                if (row1 == null || row2 == null || typeof (row1) === "undefined" || typeof (row2) === "undefined") return false;
                return row1.get(0) === row2.get(0);
            },

            //比较两个对象键值是否想等
            objCompare: function (obj1, obj2) {
                if (typeof (obj1) != "undefined" || typeof (obj2) != "undefined") return false;

                for (var key in obj1) {
                    if (obj1.hasOwnProperty(key)) {
                        if (obj1[key] !== obj2[key]) return false;
                    }
                }
                return true;
            },
            //combobox与switch显示format
            sourceFormat: function (ov, source, f) {
                if (typeof (f) == 'function') return f(ov);
                else {
                    var valueFiled = source.valueField || "val";
                    var textField = source.textField || "text";
                    for (var i = 0, sLen = source.data.length; i < sLen; i++) {
                        if (source.data[i][valueFiled].toString() === ov.toString()) {
                            return source.data[i][textField];
                        }
                    }
                    return ov;
                }
            }
        }

        //表格加载数据
        this.loadData = function () {
            //加载表格
            for (var i = 0, n = this.originData.length; i < n; i++) {
                var d = this.originData[i];
                var tr = $('<tr></tr>');
                
                if (this.isBatch) {
                    $('<td style="width:25px;"><div class="checkbox"><label><input type="checkbox" value="' + d["Id"] + '"><span class="text"></span></label></div></td>').appendTo(tr);
                }
                for (var j = 0, m = this.columns.length; j < m; j++) {

                    var f = this.columns[j]["format"];
                    var colType = this.columns[j]["type"];
                    //处理时间类型
                    var ov = d[this.columns[j]["data"]];
                    if (this.columns[j]["isformatval"]) {
                        ov = this.tool.format(ov,d, f);
                    }
                    if (colType === "combobox" || colType === "switch") {
                        var source = this.columns[j].source;
                        $('<td data-ov="' + ov + '">' + this.tool.sourceFormat(ov, source, this.columns[j]["format"]) + '</td>').appendTo(tr);

                    } else if (colType === "img") {
                        $('<td data-ov="' + ov + '"><img src="' + ov + '" style="width:120px;height:80px;"/></td>').appendTo(tr);
                    } else {
                        var display = colType === "hide" ? 'style="display:none;"' : '';
                        $('<td ' + display + ' data-ov="' + ov + '">' + this.tool.format(ov,d, f) + '</td>').appendTo(tr);
                    }
                }
                tr.appendTo(this.tab.find("tbody"));
            }

            //绑定事件
            this.tool.addRowEvent(this, this.tab.find("tbody>tr"));
        }

        this.initHead = function () {
            var tab = this;
            if (tab.isBatch)
            {
                var th=$('<th style="width:35px;"><div class="checkbox"><label><input type="checkbox"><span class="text"></span></label></div></th>')
                th.find("input:checkbox").click(function () {
                    if ($(this).is(":checked")) {
                        tab.tab.find("tbody>tr").find("td:eq(0) input:checkbox").attr("checked", true);
                    }
                    else {
                        tab.tab.find("tbody>tr").find("td:eq(0) input:checkbox").removeAttr("checked");
                    }
                });
                th.appendTo(this.tab.find("thead>tr"));
            }

            //初始化columnHash与表头
            for (var i = 0, n = this.columns.length; i < n; i++) {
                //初始化switch的数据源
                var colType = this.columns[i]["type"];
                //初始化switch的format与数据源
                if (colType === "switch" && typeof (this.columns[i]["source"]) === "undefined"){
                    this.columns[i]["source"] = { data: [{ "val": "false", "text": "否" }, { "val": "true", "text": "是" }] };
                }

                //初始化hide类型为可编辑列，方便保存数据
                if (colType === "hide") {
                    this.columns[i]["editor"] = {};
                }

                //初始化columnHash
                this.columnsHash[this.columns[i].data] = this.columns[i];
                var display = colType === "hide" ? 'style="display:none;"' : '';
                var sortHtml = this.columns[i].sortDisable ? '' : ' class="sorting"';
                var width = this.columns[i].width || '251px';
                $('<th ' + display + sortHtml + ' data-key="' + this.columns[i].data + '" style="width: '+width+';">' + this.columns[i].title + '</th>').click(function () {
                    if (typeof ($(this).attr("class")) == "undefined") return;
                    tab.queryParams.sortField = $(this).attr("data-key");
                    tab.queryParams.sortOrder = $(this).attr("class") === "sorting_asc" ? "desc" : "asc";

                    $(".sorting_asc,.sorting_desc").attr("class", "sorting");
                    tab.query();

                    $(this).attr("class", "sorting_" + tab.queryParams.sortOrder);
                }).appendTo(this.tab.find("thead>tr"));
            }
        }

        this.initFoot = function (total) {
            var tab = this;

            //初始化分页控件
            var pageDom = $('<div class="row DTTTFooter"></div>');
            $('<div class="col-sm-2"><div class="dataTables_info">共' + total + '条记录</div></div>' +
                '<div class="col-sm-10"><div class="dataTables_paginate paging_bootstrap"><ul class="pagination"></ul></div></div>').appendTo(pageDom);

            var gIndex;
            var cIndex = tab.queryParams.pageIndex;
            var pageCount = Math.ceil(total / this.queryParams.pageSize);
            var ul = pageDom.find("ul");
            var prevHtml = this.queryParams.pageIndex === 1 ? " disabled" : "";

            $('<li class="prev' + prevHtml + '"><a href="#">上一页</a></li>').click(function () {
                $(this).closest("ul").find(".active").prev("li").click();
            }).appendTo(ul);

            //计算显示的最大页序号
            if (pageCount <= 5 || pageCount - 2 <= cIndex) {
                gIndex = pageCount;
            }
            else {
                gIndex = cIndex < 3 ? 5 : cIndex + 2;
            }
            for (var i = gIndex - 4 > 1 ? gIndex - 4 : 1; i <= gIndex; i++) {
                var activeHtml = i === tab.queryParams.pageIndex ? ' class="active"' : '';
                $('<li' + activeHtml + '><a href="#">' + i + '</a></li>').click(function () {
                    if ($(this).hasClass("active")) return;
                    var index = parseInt($(this).text());
                    tab.queryParams.pageIndex = index;
                    tab.query();
                }).appendTo(ul);
            }

            var nextHtml = cIndex === pageCount ? " disabled" : "";
            $('<li class="next' + nextHtml + '"><a href="#">下一页</a></li>').click(function () {
                $(this).closest("ul").find(".active").next("li").click();
            }).appendTo(ul);
            pageDom.appendTo(this.tab.closest("div"));
        }

        this.initData = function () {
            var tab = this;
            if (tab.isBatch) {
                tab.tab.find(">thead>tr>th:eq(0)").find("input:checkbox:checked").click();
            }
            //初始化数据
            if (typeof (this.conf.ajax) != "undefined") {
                var ajax = this.conf.ajax || "";
                $.get(ajax, this.queryParams, function (data) {
                    tab.originData = data.Rows;
                    tab.loadData();

                    //绑定分页控件
                    tab.initFoot(data.Total);
                    tab.loadDataComplete(data);
                });
            }
            else {
                this.loadData();
                this.initFoot(this.originData.length);
                this.loadDataComplete(this.originData);
            }
        }

        this.initSearch = function () {
            var tab = this;
            var fieldSelect = this.tab.closest("div").find(".row:eq(0)").find("select:eq(0)");

            for (var i = 0; i < this.columns.length; i++) {
                if (this.columns[i].search) {
                    $('<option value="' + this.columns[i].data + '">' + this.columns[i].title + '</option>').appendTo(fieldSelect);
                }
            }
            fieldSelect.select2({
                //去掉搜索框
                minimumResultsForSearch: -1
            }).on("change", function (e) {
                var oprateSelect = $(this).closest("div").find("select:eq(1)");
                var dataInput = $(this).closest("div").find(".query-input");

                //chang事件
                var type = tab.columnsHash[e.target.value].type;
                var opraArr = tab.searchOperators.common.concat([]);

                //改变操作选项
                if (type === "number" || type === "datepicker" || type === "timepicker") {
                    opraArr = opraArr.concat(tab.searchOperators.struct);
                }
                if (type === "text" || type === "textarea") {
                    opraArr = opraArr.concat(tab.searchOperators.text);
                }
                oprateSelect.empty();
                for (var i = 0, n = opraArr.length; i < n; i++) {
                    $('<option value="' + opraArr[i].val + '">' + opraArr[i].text + '</option>').appendTo(oprateSelect);
                }
                oprateSelect.select2("val", opraArr[0].val);

                //移除值下拉选择框
                if ($(this).closest("div").find(".select2-container").length == 3) {
                    $(this).closest("div").find(".select2-container").eq(2).remove();
                }
                //去掉日期选择事件
                dataInput.datetimepicker('remove');

                //对switch与combobox选项的值进行处理
                if (type === "combobox" || type === "switch") {
                    var source = tab.columnsHash[e.target.value].source;
                    var valueFiled = source.valueField || "val";
                    var textField = source.textField || "text";

                    dataInput.hide();
                    var dataSelect = $('<select data-type="combobox" style="width:30%;margin-right:4px;"></select>').on("change", function () {
                        //值存到input框中
                        $(this).closest("div").find(".query-input").val($(this).select2("val"));
                    });

                    for (var i = 0, n = source.data.length; i < n; i++) {
                        $('<option value="' + source.data[i][valueFiled] + '">' + source.data[i][textField] + '</option>').appendTo(dataSelect);
                    }
                    $(this).closest("div").find("a.btn").before(dataSelect);
                    dataSelect.select2().change();
                }
                else if (type === "datepicker" || type === "timepicker") {
                    dataInput.val("").show();
                    var minView = type === "datepicker" ? 2 : 1;
                    var format = type === "datepicker" ? "yyyy-MM-dd" : "yyyy-MM-dd hh:ii";
                    dataInput.datetimepicker({
                        minView: minView,
                        todayBtn: 1,
                        language: 'zh-CN',
                        format: format,
                        weekStart: 1,
                        autoclose: 1
                    });
                }
                else {
                    dataInput.val("").show();
                }
            }).change();

            this.tab.closest("div").find(".row:eq(0)").find("select:eq(1)").select2({
                //去掉搜索框
                minimumResultsForSearch: -1
            });
        }

        //初始化
        this.init = function () {
            this.beforeInit();

            //初始化switch的数据源
            //初始化columnHash与表头
            this.initHead();

            //初始化数据
            this.initData();

            //初始化查询控件
            this.initSearch();

            this.initComplete();
        }

        this.reload = function () {
            if (this.queryParams.where) delete this.queryParams.where;
            this.query();
        }

        this.query = function () {
            this.addedRows.length = 0;
            this.editedRows.length = 0;

            this.tab.find("tbody").empty();
            this.tab.next(".DTTTFooter").remove();
            this.initData();
        }

        //执行初始化
        this.init();
    }
})(jQuery);