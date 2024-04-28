/* Webwatcher 1.9 basic config script */
/* Copyright (c) 2022-2024 Mobren */

const navbar = document.querySelector(".navbar");
const urlParams = new URLSearchParams(window.location.search);
const use_hardware_accel = document.querySelector("#use_hardware_accel");

run_animation();

use_hardware_accel.onclick = function () {
    webwatcher.saveAdvancedConfig(use_hardware_accel.checked);

    if (confirm("This setting will only apply once Webwatcher is restarted.\nDo you want to restart now?")) {
        webwatcher.restart();
    }
}

function run_animation() {
    if (!urlParams.has('disable_animations')) {
        navbar.classList.add("navbar_with_animation");
    }
}