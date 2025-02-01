function addItem(dateText, fileText, fileName, fileUrl, downloadProgress) {
    const div = document.querySelector('.changelog');

    let existingFile = Array.from(div.querySelectorAll('a.download_item')).find(a => a.textContent.startsWith(fileName));

    if (existingFile) {
        existingFile.textContent = fileText;
        existingFile.href = fileUrl;
        return;
    }

    const date = document.createElement('h2');
    const file = document.createElement('a');

    date.textContent = dateText;
    file.textContent = fileText;
    file.href = fileUrl;

    if (downloadProgress >= 0) {
        file.addEventListener('contextmenu', function (event) {
            event.preventDefault();
            webwatcher.removeDownload(fileName);
            setTimeout(function () {
                const url = new URL(window.location.href);
                url.searchParams.set('disable_animations', '');
                window.location.href = url.toString();
            }, 500);
        });
    }

    file.classList.add('download_item');

    const headers = document.querySelectorAll('h2');
    const duplicateDate = Array.from(headers).some(h2 => h2.textContent.trim() === dateText);

    if (!duplicateDate) {
        const headings = div.querySelectorAll('h2');
        if (headings.length > 0) {
            div.appendChild(document.createElement('br'));
        }
        div.appendChild(date);
    }

    div.appendChild(file);
    div.appendChild(document.createElement('br'));
}