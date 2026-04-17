const btnSold = document.querySelector(".menu-item .btn-sales");
const btnPurchases = document.querySelector(".menu-item .btn-purchases");
const btnReturned = document.querySelector(".menu-item .btn-returned");
const btnDamaged = document.querySelector(".menu-item .btn-damaged");
const btns = [btnSold, btnPurchases, btnReturned, btnDamaged];

// On page load selected the btnSold button by default.
/*window.addEventListener("load", () => {
    btnSold.classList.add("selected");
    btnSold.style.backgroundColor = "lightseagreen";
});*/

/**
 * Deselects a previously selected button.
 */
function deselectPreviouslySelectedBtn() {
    
    btns.forEach(btn => {
       
        if (btn.classList.contains("selected")) {
            btn.classList.remove("selected");
            btn.style.backgroundColor = "peru";
        }
    });
}

/**
 * Depending on the button clicked, a different tab will be displayed.
 * @param {*} tabToShow The clicked button.
 */
function showOrHideTab(selectedBtn) {
    
    if (!selectedBtn.classList.contains("selected")) {

        deselectPreviouslySelectedBtn();

        if (selectedBtn === btnSold)
            localStorage.setItem("selectedButton", "btn-sales");
        else if (selectedBtn === btnPurchases)
            localStorage.setItem("selectedButton", "btn-purchases");
        else if (selectedBtn === btnReturned)
            localStorage.setItem("selectedButton", "btn-returned");
        else if (selectedBtn === btnDamaged)
            localStorage.setItem("selectedButton", "btn-damaged");
    }
}

// add event listeners and handlers
btns.forEach(btn => btn.addEventListener("click", () => {
        
    if (btn.classList.contains("btn-sales")) {        
        window.location.href = "http://localhost:5289/Stock/ViewStockTransactions?reason=Sold";
    }
    else if (btn.classList.contains("btn-purchases")) {
        window.location.href = "http://localhost:5289/Stock/ViewStockTransactions?reason=Purchased";
    }
    else if (btn.classList.contains("btn-returned")) {
        window.location.href = "http://localhost:5289/Stock/ViewStockTransactions?reason=Returned";
    }
    else if (btn.classList.contains("btn-damaged")) {
        window.location.href = "http://localhost:5289/Stock/ViewStockTransactions?reason=Damaged";
    }

    showOrHideTab(btn)
}));

const selectedButton = localStorage.getItem("selectedButton");

// change the color of the selected button
btns.forEach(btn => {

    if (btn.classList.contains("btn-sales") && selectedButton === "btn-sales") {
        btn.classList.add("selected");
        btn.style.backgroundColor = "lightseagreen";
    }
    else if (btn.classList.contains("btn-purchases") && selectedButton === "btn-purchases") {
        btn.classList.add("selected");
        btn.style.backgroundColor = "lightseagreen";
    }
    else if (btn.classList.contains("btn-returned") && selectedButton === "btn-returned") {
        btn.classList.add("selected");
        btn.style.backgroundColor = "lightseagreen";
    }
    else if (btn.classList.contains("btn-damaged") && selectedButton === "btn-damaged") {
        btn.classList.add("selected");
        btn.style.backgroundColor = "lightseagreen";
    }
});

localStorage.clear();

// ------------------------------------------------- //

const collapsBtns = document.querySelectorAll(".summary-header .collaps");

/**
 * Handles the opening or closing of the summary content when its button is clicked.
 * @param {*} clickedBtn The button that opens or closes the summary content. 
 */
function openOrCloseSummaryContent(clickedBtn) {
    
    if (clickedBtn.textContent === "+") {

        clickedBtn.textContent = "-";
        const summaryContent = (clickedBtn.parentElement).nextElementSibling;
        
        if (!summaryContent.classList.contains("open")) {
            summaryContent.classList.add("open");
            summaryContent.classList.remove("closed");
        }
    }
    else {
        clickedBtn.textContent = "+";
        const summaryContent = (clickedBtn.parentElement).nextElementSibling;        

        if (!summaryContent.classList.contains("closed")) {
            summaryContent.classList.add("closed");
            summaryContent.classList.remove("open");
        }
    }
}

collapsBtns.forEach(btn => btn.addEventListener("click", () => openOrCloseSummaryContent(btn)));

