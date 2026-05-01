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

        if (btn.classList.contains("btn-sales-charts")) {
            
            // show the sales reports tab and hide all other tabs.            
            localStorage.setItem("clickedHomeBtn", "btn-sales-charts");
            localStorage.setItem("tabToShow", "sales-reports");
            window.location.href = "http://localhost:5289/Index?action=Sales";
        }
        else if (btn.classList.contains("btn-purchases-charts") && !btn.classList.contains("show")) {

            // show the purchases reports tab and hide all other tabs.
            localStorage.setItem("clickedHomeBtn", "btn-purchases-charts");
            localStorage.setItem("tabToShow", "purchases-reports");
            window.location.href = "http://localhost:5289/Index?action=Purchases";
        }
        else if (btn.classList.contains("btn-returns-charts") && !btn.classList.contains("show")) {

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
        }
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
    else if (btn.classList.contains("btn-returns-charts") && clickedHomeBtn === "btn-returns-charts")
        changeClickedBtnBgColorHelper(btn, ".returns-reports")
    else if (btn.classList.contains("btn-damages-charts") && clickedHomeBtn === "btn-damages-charts")
        changeClickedBtnBgColorHelper(btn, ".damages-reports")
});

localStorage.clear();

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

// ##################################### CHART CREATION CODE ##################################### //
