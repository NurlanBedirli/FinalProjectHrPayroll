document.addEventListener("DOMContentLoaded", function () {

    document.getElementById("fileimage").addEventListener("change", function () {

        var filereader = new FileReader();
        filereader.readAsDataURL(fileimage.files[0]);
        filereader.onload = function () {
            document.getElementById("img").setAttribute("src", filereader.result);
        }
    });
});