$(document).ready(function () {
    $.get("https://localhost:44358/birdquiz", function (birds, status) {
        var check = document.getElementById("content");
        var da = "";
        birds.forEach(bird => {
            da += "<tr>" +
                "<td>" + bird.number + "</td>" +
                "<td>" + bird.name + "</td>" +
                "<td>" + bird.order + "</td>" +
                "<td>" + bird.status + "</td>" +
                "<td>" + bird.habitat + "</td>" +
                "</tr>";
        });
        check.innerHTML = da;

    });
});
