$(".ServerSearchButton").click(function (e) {
    e.preventDefault();
    let inputValue = $(".searchInput").val();
    let url = $(this).attr("href");
    fetch(url, {
        method: "POST",
        body: JSON.stringify(inputValue),
        headers: {
            'Content-type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                console.log(response)
                return;
            }
            return response.json();
        })
        .then(data => {
            $("#searchedServers").empty();
            var temp = "";
            data.forEach(x => {
                temp += `<li class="d-flex serverCard col-8 ">
                                <div class="col-4 leftSide">
                                    <img src="/uploads/servers/${x.serverImage}"/>
                                </div>
                                <div class="col-8 rightSide">
                                    <div class="cardTop">
                                        <div class="ServerName text-center">
                                            <p>${x.name}</p>
                                        </div>
                                        <div class="ServerDescription text-center">
                                            <p>${x.desc}</p>
                                        </div>
                                    </div>
                                    <div class="d-flex justify-content-between">
                                        <div class="MemberCount">
                                            <p>Member:</p>
                                        </div>
                                        <a class="joinServerBtn" ${ x.isPrivate ? "href=SendJoinRequest" : "href=JoinServer" } data-id="@server.Id">${x.isPrivate? "Send Request" :"Join" }</a>
                                    </div>
                                </div>
                            </li>`
            });
            $("#searchedServers").append(temp);
        })
})
$(document).on("click", ".joinServerBtn", function (e) {
    e.preventDefault();
    var url = $(this).attr("href");
    var id = $(this).attr("data-id");
    fetch(url, {
        method: "POST",
        body: JSON.stringify(Number(id)),
        headers: {
            'Content-type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                console.log(response)
                return;
            }
            return response.json();
        })
        .then(data => {
            if (data) {
                alert("Success");
            }
            else {
                alert("You are already member");
            }
        })
})