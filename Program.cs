using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using OnlineRetailersStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRetailersStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateDB();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void CreateDB()
        {
            using var initialConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            initialConnection.Open();

            using var initialCommand = new MySqlCommand();
            initialCommand.Connection = initialConnection;

            // Create database
            initialCommand.CommandText = @"CREATE DATABASE IF NOT EXISTS Online_Retailers_Store";
            initialCommand.ExecuteNonQuery();
            initialConnection.Close();

            using var con = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString);
            con.Open();

            using var cmd = new MySqlCommand();
            cmd.Connection = con;

            // Customer
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Customer (
                CustomerId VARCHAR(36),
                FirstName VARCHAR(50),
                LastName VARCHAR(50),
                Username VARCHAR(50),
                Password VARCHAR(50),
                PRIMARY KEY (CustomerId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO Customer(CustomerId, FirstName, LastName, Username, Password) " +
                                "VALUES('6553323f-46b6-4817-8eab-3e9e51957875', 'Jake', 'Bowman', 'test1@test.com', 'test')," +
                                        "('cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c', 'Rachel', 'Davis', 'test2@test.com', 'test')," +
                                        "('a6eba3a9-becc-438b-8159-58c76fbc1b79', 'Hector', 'Diaz', 'test3@test.com', 'test')";
            cmd.ExecuteNonQuery();

            
            // Address
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Address (
                AddressId VARCHAR(36),
                Address1 VARCHAR(100),
                Address2 VARCHAR(100),
                City VARCHAR(50),
                State VARCHAR(50),
                ZipCode INT(15),
                Country VARCHAR(50),
                CustomerId VARCHAR(36),
                PRIMARY KEY (AddressId),
                FOREIGN KEY (CustomerId)
                    REFERENCES Customer (CustomerId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO Address(AddressId, Address1, Address2, City, State, ZipCode, Country, CustomerId) " +
                                "VALUES('6924f709-c8fa-4878-b3e8-bec73e0f5001', '76 Roberts Rd.', '', 'Wyoming', 'MI', 49509, 'US', '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('cc1cdb97-89e7-48be-b130-6265320b5ec0', '7913 Sunnyslope Street', '', 'Lenoir', 'NC', 28645, 'US', 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('d3227d97-26e4-495f-9f1f-791449a7215b', '7619 Magnolia Lane', '', 'Thornton', 'CO', 80241, 'US', 'a6eba3a9-becc-438b-8159-58c76fbc1b79')";
            cmd.ExecuteNonQuery();

            // Bank_Account
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Bank_Account (
                BankAccountId VARCHAR(36),
                Name VARCHAR(50),
                CardNumber VARCHAR(25),
                ExpirationDate DATE,
                Cvc INT(3),
                CustomerId VARCHAR(36),
                PRIMARY KEY (BankAccountId),
                FOREIGN KEY (CustomerId)
                    REFERENCES Customer (CustomerId)
            )";
            cmd.ExecuteNonQuery();  

            cmd.CommandText = "INSERT IGNORE INTO Bank_Account(BankAccountId, Name, CardNumber, ExpirationDate, Cvc, CustomerId) " +
                                "VALUES('16dbd362-7a3c-4d8f-a29c-d1967323e4d8', 'Bank of America', '4111111111111111', '2024-10-01', 123, '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('874a63b4-9a8b-4577-b8f3-3f7c1c1f1450', 'Wells Fargo', '4242424242424242', '2022-03-01', 345, 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('f25e253e-68d5-43fa-acaf-7167b8162901', 'Logix', '4012888888881881', '2023-05-01', 678, 'a6eba3a9-becc-438b-8159-58c76fbc1b79')";
            cmd.ExecuteNonQuery();

            // Supplier
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Supplier (
                SupplierId VARCHAR(36),
                Name VARCHAR(50),
                PRIMARY KEY (SupplierId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO Supplier(SupplierId , Name) " +
                                "VALUES('18822c07-2d1c-4296-9152-a05fbb2b5704', 'Office Depot')," +
                                        "('96262c9b-bfde-44a1-bf7a-f871403c6723', 'Staples')," +
                                        "('f1de43c0-5a73-46e5-9494-90f58ab6f077', 'Frys Electronics')";
            cmd.ExecuteNonQuery();

            // Product
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Product (
                ProductId VARCHAR(36),
                Name VARCHAR(50),
                Description VARCHAR(50),
                Price DECIMAL(15 , 2 ),
                ImageSrc varchar(255),
                Inventory INT(4),
                SupplierId VARCHAR(36),
                PRIMARY KEY (ProductId),
                FOREIGN KEY (SupplierId)
                    REFERENCES Supplier (SupplierId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO Product(ProductId , Name, Description, Price, ImageSrc, Inventory, SupplierId) " +
                                "VALUES('97e4b136-0d8d-4d82-b798-9e0706d6eb46', 'Microsoft Office 2019 Professional', 'Office Professional 2019 is for growing small businesses who want the classic Office apps plus Outlook, OneDrive, Publisher and Access1. A one-time purchase installed on 1 PC for use at work.', 439.99, 'https://file-cdn.goodoffer24.com/media/catalog/product/m/i/microsoft-office-2019-pro-plus-5-pc_go24.jpg', 5, '18822c07-2d1c-4296-9152-a05fbb2b5704')," +
                                        "('ccf38a4c-b98c-47be-a823-58e72fa60cd0', 'Adobe Acrobat Pro DC', 'More than five million organizations around the world rely on Acrobat DC to create and edit the smartest PDFs, convert PDFs to Microsoft Office formats, and so much more. When you’re on the move and you need to collaborate with colleagues in multiple locations, trust the power of Acrobat DC to make it happen.', 14.99, 'https://images-na.ssl-images-amazon.com/images/I/414BXP8VbnL._AC_.jpg', 4, '96262c9b-bfde-44a1-bf7a-f871403c6723')," +
                                        "('9a661973-d372-4dfe-b8fc-4844a61eefa9', 'Microsoft Windows 10 Pro', 'Windows 10 Pro includes all the features of Windows 10 Home, plus business functionality for encryption, remote log-in, creating virtual machines, and more', 199.99, 'https://m.media-amazon.com/images/I/61ejGUbgfWL._AC_SX679_.jpg', 3, 'f1de43c0-5a73-46e5-9494-90f58ab6f077')";
            cmd.ExecuteNonQuery();

            // Shopping_Cart
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Shopping_Cart (
                ShoppingCartId VARCHAR(36),
                LineItemId INT(3),
                Quantity INT(3),
                ProductId VARCHAR(36),
                CustomerId VARCHAR(36),
                PRIMARY KEY (ShoppingCartId),
                FOREIGN KEY (ProductId)
                    REFERENCES Product (ProductId),
                FOREIGN KEY (CustomerId)
                    REFERENCES Customer (CustomerId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO Shopping_Cart(ShoppingCartId, LineItemId, Quantity, ProductId, CustomerId) " +
                                "VALUES('6f3b70e7-9491-406c-9f83-4d0a11de5402', 1, 1, '97e4b136-0d8d-4d82-b798-9e0706d6eb46', '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('43c5ed8d-8965-4355-95ff-74a40121d67e', 1, 1, 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('217a618a-49a5-491d-81fc-5ddc971642af', 1, 2, '9a661973-d372-4dfe-b8fc-4844a61eefa9', 'a6eba3a9-becc-438b-8159-58c76fbc1b79')";
            cmd.ExecuteNonQuery();

            // Shopping_Cart
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `Order` (
                OrderId VARCHAR(36),
                CreatedDate DATETIME,
                LineItemId INT(3),
                Quantity INT(3),
                ProductId VARCHAR(36),
                CustomerId VARCHAR(36),
                PRIMARY KEY (OrderId),
                FOREIGN KEY (ProductId)
                    REFERENCES Product (ProductId),
                FOREIGN KEY (CustomerId)
                    REFERENCES Customer (CustomerId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO `Order`" +
                                "VALUES('5d9baa9c-6745-4c9b-bda7-d76135a1d8fe', '2021-04-01', 1, 1, '97e4b136-0d8d-4d82-b798-9e0706d6eb46', '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('dc1bc158-f407-43c2-9965-e6b8f0b2d395', '2021-04-2020', 1, 1, 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('3e233380-9339-420a-9cc9-c0c6b1492733', '2021-03-30', 1, 2, '9a661973-d372-4dfe-b8fc-4844a61eefa9', 'a6eba3a9-becc-438b-8159-58c76fbc1b79')";
            cmd.ExecuteNonQuery();
        }
    }
}
