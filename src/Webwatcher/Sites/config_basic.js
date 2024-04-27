/* Webwatcher 1.9 basic config script */
/* Copyright (c) 2022-2024 Mobren */

const navbar = document.querySelector(".navbar");
const urlParams = new URLSearchParams(window.location.search);
const homepage_type_def = document.querySelector("#homepage_type_def");
const homepage_type_man = document.querySelector("#homepage_type_man");
const homepage_url = document.querySelector("#homepage_url");

run_animation();

homepage_url.addEventListener('keyup', updateConfig);

homepage_type_def.onclick = function () {
    homepage_url.disabled = true;
    updateConfig();
}

homepage_type_man.onclick = function () {
    homepage_url.disabled = false;
    updateConfig();
}

function run_animation() {
    if (!urlParams.has('disable_animations')) {
        navbar.classList.add("navbar_with_animation");
    }
}

function updateConfig() {
    webwatcher.saveConfig(homepage_url.value, homepage_type_def.checked);
}