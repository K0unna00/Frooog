$(".userBtn").click(function (e) {
    e.preventDefault();
    document.querySelector(".textArea").classList.remove("displayN");
    let url = $(this).attr("href");
    let FUserName = $(this).attr("data-FUserName");
    let TUserId = $(this).attr("data-TUserId");

    document.getElementById("FUserID").setAttribute("name", FUserName);
    document.getElementById("FUserID").setAttribute("value", FUserName);

    document.getElementById("TUserID").setAttribute("name", TUserId);
    document.getElementById("TUserID").setAttribute("value", TUserId);


    fetch(url, {
        method: "POST",
        body: JSON.stringify({ "FromUserName": FUserName, "ToUserId": TUserId }),
        headers: {
            'Content-type': 'application/json',
        },
    })
        .then(response => {
            return response.json();
        })
        .then(data => {
            if (data.length>0) {
                let temp = "<ul>";
                for (let i of data) {
                    temp +=
                        `<li class="d-flex flex-wrap">
                        <div>
                            <span class="col-12">${i.fromUserName} | ${i.createdAt.slice(11, 16)}</span>
                        </div>
                        <p class="col-12">${i.text}</p>
                     </li>`
                }
                temp += "</ul>";
                $(".messageArea").html(temp);
            }
            else {
                let temp = "<ul>";
                temp +=
                    `<li class="d-flex flex-wrap text-center">
                        <p class="col-12">There Is No meassage for you :(</p>
                     </li>`;
                temp += "</ul>";
                $(".messageArea").html(temp);
            }
            
        })
        .then(() => {
            let elem = document.querySelector(".messageArea");
            elem.scrollTop = elem.scrollHeight;
        })
})