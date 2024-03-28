/* Webwatcher 1.9 about config script */
/* Copyright (c) 2022-2024 Mobren */

const checkforupdates_button = document.querySelector("#checkforupdates");
const update_info = document.querySelector("#update_info");
const latest_version = "https://raw.githubusercontent.com/9xbt/Webwatcher/main/api/latest_version?a=" + Math.random(); /* avoid caching */

update_check();

checkforupdates_button.onclick = update_check;

function update_check() {
    update_info.textContent = "Checking for updates...";

    fetch(latest_version)
        .then(a => a.text())
        .then(latest_ver => {
            if (latest_ver == webwatcher_ver) {
                // TOOD: maybe make this say how long ago you actually checked, instead of it being hard coded
                update_info.textContent = "You have the latest version (last checked just now)";
            }
            else {
                update_info.textContent = "There's a new update available (last checked just now)";
            }
        });
}