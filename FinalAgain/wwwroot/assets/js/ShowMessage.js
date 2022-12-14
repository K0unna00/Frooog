$(".serverBTN").click(function (e) {
    e.preventDefault();
    document.querySelector(".textArea").classList.remove("displayN");
    //document.querySelector(".placeHolderImg").classList.add("displayN");
    let thisBtn = $(this);
    let url = $(this).attr("href");
    let channelId = $(this).attr("id");
    let userName = $(this).attr("data-userId");
    document.getElementById("ChannelId").setAttribute("name", channelId);
    document.getElementById("ChannelId").setAttribute("value", channelId);

    connection.invoke("AddUserOnServer", Number(channelId));

    connection.invoke("ShowOthersCameras", Number(channelId));

    fetch(url, {
        method: "POST",
        body: JSON.stringify(Number(channelId)),
        headers: {
            'Content-type': 'application/json',
        },
    })
        .then(response => {
            return response.json();
        })
        .then(data => {
            let temp = "<ul>";
            for (let i of data) {
                if (i.user.userName == userName) {
                    temp +=
                        `<li class="d-flex flex-wrap">
                        <div>
                            <img src="/uploads/users/${i.user.userPP}">
                            <span class="col-12">${i.user.userName} | ${i.createdAt.slice(11, 16)}</span>
                            <a class="deleteMessage" href="Home/DeleteServerMessage" data-id="${i.id}"><i class="fa-solid fa-trash-can"></i></a>
                        </div>
                        <p >${i.text}</p>
                     </li>`
                }
                else {
                    temp +=
                        `<li class="d-flex flex-wrap">
                        <div>
                            <img src="/uploads/users/${i.user.userPP}">
                            <span class="col-12">${i.user.userName} | ${i.createdAt.slice(11, 16)}</span>
                        </div>
                        <p >${i.text}</p>
                     </li>` 
                }
                
            } 
            temp += "</ul>";
            $(".messageArea").html(temp);
            
        })
        .then(() => {
            let elem = document.querySelector(".messageArea");
            elem.scrollTop = elem.scrollHeight;
            //$(".deleteMessage").click(function (z) {
            //    //alert("s")
            //    z.preventDefault();
            //    let url = $(this).attr("href");
            //    let data = Number($(this).attr("data-id"));
            //    console.log(url);
            //    fetch(url, {
            //        method: "POST",
            //        body: JSON.stringify(data),
            //        headers: {
            //            'Content-type': 'application/json'
            //        }
            //    })
            //        .then(res => {
            //            return res.json();
            //        })
            //        .then(data => {
            //            console.log(data);
            //        })
            //    })
            });
})
$(document).on("click", ".deleteMessage", function (e) {
    e.preventDefault();
    console.log("ss");
})

connection.on("AddUserOnServer", function (id, user) {

    var check = false;

    let temp = `<li id='${user.userName}'>
                    <img src="/uploads/users/${user.userPP}" alt="">
                    <p>${user.userName}</p>
            </li>`;

    var liList = $(`.serverBTN`).next().find("ul").children();
    for (let i = 0; i < liList.length; i++) {
        if (liList[i].id == user.userName) {
            $(`#${user.userName}`).remove();
            if (user.onServerId != 0) {
                //const audio2 = new Audio();
                //audio2.src = "/assets/sounds/out_server.wav";
                //audio2.play();
            }
            else {
                //const audio1 = new Audio();
                //audio1.src = "/assets/sounds/join_server.wav";
                //audio1.play();

            }
        }
    }
    if (user.onServerId != id) {
        $(`#${id}`).next().find("ul").first().append(temp);
        if (user.onServerId==0) {
            //const audio1 = new Audio();
            //audio1.src = "/assets/sounds/join_server.wav";
            //audio1.play();
        }

       
    }
})
connection.on("DeleteUserOnServer", function (user) {
    var liList = $(`.serverBTN`).next().find("ul").children();
    for (let i = 0; i < liList.length; i++) {
        if (liList[i].id == user.userName) {
            $(`#${user.userName}`).remove();
        }
    }
})
connection.on("AddUsersOnServer", function (user) {
    let url = "/Home/GetUsers/";
    fetch(url).then(response => {
        return response.json();
    })
        .then(data => {
            data.forEach(x => {
                if (x.onServerId != 0) {
                    let Server = $(`#${x.onServerId}`);
                    let temp = `
                    <li id="${x.userName}">
                        <img src="/uploads/Users/${x.userPP}" alt="">
                        <p>${x.userName}</p>
                    </li>`;
                    Server.next().find("ul").first().append(temp);
                }
            });
        });
})
