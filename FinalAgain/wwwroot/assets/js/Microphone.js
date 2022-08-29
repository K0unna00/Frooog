
function captureUserMedia(mediaConstraints, successCallback, errorCallback) {
    navigator.mediaDevices.getUserMedia(mediaConstraints).then(successCallback).catch(errorCallback);
}

var mediaConstraints = {
    audio: true
};
function startRecording(idx) {
    $('#start-recording').disabled = true;
    captureUserMedia(mediaConstraints, onMediaSuccess, onMediaError);
};

function stopRecording() {
    $('#stop-recording').disabled = true;

    mediaRecorder.stop();
    mediaRecorder.stream.stop();


    $('.start-recording').disabled = false;
};

var mediaRecorder;

function onMediaSuccess(stream) {
    mediaRecorder = new MediaStreamRecorder(stream);
    mediaRecorder.stream = stream;
    mediaRecorder.mimeType = 'audio/wav';
    mediaRecorder.audioChannels = 1;
    mediaRecorder.ondataavailable = function (blob) {
        var data = blob.stream();
        connection.invoke("SendVoiceMessage", data);
        $('#record-audio').html("<audio controls=''><source src=" + URL.createObjectURL(blob) + "></source></audio>");
        console.log(blob, data);
        const formData = new FormData();
        formData.append("data.mp3", blob);
        fetch("/Chat/SetVoiceInbox", {
            method: "POST",
            body: blob,
            headers: {
                Authentication: 'secret'
            },
        });
    };
    
    var timeInterval = 360 * 1000;

    mediaRecorder.start(timeInterval);

    $('#stop-recording').disabled = false;
}
///////////=========================================

function onMediaError(e) {
    console.error('media error', e);
}
///////////=========================================
function bytesToSize(bytes) {
    var k = 1000;
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes === 0) return '0 Bytes';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(k)), 10);
    return (bytes / Math.pow(k, i)).toPrecision(3) + ' ' + sizes[i];
}
///////////=========================================

function getTimeLength(milliseconds) {
    var data = new Date(milliseconds);
    return data.getUTCHours() + " hours, " + data.getUTCMinutes() + " minutes and " + data.getUTCSeconds() + " second(s)";
}
///////////=========================================

window.onbeforeunload = function () {
    $('#start-recording').disabled = false;
};
document.querySelector("#start-recording").addEventListener("click", function (e) {
    e.preventDefault();
    startRecording();
})

document.querySelector("#stop-recording").addEventListener("click", function (e) {
    e.preventDefault();
    stopRecording();
})

