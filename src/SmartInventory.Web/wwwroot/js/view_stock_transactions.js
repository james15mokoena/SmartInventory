const btnAll = document.querySelector(".menu-item .btn-all");
const btnIssued = document.querySelector(".menu-item .btn-issued");
const btnReceived = document.querySelector(".menu-item .btn-received");
const btnReturned = document.querySelector(".menu-item .btn-returned");
const btnDamaged = document.querySelector(".menu-item .btn-damaged");
const btnNewProduct = document.querySelector(".menu-item .btn-new-product");

/**
 * Handles the changing of the background color of the selected
 * or deselected button.
 */
btnAll.addEventListener("click", () => {

    if (!btnAll.classList.contains("selected")) {
        localStorage.setItem("selectedBtn", "btnAll");
        window.location.href = "ViewStockTransactions";
    }
    else
        localStorage.removeItem("selectedBtn");
});

btnIssued.addEventListener("click", () => {
    if (!btnIssued.classList.contains("selected")) {
        localStorage.setItem("selectedBtn", "btnIssued");
        window.location.href = "ViewStockTransactions?reason=Issued";
    }
    else {
        localStorage.removeItem("selectedBtn");
    }
});

btnReceived.addEventListener("click", () => {
    if (!btnReceived.classList.contains("selected")) {
        localStorage.setItem("selectedBtn", "btnReceived");
        window.location.href = "ViewStockTransactions?reason=Received";
    }
    else
        localStorage.removeItem("selectedBtn");
});

btnReturned.addEventListener("click", () => {
    if (!btnReturned.classList.contains("selected")) {
        localStorage.setItem("selectedBtn", "btnReturned");
        window.location.href = "ViewStockTransactions?reason=Returned";
    }
    else
        localStorage.removeItem("selectedBtn");
});

btnDamaged.addEventListener("click", () => {
    if (!btnDamaged.classList.contains("selected")) {
        localStorage.setItem("selectedBtn", "btnDamaged");
        window.location.href = "ViewStockTransactions?reason=Damaged";
    }
    else
        localStorage.removeItem("selectedBtn");
});

btnNewProduct.addEventListener("click", () => {
    if (!btnNewProduct.classList.contains("selected")) {
        localStorage.setItem("selectedBtn", "btnNewProduct");
        window.location.href = "ViewStockTransactions?reason=New Product";
    }
    else
        localStorage.removeItem("selectedBtn");
});

// Changes the background color of the selected or deselected menu option.
if (localStorage.getItem("selectedBtn") === "btnAll") {
    btnAll.classList.add("selected");
    btnAll.style.backgroundColor = "lightseagreen";
    btnAll.classList.remove("deselected");
}
else if(localStorage.getItem("selectedBtn") === "btnIssued"){
    btnIssued.classList.add("selected");
    btnIssued.style.backgroundColor = "lightseagreen";
    btnIssued.classList.remove("deselected");
}
else if(localStorage.getItem("selectedBtn") === "btnReceived"){
    btnReceived.classList.add("selected");
    btnReceived.style.backgroundColor = "lightseagreen";
    btnReceived.classList.remove("deselected");
}
else if(localStorage.getItem("selectedBtn") === "btnReturned"){
    btnReturned.classList.add("selected");
    btnReturned.style.backgroundColor = "lightseagreen";
    btnReturned.classList.remove("deselected");
}
else if(localStorage.getItem("selectedBtn") === "btnDamaged"){
    btnDamaged.classList.add("selected");
    btnDamaged.style.backgroundColor = "lightseagreen";
    btnDamaged.classList.remove("deselected");
}
else if(localStorage.getItem("selectedBtn") === "btnNewProduct"){
    btnNewProduct.classList.add("selected");
    btnNewProduct.style.backgroundColor = "lightseagreen";
    btnNewProduct.classList.remove("deselected");
}
else {
    btnAll.classList.add("selected");
    btnAll.style.backgroundColor = "lightseagreen";
    btnAll.classList.remove("deselected");   
}