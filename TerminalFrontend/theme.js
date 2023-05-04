export default function loadTheme(theme) {
    if (theme.hasOwnProperty("currency")) {
        setCurrency(theme.currency);
    }
    if (theme.hasOwnProperty("fgColor")) {
        setFgColor(theme.fgColor);
    }
    if (theme.hasOwnProperty("bgColor")) {
        setBgColor(theme.bgColor);
    }
    if (theme.hasOwnProperty("textPrimary")) {
        setTextPrimaryColor(theme.textPrimary);
    }
    if (theme.hasOwnProperty("textSecondary")) {
        setTextSecondaryColor(theme.textSecondary);
    }
    if (theme.hasOwnProperty("btnTextColor")) {
        setBtnTextColor(theme.btnTextColor);
    }
    if (theme.hasOwnProperty("btnBgColor")) {
        setBtnBgColor(theme.btnBgColor);
    }
    if (theme.hasOwnProperty("btnActiveTextColor")) {
        setBtnActiveTextColor(theme.btnActiveTextColor);
    }
    if (theme.hasOwnProperty("btnActiveBg")) {
        setBtnActiveBg(theme.btnActiveBg);
    }
    if (theme.hasOwnProperty("btnActiveBorderColor")) {
        setBtnActiveBorderColor(theme.btnActiveBorderColor);
    }
    if (theme.hasOwnProperty("btnBorderColor")) {
        setBtnBorderColor(theme.btnBorderColor);
    }
    if (theme.hasOwnProperty("underlineColor")) {
        setUnderlineColor(theme.underlineColor);
    }
}

const setFgColor = (hexColor) => {
    setStyle(".bg-light", "background-color", hexColor)
}
const setBgColor = (hexColor) => {
    setStyle(".bg-dark", "background-color", hexColor)
}
const setBtnActiveTextColor = (hexColor) => {
    setBtnProperty('--btn-active-color', hexColor);
}
const setBtnActiveBg = (hexColor) => {
    setBtnProperty('--btn-active-bg', hexColor);
}
const setBtnActiveBorderColor = (hexColor) => {
    setBtnProperty('--btn-active-border-color', hexColor);
}
const setBtnBorderColor = (hexColor) => {
    setStyle(".btn-primary", "border-color", hexColor)
}
const setBtnBgColor = (hexColor) => {
    setStyle(".btn-primary", "background-color", hexColor);
}
const setBtnTextColor = (hexColor) => {
    setStyle(".btn-primary", "color", hexColor);
}
const setTextPrimaryColor = (hexColor) => {
    document.body.style.color = hexColor;
}
const setTextSecondaryColor = (hexColor) => {
    setStyle(".text-primary", "color", hexColor)
}
const setUnderlineColor = (hexColor) => {
    setStyle(".border-primary", "border-color", hexColor)
}

const setStyle = (selector, key, value) => {
    document.querySelectorAll(selector).forEach((e) =>
        e.style.setProperty(key, value, "important")
    );
}

const setBtnProperty = (property, value) => {
    document.querySelector('.btn-primary').style.setProperty(property, value);
};

const setCurrency = (value) => {
    document.documentElement.style.setProperty("--currency", `"${value}"`);
}
