var attachments;
function getAttachments () {
    return attachments;
}


var uploadfiles = document.querySelector('#fileinput');
uploadfiles.addEventListener('change', function () {
    var files = this.files;
    attachments = files;
    var thumbs = document.getElementById("thumbs");
    thumbs.innerHTML = "";
    for (var i = 0; i < files.length; i++) {
        previewImage(this.files, i);
    }

}, false);

function previewImage(files, id) {
    var imageType = /image.*/;

    var file = files[id];

    if (!file.type.match(imageType)) {
        throw "File Type must be an image";
    }
    var thumbs = document.getElementById("thumbs");
    var thumb = document.createElement("div");
    thumb.classList.add('thumbnail'); // Add the class thumbnail to the created div
    thumb.style = "width:50%; height:auto; float:left;";

    var button = document.createElement("button");

    button.classList.add("close");
    var buttonText = document.createTextNode("x");
    var att = document.createAttribute("data-dismiss");  
    att.value = "alert";  
    button.appendChild(buttonText);
    button.setAttributeNode(att);
    button.setAttribute("id", "\"" + id + "\"");





    thumb.appendChild(button);


    var img = document.createElement("img");
    img.file = file;
    img.width = 130;
    img.height = 130;
    var thumbInfo = document.createElement("div");
    thumbInfo.innerHTML = "<span> " + file.name + "<br>" + parseFloat(file.size / 1024).toFixed(2) + " kb </span>";
    thumbInfo.style = "width:auto; height:auto; float:left;";

    thumbs.appendChild(thumb).appendChild(thumbInfo).appendChild(img);


    document.getElementById("\"" + id + "\"").addEventListener("click", function () {
        var fileInput = document.querySelector('#fileinput');
        //fileInput.value = ""; // this will reset all the files
        document.getElementById('AttachmentsToIgnore').value += "|" + fileInput.value;
    }, false);

    var imgData = "";
    // Using FileReader to display the image content
    var reader = new FileReader();
    reader.onload = (function(aImg) {
         return function(e) {
             aImg.src = e.target.result;
             
         };
    })(img);
    reader.readAsDataURL(file);
}

function closeThumb(files, id) {
    alert(id);
    delete files[id];
    
    for (var i = 0; i < files.length; i++) {
        alert(uploadfiles[i].file.name);
    }
}

function uploadFile(file) {
    //var url = "/MyWall/UploadFile/";
    //var xhr = new XMLHttpRequest();
    var form = $("form")[0];
    var fd = new FormData(form);
    //xhr.open("POST", url, true);
    //xhr.onreadystatechange = function () {
    //    if (xhr.readyState === 4 && xhr.status === 200) {
    //        // Every thing ok, file uploaded
    //        console.log(xhr.responseText); // handle response.
    //    }
    //};
    //fd.append("uploadFile", file);
    //xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    //xhr.send(fd);

   $.ajax({
       url: "/mywall/UploadFile/",
       data :fd,
       method: "POST",
       processData: false, // NEEDED, DON'T OMIT THIS
        success: function () {
        }
    });
}
