# 🚤 E-Commerce Boat Store – ASP.NET Core MVC  

An **E-Commerce Boat Store** built with **ASP.NET Core MVC, C#, and MSSQL**, designed to facilitate boat sales and inventory management. The application is structured into four separate projects:  

### 📌 **Project 1: [MPA Store Dashboard](https://store-boatbud.pierrebadra.me)**
A secure **store dashboard** for managing products, customers, and inventory. It includes authentication and role-based authorization:  
- **Managers** can update prices, manage stock, and oversee business operations.  
- **Clerks** have limited permissions, such as processing orders but not modifying prices.  

### 🛒 **Project 2: [MPA Customer Store](https://customer-boatbud.pierrebadra.me)**
A user-friendly **e-commerce storefront** that allows customers to:  
- **Register and sign in** to manage their accounts.  
- **Browse boats**, view product details, and check availability.
- **Add boats to the cart** and proceed to checkout, dynamically updating stock levels.
> **Note:** This project integrates private SOAP services for **credit card processing** and **tax calculation**, which are not publicly accessible. As a result, the checkout functionality will not work in the deployed version. However, all other features, including browsing, adding items to the cart, and account management, remain fully functional.

### 📊 **Project 3: [SPA Manager Dashboard](https://manager-boatbud.pierrebadra.me)**
An order management dashboard that allows managers to:
- **Quickly view** recent sales and monthly orders through charts.
- **Manage products** by updating product stock, buy and sell price.
- **Browse boats**, view, sort and search product details.
- **Browse Orders**, view, sort and search order details.

### 🌐 **Project 4: [Web API](https://api-boatbud.pierrebadra.me) (no API docs/interface in production environment)**
A **RESTful API** built with **Swagger**, providing endpoints to power the **[Store Dashboard](https://store-boatbud.pierrebadra.me)**, **[Manager Dashboard](https://manager-boatbud.pierrebadra.me)** and **[Customer Store](https://customer-boatbud.pierrebadra.me)**. This API ensures secure and scalable data exchange between the front-end and back-end services.  

### 🛠 **Tech Stack & Tools**  
- **Backend:** ASP.NET Core, C#, MSSQL, Entity Framework Core  
- **Frontend:** ASP.NET Core, C#, JavaScript, TypeScript, HTML, CSS (Tailwind)  
- **Deployment & Infrastructure:** Docker

This project demonstrates **full-stack development**, integrating **authentication, role-based authorization, inventory management, and a scalable API**, making it a complete e-commerce solution.

## To Run Locally
### Prerequisites
- Visual Studio 2022 or Rider installed
- Connect to the school VPN (if remote)

### Database Configuration
1. Clone the repository.
2. Open Microsoft SQL Server Management Studio 19 (SSMS) and login using your account credentials.
3. Inside SSMS, open the SQL script **"pbadraH60A03Database.sql"** located at the root of the repository.
5. Inside the file, rename all instances of **"pbadraH60A03Database"** found in the file to the preferred database name.
6. Run the script by pressing the green play button located at the top-left of the editor.

### Running the MVC projects
1. Open the .sln file located inside the **"pbadraH60A03"** folder (It should open the project inside Visual Studio 2022).
2. For each project (pbadraH60Customer, pbadraH60Store, pbadraH60Services), open the appsettings.json file and replace the connection string to your database connection string created via the script.
3. First run the **"pbadraH60Services"** project, then the other two in whichever order.
4. Enjoy.

### Running the manager project
1. Open the **"pbadrah60manager"** inside of Visual Studio Code.
2. Once inside the code editor open the command line, and type **"npm i"**.
3. Once the dependencies are installed, you can go ahead and run the application by typing "npm run dev" inside the command line.
**NOTE: MAKE SURE THE "pbadraH60Services" IS STILL RUNNING BEFORE RUNNING THE MANAGER FRONTEND**
5. Enjoy.

### User Accounts For Testing
| **Role**   | **Email**                      | **Password**    |
|------------|--------------------------------|-----------------|
| Manager    | manager@gmail.com              | Password-123    |
| Clerk      | clerk@gmail.com                | Password-123    |
| Customer   | customer@gmail.com             | Password-123    |
| Customer   | PierreBadra@outlook.com        | Password-123    |
| Customer   | johndoe@example.com            | Password-123    |
| Customer   | sarah.smith@domain.com         | Password-123    |
| Customer   | emily.jameson@company.org      | Password-123    |
| Customer   | claude.white@gmail.com         | Password-123    |
| Customer   | ryan.somers@gmail.com          | Password-123    |
| Customer   | richard-chan@gmail.com         | Password-123    |
