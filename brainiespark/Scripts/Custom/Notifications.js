
function addNotification(event, notificationElementId, token) {
    var data = JSON.parse(event.data);
    var notificationPanel = document.getElementById(notificationElementId);
    notificationPanel.innerHTML =
        "<div class=\"alert alert-dismissible alert-success\" style=\"min-height:125px\"><button notificationId=\"" +
        data.Id +
        "\" type=\"button\" class=\"close\" data-dismiss=\"alert\" onclick=\"OnClose(" +
        data.Id + ",'" + notificationElementId + "','" + token + 
        "')\">&times;</button>\<strong>" +
        data.NotificationDate +
        " : </strong> <span>" +
        data.Message +
    "</span><button id = \"dialog\" class = \"btn btn-link\" style = \"float: right;\" onclick=\"OnRead(" +
        data.Id + ",'" + notificationElementId + "','" + token + 
        "')\">[Read...]</button></div>" +
        notificationPanel.innerHTML;

    notificationPanel.scrollTop = notificationPanel.scrollHeight;
    var selector = "#" + notificationElementId + " > div";
    var receivedCount = $(selector).size();

    //document.getElementById('messageArrived').play();
    document.getElementById("NotificationCount").innerHTML =
        "<a href=\"#notifications\" data-toggle=\"tab\" aria-expanded=\"false\">Received  <span class=\"badge\">" +
        receivedCount +
        "</span></a>";
}

//function OnRead(notificationId, notificationElementId, token)
//{
//    $.ajax({
//        url: "../ViewNotification/ViewNotifiacationStyle1/" + notificationId,
//        method: "GET",
//        success: function () {
           
//        },
//        headers: {
//            'RequestVerificationToken': token
//        }
//    });
//}

function OnClose(notificationId, notificationElementId, token) {
    $.ajax({
        url: "/api/notifications/" + notificationId,
        method: "DELETE",
        success: function () {
            var selector = "#" + notificationElementId + " > div";
            var receivedCount = $(selector).size();
            document.getElementById("NotificationCount").innerHTML =
                "<a href=\"#notifications\" data-toggle=\"tab\" aria-expanded=\"false\">Received <span class=\"badge\">" +
                receivedCount +
                "</span></a>";
        },
        headers: {
            'RequestVerificationToken' : token
        }
    });
}


function reSyncActiveNoifications(tokens) {

    $.ajax({
        url: "/api/notifications/ReSyncActiveNoifications",
        method: "PUT",
        success: function() {
        },
        headers: {
            'RequestVerificationToken': tokens
        }
    });
}


