// get the "active-products" btn
const btnActiveProducts = document.querySelector(".menu-option .btn-active-products");
// get the "deactivated-products" btn
const btnDeactivatedProducts = document.querySelector(".menu-option .btn-deactivated-products");
// get the "dropdown-prod" btn
const btnDropdown = document.querySelector(".dropbtn-prod");
// indicates the product type whether: active or deactivated.
const type = new URLSearchParams(window.location.search).get("type");
// indicates the category of products to display.
const category = new URLSearchParams(window.location.search).get("cat");
// get all the links for each category.
const categories = document.querySelectorAll(".lnk");

console.log(`Category: ${category}`);

// default behavior after the page loads.
window.addEventListener("load", function () {

    if (!type && sessionStorage.getItem("selectedVPsBtn") === "btnDeactivatedProducts") {
        sessionStorage.setItem("selectedVPsBtn", "btnDeactivatedProducts");
        location = "/Product/ViewProducts?type=deactivated";
    }
    else if (!category && sessionStorage.getItem("selectedVPsBtn")?.startsWith("lnk")) {

        let clickedLink = findClickedLink(sessionStorage.getItem("selectedVPsBtn"));
        const catName = document.querySelector(`.${clickedLink}`).textContent;
        location = `/Product/ViewProducts?cat=${encodeURIComponent(catName)}`;

    }
    else if (sessionStorage.getItem("selectedVPsBtn") === null) {
        sessionStorage.setItem("selectedVPsBtn", "btnActiveProducts");
    }
});

// event handling
btnActiveProducts.addEventListener("click", function () {
   
    if (sessionStorage.getItem("selectedVPsBtn") != "btnActiveProducts") {
        sessionStorage.setItem("selectedVPsBtn", "btnActiveProducts");
        location = "/Product/ViewProducts?type=active";
    }
});

btnDeactivatedProducts.addEventListener("click", function () {
   
    if (sessionStorage.getItem("selectedVPsBtn") != "btnDeactivatedProducts") {
        sessionStorage.setItem("selectedVPsBtn", "btnDeactivatedProducts");
        sessionStorage.setItem("IsDeactivatedShown", "true");
        location = "/Product/ViewProducts?type=deactivated";
    }
});

categories.forEach(cat => {

    cat.addEventListener("click", function () {
        sessionStorage.setItem("selectedVPsBtn", cat.classList[1]);
    });
});

/**
 * Obtains the category link that has been clicked.
 * @param {string} selectedVPsBtn A selected button.
* @returns category link that has been clicked.
 */
function findClickedLink(selectedVPsBtn) {
    
    let clickedLink = null;

    categories.forEach(cat => {
        if (cat.classList[1] === selectedVPsBtn) {
            clickedLink = cat.classList[1];
            return cat.classList[1];
        }
    })

    return clickedLink;
}

// get the clicked link if any.
const clickedLink = findClickedLink(sessionStorage.getItem("selectedVPsBtn"));

// apply styles after page loads
if (sessionStorage.getItem("selectedVPsBtn") === "btnActiveProducts") {
    btnActiveProducts.style.backgroundColor = "lightseagreen";
}
else if (sessionStorage.getItem("selectedVPsBtn") === "btnDeactivatedProducts") {
    btnDeactivatedProducts.style.backgroundColor = "lightseagreen";
}
else if (sessionStorage.getItem("selectedVPsBtn") === clickedLink) {
    document.querySelector(`.${clickedLink}`).style.backgroundColor = "lightseagreen";
}

