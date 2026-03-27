// get the navigation items

const navItems = document.querySelectorAll(".admin-nav .nav-item");
const home = document.querySelector(".admin-nav .home");
const user = document.querySelector(".admin-nav .user");
const supplier = document.querySelector(".admin-nav .supplier");
const stock = document.querySelector(".admin-nav .stock");
const sales = document.querySelector(".admin-nav .sales");
const product = document.querySelector(".admin-nav .product-lnk");
const procurement = document.querySelector(".admin-nav .procurement");
const permission = document.querySelector(".admin-nav .permission");
const login = document.querySelector(".admin-nav .login");

/**
 * Assigns the "active" class to the clicked navigation item.
 * @param {*} navItem The navigation item that is clicked.
 * @param {*} url The url to the page to be loaded.
 */
function navItemClickHandler(navItem, navItemName, url) {
    
    if (!navItem.classList.contains("active")) {
        localStorage.setItem("selectedLink", navItemName);
        window.location.href = url;
    }
}

// add event listeners

home.addEventListener("click", () => navItemClickHandler(home, "home", "Index"));

user.addEventListener("click", () => navItemClickHandler(user, "user", "User/Index"));

supplier.addEventListener("click", () => navItemClickHandler(supplier,"supplier","Supplier/Index"));

stock.addEventListener("click", () => navItemClickHandler(stock,"stock","Stock/Index"));

sales.addEventListener("click", () => navItemClickHandler(sales,"sales","Sales/Index"));

product.addEventListener("click", () => navItemClickHandler(product,"product","Product/Index"));

procurement.addEventListener("click", () => navItemClickHandler(procurement,"procurement","Procurement/Index"));

permission.addEventListener("click", () => navItemClickHandler(permission,"permission","Permission/Index"));

login.addEventListener("click", () => navItemClickHandler(login,"login","Login"));

/**
 * Changes the background color of the clicked navigation item.
 * @param {*} navItem The selected navigation item. 
 */
function changeNavItemBgColor(navItem) {
    navItem.classList.add("active");
    navItem.style.backgroundColor = "lightseagreen";
}

// Change the colors of the selected and deselected navigation items.

if (localStorage.getItem("selectedLink") === "home") {
    changeNavItemBgColor(home);
}
else if (localStorage.getItem("selectedLink") === "user") {
    changeNavItemBgColor(user);
}
else if (localStorage.getItem("selectedLink") === "supplier") {
    changeNavItemBgColor(supplier);
}
else if (localStorage.getItem("selectedLink") === "stock") {
    changeNavItemBgColor(stock);
}
else if (localStorage.getItem("selectedLink") === "sales") {
    changeNavItemBgColor(sales);
}
else if (localStorage.getItem("selectedLink") === "product") {
    changeNavItemBgColor(product);
}
else if (localStorage.getItem("selectedLink") === "procurement") {
    changeNavItemBgColor(procurement);
}
else if (localStorage.getItem("selectedLink") === "permission") {
    changeNavItemBgColor(permission);
}
else if (localStorage.getItem("selectedLink") === "login") {
    changeNavItemBgColor(login)
}