/* Webwatcher 1.9 search engine config script */
/* Copyright (c) 2022-2024 Mobren */

const navbar = document.querySelector(".navbar");
const urlParams = new URLSearchParams(window.location.search);
const search_engine_google = document.querySelector("#search_engine_google");
const search_engine_duckduckgo = document.querySelector("#search_engine_duckduckgo");

run_animation();

search_engine_google.onclick = function () {
    updateConfig("google");
}

search_engine_duckduckgo.onclick = function () {
    updateConfig("duckduckgo");
}

function run_animation() {
    if (!urlParams.has('disable_animations')) {
        navbar.classList.add("navbar_with_animation");
    }
}

function updateConfig(engine) {
    webwatcher.saveSearchEngineConfig(engine);
}