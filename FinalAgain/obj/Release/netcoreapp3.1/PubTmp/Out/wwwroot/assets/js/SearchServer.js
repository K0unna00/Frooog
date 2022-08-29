$(".searchButton").click(function (e) {
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
            let searchedData = [];
            data.forEach(x => {
                if (x.name == inputValue) {
                    searchedData.push(x);
                }
            })
            if (searchedData.length > 0) {
                $(".ServerList").empty();
                let temp;
                searchedData.forEach(x => {
                    if (x.name.length > 10)
                        x.name = x.name.substring(0, 10) + "...";
                    temp +=
                        `<li>
                            <a class="server serverBTN" id="${x.id}" href="/home/GetMessages">
                                <img src="/uploads/servers/${x.serverImage}" alt="">
                                <p> ${x.name} </p>
                            </a>
                        </li> `;
                })
                $(".ServerList").append(temp);
                //$(".ServerList").find(':first-child').remove();

            }
            else {
                alert("No servers match your input")
            }
        })
    //document.getElemensByClassName("searchInput").value = "";
})