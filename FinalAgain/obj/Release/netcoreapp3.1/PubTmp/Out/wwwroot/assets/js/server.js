$(".userList-toggle").click(function(){
    let serverList=document.querySelector(".serverUsers");
    if(window.getComputedStyle(serverList).display=="block"){
        document.getElementById("SL").style.display = "none";
    }
    else{
        document.getElementById("SL").style.display = "block";
    }
})