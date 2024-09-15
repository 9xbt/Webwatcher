/* Webwatcher 1.9 about page script */
/* Copyright (c) 2022-2024 Mobren */

const navbar = document.querySelector(".navbar");
const urlParams = new URLSearchParams(window.location.search);
const update_info = document.querySelector("#update_info");
const latest_version = "https://raw.githubusercontent.com/9xbt/Webwatcher/main/api/latest_version";

run_animation();
setTimeout(() => do_update_check(), 5);

function run_animation() {
    if (!urlParams.has('disable_animations')) {
        navbar.classList.add("navbar_with_animation");
    }
}

function do_update_check() {
    version_str.textContent = "Version " + webwatcher_ver;
    update_info.textContent = "Checking for updates...";

    fetch(latest_version)
        .then(a => a.text())
        .then(latest_ver => {
            if (latest_ver == webwatcher_ver) {
                update_info.textContent = "You have the latest version (last checked just now)";
            }
            else {
                update_info.textContent = "There's a new update available on GitHub (version " + latest_ver + ")";
            }
        });
}

function show_changelog() {
    location.href = "changelog.html";
}