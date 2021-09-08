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

$(document).ready(function () {
    // the function compulsoryField() is not called here, as then the alert will be generated on page load
    // i.e. when the user hasn't had a chance to enter any data
});