let localStream;
let JSuser;
let init = async () => {
    localStream = await navigator.mediaDevices.getUserMedia({ video: false, audio: true })
    document.querySelectorAll(".video-player").forEach(x => {
        let user = document.getElementById(`${x.id}`)
        user.setAttribute('playsinline', '');
        user.setAttribute('autoplay', '');
        user.srcObject = localStream;
    })
}
$("#onCam").click(function () {
    let videoBox = document.querySelector(".videoBox");
    var currentUser = $(this).attr("data-userName");
    let check = true;
    for (let i = 0; i < videoBox.childElementCount; i++) {
        if (videoBox.children[i].classList.contains(`${currentUser}`)) {
            connection.invoke("CloseCamera", i);
            check = false;
            break;
        }
    }
    if (check) {
        connection.invoke("ShowCameraData");
    }
})
connection.on("ReceiveCameraData", function (JSuser) {
    init();
    let videoBox = document.querySelector(".videoBox");
    let col = document.createElement("div");
    let p = document.createElement("p");
    let videoDiv = document.createElement("div");
    let video = document.createElement("video");
    video.setAttribute("id", `${JSuser.userName}`);
    video.setAttribute("data-userId", `${JSuser.userName}`);
    video.classList.add("video-player");    
    p.innerText = JSuser.userName;
    videoDiv.appendChild(video);
    col.appendChild(p);
    col.appendChild(videoDiv);
    col.classList.add("videoDiv");
    col.classList.add(`${JSuser.userName}`);
    videoBox.appendChild(col);


    
})
connection.on("CloseCameraFromAllUser", function (i) {
        let videoBox = document.querySelector(".videoBox");
        videoBox.removeChild(videoBox.children[i]);
})
connection.on("CloseCameraFromAllUserWhenDisconnect", function (name) {
    let videoBox = document.querySelector(".videoBox");
    for (let i = 0; i < videoBox.childElementCount; i++) {
        if (videoBox.children[i].classList.contains(`${name}`)) {
            videoBox.removeChild(videoBox.children[i]);
            check = false;
            break;
        }
    }
})
connection.on("ShowCameraToNewUser", function () {
    alert("ss");
})