# Boatbud
## Boat E-Commnerce Store

## Prerequisites
- Visual Studio 2022 installed
- Connect to the school VPN (If remote)

## Getting Started

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
