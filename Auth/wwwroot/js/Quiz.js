function changeQuestionNumber() {
    var qNumber = document.getElementById("changeQNumber").innerText;
    var change = parseInt(qNumber, 10) + 1;
    document.getElementById("changeQNumber").innerHTML = change.toString();    
}

function dropDownOptions() {
    $.get("https://localhost:44358/birdquiz", function (birds) {
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
}

function getNextQuestion() {
    $.get("https://localhost:44358/getQuestion", function (bird) {
        // Set the source of the music element DYNAMICALLY so that it gets the sound based on the 'bird._id'
        var audio = document.getElementById("idOfMusicElement");

        //obtain value user selected from dropdown
        var strUser = $("#12345 :selected").text();
        
        audio.src = bird.sound1;
        audio.load();

        $("#answer").text(bird.name);

        //below code checks for user-selected option vs correct answer
        if (strUser != bird.name) {
            $("#result").text("Your previous answer is wrong. Correct answer is: " + bird.name);
        }
        else if (strUser == bird.name) {
            $("#result").text("Your answer " + bird.name + "is correct!!!");
        }

        changeQuestionNumber();        
        
    });

    //DO NOT DELETE BELOW CODE: Example of how to link to a button click function
    //$("#hellobtn").click(function () {        
    //});
}


   
$(document).ready(function ()
{    
    dropDownOptions();
           
});


 