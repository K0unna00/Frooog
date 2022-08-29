$(".UserSearchButton").click(function (e) {
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
            if (data) {
                $("#searchedUser").empty();
                let temp="";
                temp +=
                    `<li class="d-flex col-3 mt-3 justify-content-between RequestUserListItem">
                                    <div class="user">
                                        <img src="/uploads/users/${data.userPP}" alt="">
                                        <p class="text-white " style="font-size:20px;" >${data.userName}</p>
                                    </div>
                                    <button href="/Friendship_FriendRequest/SendRequest" class="SendRequest" id="${data.id}">Send Request</button>
                    </li>`;
                $("#searchedUser").append(temp);
                $(".SendRequest").click(function () {
                    var url = $(this).attr("href");
                    var value = $(this).attr("id");
                    fetch(url, {
                        method: "POST",
                        body: JSON.stringify(value),
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
                            if (data == true) {
                                alert("Succeed");
                            }
                            else (alert("Something went wrong"));
                        })
                })
            }
            else
                alert("No user match your input or User is allready your friend")
    })
    document.querySelector(".searchInput").value = "";
    console.log($(".SendRequest"));
})

