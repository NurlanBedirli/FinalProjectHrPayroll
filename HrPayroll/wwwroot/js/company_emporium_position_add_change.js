document.addEventListener("DOMContentLoaded", function () {

    var holding = $("#Holding option").val();

    $.ajax({
        url: "/Admin/Reciurment/AjaxHoldingComBox",
        dataType: "Json",
        type: "post",
        data: {
            value: holding
        },
        success: function (respons) {
            if (respons.message == 202) {
                $("#emporiaName option").remove();
                $("#PositionName option").remove();
                $("#Salary option").remove();
                for (var i = 0; i < respons.company.length; i++) {
                    var option = document.createElement('option');
                    option.value = respons.company[i].id;
                    option.innerText = respons.company[i].name;
                    $("#companyName").append(option);
                }
            }
        }
    });

    $("#companyName").on("change", function () {

        $.ajax({
            url: "/Admin/Reciurment/AjaxEmporiumComboBox",
            dataType: "Json",
            type: "post",
            data: {
                id: $(this).val()
            },
            success: function (respons) {
                if (respons.message == 202) {
                    $("#emporiaName option").remove();
                    $("#PositionName option").remove();
                    $("#DepartamentName option").remove();
                    $("#Salary option").remove();
                    for (var i = 0; i < respons.emporia.length; i++) {
                        var option = document.createElement('option');
                        option.value = respons.emporia[i].id;
                        option.innerText = respons.emporia[i].name;
                        $("#emporiaName").append(option);
                    }
                }
            }
        });

    });

    $("#emporiaName").on("blur", function () {

        $.ajax({
            url: "/Admin/Reciurment/AjaxPositionsComboBox",
            dataType: "Json",
            type: "post",
            data: {
                id: $(this).val()
            },
            success: function (respons) {
                if (respons.message == 202) {
                    $("#PositionName option").remove();
                    $("#DepartamentName option").remove();
                    $("#Salary option").remove();
                    for (var i = 0; i < respons.positionss.length; i++) {
                        var option = document.createElement('option');
                        option.value = respons.positionss[i].id;
                        option.innerText = respons.positionss[i].name;
                        $("#PositionName").append(option);
                    }
                }
            }
        });

    });

    $("#PositionName").on("change", function () {

        $.ajax({
            url: "/Admin/Reciurment/AjaxSalaryComboBox",
            dataType: "Json",
            type: "post",
            data: {
                id: $(this).val()
            },
            success: function (respons) {
                if (respons.message == 202) {
                    $("#Salary option").remove();
                    $("#DepartamentName option").remove();
                    for (var z = 0; z < respons.salary.length; z++) {
                        var option1 = document.createElement('option');
                        option1.value = respons.salary[z].id;
                        option1.innerText = respons.salary[z].salary;
                        $("#Salary").append(option1);
                    }
                    for (var i = 0; i < respons.departament.length; i++) {
                        var option = document.createElement('option');
                        option.value = respons.departament[i].id;
                        option.innerText = respons.departament[i].name;
                        $("#DepartamentName").append(option);
                    }
                }
            }
        });

    });

});