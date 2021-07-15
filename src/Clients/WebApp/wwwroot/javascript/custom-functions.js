function renderJSON(jsonStr, divId) {
    setTimeout(() => {
            const formatter = new JSONFormatter(jsonStr);
            var container = document.getElementById(divId);
            container.appendChild(formatter.render());
        },
        1);
    return;
}