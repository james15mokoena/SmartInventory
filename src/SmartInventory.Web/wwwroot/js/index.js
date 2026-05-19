// ##################################### EVENT HANDLING ##################################### //

const btnViewMoreInfoList = document.querySelectorAll(".btn-view-more-info");

// Add event listeners and event handlers.
btnViewMoreInfoList.forEach(btn => {

    btn.addEventListener('click', () => {

        const sibling = btn.nextElementSibling
        if (sibling.classList.contains("hide")) {
            sibling.classList.remove("hide");
            sibling.classList.add("show");
            btn.style.backgroundColor = "lightseagreen";
        }
        else {
            sibling.classList.remove("show");
            sibling.classList.add("hide");
            btn.style.backgroundColor = "lightblue";
        }
            
    })
});

let homeMenuBtns = document.querySelectorAll(".btn-item");

/**
 * When a button is clicked, it deselects other buttons.
 */
function deselectBtns() {
    homeMenuBtns.forEach(btn => btn.style.backgroundColor = "peru");
}

/**
 * Hides all reports tab before the selected one is displayed.
 */
function hideReportsTabs() {
    
    const reportsDiv = document.querySelector(".charts");
    
    if (reportsDiv != null) {

        const tabs = reportsDiv.children;

        for (i = 0; i < tabs.length; ++i) {

            tabs[i].classList.remove("show-reports-tab");
            tabs[i].classList.add("hide-reports-tab");
        }
    }
}

homeMenuBtns.forEach(btn => {

    btn.addEventListener('click', () => {

        deselectBtns();

        var monthIdxHome = null;
        monthIdxHome = localStorage.getItem("monthIdxHome");

        if (btn.classList.contains("btn-sales-charts") && !btn.classList.contains("show")) {
            
            // show the sales reports tab and hide all other tabs.            
            localStorage.setItem("clickedHomeBtn", "btn-sales-charts");
            localStorage.setItem("tabToShow", "sales-reports");
            window.location.href = `http://localhost:5289/Index?action=Sales&monthIdx=${monthIdxHome ?? 1}`;

        }
        else if (btn.classList.contains("btn-purchases-charts") && !btn.classList.contains("show")) {

            // show the purchases reports tab and hide all other tabs.
            localStorage.setItem("clickedHomeBtn", "btn-purchases-charts");
            localStorage.setItem("tabToShow", "purchases-reports");
            window.location.href = `http://localhost:5289/Index?action=Purchases&monthIdx=${monthIdxHome ?? 1}`;
        }
        /*else if (btn.classList.contains("btn-returns-charts") && !btn.classList.contains("show")) {

            // show the returns reports tab and hide all other tabs.
            localStorage.setItem("clickedHomeBtn", "btn-returns-charts");
            localStorage.setItem("tabToShow", "returns-reports");
            window.location.href = "http://localhost:5289/Index?action=Returns";
        }
        else if (btn.classList.contains("btn-damages-charts") && !btn.classList.contains("show")) {

            // show the damages reports tab and hide all other tabs.
            localStorage.setItem("clickedHomeBtn", "btn-damages-charts");
            localStorage.setItem("tabToShow", "damages-reports");
            window.location.href = "http://localhost:5289/Index?action=Damages";
        }*/
    });
});

// After page reload change the background color of the clicked button and show its corresponding tab/content.
let clickedHomeBtn = localStorage.getItem("clickedHomeBtn");
let tabToShow = localStorage.getItem("tabToShow");

/**
 * Changes the background color of the selected button and shows the corresponding
 * tab.
 * @param {*} clickedBtn The clicked button.
 * @param {*} tabToShowClassName The class name of the tab to be displayed.
 */
function changeClickedBtnBgColorHelper(clickedBtn, tabToShowClassName) {
    
    clickedBtn.style.backgroundColor = "lightseagreen";
    hideReportsTabs();
    let tab = document.querySelector(tabToShowClassName);
    tab.classList.remove("hide-reports-tab");
    tab.classList.add("show-reports-tab");
}

homeMenuBtns.forEach(btn => {

    if (btn.classList.contains("btn-sales-charts") && clickedHomeBtn === "btn-sales-charts")
        changeClickedBtnBgColorHelper(btn, ".sales-reports")
    else if (btn.classList.contains("btn-purchases-charts") && clickedHomeBtn === "btn-purchases-charts")
        changeClickedBtnBgColorHelper(btn, ".purchases-reports")
    /*else if (btn.classList.contains("btn-returns-charts") && clickedHomeBtn === "btn-returns-charts")
        changeClickedBtnBgColorHelper(btn, ".returns-reports")
    else if (btn.classList.contains("btn-damages-charts") && clickedHomeBtn === "btn-damages-charts")
        changeClickedBtnBgColorHelper(btn, ".damages-reports")*/
});


/**
 * Finds a reports tab that is currently displayed.
 * @returns A class name that uniquely identifies this reports tab.
 */
function findActiveReportsTab() {
    const reportsTabsCont = document.querySelector(".charts");
    const children = reportsTabsCont.children;

    for (i = 0; i < children.length; ++i){
        if (children[i].classList.contains("show-reports-tab"))
            return children[i].classList[0];
    }
}

const btnPrevMonth = document.querySelector(".prev-month");
const btnNextMonth = document.querySelector(".next-month");
const selectedMonth = document.querySelector(".selected-month");


/**
 * Given a month name, it gets the preceeding month's index.
 * @param {*} month The name of the month whose preceeding month's index is to be returned.
 */
function getPreceedingMonth() {

    var selectedMo = selectedMonth.textContent;

    if (getMonthIndex(selectedMo) >= 1) {
        var preceedingIdx = getMonthIndex(selectedMo) - 1;
        selectedMonth.textContent = getMonth(preceedingIdx);
        localStorage.setItem("currentMonth", selectedMonth.textContent);
        ++preceedingIdx;
        localStorage.setItem("monthIdxHome", preceedingIdx);
        if (findActiveReportsTab() === "sales-reports") {
            window.location.href = `http://localhost:5289/Index?action=Sales&monthIdx=${preceedingIdx}`;
        }
    }
}

/**
 * Given a month name, it gets the next month's index.
 * @param {*} month The name of the month whose next month's index is to be returned.
 */
function getNextMonth() {

    var selectedMo = selectedMonth.textContent;

    if (getMonthIndex(selectedMo) <= 10) {
        var nextIdx = getMonthIndex(selectedMo) + 1;
        selectedMonth.textContent = getMonth(nextIdx);
        localStorage.setItem("currentMonth", selectedMonth.textContent);
        ++nextIdx;
        localStorage.setItem("monthIdxHome", nextIdx);
        if (findActiveReportsTab() === "sales-reports") {
            window.location.href = `http://localhost:5289/Index?action=Sales&monthIdx=${nextIdx}`;
        }
            
    }
}

selectedMonth.textContent = localStorage.getItem("currentMonth") ?? selectedMonth.textContent;

// register event listener and event handler
btnPrevMonth.addEventListener("click", getPreceedingMonth);
btnNextMonth.addEventListener("click", getNextMonth);


// ##################################### UTILITY FUNCTIONS ##################################### //

/**
 * Will return all the preceeding months in the current year up to this
 * current month.
 */
function getMonths() {
    
    let monthIdx = new Date().getMonth();
    let months = [];
    
    while (monthIdx >= 0) {

        switch (monthIdx) {
            case 0:
                months.push("JAN");
                break;
            case 1:
                months.push("FEB");
                break;
            case 2:
                months.push("MAR");
                break;
            case 3:
                months.push("APR");
                break;
            case 4:
                months.push("MAY");
                break;
            case 5:
                months.push("JUN");
                break;
            case 6:
                months.push("JUL");
                break;
            case 7:
                months.push("AUG");
                break;
            case 8:
                months.push("SEP");
                break;
            case 9:
                months.push("OCT");
                break;
            case 10:
                months.push("NOV");
                break;
            case 11:
                months.push("DEC");
                break;
            default:
                break;
        }
        --monthIdx;
    }

    months.reverse();
    return months;
}

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

// clear the local storage
//localStorage.clear();

// ##################################### CHART CREATION CODE ##################################### //
