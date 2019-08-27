﻿document.addEventListener("DOMContentLoaded", function () {

    var name = document.getElementById("name");
    var surname = document.getElementById("surname");
    var plasier = document.getElementById("pscode");
    var pagecount = document.getElementById("pageItem");

    people = [];
    document.addEventListener("keyup", function () {
        var nameValue = name.value;
        var surnameValue = surname.value;
        var plasierValue = plasier.value;
        var page = parseFloat(pagecount.value);
        
        $.ajax({
            url: "/Admin/Employee/AllTableEmployee",
            type: "Post",
            dataType: "json",
            data: {
                name: nameValue,
                surname: surnameValue,
                plasier: plasierValue,
                elmPage: page
            },
            success: function (respons) {
                if (respons.message == 202) {
                    $("tbody tr").remove();
                    for (var i = 0; i < respons.data.length; i++) {
                        SearcFindItemTable(respons.data[i]);
                    }
                }
                if (respons.message == 202) {
                    people =  respons.allData
                }
                if (respons.message == 202) {
                    $("#paging li").remove();
                    for (var i = 1; i <= respons.zz; i++) {
                        CreatePaging(i);
                    }
                }
                if (respons.nextElement) {
                    $("#PageNext a").remove();
                    var pag = document.getElementById("paging");
                    var li2 = document.createElement('li');
                    var a = document.createElement('a');
                    a.setAttribute("class", "page-link btn-danger");
                    a.style.borderRadius = "35px";
                    a.id = "nextSearch";
                    a.style.cursor = "pointer";
                    a.style.marginLeft = "5px";
                    a.setAttribute("name", `${respons.currentPage + 1}`);
                    a.innerText = "Next";
                    li2.append(a);
                    pag.append(li2);
                    if (respons.currentPage >= 1) {
                        $("#PagePrev a").remove();
                    }
                }
            }
        });
    });

    $("#pageItem").on("change", function () {
        var nameValue = name.value;
        var surnameValue = surname.value;
        var plasierValue = plasier.value;
        var page = parseFloat(pagecount.value);

        $.ajax({
            url: "/Admin/Employee/AllTableEmployee",
            type: "Post",
            dataType: "json",
            data: {
                name: nameValue,
                surname: surnameValue,
                plasier: plasierValue,
                elmPage: page
            },
            success: function (respons) {
                if (respons.message == 202) {
                    $("tbody tr").remove();
                    for (var i = 0; i < respons.data.length; i++) {
                        SearcFindItemTable(respons.data[i]);
                    }
                }
                if (respons.message == 202) {
                    people = respons.allData
                }
                if (respons.message == 202) {
                    $("#paging li").remove();
                    for (var i = 1; i <= respons.zz; i++) {
                        CreatePaging(i);
                    }
                }
                if (respons.nextElement) {
                    $("#PageNext a").remove();
                    var pag = document.getElementById("paging");
                    var li2 = document.createElement('li');
                    var a = document.createElement('a');
                    a.setAttribute("class", "page-link btn-danger");
                    a.style.borderRadius = "35px";
                    a.id = "nextSearch";
                    a.style.cursor = "pointer";
                    a.style.marginLeft = "5px";
                    a.setAttribute("name", `${respons.currentPage + 1}`);
                    a.innerText = "Next";
                    li2.append(a);
                    pag.append(li2);
                    if (respons.currentPage >= 1) {
                        $("#PagePrev a").remove();
                    }
                }
            }
        });


    });

    //Search Pagination Click LocalStorage data
    $("#paging").on("click", "li #pagination", function () {
        var page = parseInt($(this).text());
        
        elmPage: page
        if (page == "") {
            parseInt("page", 1);
        }
        var model = new PagingModel();
        model.currentpage = page;
        model.itempage = parseFloat(pagecount.value);
        model.prev = page > 1;
        
        model.totalitems = Math.ceil(people.length / model.itempage);
        model.next = page < model.totalitems;

        var z = parseInt((page - 1) * model.itempage);
        var az = z + model.itempage;

        $("tbody tr").remove();
        for (var i = z; i <= people.length; i++) {
            if (i < az && i != people.length) {
                SearcFindItemTable(people[i]);
            }
        }
        PrevNextPagination(model);
    });


    //Search Pagination Click LocalStorage data
    $("#paging").on("click", "li #nextSearch", function () {
        var page = parseInt($(this).attr('name'));

        var model = new PagingModel();
        model.currentpage = page;
        model.itempage = parseFloat(pagecount.value);
        model.prev = page > 1;

        
        model.totalitems = Math.ceil(people.length / model.itempage);
        model.next = page < model.totalitems;

        var z = parseInt((page - 1) * model.itempage);
        var az = z + model.itempage;
        $("tbody tr").remove();

        for (var i = z; i <= people.length; i++) {
            if (i < az && i != people.length) {
                var tr = document.createElement('tr');
                var td = document.createElement('td');
                var a = document.createElement('img');
                a.setAttribute("style", "width:40px; border-radius:100px ");
                a.src = `/img/${people[i].photo}`;
                var _a = document.createElement('a');
                var h2 = document.createElement('h2');
                h2.setAttribute("style", "margin-right:5px");
                var _h2 = document.createElement('h2');
                _a.innerText = `  ${people[i].name} ${people[i].surname}`;
                _a.style.color = "cyan";
                _a.setAttribute("href", "/Admin/Employee/AboutEmployee?page=" + people[i].id);
                h2.append(a);
                td.append(h2);
                _h2.append(_a);
                td.append(_h2);
                tr.append(td);
                CreateTd(people[i], tr);
                document.querySelector("table tbody").append(tr);
            }
        }
        PrevNextPagination(model);
    });

    $("#paging").on("click", "li #prevSearch", function () {
        var index = $(this).attr('name');
        var page = parseInt(index);

        var model = new PagingModel();
        model.currentpage = page;
        model.itempage = parseFloat(pagecount.value);
        model.prev = page > 1;

        
        model.totalitems = Math.ceil(people.length / model.itempage);
        model.next = page < model.totalitems;

        var z = parseInt((page - 1) * model.itempage);
        var az = z + model.itempage;
        $("tbody tr").remove();
        for (var i = z; i <= people.length; i++) {
            if (i < az && i != people.length) {
                SearcFindItemTable(people[i]);
            }
        }
        PrevNextPagination(model);
    });


    function SearcFindItemTable(n) {
        var tr = document.createElement('tr');
        var td = document.createElement('td');
        var a = document.createElement('img');
        a.setAttribute("style", "width:40px; border-radius:100px ");
        a.src = `/img/${n.photo}`;
        var _a = document.createElement('a');
        var h2 = document.createElement('h2');
        h2.setAttribute("style", "margin-right:5px");
        var _h2 = document.createElement('h2');
        _h2.style.cursor = "pointer";
        _a.innerText = `  ${n.name} ${n.surname}`;
        _a.style.color = "cyan";
        _a.setAttribute("href", "/Admin/Employee/AboutEmployee?page=" + n.id);
        h2.append(a);
        td.append(h2);
        _h2.append(_a);
        td.append(_h2);
        tr.append(td);
        CreateTd(n, tr);
        document.querySelector("table tbody").append(tr);
    }

    //Pagination Click
    $("#paging").on("click", "li #pagination-ajax", function () {
        var pageCount = $(this).text();
        var page = parseFloat(pagecount.value);
        
        $.ajax({
            url: "/Admin/Employee/PagingAjax/",
            type: "post",
            dataType: "JSON",
            data: {
                count: pageCount,
                elmPage: page
            },
            success: function (response) {
                if (response.message == 202) {
                    $("tbody tr").remove();
                    for (var i = 0; i < response.currentData.length; i++) {
                        var tr = document.createElement('tr');
                        var td = document.createElement('td');
                        var a = document.createElement('img');
                        a.setAttribute("style", "width:40px; border-radius:100px ");
                        a.src = `/img/${response.currentData[i].photo}`;
                        var _a = document.createElement('a');
                        var h2 = document.createElement('h2');
                        h2.setAttribute("style", "margin-right:5px");
                        var _h2 = document.createElement('h2');
                        _a.innerText = `  ${response.currentData[i].name} ${response.currentData[i].surname}`;
                        _a.setAttribute("href", "/Admin/Employee/AboutEmployee?page=" + response.currentData[i].id);
                        _a.style.color = "cyan";
                        h2.append(a);
                        td.append(h2);
                        _h2.append(_a);
                        td.append(_h2);
                        tr.append(td);
                        CreateTd(response.currentData[i], tr);
                        document.querySelector("table tbody").append(tr);
                    }
                    if (response.nextElement) {
                        $("#PageNext a").remove();
                        var a = document.createElement('a');
                        a.setAttribute("class", "page-link btn-danger");
                        a.style.borderRadius = "35px";
                        a.style.cursor = "pointer";
                        a.style.marginLeft = "5px";
                        a.id = "next";
                        a.setAttribute("name", `${response.currentPage + 1}`);
                        a.innerText = "Next";
                        $("#PageNext").append(a);
                        if (response.currentPage >= 1) {
                            $("#PagePrev a").remove();
                        }
                    }
                    if (response.prevElement) {
                        $("#PagePrev a").remove();
                        var a = document.createElement('a');
                        a.setAttribute("class", "page-link btn-danger");
                        a.style.cursor = "pointer";
                        a.style.marginLeft = "5px";
                        a.style.borderRadius = "35px";
                        a.id = "next";
                        a.setAttribute("name", `${response.currentPage - 1}`);
                        a.innerText = "Prev";
                        $("#PagePrev").append(a);
                        if (response.currentPage == response.pageCount) {
                            $("#PageNext a").remove();
                        }
                    }
                }
            }
        });
    });

    //DataBase Craete Next Prev Pagination 
    $("#paging").on("click", "li #next", function () {
        var pageCount = $(this).attr('name');
        var page = parseFloat(pagecount.value);
        $.ajax({
            url: "/Admin/Employee/PagingAjax/",
            type: "post",
            dataType: "JSON",
            data: {
                count: pageCount,
                elmPage: page
            },
            success: function (response) {
                if (response.message == 202) {
                    $("tbody tr").remove();
                    for (var i = 0; i < response.currentData.length; i++) {
                        var tr = document.createElement('tr');
                        var td = document.createElement('td');
                        var a = document.createElement('img');
                        a.setAttribute("style", "width:40px; border-radius:100px ");
                        a.src = `/img/${response.currentData[i].photo}`;
                        var _a = document.createElement('a');
                        var h2 = document.createElement('h2');
                        h2.setAttribute("style", "margin-right:5px");
                        var _h2 = document.createElement('h2');
                        _a.innerText = `  ${response.currentData[i].name} ${response.currentData[i].surname}`;
                        _a.style.color = "cyan";
                        h2.append(a);
                        td.append(h2);
                        _h2.append(_a);
                        td.append(_h2);
                        tr.append(td);
                        CreateTd(response.currentData[i], tr);
                        document.querySelector("table tbody").append(tr);
                    }
                    if (response.nextElement) {
                        $("#PageNext a").remove();
                        var a = document.createElement('a');
                        a.setAttribute("class", "page-link btn-danger");
                        a.style.borderRadius = "35px";
                        a.id = "next";
                        a.style.cursor = "pointer";
                        a.style.marginLeft = "5px";
                        a.setAttribute("name", `${response.currentPage + 1}`);
                        a.innerText = "Next";
                        $("#PageNext").append(a);
                        if (response.currentPage >= 1) {
                            $("#PagePrev a").remove();
                        }
                    }
                    if (response.prevElement) {
                        $("#PagePrev a").remove();
                        var a = document.createElement('a');
                        a.setAttribute("class", "page-link btn-danger");
                        a.style.borderRadius = "35px";
                        a.id = "next";
                        a.style.cursor = "pointer";
                        a.style.marginLeft = "5px";
                        a.setAttribute("name", `${response.currentPage - 1}`);
                        a.innerText = "Prev";
                        $("#PagePrev").append(a);
                        if (response.currentPage == response.pageCount) {
                            $("#PageNext a").remove();
                        }
                    }
                }
            }
        });
    });

    function CreatePaging(n) {
        var pag = document.getElementById("paging");
        var li = document.createElement('li');
        li.setAttribute("class", "paginate_button page-item next");
        li.style.marginLeft = "5px";
        li.style.cursor = "pointer";
        li.id = "DataTables_Table_0_next";
        var a = document.createElement('a');
        a.setAttribute("class", "page-link");
        a.style.borderRadius = "35px";
        a.id = "pagination";
        a.innerText = n;
        li.append(a);
        pag.append(li);
    }

    function CreateTd(n, tr) {
        var td1 = document.createElement('td');
        td1.innerText = `${n.plasiyerCode}`;
        var td2 = document.createElement('td');
        td2.innerText = `${n.number}`;
        var td3 = document.createElement('td');
        td3.innerText = `${n.nationally}`;
        var td4 = document.createElement('td');
        td4.innerText = `${n.email}`;
        var td5 = document.createElement('td');
        td5.innerText = `${n.birthDay}`;

        var td6 = document.createElement('td');
        td6.setAttribute("class", "text-right");
        var div = document.createElement('div');
        div.setAttribute("class", "form-check");

        var input = document.createElement('input');
        input.type = "checkbox";
        input.setAttribute("class", "form-check-input");
        input.value = `${n.id}`;
        input.id = "btnCheck";
        input.checked = false;
        input.click = "MyFunction()";
        var label = document.createElement('label');
        label.setAttribute("class", "form-check-label")
        label.for = "materialIndeterminate2";

        div.append(input);
        div.append(label);
        td6.append(div);
        tr.append(td1);
        tr.append(td2);
        tr.append(td3);
        tr.append(td4);
        tr.append(td5);
        tr.append(td6);
    }
    //Search Create  Prev Next Pagination
    function PrevNextPagination(n) {
        if (n.prev) {
            $("#prevSearch").remove();
            var pag = document.getElementById("paging");
            var li2 = document.createElement('li');
            var a = document.createElement('a');
            a.setAttribute("class", "page-link btn-danger");
            a.style.borderRadius = "35px";
            a.id = "prevSearch";
            a.style.cursor = "pointer";
            a.style.marginLeft = "5px";
            a.setAttribute("name", `${n.currentpage - 1}`);
            a.innerText = "Prev";
            li2.append(a);
            pag.append(li2);
            if (n.currentpage == n.totalitems) {
                $("#nextSearch").remove();
            }
        }
        $("#paging #DataTables_Table_0_next").remove();
        for (var i = 1; i <= n.totalitems; i++) {
            CreatePaging(i);
        }
        if (n.next) {
            $("#nextSearch").remove();
            var pag = document.getElementById("paging");
            var li2 = document.createElement('li');
            var a = document.createElement('a');
            a.setAttribute("class", "page-link btn-danger");
            a.style.borderRadius = "35px";
            a.id = "nextSearch";
            a.style.cursor = "pointer";
            a.style.marginLeft = "5px";
            a.setAttribute("name", `${n.currentpage + 1}`);
            a.innerText = "Next";
            li2.append(a);
            pag.append(li2);
            if (n.currentpage <= 1) {
                $("#prevSearch").remove();
            }
        }
    }

    //Add Click Employee Education OldWorkPlace
    $("#tbody").on("click", "tbody tr td div #btnCheck ", function () {
        var id = $(this).val();

        if ($(this).prop("checked") == true) {
            var elm1 = document.querySelector(".showw1");
            var elm2 = document.querySelector(".showw2");
            elm1.style.display = "block";
            elm1.querySelector("a").setAttribute("href", "/Admin/Employee/AddEmployeeEducation/" + id);
            elm2.style.display = "block";
            elm2.querySelector("a").setAttribute("href", "/Admin/Employee/AddEmployeeOldWorkPlace/" + id);
        }
        else {
            var elm1 = document.querySelector(".showw1");
            var elm2 = document.querySelector(".showw2");
            elm1.style.display = "none";
            elm2.style.display = "none";
        }
    });

  
});
        
        
        
