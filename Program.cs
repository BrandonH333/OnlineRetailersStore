using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using OnlineRetailersStore.Data;

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

            // User
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS User (
                UserId VARCHAR(36),
                FirstName VARCHAR(50),
                LastName VARCHAR(50),
                Username VARCHAR(50),
                Password VARCHAR(50),
                Retailer TINYINT(1),
                PRIMARY KEY (UserId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO User(UserId, FirstName, LastName, Username, Password, Retailer) " +
                                "VALUES('6553323f-46b6-4817-8eab-3e9e51957875', 'Jake', 'Bowman', 'test1@test.com', 'test', 0)," +
                                        "('cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c', 'Rachel', 'Davis', 'test2@test.com', 'test', 0)," +
                                        "('a6eba3a9-becc-438b-8159-58c76fbc1b79', 'Hector', 'Diaz', 'test3@test.com', 'test', 0)," +
                                        "('7123c878-6878-4415-8e31-aebe87759958', 'Viola', 'Franklin', 'officedepot@test.com', 'test', 1)," +
                                        "('328c4004-669b-456a-9182-038cf033d066', 'Cristina', 'Munoz', 'staples@test.com', 'test', 1)," +
                                        "('aabfff76-a12f-4d5c-ad1d-5a49bca3279a', 'Ben', 'Carroll', 'frys@test.com', 'test', 1)";
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
                UserId VARCHAR(36),
                PRIMARY KEY (AddressId),
                FOREIGN KEY (UserId)
                    REFERENCES User (UserId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO Address(AddressId, Address1, Address2, City, State, ZipCode, Country, UserId) " +
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
                UserId VARCHAR(36),
                PRIMARY KEY (BankAccountId),
                FOREIGN KEY (UserId)
                    REFERENCES User (UserId)
            )";
            cmd.ExecuteNonQuery();  

            cmd.CommandText = "INSERT IGNORE INTO Bank_Account(BankAccountId, Name, CardNumber, ExpirationDate, Cvc, UserId) " +
                                "VALUES('16dbd362-7a3c-4d8f-a29c-d1967323e4d8', 'Bank of America', '4111111111111111', '2024-10-01', 123, '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('874a63b4-9a8b-4577-b8f3-3f7c1c1f1450', 'Wells Fargo', '4242424242424242', '2022-03-01', 345, 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('f25e253e-68d5-43fa-acaf-7167b8162901', 'Logix', '4012888888881881', '2023-05-01', 678, 'a6eba3a9-becc-438b-8159-58c76fbc1b79')";
            cmd.ExecuteNonQuery();

            // Supplier
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Supplier (
                SupplierId VARCHAR(36),
                Name VARCHAR(50),
                UserId VARCHAR(36),
                PRIMARY KEY (SupplierId),
                FOREIGN KEY (UserId)
                    REFERENCES User (UserId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO Supplier(SupplierId , Name, UserId) " +
                                "VALUES('18822c07-2d1c-4296-9152-a05fbb2b5704', 'Office Depot', '7123c878-6878-4415-8e31-aebe87759958')," +
                                        "('96262c9b-bfde-44a1-bf7a-f871403c6723', 'Staples', '328c4004-669b-456a-9182-038cf033d066')," +
                                        "('f1de43c0-5a73-46e5-9494-90f58ab6f077', 'Frys Electronics', 'aabfff76-a12f-4d5c-ad1d-5a49bca3279a')";
            cmd.ExecuteNonQuery();

            // Product
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Product (
                ProductId VARCHAR(36),
                Name VARCHAR(50),
                Description VARCHAR(1000),
                Price DECIMAL(15, 2),
                ImageSrc varchar(255),
                Inventory INT(4),
                SupplierId VARCHAR(36),
                KEY `Name` (`Name`),
                KEY `Price_idx` (`Price`),
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
                UserId VARCHAR(36),
                PRIMARY KEY (ShoppingCartId),
                FOREIGN KEY (ProductId)
                    REFERENCES Product (ProductId),
                FOREIGN KEY (UserId)
                    REFERENCES User (UserId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO Shopping_Cart(ShoppingCartId, LineItemId, Quantity, ProductId, UserId) " +
                                "VALUES('6f3b70e7-9491-406c-9f83-4d0a11de5402', 1, 1, '97e4b136-0d8d-4d82-b798-9e0706d6eb46', '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('43c5ed8d-8965-4355-95ff-74a40121d67e', 1, 1, 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('217a618a-49a5-491d-81fc-5ddc971642af', 1, 2, '9a661973-d372-4dfe-b8fc-4844a61eefa9', 'a6eba3a9-becc-438b-8159-58c76fbc1b79')";
            cmd.ExecuteNonQuery();

            // Order
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `Order` (
                OrderId VARCHAR(36),
                CreatedDate DATETIME,
                LineItemId INT(3),
                Quantity INT(3),
                ProductId VARCHAR(36),
                UserId VARCHAR(36),
                KEY `Quantity_idx` (`Quantity`),
                KEY `CreatedDate` (`CreatedDate`),
                KEY `LineItemId_idx` (`LineItemId`),
                PRIMARY KEY (OrderId),
                FOREIGN KEY (ProductId)
                    REFERENCES Product (ProductId),
                FOREIGN KEY (UserId)
                    REFERENCES User (UserId)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO `Order`" +
                                "VALUES('5d9baa9c-6745-4c9b-bda7-d76135a1d8fe', '2021-04-01', 1, 1, '97e4b136-0d8d-4d82-b798-9e0706d6eb46', '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('dc1bc158-f407-43c2-9965-e6b8f0b2d395', '2021-04-02', 1, 1, 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('th1bc158-4tc2-ft07-9965-gnh8f0b2d395', '2021-03-05', 1, 1, '97e4b136-0d8d-4d82-b798-9e0706d6eb46', 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('kc1bc284-f407-9965-43c2-e6b8f0b25gnt', '2021-03-06', 1, 1, '9a661973-d372-4dfe-b8fc-4844a61eefa9', '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('jc1bc358-9965-43c2-f407-hmj8f0b2jnk2', '2021-03-07', 1, 1, 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', '6553323f-46b6-4817-8eab-3e9e51957875')," +
                                        "('gfdvc750-g400-43c2-9965-kmgff0b2d395', '2021-02-09', 1, 1, '9a661973-d372-4dfe-b8fc-4844a61eefa9', 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c')," +
                                        "('hc1bc959-k509-43c2-6545-nhb8thvdd395', '2021-01-08', 1, 1, '97e4b136-0d8d-4d82-b798-9e0706d6eb46', 'a6eba3a9-becc-438b-8159-58c76fbc1b79')," +
                                        "('kc1bc752-h402-43c2-8745-dvbgf0b2d395', '2021-02-03', 1, 1, 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', 'a6eba3a9-becc-438b-8159-58c76fbc1b79')," +
                                        "('4e233981-9339-420a-9hgf-c0c6b149gvd4', '2021-03-30', 1, 2, '9a661973-d372-4dfe-b8fc-4844a61eefa9', 'a6eba3a9-becc-438b-8159-58c76fbc1b79')";
            cmd.ExecuteNonQuery();

            // Order_history
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `order_history` (
                `OrderHistoryId` INT(3),
                `UserId` VARCHAR(36),
                `ProductId` VARCHAR(36),
                `Quantity` INT(3),
                `Price` DECIMAL(15, 2),
                `Name` VARCHAR(50),
                `OrderId` VARCHAR(36),
                `LineItemId` INT(3),
                `CreatedDate` DATETIME,
                KEY `Name` (`Name`),
                KEY `OrderId_idx` (`OrderId`),
                KEY `Price_idx` (`Price`),
                KEY `ProductId_idx` (`ProductId`),
                KEY `Quantity_idx` (`Quantity`),
                KEY `UserId_idx` (`UserId`),
                KEY `OrderHistoryId_idx` (`OrderHistoryId`),
                PRIMARY KEY (`OrderHistoryId`),
                FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`),
                FOREIGN KEY (`Name`) REFERENCES `Product` (`Name`),
                FOREIGN KEY (`OrderId`) REFERENCES `Order` (`OrderId`),
                FOREIGN KEY (`LineItemId`) REFERENCES `Order` (`LineItemId`),
                FOREIGN KEY (`Price`) REFERENCES `Product` (`Price`),
                FOREIGN KEY (`ProductId`) REFERENCES `Product` (`ProductId`),
                FOREIGN KEY (`Quantity`) REFERENCES `Order` (`Quantity`)
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT IGNORE INTO order_history(OrderHistoryId, UserId, ProductId, Quantity, Price, Name, OrderId, CreatedDate, LineItemId)" +
                                "VALUES(1, 'a6eba3a9-becc-438b-8159-58c76fbc1b79', '9a661973-d372-4dfe-b8fc-4844a61eefa9', 2, 199.99, 'Microsoft Windows 10 Pro', '4e233981-9339-420a-9hgf-c0c6b149gvd4', '2021-03-30', 1)," +
                                        "(2, '6553323f-46b6-4817-8eab-3e9e51957875', '97e4b136-0d8d-4d82-b798-9e0706d6eb46', 1, 439.99, 'Microsoft Office 2019 Professional', '5d9baa9c-6745-4c9b-bda7-d76135a1d8fe', '2021-04-01', 1)," +
                                        "(3, '6553323f-46b6-4817-8eab-3e9e51957875', 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', 1, 14.99, 'Adobe Acrobat Pro DC', 'jc1bc358-9965-43c2-f407-hmj8f0b2jnk2', '2021-03-07', 1)," +
                                        "(4, 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c', 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', 1, 14.99, 'Adobe Acrobat Pro DC', 'dc1bc158-f407-43c2-9965-e6b8f0b2d395', '2021-04-02', 1)," +
                                        "(5, '6553323f-46b6-4817-8eab-3e9e51957875', '9a661973-d372-4dfe-b8fc-4844a61eefa9', 1, 199.99, 'Microsoft Windows 10 Pro', 'kc1bc284-f407-9965-43c2-e6b8f0b25gnt', '2021-03-06', 1)," +
                                        "(6, 'a6eba3a9-becc-438b-8159-58c76fbc1b79', 'ccf38a4c-b98c-47be-a823-58e72fa60cd0', 1, 14.99, 'Adobe Acrobat Pro DC', 'kc1bc752-h402-43c2-8745-dvbgf0b2d395', '2021-02-03', 1)," +
                                        "(7, 'a6eba3a9-becc-438b-8159-58c76fbc1b79', '97e4b136-0d8d-4d82-b798-9e0706d6eb46', 1, 439.99, 'Microsoft Office 2019 Professional', 'hc1bc959-k509-43c2-6545-nhb8thvdd395', '2021-01-08', 1)," +
                                        "(8, 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c', '97e4b136-0d8d-4d82-b798-9e0706d6eb46', 1, 439.99, 'Microsoft Office 2019 Professional', 'th1bc158-4tc2-ft07-9965-gnh8f0b2d395', '2021-03-05', 1)," +
                                        "(9, 'cfaad4ca-2a0f-4f66-818f-6cd30bffcf4c', '9a661973-d372-4dfe-b8fc-4844a61eefa9', 2, 199.99, 'Microsoft Windows 10 Pro', 'gfdvc750-g400-43c2-9965-kmgff0b2d395', '2021-02-09', 1)";
            cmd.ExecuteNonQuery();

        }
    }
}
