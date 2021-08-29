// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var audioTrack = WaveSurfer.create({
    container: ".audio",
    waveColor: "white",
    progressColor: "green",
    barWidth: 2,
});

/*TODO Find out how to do this reference to the .wav file*/
audioTrack.load("bircall.wav");

const playBtn = document.querySelector(".play-btn");
const stopBtn = document.querySelector(".stop-btn");
const muteBtn = document.querySelector(".mute-btn");
const volumeSlider = document.querySelector(".volume-slider");

playBtn.addEventListener("click", () => {
    audioTrack.playPause();

    if (audioTrack.isPlaying()) {
        playBtn.classList.add("playing");
    } else {
        playBtn.classList.remove("playing");
    }
});

stopBtn.addEventListener("click", () => {
    audioTrack.stop();
    playBtn.classList.remove("playing");
});

volumeSlider.addEventListener("mouseup", () => {
    changeVolume(volumeSlider.value);
});

const changeVolume = (volume) => {
    if (volume == 0) {
        muteBtn.classList.add("muted");
    } else {
        muteBtn.classList.remove("muted");
    }

    audioTrack.setVolume(volume);
};

muteBtn.addEventListener("click", () => {
    if (muteBtn.classList.contains("muted")) {
        muteBtn.classList.remove("muted");
        audioTrack.setVolume(0.5);
        volumeSlider.value = 0.5;
    } else {
        audioTrack.setVolume(0);
        muteBtn.classList.add("muted");
        volumeSlider.value = 0;
    }
});
