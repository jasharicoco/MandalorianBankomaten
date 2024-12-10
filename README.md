# Mandalorian Bankomaten

**Mandalorian Bankomaten** is a simple console-based banking system where users can manage their accounts, perform transactions, and handle loans. Admins can manage users and oversee the system.

## Features
### Classes:
### Bank
- Displays menu
- User interactions such as login, money transfers, loans and more.
### User
- View, create, or delete accounts.
- Transfer money between accounts or to other users.
- Apply for loans and make repayments.

### Admin
- Create and delete user accounts.

### SavingAccount
- Specialized type of account, savings account
- It inherits from Account base class
- Users earn interest on their balance

### Helper
- Provides functionality for the application
- Securely capturing a user's password without displaying the characters typed

### Seeder
- Creates seed users, allowing immediate login and testing
- Creates seed accounts, ensuring that every user has accounts with initial balances

## How to Use
1. **Run the Program:** Open the project in your IDE and start the application.  
2. **Login:** Use the sample credentials below or create new accounts:
   - Admin: Username: `admin`, Password: `0000`
   - User: Username: `user1`, Password: `password1`
3. **Navigate the Menu:** Use arrow keys to highlight options and press `Enter` to select.  

## Additional Notes
- Loan limits depend on the user's total account balance.
- Each user has their own accounts and loans.  
- Admins have full control over user management.  

## Link to our kanban-board
https://trello.com/b/IPnrlnPr/the-mandolorians

## UML
![image](https://github.com/user-attachments/assets/fe33b2e1-7f8c-44d4-ac49-e78f1db6b521)

**Created by Team Mandalorian**
