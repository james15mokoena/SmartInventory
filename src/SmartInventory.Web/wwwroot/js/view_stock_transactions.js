const btnAll = document.querySelector(".menu-item .btn-all");
const btnIssued = document.querySelector(".menu-item .btn-issued");
const btnReceived = document.querySelector(".menu-item .btn-received");
const btnReturned = document.querySelector(".menu-item .btn-returned");
const btnDamaged = document.querySelector(".menu-item .btn-damaged");
const btnNewProduct = document.querySelector(".menu-item .btn-new-product");

/**
 * Assigns the "select" property to the clicked button.
 * @param {*} selectedBtn The selected button.
 * @param {*} selectedBtnName The name of the selected button
 * @param {*} url The url of the page to be loaded.
 */
function selectOrDeselectBtnHandler(selectedBtn, selectedBtnName, url) {
    if (!selectedBtn.classList.contains("selected")) {
        localStorage.setItem("selectedBtn", selectedBtnName);
        if (url != null)
            window.location.href = url;
    }
}

/**
 * Handles the changing of the background color of the selected
 * or deselected button.
 */
btnAll.addEventListener("click", () => selectOrDeselectBtnHandler(btnAll, "btnAll", "ViewStockTransactions"));

btnIssued.addEventListener("click", () => selectOrDeselectBtnHandler(btnIssued, "btnIssued", "ViewStockTransactions?reason=Issued"));

btnReceived.addEventListener("click", () => selectOrDeselectBtnHandler(btnReceived,"btnReceived","ViewStockTransactions?reason=Received"));

btnReturned.addEventListener("click", () => selectOrDeselectBtnHandler(btnReturned,"btnReturned","ViewStockTransactions?reason=Returned"));

btnDamaged.addEventListener("click", () => selectOrDeselectBtnHandler(btnDamaged,"btnDamaged","ViewStockTransactions?reason=Damaged"));

btnNewProduct.addEventListener("click", () =>
    selectOrDeselectBtnHandler(btnNewProduct, "btnNewProduct", "ViewStockTransactions?reason=New Product"));

/**
 * Changes the background color of the selected button.
 * @param {*} selectedBtn The selected button.
 */
function changeButtonBgColor(selectedBtn) {
    selectedBtn.classList.add("selected");
    selectedBtn.style.backgroundColor = "lightseagreen";
}

// Changes the background color of the selected or deselected menu option.
if (localStorage.getItem("selectedBtn") === "btnAll") {
    changeButtonBgColor(btnAll);
}
else if(localStorage.getItem("selectedBtn") === "btnIssued"){
    changeButtonBgColor(btnIssued);
}
else if(localStorage.getItem("selectedBtn") === "btnReceived"){
    changeButtonBgColor(btnReceived);
}
else if(localStorage.getItem("selectedBtn") === "btnReturned"){
    changeButtonBgColor(btnReturned);
}
else if(localStorage.getItem("selectedBtn") === "btnDamaged"){
    changeButtonBgColor(btnDamaged);
}
else if(localStorage.getItem("selectedBtn") === "btnNewProduct"){
    changeButtonBgColor(btnNewProduct);
}
else {
    changeButtonBgColor(btnAll);
}