// Make User enter number of questions they want to answer
// This has been decided to be made compulsory, as we cannot have infinate number of questions
// i.e. user will never get their results, as there won't be a quiz end

function compulsoryField() {
    //connects to quizOptions form and the number text-field
    let x = document.forms["quizOptions"]["number"].value;
    if (x == "") {
        // generates an alert asking to enter number of questions
        alert("Please enter number of questions you wish to answer in order to proceed with the quiz");
        // creates a focus on the field i.e. the typing bar is automatically placed in the required field box
        document.getElementById("compulsoryNumber").focus();
        return false;
    }
}



function orderOptions() {
    $.get("https://localhost:44358/order", function (birdOrders, status) {

        // loop through all data (list of bird objects) returned from the server
        $.each(birdOrders, function (i, birdOrder) {

            var radioOrd = [birdOrder];
            for (var value of radioOrd) {
                $('#orderButtons')
                    .append(`<input type="checkbox" id="${value}" name="contact" value="${value}">`)
                    .append(`<label for="${value}">${value}</label></div>`)
                    .append(` `);
            }
        });
    });
}


function difficultyOptions() {
    $.get("https://localhost:44358/difficultyLevel", function (questionLevel, status) {

        // loop through all data (list of bird objects) returned from the server
        $.each(questionLevel, function (i, difficulty) {

            var radioDiff = [difficulty];
            for (var value of radioDiff) {
                $('#difficultyButtons')
                    .append(`<input type="checkbox" id="${value}" name="contact" value="${value}">`)
                    .append(`<label for="${value}">${value}</label></div>`)
                    .append(` `);
            }
        });
    });
}


function statusOptions() {
    $.get("https://localhost:44358/status", function (birdStatuses, status) {

        // loop through all data (list of bird objects) returned from the server
        $.each(birdStatuses, function (i, birdStatus) {

            var radioOpt = [birdStatus];
            for (var value of radioOpt) {
                $('#statusButtons')
                    .append(`<input type="checkbox" id="${value}" name="contact" value="${value}">`)
                    .append(`<label for="${value}">${value}</label></div>`)
                    .append(` `);
            }
        });
    });
}


// function to create checkbox options for bird habitats
// by taking data from the habitats endpoint in HomeController.cs which
// gets habitat data from database, splits it by individual words (as the original list provided
// by DR Phil has multiple habitats in one string)
// The code also only obtains unique values
function habitatOptions() {
    $.get("https://localhost:44358/habitats", function (birdHabitats, status) {
        
        // loop through all data (list of bird objects) returned from the server
        $.each(birdHabitats, function (i, birdHabitat) {
            
            var radioHab = [birdHabitat];
            for (var value of radioHab) {
                $('#habitatButtons')
                        .append(`<input type="checkbox" id="${value}" name="contact" value="${value}">`)
                        .append(`<label for="${value}">${value}</label></div>`)
                    .append(` `);
            }
        });
    });
}

$(document).ready(function () {
    // the function compulsoryField() is not called here, as then the alert will be generated on page load
    // i.e. when the user hasn't had a chance to enter any data
    statusOptions();
    habitatOptions();
    orderOptions()
});