import {Notyf} from "https://cdn.jsdelivr.net/npm/notyf@3.10.0/+esm";

let notyf = new Notyf();

function updateCartSummary() {
    const cartItems = document.querySelectorAll('[id^="cart-item-"]');
    const subtotalElement = document.querySelector('.cart-subtotal');
    const checkoutContainer = document.querySelector('.checkout-container');
    const emptyCartMessage = document.querySelector('.empty-cart-message');

    if (cartItems.length === 0) {
        if (checkoutContainer) {
            checkoutContainer.style.display = 'none';
        }

        if (emptyCartMessage) {
            emptyCartMessage.style.display = 'block';
        }
    } else {
        let subtotal = 0;
        cartItems.forEach(item => {
            const priceElement = item.querySelector('.item-total-price');
            if (priceElement) {
                const itemPrice = parseFloat(priceElement.textContent.replaceAll('$', '').replaceAll(',', ''));
                subtotal += itemPrice;
            }
        });

        if (subtotalElement) {
            subtotalElement.textContent = `$${subtotal.toLocaleString('en-US', {
                minimumFractionDigits: 0,
                maximumFractionDigits: 0
            })}`;
        }

        if (checkoutContainer) {
            checkoutContainer.style.display = 'flex';
        }

        if (emptyCartMessage) {
            emptyCartMessage.style.display = 'none';
        }
    }
}

function createSpinner(cartItemId) {
    //TODO
    let parentContainer = document.getElementById(`cart-item-${cartItemId}`);
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

function deleteSpinner(cartItemId) {
    let parentContainer = document.getElementById(`cart-item-${cartItemId}`);
    let spinnerContainer = parentContainer.querySelector("[role='status']");
    spinnerContainer.remove();
}

window.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll('[data-item-id]');
    const incrementDecrementQuantityContainer = document.querySelectorAll('[data-item-quantity-id]');
    
    deleteButtons.forEach(deleteButton => {
        deleteButton.addEventListener("click", function () {
            const cartItemId = deleteButton.getAttribute("data-item-id");
            createSpinner(cartItemId);

            //TODO: Change 'localhost' to webapi when deploying on docker
            fetch(`http://localhost:5202/api/CartItems/${cartItemId}`, {
                method: "DELETE",
            })
                .then(response => {
                    if (response.ok) {
                        notyf.success("Item deleted successfully");

                        const cartItemDiv = document.getElementById(`cart-item-${cartItemId}`);
                        if (cartItemDiv) {
                            cartItemDiv.remove();
                        }

                        updateCartSummary();
                    } else {
                        throw new Error();
                    }
                })
                .catch(error => {
                    deleteSpinner(cartItemId);
                    notyf.error("Insufficient stock. Quantity reverted");
                });
        });
    });
    if (incrementDecrementQuantityContainer) {
        incrementDecrementQuantityContainer.forEach(container => {
            let incrementButton = container.querySelector('[data-increment-button]');
            let decrementButton = container.querySelector('[data-decrement-button]');
            let quantityValue = container.querySelector('[data-quantity-value]');


            function updateCartItemTotal(cartItemId, quantity, pricePerUnit) {
                const cartItemElement = document.getElementById(`cart-item-${cartItemId}`);
                const itemTotalPriceElement = cartItemElement.querySelector('.item-total-price');
                
                const itemTotal = quantity * pricePerUnit;
                
                if (itemTotalPriceElement) {
                    itemTotalPriceElement.textContent = `$${itemTotal.toLocaleString('en-US', {
                        minimumFractionDigits: 0,
                        maximumFractionDigits: 0
                    })}`;
                }

                updateCartSummary();
            }

            incrementButton.addEventListener("click", async function () {
                let quantityNumber = parseInt(quantityValue.textContent);
                let initialValue = quantityNumber;
                quantityNumber += 1;

                let cartItemId = incrementButton.getAttribute("data-increment-button");
                createSpinner(cartItemId);
                let pricePerUnit = parseFloat(container.querySelector('[name="price"]').value);

                let cartId = container.querySelector('[name="cartId"]');
                let productId = container.querySelector('[name="productId"]');
                let price = container.querySelector('[name="price"]');

                const updateBody = {
                    cartItemId: parseInt(cartItemId),
                    cartId: parseInt(cartId.value),
                    productId: parseInt(productId.value),
                    quantity: quantityNumber,
                    initialQuantity: initialValue,
                    price: parseInt(price.value),
                    product: null
                };

                try {
                    const response = await fetch(`http://localhost:5202/api/CartItems/${cartItemId}`, {
                        method: "PUT",
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(updateBody)
                    });

                    if (!response.ok) {
                        throw new Error('Failed to update cart item');
                    }

                    // Update the UI only if the API call was successful
                    quantityValue.textContent = quantityNumber.toString();
                    updateCartItemTotal(cartItemId, quantityNumber, pricePerUnit);
                    notyf.success("Cart item updated successfully");
                } catch (error) {
                    notyf.error("Insufficient stock. Quantity reverted.");
                } finally {
                    deleteSpinner(cartItemId);
                }
            });

            decrementButton.addEventListener("click", async function () {
                let quantityNumber = parseInt(quantityValue.textContent);

                if (quantityNumber > 1) {
                    let initialValue = quantityNumber;
                    quantityNumber -= 1;

                    let cartItemId = decrementButton.getAttribute("data-decrement-button");
                    createSpinner(cartItemId);
                    let pricePerUnit = parseFloat(container.querySelector('[name="price"]').value);

                    let cartId = container.querySelector('[name="cartId"]');
                    let productId = container.querySelector('[name="productId"]');
                    let price = container.querySelector('[name="price"]');

                    const updateBody = {
                        cartItemId: parseInt(cartItemId),
                        cartId: parseInt(cartId.value),
                        productId: parseInt(productId.value),
                        quantity: quantityNumber,
                        initialQuantity: initialValue,
                        price: parseInt(price.value),
                        product: null
                    };

                    try {
                        const response = await fetch(`http://localhost:5202/api/CartItems/${cartItemId}`, {
                            method: "PUT",
                            headers: {
                                'Content-Type': 'application/json',
                            },
                            body: JSON.stringify(updateBody)
                        });

                        if (!response.ok) {
                            throw new Error('Failed to update cart item');
                        }

                        quantityValue.textContent = quantityNumber.toString();
                        updateCartItemTotal(cartItemId, quantityNumber, pricePerUnit);
                        notyf.success("Cart item updated successfully");
                    } catch (error) {
                        notyf.error("Insufficient stock. Quantity reverted.");
                    } finally {
                        deleteSpinner(cartItemId);
                    }
                }
            });
        });
    }
});