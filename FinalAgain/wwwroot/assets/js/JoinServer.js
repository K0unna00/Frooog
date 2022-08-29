//$(".joinServerBtn").click(function (e) {
//    e.preventDefault();
//    var url = $(this).attr("href");
//    var id = $(this).attr("data-id");
//    fetch(url, {
//        method: "POST",
//        body: JSON.stringify(Number(id)),
//        headers: {
//            'Content-type': 'application/json',
//        },
//    })
//        .then(response => {
//            if (!response.ok) {
//                console.log(response)
//                return;
//            }
//            return response.json();
//        })
//        .then(data => {
//            if (data) {
//                alert("Success");
//            }
//            else {
//                alert("You are already member");
//            }
//        })
//})