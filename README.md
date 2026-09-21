### Note on Order Creation

Currently, **User CRUD operations and JWT authentication** are not yet implemented in this project. 

In order to successfully test the "Create Order" endpoints, a valid user must exist in the database. After applying the migrations, please run the following SQL query manually in your database to create a dummy user before creating any orders:

```sql
INSERT INTO Users (UserName, Age, Email, Address, CreatedAt)
VALUES ('testuser', 25, 'test@example.com', '123 Test Avenue, Test City', CURRENT_TIMESTAMP);