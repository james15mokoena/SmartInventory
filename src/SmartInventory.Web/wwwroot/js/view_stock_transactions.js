const btnSold = document.querySelector(".menu-item .btn-sales");
const btnPurchases = document.querySelector(".menu-item .btn-purchases");
/*const btnReturned = document.querySelector(".menu-item .btn-returned");
const btnDamaged = document.querySelector(".menu-item .btn-damaged");*/
const btns = [btnSold, btnPurchases];//, btnReturned, btnDamaged];
// get the url parameters
let reason = new URLSearchParams(window.location.search).get("reason");
let monthIndex = new URLSearchParams(window.location.search).get("monthIdx");

console.log(`Reason: ${reason}\nMonth Index: ${monthIndex}`);

// On page load selected the btnSold button by default.
window.addEventListener("load", function () {

    console.log(`MonthIdxVst: ${sessionStorage.getItem("monthIdxVst")}`);
    
    if (!reason && !monthIndex && !sessionStorage.getItem("monthIdxVst")) {
        sessionStorage.setItem("selectedButton", "btn-sales");
        sessionStorage.setItem("monthIdxVst", 0);
        location = `/Stock/ViewStockTransactions?reason=${encodeURIComponent("Sold")}&monthIdx=${encodeURIComponent(1)}`;
    }
    else if (!reason && !monthIndex && sessionStorage.getItem("monthIdxVst")) {

        if (sessionStorage.getItem("selectedButton") === "btn-sales") {
            location = `/Stock/ViewStockTransactions?reason=${encodeURIComponent("Sold")}&monthIdx=${encodeURIComponent(sessionStorage.getItem("monthIdxVst"))}`;
        }
        else if (sessionStorage.getItem("selectedButton") === "btn-purchases"){
            location = `/Stock/ViewStockTransactions?reason=${encodeURIComponent("Purchased")}&monthIdx=${encodeURIComponent(sessionStorage.getItem("monthIdxVst"))}`;
        }
    }
    else if (reason && reason === "Sold" && monthIndex && !sessionStorage.getItem("monthIdxVst")) {
        sessionStorage.setItem("selectedButton", "btn-sales");
        sessionStorage.setItem("monthIdxVst", monthIndex);
        location = `/Stock/ViewStockTransactions?reason=${encodeURIComponent("Sold")}&monthIdx=${encodeURIComponent(monthIndex)}`;
    }
    else if (reason && reason === "Purchased" && monthIndex && !sessionStorage.getItem("monthIdxVst")) {
        sessionStorage.setItem("selectedButton", "btn-purchases");
        sessionStorage.setItem("monthIdxVst", monthIndex);
        location = `/Stock/ViewStockTransactions?reason=${encodeURIComponent("Purchased")}&monthIdx=${encodeURIComponent(monthIndex)}`;
    }
});

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
            sessionStorage.setItem("selectedButton", "btn-sales");
        else if (selectedBtn === btnPurchases)
            sessionStorage.setItem("selectedButton", "btn-purchases");
        /*else if (selectedBtn === btnReturned)
            sessionStorage.setItem("selectedButton", "btn-returned");
        else if (selectedBtn === btnDamaged)
            sessionStorage.setItem("selectedButton", "btn-damaged");*/
    }
}

// add event listeners and handlers
btns.forEach(btn => btn.addEventListener("click", () => {

    var monthIdx = null;
    monthIdx = sessionStorage.getItem("monthIdxVst");

    if (btn.classList.contains("btn-sales")) {
        location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Sold&monthIdx=${monthIdx ?? 1}`;
    }
    else if (btn.classList.contains("btn-purchases")) {
        location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Purchased&monthIdx=${monthIdx ?? 1}`;
    }
    /*else if (btn.classList.contains("btn-returned")) {
        location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Returned&monthIdx=${monthIdx !== null ? monthIdx : 1}`;
    }
    else if (btn.classList.contains("btn-damaged")) {
        location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Damaged&monthIdx=${monthIdx !== null ? monthIdx : 1}`;
    }*/

    showOrHideTab(btn)
}));

const selectedButton = sessionStorage.getItem("selectedButton");

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
        sessionStorage.setItem("currentMonthVst", selectedMonthVst.textContent);
        ++preceedingIdx;
        sessionStorage.setItem("monthIdxVst", preceedingIdx);
        if (selectedButton === "btn-sales") {
            location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Sold&monthIdx=${preceedingIdx}`;
        }
        else if (selectedButton === "btn-purchases") {
            location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Purchased&monthIdx=${preceedingIdx}`;
        }
        /*else if (selectedButton === "btn-returned") {
            location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Returned&monthIdx=${preceedingIdx}`;
        }
        else if (selectedButton === "btn-damaged") {
            location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Damaged&monthIdx=${preceedingIdx}`;
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
        sessionStorage.setItem("currentMonthVst", selectedMonthVst.textContent);
        ++nextIdx;
        console.log("New Index: " + nextIdx);
        console.log("New Month: " + selectedMonthVst.textContent);
        sessionStorage.setItem("monthIdxVst", nextIdx);
        if (selectedButton === "btn-sales") {
            location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Sold&monthIdx=${nextIdx}`;
        }
        else if (selectedButton === "btn-purchases") {
            location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Purchased&monthIdx=${nextIdx}`;
        }
        /*else if (selectedButton === "btn-returned") {
            location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Returned&monthIdx=${nextIdx}`;
        }
        else if (selectedButton === "btn-damaged") {
            location = `http://localhost:5289/Stock/ViewStockTransactions?reason=Damaged&monthIdx=${nextIdx}`;
        }*/
    }
}

selectedMonthVst.textContent = sessionStorage.getItem("currentMonthVst") ?? selectedMonthVst.textContent;

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