function renderJSON(jsonStr, divId) {
    setTimeout(() => {
            const formatter = new JSONFormatter(jsonStr);
            var container = document.getElementById(divId);
            if (container.children.length > 0) {
                container.replaceChild(formatter.render(), container.children);
                return;
            }
            container.appendChild(formatter.render());
        },
        1);
    return;
}