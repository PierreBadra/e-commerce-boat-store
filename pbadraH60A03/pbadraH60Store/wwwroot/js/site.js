let searchButton = document.getElementById("search-button");
let searchInput = document.getElementById("search-input");
let searchCustomerForm = document.getElementById("searchCustomerForm");

searchCustomerForm.addEventListener("submit", async function (e) {
    e.preventDefault();
    let customers = await fetchCustomers();
    populateTable(customers);
});

async function fetchCustomers() {
    let endpoint = "https://api-boatbud.pierrebadra.me/api/Customers";

    if (searchInput && searchInput.value.trim() !== "") {
        endpoint = `https://api-boatbud.pierrebadra.me/api/Customers?search=${encodeURIComponent(searchInput.value.trim())}`;
    }

    try {
        let response = await fetch(endpoint, { method: "GET" });
        let data = await response.json();

        return data;
    } catch (error) {
        return null;
    }
}

function populateTable(customers) {
    let customerTable = document.getElementById("customer-table");
    let tableBody = customerTable.querySelector("tbody")
    let resultCount = document.getElementById("result-count");
    resultCount.textContent = `${customers.$values.length} ${customers.$values.length > 1 ? "Results" : "Result"}`

    tableBody.innerHTML = "";

    if (customers.$values.length === 0) {
        tableBody.innerHTML = `
            <tr>
                <td class="text-center" colspan="6">No Customers Found.</td>
            </tr>
        `;
        return;
    }

    customers.$values.forEach((customer, i) => {
        let rowClass = i % 2 == 0 ? "table-light" : "";
        let modalId = `deleteModal_${customer.customerId}`;
        tableBody.innerHTML += ` 
        <tr class="${rowClass}">
            <td class="border-end d-none d-md-table-cell">${customer.firstName ?? ""} ${customer.lastName ?? ""}</td>
            <td class="border-end d-none d-md-table-cell">${customer.phoneNumber ?? ""}</td>
            <td class="border-end">${customer.email ?? ""}</td>
            <td class="border-end d-none d-lg-table-cell">${customer.province ?? ""}</td>
            <td>
                <a href="Customers/Edit/${customer.customerId}" title="Edit ${customer.firstName ?? ""} ${customer.lastName ?? ""} Account"><i class='bx bxs-edit'></i></a>
                <a href="" data-bs-toggle="modal" data-bs-target="#${modalId}" title="Delete ${customer.firstName ?? ""} ${customer.lastName ?? ""} Account" class="text-danger"><i class='bx bx-trash'></i></a>
            </td>
        </tr>
    `;
    });
}