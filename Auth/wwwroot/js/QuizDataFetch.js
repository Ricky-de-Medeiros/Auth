$(document).ready(function () {

    this.state = {
            birdData: [],
            habitatsData: [],
            questionData: [],
            orderData: []
    };

    $.get("https://localhost:44358/birdquiz", function (birds, status) {
        // get select (dropdown box)
        var selectBox = $("#12345");

        // loop through all data (list of bird objects) returned from the server
        $.each(birds, function (i, bird) {
            $(selectBox).append($('<option>', {
                value: bird._id,
                text: bird.name
            }));
        });

        // for each bird - make a dropdown option (html element) within/add to the select box element?
        
    });

    //DO NOT DELETE BELOW CODE: Example of how to link to a button click function
    //$("#hellobtn").click(function () {        
    //});
});

