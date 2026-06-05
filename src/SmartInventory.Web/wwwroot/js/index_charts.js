/**********************************************************************
* The code in this file are for building charts for the Home page.    *
**********************************************************************/

// ++++++++++ Sales Reports ++++++++++ //

/**
 * Builds a chart that shows the monthly revenues for the past months
 * until the current month in the current year.
 * @param {*} revenueChartCtx A graphics context for drawing the chart onto the canvas.
 * @param {*} months A list of months for which revenues are to be displayed.
 * @param {*} revenues A list of revenues for the months to be displayed.
 * @returns A chart showing the monthly revenues in the current year.
 */
function buildRevenueChart(revenueChartCtx, months, revenues) {
    
    return new Chart(revenueChartCtx, {
        type: "line",
        data: {
            // x-axis
            labels: months,
            datasets: [
                {
                    label: "Monthly Revenue Trends (R)",
                    data: revenues
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                }
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Months',
                        font: {
                            size: 18
                        },
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Revenue (R)',
                        font: {
                            size: 18
                        },
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                }
            }
        }
    });
}

/**
 * Builds a chart showing the top 5 selling product categories in the
 * specified month of the current year.
 * @param {*} graphicsCtx A graphics context used to draw the chart onto a canvas.
 * @param {*} categories A list of top 5 categories.
 * @param {*} sales A list of the sales of the top 5 categories.
 * @returns A chart showing the top 5 selling categories.
 */
function buildTop5SellingCategoriesChart(graphicsCtx, categories, sales) {

    return new Chart(graphicsCtx, {
        type: 'bar',
        data: {
            labels: categories,
            datasets: [
                {
                    label: 'Top 5 Selling Categories',
                    data: sales,
                    backgroundColor: ['green', 'lightblue', 'purple', 'maroon', 'orange'],
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                }
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Categories',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Sales (R)',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                }
            }
        }
    });
}

/**
 * Builds a chart showing the five least selling categories in the specified month.
 * @param {*} graphicsCtx A graphics context for drawing the chart onto the canvas.
 * @param {*} categories A list of five least selling categories.
 * @param {*} sales A list of the sales of the five least selling categories.
 * @returns A chart showing five least selling categories.
 */
function buildFiveLeastSellingCategoriesChart(graphicsCtx, categories, sales) {

    return new Chart(graphicsCtx, {
        type: 'bar',
        data: {
            labels: categories,
            datasets: [
                {
                    label: 'Five Least Selling Categories',
                    data: sales,
                    backgroundColor: ['green', 'lightblue', 'purple', 'maroon', 'orange'],
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                }
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Categories',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Sales (R)',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                }
            }
        }
    });
}

/**
 * Builds a chart showing categories contributing more than 50% towards the monthly
 * revenue.
 * @param {*} graphicsCtx A graphics context for drawing the chart onto a canvas.
 * @param {*} categories The categories contributing more than 50%.
 * @param {*} percentages The percentage contributions of the categories.
 * @returns A chart showing categories contributing more than 50% towards monthly revenue.
 */
function buildCategoriesContributingMT50PercentChart(graphicsCtx, categories, percentages) {
    return new Chart(graphicsCtx, {
        type: 'pie',
        data: {
            labels: categories,
            datasets: [
                {
                    data: percentages
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                },
                title: {
                    display: true,
                    text: 'Categories Contributing More Than 50% Towards Monthly Revenue',
                    font: {
                        size: 18
                    }
                }
            }            
        }
    });
}

/**
 * Builds a chart showing categories contributing at most 50% towards monthly revenue.
 * @param {*} graphicsCtx A graphics context for drawing the chart onto the canvas.
 * @param {*} categories A list of categories contributing at most 50%.
 * @param {*} percentages A list of percentages for the categories.
 * @returns A chart showing categories contributing at most 50% towards monthly revenue.
 */
function buildCategoriesContributingAM50PercentChart(graphicsCtx, categories, percentages){
    return new Chart(graphicsCtx, {
        type: 'pie',
        data: {
            labels: categories,
            datasets: [
                {
                    data: percentages
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                },
                title: {
                    display: true,
                    text: 'Categories Contributing At Most 50% Towards Monthly Revenue (%)',
                    font: {
                        size: 18
                    }
                }
            }            
        }
    });
}

/**
 * Builds a chart showing the top 5 selling products in the specified month.
 * @param {*} graphicsCtx A graphics context for drawing the chart onto the canvas.
 * @param {*} products A list of top 5 selling products.
 * @param {*} sales A list of the sales of the products.
 * @returns A chart showing the top 5 selling products in the specified month.
 */
function buildTop5SellingProductsChart(graphicsCtx, products, sales) {
    return new Chart(graphicsCtx, {
        type: 'bar',
        data: {
            labels: products,
            datasets: [
                {
                    label: 'Top 5 Selling Products',
                    data: sales,
                    backgroundColor: ['green', 'lightblue', 'purple', 'maroon', 'orange'],
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                }
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Product',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Sales (R)',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                }
            }
        }
    });
}

/**
 * Builds a chart showing the five least selling products in the specified month.
 * @param {*} graphicsCtx A graphics context for drawing the chart onto the canvas.
 * @param {*} products A list of five least selling products.
 * @param {*} sales A list of the products' sales.
 * @returns A chart showing the five least selling products in the specified month.
 */
function buildFiveLeastSellingProductsChart(graphicsCtx, products, sales) {
    return new Chart(graphicsCtx, {
        type: 'bar',
        data: {
            labels: products,
            datasets: [
                {
                    label: 'Five Least Selling Products',
                    data: sales,
                    backgroundColor: ['green', 'lightblue', 'purple', 'maroon', 'orange'],
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                }
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Product',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Sales (R)',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                }
            }
        }
    });
}

// ---------- Purchases Reports ---------- //

/**
 * Builds a chart showing the monthly inventory purchases total costs.
 * @param {*} ctx A graphics context for drawing the chart onto the canvas.
 * @param {*} months A list of months during which inventory purchases occurred.
 * @param {*} costs A list of the montly costs.
 * @returns A chart showing the monthly inventory purchases total costs.
 */
function buildInventoryPurchasesMonthlyTotalCostChart(ctx, months, costs) {

    return new Chart(ctx, {
        type: 'line',
        data: {
            // x-axis
            labels: months,
            datasets: [
                {
                    label: 'Monthly Inventory Total Costs Trends (R)',
                    data: costs,
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                }
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Months',
                        font: {
                            size: 18
                        },
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Total Cost (R)',
                        font: {
                            size: 18
                        },
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                }
            }
        }
    });
}

/**
 * Builds a chart showing the top 5 categories with high inventory purchase costs.
 * @param {*} ctx A graphics context for drawing the chart onto the canvas.
 * @param {*} categories A list of 5 categories with high inventory purchase costs.
 * @param {*} costs A list of purchase costs for the categories.
 * @returns A chart showing the top 5 categories with high inventory purchase costs.
 */
function buildTop5CategoriesWithHighInventoryCostsChart(ctx, categories, costs) {

    return new Chart(ctx, {
        type: 'bar',
        data: {
            labels: categories,
            datasets: [
                {
                    label: 'Top 5 Categories With High Inventory Purchase Costs',
                    data: costs,
                    backgroundColor: ['green', 'lightblue', 'purple', 'maroon', 'orange'],
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                }
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Categories',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Cost (R)',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                }
            }
        }
    });
}

/**
 * Builds a chart showing the five least categories with low inventory purchase costs.
 * @param {*} ctx A graphics context for drawing the chart onto the canvas.
 * @param {*} categories A list of five categories with low inventory purchase costs.
 * @param {*} costs A list of the purchase costs for the categories.
 * @returns A chart showing the five least categories with low inventory purchase costs.
 */
function buildFiveLeastCategoriesWithLowInventoryPurchaseCostsChart(ctx, categories, costs) {

    return new Chart(ctx, {
        type: 'bar',
        data: {
            labels: categories,
            datasets: [
                {
                    label: 'Five Least Categories with Low Inventory Purchase Costs',
                    data: costs,
                    backgroundColor: ['green', 'lightblue', 'purple', 'maroon', 'orange'],
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        font: {
                            size: 18
                        }
                    }
                }
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Categories',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Cost (R)',
                        font: {
                            size: 18
                        }
                    },
                    ticks: {
                        font: {
                            size: 18
                        }
                    }
                }
            }
        }
    });
}