document.addEventListener("DOMContentLoaded", function () {

    var search = document.getElementById("searchWork");
    var pagecount = document.getElementById("pageItem");
    people = [];

    document.addEventListener("keyup", function () {
        var searchValue = search.value;
        var decimal = parseFloat(searchValue);
        var page = parseFloat(pagecount.value);

        $.ajax({
            url: "/Admin/Reciurment/SearchWorkPlace/",
            type: "post",
            dataType: "JSON",
            data: {
                value: searchValue,
                salary: decimal,
                elmPage: page
            },
            success: function (response) {
                if (response.message == 202) {
                    $("tbody tr").remove();
                    for (var i = 0; i < response.data.length; i++) {
                        var content = $("#allSalary").clone();
                        content.removeAttr('hidden');
                        content.find("a.href").attr("href", "/Admin/Employee/AboutEmployee/?page=" + response.data[i].id);
                        if (response.data[i].photo != null) {
                            content.find("img.avatar").attr("src", `/img/${response.data[i].photo}`);
                        }
                        else {
                            content.find("img.avatar").attr("src", `/img/user.jpg`);
                        }
                        content.find("a.href-name").attr("href", "/Admin/Employee/AboutEmployee/?page=" + response.data[i].id);
                        content.find("a.href-name").text(`${response.data[i].name}`);
                        content.find("td.item1").text(`${response.data[i].plasierCode}`);
                        content.find("td.item2").text(`${response.data[i].companyName}`);
                        content.find("td.item3").text(`${response.data[i].emperiumName}`);
                        content.find("a.positionName").text(`${response.data[i].positionName}`);
                        content.find("td.item4").text(`${response.data[i].startDate}`);
                        content.find("td.item5").text(`${response.data[i].salary}`);
                        content.find("td.item5").append(` <span> Azn</span>`);
                        $("#all-emp-work").append(content);
                    }
                    if (response.message == 202) {
                        people = response.allData
                    }
                    if (response.message == 202) {
                        $("#paging li").remove();
                        for (var i = 1; i <= response.pageCount; i++) {
                            CreatePaging(i);
                        }
                    }
                    if (response.nextElement) {
                        $("#PageNext a").remove();
                        var pag = document.getElementById("paging");
                        var li2 = document.createElement('li');
                        var a = document.createElement('a');
                        a.setAttribute("class", "page-link btn-danger");
                        a.style.borderRadius = "35px";
                        a.id = "nextSearch";
                        a.style.cursor = "pointer";
                        a.style.marginLeft = "5px";
                        a.setAttribute("name", `${response.currentPage + 1}`);
                        a.innerText = "Next";
                        li2.append(a);
                        pag.append(li2);
                        if (response.currentPage >= 1) {
                            $("#PagePrev a").remove();
                        }
                    }
                }
            }
        });
    });

    $("#pageItem").on("change", function () {
        var searchValue = search.value;
        var decimal = parseFloat(searchValue);
        var page = parseFloat(pagecount.value);
        $.ajax({
            url: "/Admin/Reciurment/SearchWorkPlace/",
            type: "post",
            dataType: "JSON",
            data: {
                value: searchValue,
                salary: decimal,
                elmPage: page
            },
            success: function (response) {
                if (response.message == 202) {
                    $("tbody tr").remove();
                    for (var i = 0; i < response.data.length; i++) {
                        var content = $("#allSalary").clone();
                        content.removeAttr('hidden');
                        content.find("a.href").attr("href", "/Admin/Employee/AboutEmployee/?page=" + response.data[i].id);
                        if (response.data[i].photo != null) {
                            content.find("img.avatar").attr("src", `/img/${response.data[i].photo}`);
                        }
                        else {
                            content.find("img.avatar").attr("src", `/img/user.jpg`);
                        }
                        content.find("a.href-name").attr("href", "/Admin/Employee/AboutEmployee/?page=" + response.data[i].id);
                        content.find("a.href-name").text(`${response.data[i].name}`);
                        content.find("td.item1").text(`${response.data[i].plasierCode}`);
                        content.find("td.item2").text(`${response.data[i].companyName}`);
                        content.find("td.item3").text(`${response.data[i].emperiumName}`);
                        content.find("a.positionName").text(`${response.data[i].positionName}`);
                        content.find("td.item4").text(`${response.data[i].startDate}`);
                        content.find("td.item5").text(`${response.data[i].salary}`);
                        content.find("td.item5").append(` <span> Azn</span>`);
                        $("#all-emp-work").append(content);
                    }
                    if (response.message == 202) {
                        people = response.allData
                    }
                    if (response.message == 202) {
                        $("#paging li").remove();
                        for (var i = 1; i <= response.pageCount; i++) {
                            CreatePaging(i);
                        }
                    }
                    if (response.nextElement) {
                        $("#PageNext a").remove();
                        var pag = document.getElementById("paging");
                        var li2 = document.createElement('li');
                        var a = document.createElement('a');
                        a.setAttribute("class", "page-link btn-danger");
                        a.style.borderRadius = "35px";
                        a.id = "nextSearch";
                        a.style.cursor = "pointer";
                        a.style.marginLeft = "5px";
                        a.setAttribute("name", `${response.currentPage + 1}`);
                        a.innerText = "Next";
                        li2.append(a);
                        pag.append(li2);
                        if (response.currentPage >= 1) {
                            $("#PagePrev a").remove();
                        }
                    }
                }
            }
        });




    });


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
                var content = $("#allSalary").clone();
                content.removeAttr('hidden');
                content.find("a.href").attr("href", "/Admin/Employee/AboutEmployee/?page=" + people[i].id);
                if (people[i].photo != null) {
                    content.find("img.avatar").attr("src", `/img/${people[i].photo}`);
                }
                else {
                    content.find("img.avatar").attr("src", `/img/user.jpg`);
                }
                content.find("a.href-name").attr("href", "/Admin/Employee/AboutEmployee/?page=" + people[i].id);
                content.find("a.href-name").text(`${people[i].name}`);
                content.find("td.item1").text(`${people[i].plasierCode}`);
                content.find("td.item2").text(`${people[i].companyName}`);
                content.find("td.item3").text(`${people[i].emperiumName}`);
                content.find("a.positionName").text(`${people[i].positionName}`);
                content.find("td.item4").text(`${people[i].startDate}`);
                content.find("td.item5").text(`${people[i].salary}`);
                content.find("td.item5").append(` <span> Azn</span>`);
                $("#all-emp-work").append(content);
            }
        }
        PrevNextPagination(model);
    });

    $("#paging").on("click", "li #pagination", function () {
        var page = parseInt($(this).text());
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
                var content = $("#allSalary").clone();
                content.removeAttr('hidden');
                content.find("a.href").attr("href", "/Admin/Employee/AboutEmployee/?page=" + people[i].id);
                if (people[i].photo != null) {
                    content.find("img.avatar").attr("src", `/img/${people[i].photo}`);
                }
                else {
                    content.find("img.avatar").attr("src", `/img/user.jpg`);
                }
                content.find("a.href-name").attr("href", "/Admin/Employee/AboutEmployee/?page=" + people[i].id);
                content.find("a.href-name").text(`${people[i].name}`);
                content.find("td.item1").text(`${people[i].plasierCode}`);
                content.find("td.item2").text(`${people[i].companyName}`);
                content.find("td.item3").text(`${people[i].emperiumName}`);
                content.find("a.positionName").text(`${people[i].positionName}`);
                content.find("td.item4").text(`${people[i].startDate}`);
                content.find("td.item5").text(`${people[i].salary}`);
                content.find("td.item5").append(` <span> Azn</span>`);
                $("#all-emp-work").append(content);
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
                var content = $("#allSalary").clone();
                content.removeAttr('hidden');
                content.find("a.href").attr("href", "/Admin/Employee/AboutEmployee/?page=" + people[i].id);
                if (people[i].photo != null) {
                    content.find("img.avatar").attr("src", `/img/${people[i].photo}`);
                }
                else {
                    content.find("img.avatar").attr("src", `/img/user.jpg`);
                }
                content.find("a.href-name").attr("href", "/Admin/Employee/AboutEmployee/?page=" + people[i].id);
                content.find("a.href-name").text(`${people[i].name}`);
                content.find("td.item1").text(`${people[i].plasierCode}`);
                content.find("td.item2").text(`${people[i].companyName}`);
                content.find("td.item3").text(`${people[i].emperiumName}`);
                content.find("a.positionName").text(`${people[i].positionName}`);
                content.find("td.item4").text(`${people[i].startDate}`);
                content.find("td.item5").text(`${people[i].salary}`);
                content.find("td.item5").append(` <span> Azn</span>`);
                $("#all-emp-work").append(content);
            }
        }
        PrevNextPagination(model);
    });


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



//Pagination Click
$("#paging").on("click", "li #pagination-ajax-workplace", function () {
    var pageCount = $(this).text();
    var page = parseFloat(pagecount.value);
    $.ajax({
        url: "/Admin/Reciurment/PagingWorkPlace/",
        type: "post",
        dataType: "JSON",
        data: {
            id: pageCount,
            elmPage: page
        },
        success: function (response) {
            if (response.message == 202) {
                $("tbody tr").remove();
                for (var i = 0; i < response.currentData.length; i++) {
                    var content = $("#allSalary").clone();
                    content.removeAttr('hidden');
                    content.find("a.href").attr("href", "/Admin/Employee/AboutEmployee/?page=" + response.currentData[i].id);
                    if (response.currentData[i].photo != null) {
                        content.find("img.avatar").attr("src", `/img/${response.currentData[i].photo}`);
                    }
                    else {
                        content.find("img.avatar").attr("src", `/img/user.jpg`);
                    }
                    content.find("a.href-name").attr("href", "/Admin/Employee/AboutEmployee/?page=" + response.currentData[i].id);
                    content.find("a.href-name").text(`${response.currentData[i].name}`);
                    content.find("td.item1").text(`${response.currentData[i].plasierCode}`);
                    content.find("td.item2").text(`${response.currentData[i].companyName}`);
                    content.find("td.item3").text(`${response.currentData[i].emperiumName}`);
                    content.find("a.positionName").text(`${response.currentData[i].positionName}`);
                    content.find("td.item4").text(`${response.currentData[i].startDate}`);
                    content.find("td.item5").text(`${response.currentData[i].salary}`);
                    $("#all-emp-work").append(content);
                }
                if (response.nextElement) {
                    $("#PageNext a").remove();
                    var a = document.createElement('a');
                    a.setAttribute("class", "page-link btn-danger");
                    a.style.borderRadius = "35px";
                    a.style.cursor = "pointer";
                    a.style.marginLeft = "5px";
                    a.id = "next1";
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
                    a.id = "next1";
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
$("#paging").on("click", "li #next1", function () {
    var pageCount = $(this).attr('name');
    var page = parseFloat(pagecount.value);
    
    $.ajax({
        url: "/Admin/Reciurment/PagingWorkPlace/",
        type: "post",
        dataType: "JSON",
        data: {
            id: pageCount,
            elmPage: page
        },
        success: function (response) {
            if (response.message == 202) {
                $("tbody tr").remove();
                for (var i = 0; i < response.currentData.length; i++) {
                    var content = $("#allSalary").clone();
                    content.removeAttr('hidden');
                    content.find("a.href").attr("href", "/Admin/Employee/AboutEmployee/?page=" + response.currentData[i].id);
                    if (response.currentData[i].photo != null) {
                        content.find("img.avatar").attr("src", `/img/${response.currentData[i].photo}`);
                    }
                    else {
                        content.find("img.avatar").attr("src", `/img/user.jpg`);
                    }
                    content.find("a.href-name").attr("href", "/Admin/Employee/AboutEmployee/?page=" + response.currentData[i].id);
                    content.find("a.href-name").text(`${response.currentData[i].name}`);
                    content.find("td.item1").text(`${response.currentData[i].plasierCode}`);
                    content.find("td.item2").text(`${response.currentData[i].companyName}`);
                    content.find("td.item3").text(`${response.currentData[i].emperiumName}`);
                    content.find("a.positionName").text(`${response.currentData[i].positionName}`);
                    content.find("td.item4").text(`${response.currentData[i].startDate}`);
                    content.find("td.item5").text(`${response.currentData[i].salary}`);
                    content.find("td.item5").append(` <span> Azn</span>`);
                    $("#all-emp-work").append(content);
                }
                if (response.nextElement) {
                    $("#PageNext a").remove();
                    var a = document.createElement('a');
                    a.setAttribute("class", "page-link btn-danger");
                    a.style.borderRadius = "35px";
                    a.id = "next1";
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
                    a.id = "next1";
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


});
