/* Webwatcher 1.9 basic config script */
/* Copyright (c) 2022-2024 Mobren */

const navbar = document.querySelector(".navbar");
const urlParams = new URLSearchParams(window.location.search);
const homepage_type_def = document.querySelector("#homepage_type_def");
const homepage_type_man = document.querySelector("#homepage_type_man");
const homepage_url = document.querySelector("#homepage_url");
const search_engine_google = document.querySelector("#search_engine_google");
const search_engine_duckduckgo = document.querySelector("#search_engine_duckduckgo");
const search_engine_yahoo = document.querySelector("#search_engine_yahoo");

run_animation();

homepage_url.addEventListener('keyup', saveBasicConfig);

homepage_type_def.onclick = function () {
    homepage_url.disabled = true;
    webwatcher.saveBasicConfig(homepage_url.value, homepage_type_def.checked, search_engine);
}

homepage_type_man.onclick = function () {
    homepage_url.disabled = false;
    webwatcher.saveBasicConfig(homepage_url.value, homepage_type_def.checked, search_engine);
}

search_engine_google.onclick = function () {
    search_engine = "google";
    webwatcher.saveBasicConfig(homepage_url.value, homepage_type_def.checked, "google");
}

search_engine_duckduckgo.onclick = function () {
    search_engine = "duckduckgo";
    webwatcher.saveBasicConfig(homepage_url.value, homepage_type_def.checked, "duckduckgo");
}

search_engine_yahoo.onclick = function () {
    search_engine = "yahoo";
    webwatcher.saveBasicConfig(homepage_url.value, homepage_type_def.checked, "yahoo");
}

function run_animation() {
    if (!urlParams.has('disable_animations')) {
        navbar.classList.add("navbar_with_animation");
    }
}

function saveBasicConfig() {
    webwatcher.saveBasicConfig(homepage_url.value, homepage_type_def.checked, search_engine);
}