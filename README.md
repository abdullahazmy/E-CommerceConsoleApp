# 🛍️ E-Commerce System (C#) – Showcasing OOP Skills

## 📌 Task Description

The challenge was to design and implement an object-oriented e-commerce system with the following core features:

### 1. 🧱 Product Management
- Define products with `Name`, `Price`, and `Quantity`.
- Support both **Perishable** (e.g., Cheese, Biscuits) and **Non-Perishable** (e.g., TV, Mobile) products.
- Some products are **shippable** and require weight specification (e.g., TVs), while others are not (e.g., mobile scratch cards).

### 2. 🛒 Shopping Cart Functionality
- Allow customers to add products to their cart.
- Validate quantity before adding (no more than available stock).

### 3. 💳 Checkout Process
- Calculate subtotal, shipping fees, and final total.
- Deduct the amount from the customer's balance upon successful payment.
- Print a detailed receipt to the console.

### 4. 🛑 Error Handling
- Detect and report when the cart is empty.
- Prevent checkout when:
  - Customer balance is insufficient
  - Cart contains expired or out-of-stock items

### 5. 🚚 Shipping Integration
- Collect only shippable items.
- Send them to `ShippingService`, which processes objects implementing the `IShippable` interface.

---

## 💡 Solution Overview

This C# implementation demonstrates object-oriented principles through a modular and extensible design. It features abstraction, inheritance, interfaces, encapsulation, and separation of concerns.

---

## 🧱 Product Hierarchy

### 🔹 `Product` (Abstract Base Class)
Defines shared properties:
- `Name`
- `Price`
- `Quantity`

### 🔹 `NonPerishableProduct`
For products that do **not** expire, e.g.:
- TVs
- Mobile Phones

### 🔹 `PerishableProduct`
For products that have an **expiration date**, e.g.:
- Cheese
- Biscuits

---

## 🔌 Interfaces

### ✅ `IShippable`
Implemented by products that can be shipped. Includes:
- `Weight` property

### 🕒 `IPerishable`
Implemented by products that can expire. Includes:
- `ExpirationDate` property

---

## 🛒 Core Services

### 🛍️ `ShoppingCart`
Handles:
- Adding and removing `CartItem`s
- Validating product quantities

### 💳 `CheckoutService`
Responsible for:
- Calculating total amount (subtotal + shipping)
- Validating cart items and balance
- Deducting from customer's account
- Printing a receipt

### 🚚 `ShippingService`
Processes all shippable items implementing `IShippable`. Simulates shipment logic.

---

## 📦 Supporting Classes

### 📄 `CartItem`
Associates a `Product` with a specific quantity.

### 👤 `Customer`
Tracks:
- Customer `Name`
- `Balance`
- List of purchased items

---

## ✅ Object-Oriented Principles Demonstrated

- **Abstraction**: Abstract `Product` class defines common structure
- **Encapsulation**: Internal state of classes protected through access modifiers
- **Inheritance**: `PerishableProduct` and `NonPerishableProduct` derive from `Product`
- **Polymorphism**: Shipping handled uniformly through `IShippable`
- **Interface Segregation**: Only products needing shipping implement `IShippable`, and only expirable ones implement `IPerishable`

---

## 🖥️ Console Output

Receipt is printed to the console with:
- Itemized list of products
- Subtotal and shipping fee
- Total cost and remaining balance

---

> Thank You

