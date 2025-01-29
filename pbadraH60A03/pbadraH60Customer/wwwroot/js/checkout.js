import {Notyf} from "https://cdn.jsdelivr.net/npm/notyf@3.10.0/+esm";

let notyf = new Notyf();

function createSpinner() {
    //TODO
    let parentContainer = document.getElementById(`payment-summary`);
    let spinnerContainer = document.createElement("div");
    spinnerContainer.classList.add(["absolute", "-inset-4", "z-10", "flex", "items-center", "justify-center", "bg-white/30", "backdrop-blur-md"]);
    spinnerContainer.role = "status";
    spinnerContainer.innerHTML = "<div role=\"status\" class=\"absolute -inset-4 z-10 flex items-center justify-center bg-white/30 backdrop-blur-md\">\n" +
        "    <svg aria-hidden=\"true\" class=\"w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600\"\n" +
        "         viewBox=\"0 0 100 101\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\n" +
        "        <path\n" +
        "            d=\"M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z\"\n" +
        "            fill=\"currentColor\"/>\n" +
        "        <path\n" +
        "            d=\"M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z\"\n" +
        "            fill=\"currentFill\"/>\n" +
        "    </svg>\n" +
        "    <span class=\"sr-only\">Loading...</span>\n" +
        "</div>";
    parentContainer.appendChild(spinnerContainer);
}

function deleteSpinner() {
    let parentContainer = document.getElementById(`payment-summary`);
    let spinnerContainer = parentContainer.querySelector("[role='status']");
    spinnerContainer.remove();
}

async function fetchProvinceTax(url, method) {
    const response = await fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json',
        },
    });

    if (!response.ok) {
        notyf.error(response.text());
    }
    
    return await response;
}

document.addEventListener('DOMContentLoaded', async () => {
    let amount = document.getElementById('amount').textContent.replaceAll("$", "").replaceAll(",", "");
    let provinceCode = document.getElementById('provinceCode');
    let tax = 0;
    let taxHiddenField = document.querySelector("[name=tax]");
    let totalHiddenField = document.querySelector("[name=total]");
    createSpinner();
    let response = await fetchProvinceTax(`http://localhost:5202/api/Checkout/CalculateTax/${provinceCode.value}/${amount}`, "GET");
    deleteSpinner();
    tax = parseFloat(await response.text());
    
    let total = parseFloat(amount) + tax;
    
    let totalElement = document.getElementById('total');
    totalElement.textContent = `$${total.toLocaleString('en-US', {
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    })}`;
    totalHiddenField.value = total;
    
    let taxElement = document.getElementById('tax');
    taxElement.textContent = `$${tax.toLocaleString('en-US', {
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    })}`
    taxHiddenField.value = tax;
    
    
    provinceCode.addEventListener('change', async function (event) {
        createSpinner()
        let response = await fetchProvinceTax(`http://localhost:5202/api/Checkout/CalculateTax/${provinceCode.value}/${amount}`, "GET");
        deleteSpinner();

        tax = parseFloat(await response.text());

        let total = parseFloat(amount) + tax;

        let totalElement = document.getElementById('total');
        totalElement.textContent = `$${total.toLocaleString('en-US', {
            minimumFractionDigits: 0,
            maximumFractionDigits: 0
        })}`;
        totalHiddenField.value = total;

        let taxElement = document.getElementById('tax');
        taxElement.textContent = `$${tax.toLocaleString('en-US', {
            minimumFractionDigits: 0,
            maximumFractionDigits: 0
        })}`
        taxHiddenField.value = tax;
    })
});