$(".searchFooorm").click(function (e) {
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
            $(".fooormList").remove();
            data.forEach(x => {
                let temp = `<li class="fooormList">
                                <a data-href="Fooorm/ShowFooormItem" class="fooormListA" id="${x.id}">
                                    <div class="header">
                                        <p>${x.header}</p>
                                    </div>
                                    <div class="info text-start">
                                        <p> ${x.answerCount} answer </p>
                                        <p> ${x.viewCount} view </p>
                                    </div>
                                </a>
                            </li>`;
                $(".newHeadersList").append(temp);
            });
        })
})
//$(document).on("click",".fooormListA" , function (e) {
//    document.querySelector(".fooormInputDiv").classList.remove("displayN");
//    e.preventDefault();
//    let url = $(this).attr("data-href");
//    let id = $(this).attr("id");
//    fetch(url, {
//        method: "POST",
//        body: JSON.stringify(Number(id)),
//        headers: {
//            'Content-type': 'application/json',
//        },
//    })
//        .then(response => {
//            return response.json();
//        })
//        .then(data => {
//            document.querySelector(".fooormInput").setAttribute("id", data.id);
//            $(".headerHTML").empty();
//            let headerHTML = `<div class="Title text-center mt-1">
//                        <h3>${data.header}</h3>
//                        </div>
//                        <div class="Body">
//                        <h4>
//                        ${data.text}
//                        </h4>
//                        </div>
//                        <hr class="mt-2 fooormChatHr mx-auto" />`;
//            $(".headerHTML").append(headerHTML);
//        })
//    fetch("/Fooorm/ShowFroomAnswers", {
//        method: "POST",
//        body: JSON.stringify(Number(id)),
//        headers: {
//            'Content-type': 'application/json',
//        },
//    })
//        .then(response => {
//            return response.json();
//        })
//        .then(data => {
//            let bodyHTML = "";
//            data.forEach(x => {
//                bodyHTML += `<li>
//                            <div>
//                            <img src="/uploads/users/${x.user.userPP}">
//                            <span class="col-12">${x.user.userName} |  ${x.createdAt.slice(11, 16)} </span>
//                            </div>
//                            <p class="col-12">${x.text}</p>
//                            <hr>
//                            </li>`;
//            })
//            $(".answerList").empty();
//            $(".answerList").append(bodyHTML);
//        })
//        .then(() => {
//            let elem = document.querySelector(".answerList");
//            elem.scrollTop = elem.scrollHeight;
//        })
//})