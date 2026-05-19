const btnSold = document.querySelector(".menu-item .btn-sales");
const btnPurchases = document.querySelector(".menu-item .btn-purchases");
/*const btnReturned = document.querySelector(".menu-item .btn-returned");
const btnDamaged = document.querySelector(".menu-item .btn-damaged");*/
const btns = [btnSold, btnPurchases];//, btnReturned, btnDamaged];

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
        /*else if (selectedBtn === btnReturned)
            localStorage.setItem("selectedButton", "btn-returned");
        else if (selectedBtn === btnDamaged)
            localStorage.setItem("selectedButton", "btn-damaged");*/
    }
}

// add event listeners and handlers
btns.forEach(btn => btn.addEventListener("click", () => {

    var monthIdx = null;
    monthIdx = localStorage.getItem("monthIdxVst");

    if (btn.classList.contains("btn-sales")) {
        window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Sold&monthIdx=${monthIdx ?? 1}`;
    }
    else if (btn.classList.contains("btn-purchases")) {
        window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Purchased&monthIdx=${monthIdx ?? 1}`;
    }
    /*else if (btn.classList.contains("btn-returned")) {
        window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Returned&monthIdx=${monthIdx !== null ? monthIdx : 1}`;
    }
    else if (btn.classList.contains("btn-damaged")) {
        window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Damaged&monthIdx=${monthIdx !== null ? monthIdx : 1}`;
    }*/

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
    /*else if (btn.classList.contains("btn-returned") && selectedButton === "btn-returned") {
        btn.classList.add("selected");
        btn.style.backgroundColor = "lightseagreen";
    }
    else if (btn.classList.contains("btn-damaged") && selectedButton === "btn-damaged") {
        btn.classList.add("selected");
        btn.style.backgroundColor = "lightseagreen";
    }*/
});

//localStorage.clear();

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

const btnPrevMonthVst = document.querySelector(".prev-month-vst");
const btnNextMonthVst = document.querySelector(".next-month-vst");
const selectedMonthVst = document.querySelector(".selected-month-vst");

/**
 * Given a month name, it gets the preceeding month's index.
 * @param {*} month The name of the month whose preceeding month's index is to be returned.
 */
function getPreceedingMonth() {

    var selectedMo = selectedMonthVst.textContent;

    if (getMonthIndex(selectedMo) >= 1) {
        var preceedingIdx = getMonthIndex(selectedMo) - 1;
        selectedMonthVst.textContent = getMonth(preceedingIdx);
        localStorage.setItem("currentMonthVst", selectedMonthVst.textContent);
        ++preceedingIdx;
        localStorage.setItem("monthIdxVst", preceedingIdx);
        if (selectedButton === "btn-sales") {
            window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Sold&monthIdx=${preceedingIdx}`;
        }
        else if (selectedButton === "btn-purchases") {
            window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Purchased&monthIdx=${preceedingIdx}`;
        }
        /*else if (selectedButton === "btn-returned") {
            window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Returned&monthIdx=${preceedingIdx}`;
        }
        else if (selectedButton === "btn-damaged") {
            window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Damaged&monthIdx=${preceedingIdx}`;
        }*/
    }
}

/**
 * Given a month name, it gets the next month's index.
 * @param {*} month The name of the month whose next month's index is to be returned.
 */
function getNextMonth() {

    var selectedMo = selectedMonthVst.textContent;

    if (getMonthIndex(selectedMo) <= 10) {
        var nextIdx = getMonthIndex(selectedMo) + 1;
        selectedMonthVst.textContent = getMonth(nextIdx);
        localStorage.setItem("currentMonthVst", selectedMonthVst.textContent);
        ++nextIdx;
        localStorage.setItem("monthIdxVst", nextIdx);
        if (selectedButton === "btn-sales") {
            window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Sold&monthIdx=${nextIdx}`;
        }
        else if (selectedButton === "btn-purchases") {
            window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Purchased&monthIdx=${nextIdx}`;
        }
        /*else if (selectedButton === "btn-returned") {
            window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Returned&monthIdx=${nextIdx}`;
        }
        else if (selectedButton === "btn-damaged") {
            window.location.href = `http://localhost:5289/Stock/ViewStockTransactions?reason=Damaged&monthIdx=${nextIdx}`;
        }*/
    }
}

selectedMonthVst.textContent = localStorage.getItem("currentMonthVst") ?? selectedMonthVst.textContent;

// register event listener and event handler
btnPrevMonthVst.addEventListener("click", getPreceedingMonth);
btnNextMonthVst.addEventListener("click", getNextMonth);

// *** Utility methods *** //


/**
 * Given a month name, it returns its index.
 * @param {*} month The name of the month.
 */
function getMonthIndex(month) {
    
    switch (month) {
        case "January":
            return 0;
            break;
        case "February":
            return 1;
            break;
        case "March":
            return 2;
            break;
        case "April":
            return 3;
            break;
        case "May":
            return 4;
            break;
        case "June":
            return 5;
            break;
        case "July":
            return 6;
            break;
        case "August":
            return 7;
            break;
        case "September":
            return 8;
            break;
        case "October":
            return 9;
            break;
        case "November":
            return 10;
            break;
        case "December":
            return 11;
            break;
        default:
            break;
    }
}

/**
 * Given a month's index, it returns the name of the month.
 * @param {*} monthIdx The index of the month.
 */
function getMonth(monthIdx) {
    
    switch (monthIdx) {
        case 0:
            return "January";
            break;
        case 1:
            return "February";
            break;
        case 2:
            return "March";
            break;
        case 3:
            return "April";
            break;
        case 4:
            return "May";
            break;
        case 5:
            return "June";
            break;
        case 6:
            return "July";
            break;
        case 7:
            return "August";
            break;
        case 8:
            return "September";
            break;
        case 9:
            return "October";
            break;
        case 10:
            return "November";
            break;
        case 11:
            return "December";
            break;
        default:
            break;
    }
}