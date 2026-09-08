function clone(value) {
    return JSON.parse(JSON.stringify(value));
}

export function parseBoolean(value) {
    if (typeof value === "boolean") return value;
    if (typeof value === "number") return value !== 0;
    if (typeof value === "string") {
        const normalized = value.trim().toLowerCase();
        return normalized === "1" || normalized === "true";
    }
    return Boolean(value);
}

export function utcToLocal(value) {

    const date = new Date(`${value.replace(" ", "T")}Z`);

    const pad = value => String(value).padStart(2, "0");

    return [
        date.getFullYear(),
        pad(date.getMonth() + 1),
        pad(date.getDate())
    ].join("-") + "T" + [
        pad(date.getHours()),
        pad(date.getMinutes()),
        pad(date.getSeconds())
    ].join(":");

}

export function hideLabel(element) {
    const umbPropertyLayout = element.parentElement?.parentElement;
    const headerColumn = umbPropertyLayout?.shadowRoot?.querySelector("#headerColumn");
    if (!headerColumn) return;
    umbPropertyLayout.setAttribute("orientation", "vertical");
    headerColumn.style.display = "none";
}