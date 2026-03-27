// get the "active-products" btn
const btnActiveProducts = document.querySelector(".menu-option .btn-active-products");
// get the "deactivated-products" btn
const btnDeactivatedProducts = document.querySelector(".menu-option .btn-deactivated-products");
// get the "dropdown-prod" btn
const btnDropdown = document.querySelector(".dropbtn-prod");

/**
 * Assigns the "select" class to a clicked button.
 * @param {*} selectedBtn The selected button.
 * @param {*} selectedBtnName The name of the selected button.
 * @param {*} url The url of the page to be opened.
 */
function selectOrDeselectBtnHandler(selectedBtn, selectedBtnName, url) {
    if (!selectedBtn.classList.contains("select")) {
        localStorage.setItem("selectedBtn", selectedBtnName);
        if(url != null)
            window.location.href = url;
    }
}

/**
 * Responsible for selecting or deselecting the "active-products" link,
 * when it is clicked.
 */
btnActiveProducts.addEventListener("click", () =>
    selectOrDeselectBtnHandler(btnActiveProducts, "btnActiveProducts", "ViewProducts?type=active"));

/**
 * Responsible for selecting or deselecting the "deactivated-products" link,
 * when it is clicked.
 */
btnDeactivatedProducts.addEventListener("click", () =>
    selectOrDeselectBtnHandler(btnDeactivatedProducts, "btnDeactivatedProducts", "ViewProducts?type=deactivated"));

btnDropdown.addEventListener("mouseover", () => selectOrDeselectBtnHandler(btnDropdown, "btnDropdown", null));

/**
 * Changes the background color of the selected button.
 * @param {*} selectedBtn The selected button.
 * @param {*} selectedBtnName The name of the selected button.
 */
function changeButtonBgColor(selectedBtn, selectedBtnName) {
    selectedBtn.classList.add("select");
    selectedBtn.style.backgroundColor = "lightseagreen";
    selectedBtn.classList.remove("deselect");
}

// executes after the page reloads.
if (localStorage.getItem("selectedBtn") === "btnActiveProducts") {
    changeButtonBgColor(btnActiveProducts, "btnActiveProducts");
}
else if (localStorage.getItem("selectedBtn") === "btnDeactivatedProducts") {
    changeButtonBgColor(btnDeactivatedProducts, "btnDeactivatedProducts");
}
else if (localStorage.getItem("selectedBtn") === "btnDropdown") {
    changeButtonBgColor(btnDropdown, "btnDropdown");
}