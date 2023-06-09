import loadTheme from "./theme.js";
import { globalSettings } from "./settings.js";
let globalJson;

function getQueryVariable(variable) {
    const query = window.location.search.substring(1);
    const searchParams = new URLSearchParams(query);
    if (searchParams.has(variable)) {
        return searchParams.get(variable);
    }
}

function calculateTotal() {
    let total = 0;
    globalJson.sections.forEach(section => {
        section.items.forEach(item => total += item.count * item.price);
    });
    return total;
}

function updateTotal() {
    const total = calculateTotal();
    document.getElementById("totalPrice").innerText = total;
    document.getElementById("orderButton").disabled = total === 0;
}

function generateInput(item) {
    function makeId(length) {
        let result = '';
        const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        const charactersLength = characters.length;
        let counter = 0;
        while (counter < length) {
          result += characters.charAt(Math.floor(Math.random() * charactersLength));
          counter += 1;
        }
        return result;
    }
    const container = document.createElement("div");
    container.classList.add("col-auto", "clearfix", "p-0");
    const float = document.createElement("div");
    float.classList.add("float-md-end", "mb-1", "ms-md-3");
    const inputGroup = document.createElement("div");
    inputGroup.classList.add("input-group", "border", "rounded-3", "quantity-input");

    const inputId = makeId(5);
    const minusSpan = document.createElement("span");
    minusSpan.classList.add("input-group-btn");
    const minusButton = document.createElement("button");
    minusButton.classList.add("btn", "btn-default", "btn-number");
    minusButton.innerText = "➖";
    minusButton.addEventListener("click", function() {
        const input = document.getElementById(inputId);
        if (input.value != null && Number(input.value) > 0) {
            input.value = Number(input.value) - 1;
        }
        item.count = input.value;
        updateTotal();
    });
    minusSpan.appendChild(minusButton);

    const input = document.createElement("input");
    input.type = "number";
    input.id = inputId;
    input.value = item.count;
    input.classList.add("form-control");
    input.min = "0";
    input.addEventListener("input", function() {
        item.count = Number(input.value);
        updateTotal();
    });

    const plusSpan = document.createElement("span");
    plusSpan.classList.add("input-group-btn");
    const plusButton = document.createElement("button");
    plusButton.classList.add("btn", "btn-default", "btn-number");
    plusButton.innerHTML = "➕";
    plusButton.addEventListener("click", function() {
        const input = document.getElementById(inputId);
        input.value = Number(input.value) + 1;
        item.count = input.value;
        updateTotal();
    });
    plusSpan.appendChild(plusButton);

    inputGroup.appendChild(minusSpan);
    inputGroup.appendChild(input);
    inputGroup.appendChild(plusSpan);

    float.appendChild(inputGroup);
    container.appendChild(float);
    return container;
}

function generateNameAndPriceColumn(item) {
    const nameAndPriceColumn = document.createElement("div");
    nameAndPriceColumn.classList.add("col");
    const name = document.createElement("h3");
    name.textContent = item.name;
    const price = document.createElement("h3");
    price.classList.add("text-primary", "currency");
    price.textContent = item.price;
    nameAndPriceColumn.appendChild(name);
    nameAndPriceColumn.appendChild(price);
    return nameAndPriceColumn;
}

function generateDescription(description) {
    const p = document.createElement("p");
    p.innerText = description == null ? "": description;
    return p;
}

function insertItem(item, destinationContainer) {
    item.count = 0;
    const article = document.createElement("article");
    article.classList.add("mt-5");
    const container = document.createElement("div");
    container.classList.add("container", "px-2");
    const row = document.createElement("div");
    row.classList.add("row");
    row.appendChild(generateNameAndPriceColumn(item));
    row.appendChild(generateInput(item));
    container.appendChild(row);
    container.appendChild(generateDescription(item.description));
    article.appendChild(container);
    destinationContainer.appendChild(article);
}

function loadSections(sections) {
    const container = document.getElementById("sectionsContainer");
    sections.forEach(section => {
        const s = document.createElement("section");
        s.classList.add("col");
        const t = document.createElement("h2");
        t.classList.add("border-bottom", "border-primary");
        t.textContent = section.name;
        s.appendChild(t);
        section.items.forEach(item => insertItem(item, s));
        container.appendChild(s);
    });
}

function loadTitle(title) {
    document.getElementById("mainTitle").innerText = title;
}

function loadLogo(logoUrl) {
    document.getElementById("mainLogo").src = logoUrl;
}

function loadData(json) {
    globalJson = json;
    if(json.hasOwnProperty("sections")) {
        loadSections(globalJson.sections);
    }
    if(json.hasOwnProperty("theme")) {
        loadTheme(globalJson.theme);
    }
    if(json.hasOwnProperty("title")) {
        loadTitle(globalJson.title);
    }
    if(json.hasOwnProperty("logo")) {
        loadLogo(globalJson.logo);
    }
    updateTotal();
}

function getOrderBody() {
    return JSON.stringify({
        Items: globalJson.sections.map(
            section => section.items.filter(
                item => item.count > 0
            ).map(
                item => `${item.count}x ${item.name}`
            )
        ).flat(1).toString(),
        TableNumber: 4
    });
}

function getSaleQueryString() {
    const params = new URLSearchParams({
        Key: globalSettings.apiKey,
        Password: globalSettings.apiPassword,
        TerminalId: globalSettings.terminalId,
        Command: "sale",
        MerchantId: "",
        RefId: sessionStorage.getItem("refId"),
        PaymentType: "",
        Amount: calculateTotal(),
        InvoiceNumber: "",
        ExpDate: ""
    });
    return `runTransaction?${params.toString()}`;
}

function sendPaymentRequest() {
    const url = globalSettings.eConduitUrl + getSaleQueryString();
    fetch(url).catch(exception => {
        console.error(exception);
        showErrorModal("Error - Please contact staff", exception);
    });
}

function sendOrderPaid() {
    console.log("order paid");
    fetch(globalSettings.managementSystemUrl + "/api/order/status", {
        method: "PUT",
        body: JSON.stringify({
            id: sessionStorage.getItem("orderId"),
            status: "paid"
        }),
        headers: new Headers({
            "Content-Type": "application/json",
            "authorization": "Bearer 1234"
        })
    }).then(
        console.log("sent order paid")
    ).catch(exception => {
        showErrorModal("Error - Please contact staff", exception);
    });
}

function orderRejected(message) {
    showErrorModal("Your order was rejected", message);
}

function hidePleaseWaitModal() {
    const modalEl = document.getElementById("pleaseWaitModal");
    modalEl.addEventListener('shown.bs.modal', () => {
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal.hide();
    });
}

function showErrorModal(title, message) {
    const modalEl = document.getElementById("errorModal");
    document.getElementById("errorModalTitle").innerText = title;
    document.getElementById("errorModalText").innerText = message;
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();
}

function sendOrder() {
    sessionStorage.setItem("refId", Date.now());
    fetch(globalSettings.managementSystemUrl + "/api/order", {
        method: "POST",
        body: getOrderBody(),
        headers: new Headers({
            "Content-Type": "application/json",
            "authorization": "Bearer 1234"
        })
    }).then(
        response => response.json()
    ).then((data) => {
        if (data.status === "confirmed") {
            sessionStorage.setItem("orderId", data.id);
            sendPaymentRequest();
        }
        else {
            hidePleaseWaitModal();
            orderRejected(data.message);
            sessionStorage.removeItem("refId");
            sessionStorage.removeItem("orderId");
        }
    }).catch(exception => {
        console.error(exception);
        showErrorModal("Error - Please contact staff", exception);
        sessionStorage.removeItem("refId");
        sessionStorage.removeItem("orderId");
    });
}
function getDataJSON() {
    fetch("data.json")
    .then(response => response.json())
    .then(json => loadData(json))
    .catch(exception => {
        showErrorModal("Error - Please contact staff", exception);
    });
}

function setGlobalSettingsFromQueryString() {
    globalSettings.apiKey = getQueryVariable("apiKey");
    globalSettings.apiPassword = getQueryVariable("apiPassword");
    globalSettings.terminalId = getQueryVariable("terminalId");
    globalSettings.tableNumber = getQueryVariable("tableNumber");
}

function getCheckStatusQueryString(refId) {
    const formatDateToMMDDYYY = date => {
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const days = date.getDate().toString().padStart(2, '0');
        const year = date.getFullYear().toString();
        return `${month}${days}${year}`;
    };
    const params = new URLSearchParams({
        Key: globalSettings.apiKey,
        Password: globalSettings.apiPassword,
        TerminalId: globalSettings.terminalId,
        Command: "sale",
        RefId: refId,
        Date: formatDateToMMDDYYY(new Date())
    });
    return `checkStatus?${params.toString()}`;
}

function sendCheckStatus(refId) {
    const url = globalSettings.eConduitUrl + getCheckStatusQueryString(refId);
    fetch(url)
    .then(
        response => {
            return response.json();
        })
    .then((data) => {
        console.log(data);
        if(data.ResultCode === "Approved") {
            sendOrderPaid();
        }
        else {
            throw new Error("failed to parse data", data);
        }
    }).catch(exception => {
        console.error(exception);
        showErrorModal("Error - Error with payment - please order again", exception);
    }).finally(() => {
        sessionStorage.removeItem("refId");
        sessionStorage.removeItem("orderId");
    });
}

function ready() {
    getDataJSON();
    setGlobalSettingsFromQueryString();
    document.getElementById("orderButton").addEventListener("click", sendOrder);
    //setTimeout(getDataJSON, 5000); //refresh the data every 30 seconds
    if (sessionStorage.getItem("refId") != null) {
        sendCheckStatus(sessionStorage.getItem("refId"));
    }
}

document.addEventListener("DOMContentLoaded", ready);
