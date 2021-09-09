//datafetch
//loop
//table in html

function myFunctionSet(x) {
    //alert("Row index is: " + x.rowIndex);
    // for whatever row index selected, match it up with the bird data from db
    var bird = this.state.birdData[x.rowIndex - 1];

    //populating the textboxes
    var inputName = $("#bname");
    inputName.val(inputName.val() + bird.name);
    var inputOrder = $("#border");
    inputOrder.val(inputOrder.val() + bird.order);
    var inputStatus = $("#bstatus");
    inputStatus.val(inputStatus.val() + bird.status);
    var inputHabitat = $("#bhabitat");
    inputHabitat.val(inputHabitat.val() + bird.habitat);
    var inputID = $("#bID");
    inputID.val(inputID.val() + bird._id);
    var inputNum = $("#bnum");
    inputNum.val(inputNum.val() + bird.number);
    var inputCall = $("#bcall");
    inputCall.val(inputCall.val() + bird.call);
}

//clears all the textbox data, so when person clicks on another list item
//the textbox refreshes rather than adding data to existing data
function myFunctionClear(x) {
    var inputName = $("#bname");
    inputName.val(null);
    var inputOrder = $("#border");
    inputOrder.val(null);
    var inputStatus = $("#bstatus");
    inputStatus.val(null);
    var inputHabitat = $("#bhabitat");
    inputHabitat.val(null);
    var inputID = $("#bID");
    inputID.val(null);
    var inputNum = $("#bnum");
    inputNum.val(null);
}


var state = {
    birdData: [],
    habitatsData: [],
    questionData: [],
    orderData: []
};

$(document).ready(function () {
            
    $.get("https://localhost:44358/birdquiz", function (birds, status) {
        state.birdData = birds;
        // get select (dropdown box)
        var birdTable = $("#tableAdminBirdName");        
        // loop through all data (list of bird objects) returned from the server
        $.each(birds, function (i, bird) {
            $(birdTable).append(
                '<tr onclick="myFunctionClear(this); myFunctionSet(this);">'
                    + '<td>' + bird.name + '</td>'
                    //+ '<td>' + bird.order + '</td>'
                    //+ '<td>' + bird.status + '</td>'
                + '</tr>'
            );
                          
        });

        // for each bird - make a dropdown option (html element) within/add to the select box element?

    });

    //DO NOT DELETE BELOW CODE: Example of how to link to a button click function
    //$("#hellobtn").click(function () {        
    //});
});


