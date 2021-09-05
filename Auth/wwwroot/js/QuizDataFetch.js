
   
$(document).ready(function ()
{

    //this.state = {
    //        birdData: [],
    //        habitatsData: [],
    //        questionData: [],
    //        orderData: [],
    //};

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
    });

    $.get("https://localhost:44358/getQuestion", function (bird, status) {

        // Set the source of the music element DYNAMICALLY so that it gets the sound based on the 'bird._id'
        // I.e.  
        $("#idOfMusicElement").attr("src", "https://localhost:44358/getSound/" + bird._id);
        
    });

    //DO NOT DELETE BELOW CODE: Example of how to link to a button click function
    //$("#hellobtn").click(function () {        
    //});

   

});


  
