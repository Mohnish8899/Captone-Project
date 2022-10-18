export function getCookie(key: string) {
    const x = document.cookie.match("(^|;)\\s*" + key + "\\s*=\\s*([^;]+)");
    return x ? x.pop() : "";
}

