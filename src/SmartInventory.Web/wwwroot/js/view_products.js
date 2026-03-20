// get the "active-products" link
const lnkActiveProducts = document.querySelector("#active-products");
// get the "deactivated-products" link
const lnkDeactivatedProducts = document.querySelector("#deactivated-products");

/**
 * Responsible for selecting or deselecting the "active-products" link,
 * when it is clicked.
 */
function selectOrDeselectActiveProductsLink() {
    
    if (!lnkActiveProducts.classList.contains("select")){
        
        // select the link
        lnkActiveProducts.classList.add("select");
        lnkActiveProducts.classList.remove("deselect");
        // deselect other links
        lnkDeactivatedProducts.classList.remove("select");
        lnkDeactivatedProducts.classList.add("deselect");
        // change the colors
        lnkActiveProducts.style.backgroundColor = "lightseagreen";
        lnkDeactivatedProducts.style.backgroundColor = "peru";
    }

    lnkActiveProducts.style.color = "red";
}

/**
 * Responsible for selecting or deselecting the "deactivated-products" link,
 * when it is clicked.
 */
function selectOrDeselectDeactivatedProductsLink() {
    
    if (!lnkDeactivatedProducts.classList.contains("select")){
        
        // select the link
        lnkDeactivatedProducts.classList.add("select");
        lnkDeactivatedProducts.classList.remove("deselect");
        // deselect other links
        lnkActiveProducts.classList.remove("select");
        lnkActiveProducts.classList.add("deselect");
        // change the colors
        lnkDeactivatedProducts.style.backgroundColor = "lightseagreen";
        lnkActiveProducts.style.backgroundColor = "peru";
    }
}

// add listeners
lnkActiveProducts.addEventListener("click", selectOrDeselectActiveProductsLink);
lnkDeactivatedProducts.addEventListener("click", selectOrDeselectDeactivatedProductsLink);

/**
 * FIX THE CODE ABOVE!!
 */