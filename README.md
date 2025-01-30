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

---

## 🚀 **To Run Locally**  

### 📌 **Prerequisites**  
Before running the project, ensure you have:  
- **Visual Studio 2022** or **JetBrains Rider** installed  
- **Microsoft SQL Server Management Studio 19 (SSMS)** installed  
- **Access to the school VPN** (if connecting remotely)  

### 🗄 **Database Configuration**  
1. **Clone the repository**:  
   ```sh
   git clone https://github.com/your-repository.git
   ```  
2. **Open Microsoft SQL Server Management Studio 19 (SSMS)** and log in.  
3. Inside SSMS, open the SQL script **`pbadraH60A03Database.sql`** (located at the root of the repository).  
4. **Rename all instances** of `"pbadraH60A03Database"` in the script to your preferred database name.  
5. **Run the script** by clicking the **Execute** (▶) button.  

### 🏗 **Running the MVC Projects**  
1. **Open the solution**:  
   - Locate the `.sln` file inside the **`pbadraH60A03`** folder.  
   - Open it in **Visual Studio 2022**.  
2. **Update database connection strings**:  
   - In each project's `appsettings.json` file (`pbadraH60Customer`, `pbadraH60Store`, `pbadraH60Services`), replace the default connection string with your database connection string.  
3. **Start the services**:  
   - First, **run** the **`pbadraH60Services`** project.  
   - Then, run the **other two projects** (`pbadraH60Customer` and `pbadraH60Store`) in any order.  

✅ The **Customer Store** and **Store Dashboard** should now be running.  

### 📊 **Running the Manager Dashboard**  
1. **Open the project**:  
   - Navigate to the **`pbadrah60manager`** folder and open it in **Visual Studio Code**.  
2. **Install dependencies**:  
   ```sh
   npm install
   ```  
3. **Run the application**:  
   ```sh
   npm run dev
   ```  
   **⚠️ Ensure `pbadraH60Services` is still running before starting the manager frontend.**  

✅ The **Manager Dashboard** should now be running.  

### 🔑 **User Accounts for Testing**  
| **Role**   | **Email**                      | **Password**    |
|------------|--------------------------------|-----------------|
| Manager    | `manager@gmail.com`            | `Password-123`  |
| Clerk      | `clerk@gmail.com`              | `Password-123`  |
| Customer   | `customer@gmail.com`           | `Password-123`  |
| Customer   | `PierreBadra@outlook.com`      | `Password-123`  |
| Customer   | `johndoe@example.com`          | `Password-123`  |
| Customer   | `sarah.smith@domain.com`       | `Password-123`  |
| Customer   | `emily.jameson@company.org`    | `Password-123`  |
| Customer   | `claude.white@gmail.com`       | `Password-123`  |
| Customer   | `ryan.somers@gmail.com`        | `Password-123`  |
| Customer   | `richard-chan@gmail.com`       | `Password-123`  |
