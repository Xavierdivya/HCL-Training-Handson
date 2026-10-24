# 🛒 Grocery Store Web Page

## 📌 Project Description

The Grocery Store Web Page is a simple web application developed using HTML, CSS, and JavaScript. It allows users to select grocery products, enter quantities, add items to a shopping list, and calculate the total bill amount.

The project demonstrates basic web development concepts such as HTML forms, CSS styling, JavaScript functions, arrays, and the Map data structure.

## ✨ Features

- **Product Selection:** Choose grocery products from a dropdown list.
- **Quantity Input:** Enter the required quantity for each product.
- **Add Products:** Add selected products to the shopping list.
- **Display Items:** View the selected products, their quantities, and prices.
- **Bill Calculation:** Calculate the total bill based on product prices and quantities.
- **Responsive Design:** A user-friendly interface that adapts to different screen sizes.
- **Modern UI:** Gradient background, rounded corners, and interactive buttons.

## 🧰 Technologies Used

- **HTML5** – Creates the structure of the web page.
- **CSS3** – Styles the page and makes the interface visually appealing.
- **JavaScript** – Handles product selection, shopping list management, and bill calculation.

## 📂 Project Structure

```text
Grocery-Store/
│
├── index.html      # HTML structure of the grocery store
├── Grocery.css     # CSS styling
├── Grocery.js      # JavaScript functionality
└── README.md       # Project documentation
```

## 🛍️ Available Products and Prices

| Product | Price |
|---|---:|
| Apple | 100 |
| Banana | 50 |
| Orange | 150 |
| Milk | 230 |
| Meat | 500 |
| Oil | 350 |
| Water | 270 |
| Sugar | 370 |

*Note: Prices are sample values in unspecified currency units and can be modified in the JavaScript file.*

## 🚀 How to Run the Project

1. Download or clone the project repository.
2. Make sure `index.html`, `Grocery.css`, and `Grocery.js` are in the same folder.
3. Open `index.html` in any modern web browser.
4. Select a product from the dropdown menu.
5. Enter the desired quantity.
6. Click the **Add** button to add the product to the list.
7. Click **Bill Amt** to display the selected products and calculate the total bill.

## 🧮 How It Works

1. A JavaScript `Map` stores the product names and their prices.
2. An array named `List_of_Items` stores the selected products, quantities, and prices.
3. The `addproduct()` function adds the selected product details to the array.
4. The `showproduct()` function displays the shopping list and calculates the total bill.

### Example

If a customer selects:

- Apple — Quantity: 2 — Price: 100 each
- Banana — Quantity: 3 — Price: 50 each

The total bill is calculated as:

```text
Apple:  2 × 100 = 200
Banana: 3 × 50  = 150

Total Bill = 350
```

## 🎯 Project Objectives

- Understand the fundamentals of HTML, CSS, and JavaScript.
- Learn how to handle user input using HTML form elements.
- Practice using arrays and JavaScript Map objects.
- Implement basic shopping list management.
- Perform arithmetic calculations dynamically.
- Create a responsive and visually appealing web interface.

## 🔮 Future Enhancements

- Add input validation for quantity fields.
- Display product prices and subtotals in a formatted table.
- Add the ability to remove products from the shopping list.
- Support updating quantities and adding multiple items of the same product.
- Display prices using a specified currency.
- Add tax, discounts, and final bill summaries.
- Store shopping list data using local storage.
- Add a print or download bill feature.

## 👩‍💻 Author

Developed as a beginner-friendly front-end web development project.

## 📄 License

This project is available for educational and personal learning purposes.
