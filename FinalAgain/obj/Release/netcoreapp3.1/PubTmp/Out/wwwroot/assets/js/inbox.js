$(".userList-toggle").click(function(){
    let userList=document.querySelector(".inboxUserList");
    if(window.getComputedStyle(userList).display=="block"){
        document.getElementById("UL").style.display = "none";
    }
    else{
        document.getElementById("UL").style.display = "block";
    }
    let serverList=document.querySelector(".serverUsers");
})