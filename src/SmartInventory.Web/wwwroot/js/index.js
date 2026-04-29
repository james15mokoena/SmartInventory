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
    
    homeMenuBtns.forEach(btn => {
        btn.classList.remove("show");
        btn.style.backgroundColor = "peru";
    });
}

/**
 * Shows or hide a tab based on the selected menu button.
 * @param {*} tabClassName A class name for the tab to be shown.
 */
function showOrHideReportsTab(tabClassName) {
    
    const reportsDiv = document.querySelector(".charts");
    const reportsTabs = reportsDiv.children;

    for (i = 0; i < reportsTabs.length; ++i){

        if (!reportsTabs[i].classList.contains(tabClassName)) {
            reportsTabs[i].classList.remove("show-reports-tab");
            reportsTabs[i].classList.add("hide-reports-tab");
        } else {
            reportsTabs[i].classList.remove("hide-reports-tab");
            reportsTabs[i].classList.add("show-reports-tab");
        }
            
    }
}

homeMenuBtns.forEach(btn => {

    btn.addEventListener('click', () => {

        deselectBtns();

        const reportsDiv = document.querySelector(".charts");

        if (btn.classList.contains("btn-sales-charts") && !btn.classList.contains("show")) {
            btn.classList.add("show");

            // show the sales reports tab and hide all other tabs.
            const reportsTabs = reportsDiv.children;

            showOrHideReportsTab("sales-reports");
            
            console.log(reportsTabs);
        }
        else if (btn.classList.contains("btn-purchases-charts") && !btn.classList.contains("show")) {
            btn.classList.add("show");

            // show the purchases reports tab and hide all other tabs.
            const reportsTabs = reportsDiv.children;
            showOrHideReportsTab("purchases-reports");
            console.log(reportsTabs);
        }
        else if (btn.classList.contains("btn-returns-charts") && !btn.classList.contains("show")) {
            btn.classList.add("show");

            // show the returns reports tab and hide all other tabs.
            const reportsTabs = reportsDiv.children;
            showOrHideReportsTab("returns-reports");
            console.log(reportsTabs);
        }
        else if (btn.classList.contains("btn-damages-charts") && !btn.classList.contains("show")) {
            btn.classList.add("show");

            // show the damages reports tab and hide all other tabs.
            const reportsTabs = reportsDiv.children;
            showOrHideReportsTab("damages-reports");
            console.log(reportsTabs);
        }

        btn.style.backgroundColor = "lightseagreen";
    });
});

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
