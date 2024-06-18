USE CarDealership;

-- Добавление записей в таблицу Brands
INSERT INTO `Brands` (`BrandName`, `Country`, `Manufacturer`, `Address`) VALUES
('Ford', 'США', 'Ford Motor Company', 'Dearborn, Michigan, USA'),
('BMW', 'Германия', 'Bayerische Motoren Werke AG', 'Munich, Bavaria, Germany'),
('Honda', 'Япония', 'Honda Motor Co., Ltd.', 'Minato, Tokyo, Japan'),
('Audi', 'Германия', 'Audi AG', 'Ingolstadt, Bavaria, Germany'),
('Chevrolet', 'США', 'Chevrolet Division of General Motors Company', 'Detroit, Michigan, USA'),
('Hyundai', 'Южная Корея', 'Hyundai Motor Company', 'Seoul, South Korea'),
('Nissan', 'Япония', 'Nissan Motor Co., Ltd.', 'Yokohama, Kanagawa, Japan'),
('Kia', 'Южная Корея', 'Kia Corporation', 'Seoul, South Korea'),
('Volkswagen', 'Германия', 'Volkswagen AG', 'Wolfsburg, Lower Saxony, Germany'),
('Toyota', 'Япония', 'Toyota Motor Corporation', 'Toyota City, Aichi, Japan'),
('Mercedes-Benz', 'Германия', 'Daimler AG', 'Stuttgart, Baden-Württemberg, Germany'),
('Subaru', 'Япония', 'Subaru Corporation', 'Ebisu, Tokyo, Japan'),
('Mazda', 'Япония', 'Mazda Motor Corporation', 'Fuchū, Hiroshima, Japan'),
('Lexus', 'Япония', 'Lexus Division of Toyota Motor Corporation', 'Nagoya, Aichi, Japan'),
('Porsche', 'Германия', 'Porsche AG', 'Stuttgart, Baden-Württemberg, Germany');

-- Добавление записей в таблицу Cars
INSERT INTO `Cars` (`CarName`, `BrandID`, `YearOfProduction`, `Color`, `Category`, `Price`) VALUES
('Mustang', 1, 2020, 'Red', 'Coupe', 55000.00),
('X5', 2, 2019, 'Black', 'SUV', 60000.00),
('Accord', 3, 2021, 'White', 'Sedan', 30000.00),
('Q7', 4, 2018, 'Blue', 'SUV', 70000.00),
('Camaro', 5, 2020, 'Yellow', 'Coupe', 45000.00),
('Sonata', 6, 2022, 'Silver', 'Sedan', 25000.00),
('Altima', 7, 2021, 'Grey', 'Sedan', 28000.00),
('Sportage', 8, 2020, 'Green', 'SUV', 32000.00),
('Golf', 9, 2019, 'White', 'Hatchback', 23000.00),
('Corolla', 10, 2021, 'Red', 'Sedan', 20000.00),
('C-Class', 11, 2020, 'Black', 'Sedan', 40000.00),
('Outback', 12, 2022, 'Blue', 'SUV', 35000.00),
('CX-5', 13, 2018, 'Grey', 'SUV', 27000.00),
('RX', 14, 2021, 'Silver', 'SUV', 45000.00),
('911', 15, 2020, 'Yellow', 'Coupe', 120000.00);

-- Добавление записей в таблицу Customers
INSERT INTO `Customers` (`FullName`, `PassportData`, `Address`, `BirthDate`, `Gender`, `ContactDetails`, `Password`) VALUES
('John Doe', 'AB1234567', '123 Main St, Anytown, USA', '1980-01-01', 1, 'john.doe@example.com', 'password123'),
('Jane Smith', 'CD7654321', '456 Elm St, Othertown, USA', '1990-02-02', 0, 'jane.smith@example.com', 'password123'),
('Emily Johnson', 'EF2345678', '789 Oak St, Sometown, USA', '1985-03-03', 0, 'emily.johnson@example.com', 'password123'),
('Michael Brown', 'GH8765432', '321 Pine St, Anothertown, USA', '1975-04-04', 1, 'michael.brown@example.com', 'password123'),
('Sarah Davis', 'IJ3456789', '654 Maple St, Towerville, USA', '1995-05-05', 0, 'sarah.davis@example.com', 'password123'),
('David Wilson', 'KL9876543', '987 Birch St, Villagetown, USA', '1983-06-06', 1, 'david.wilson@example.com', 'password123'),
('Laura Martin', 'MN4567890', '123 Cedar St, Cityville, USA', '1992-07-07', 0, 'laura.martin@example.com', 'password123'),
('Robert Lee', 'OP0987654', '456 Spruce St, Hamletville, USA', '1978-08-08', 1, 'robert.lee@example.com', 'password123'),
('Jessica White', 'QR5678901', '789 Fir St, Burgville, USA', '1988-09-09', 0, 'jessica.white@example.com', 'password123'),
('William Harris', 'ST1234987', '321 Palm St, Hamletville, USA', '1981-10-10', 1, 'william.harris@example.com', 'password123'),
('Olivia Jones', 'UV1234567', '123 Chestnut St, Metropolis, USA', '1987-11-11', 0, 'olivia.jones@example.com', 'password123'),
('Liam Garcia', 'WX7654321', '456 Willow St, Capitol City, USA', '1979-12-12', 1, 'liam.garcia@example.com', 'password123'),
('Sophia Martinez', 'YZ2345678', '789 Redwood St, Oldtown, USA', '1986-01-13', 0, 'sophia.martinez@example.com', 'password123'),
('James Anderson', 'BA8765432', '321 Palm St, Hamletville, USA', '1973-02-14', 1, 'james.anderson@example.com', 'password123'),
('Ava Thomas', 'CB3456789', '654 Oak St, Newtown, USA', '1994-03-15', 0, 'ava.thomas@example.com', 'password123');

-- Добавление записей в таблицу Employees
INSERT INTO `Employees` (`FullName`, `WorkExperience`, `Salary`, `ContactDetails`, `Password`) VALUES
('Bob Blue', 10, 60000.00, 'bob.blue@cardealership.com', 'password123'),
('Charlie Black', 3, 45000.00, 'charlie.black@cardealership.com', 'password123'),
('Diane Red', 8, 55000.00, 'diane.red@cardealership.com', 'password123'),
('Edward White', 15, 70000.00, 'edward.white@cardealership.com', 'password123'),
('Fiona Yellow', 2, 40000.00, 'fiona.yellow@cardealership.com', 'password123'),
('George Purple', 7, 52000.00, 'george.purple@cardealership.com', 'password123'),
('Hannah Pink', 6, 48000.00, 'hannah.pink@cardealership.com', 'password123'),
('Ivan Gray', 4, 46000.00, 'ivan.gray@cardealership.com', 'password123'),
('Julia Green', 5, 50000.00, 'julia.green@cardealership.com', 'password123'),
('Kevin Brown', 11, 65000.00, 'kevin.brown@cardealership.com', 'password123'),
('Laura White', 9, 57000.00, 'laura.white@cardealership.com', 'password123'),
('Mike Black', 12, 68000.00, 'mike.black@cardealership.com', 'password123'),
('Nina Red', 13, 72000.00, 'nina.red@cardealership.com', 'password123'),
('Oscar Blue', 14, 75000.00, 'oscar.blue@cardealership.com', 'password123'),
('Paul Yellow', 16, 80000.00, 'paul.yellow@cardealership.com', 'password123');

-- Добавление записей в таблицу CarSales
INSERT INTO `CarSales` (`SaleDate`, `EmployeeID`, `CarID`, `CustomerID`) VALUES
('2023-01-01', 1, 1, 1),
('2023-01-02', 2, 2, 2),
('2023-01-03', 3, 3, 3),
('2023-01-04', 4, 4, 4),
('2023-01-05', 5, 5, 5),
('2023-01-06', 6, 6, 6),
('2023-01-07', 7, 7, 7),
('2023-01-08', 8, 8, 8),
('2023-01-09', 9, 9, 9),
('2023-01-10', 10, 10, 10),
('2023-01-11', 11, 11, 11),
('2023-01-12', 12, 12, 12),
('2023-01-13', 13, 13, 13),
('2023-01-14', 14, 14, 14),
('2023-01-15', 15, 15, 15);
