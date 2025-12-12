Scroll down for Georgian

Warehouse Inventory Management System
Project Description

Our project is Warehouse Inventory Management System, a small but quite functional warehouse management program that we created using the C# language and the .NET platform. We do not use a real database in the project, so all information is stored in CSV files, which are managed by specially written classes.
The main idea of ​​the project is to create a system that can record products, update stocks, search/filter products and store the history of all operations.

Project Purpose
The project aims to simplify the work of the warehouse. The program allows you to:
1. Add a new product
2. Change existing product data
3. Delete a product (soft or hard delete)
4. Increase or decrease its quantity
5. View log history
6. Filter products by category
7. View items with low stock
8. Count various types of statistics

Architecture
We divided the project into several layers to make the code more organized.

1) Domain Layer - mainly models
Here are the main classes:
Product
Stores the product name, price, quantity, category, etc. Functions are also described here, such as:
• Increase/decrease stock
• Calculate product cost
• Check for low stock
StockMovement
This is a class that stores information about what happened to the products: added, subtracted, created, or deleted.
BaseEntity
A common class for all objects (Id, activity, creation date, etc.).

2) Data Layer - CSV file management
Here is the CsvDataManager, which we used as a “file database”.
It can:
• Read CSV files
• Write data
• Convert products and movements from CSV to objects and vice versa
This layer performs the function of a mini-ORM on CSV files.
3) Repository Layer - CRUD logic
Here we have a generic BaseRepository, which contains:
• Create
• Read
• Update
• Delete (soft and hard)
ProductRepository and StockMovementRepository additionally contain more complex functions, such as:
• Search by name
• Filter by category
• Low stock products
• Movement filter
• Statistics

4) Services Layer - Business logic
The most active layer.
InventoryService
Here is the entire warehouse logic:
• Create products
• Edit
• Delete
• Change inventory
• Add record to history
The service actually manages what happens to any action.

CSV files
The project uses two CSVs:
products.csv - the entire list of products is stored.
movements.csv - this is where what happened to the products is stored - added, subtracted, deleted, etc.

Additional functions
The project also has:
• Calculate the total warehouse value
• The most expensive products
• Count products by category
• Count movements by type

Final assessment
In sum, the project is a complete, fast and convenient system for warehouse management. Despite the fact that the data is stored in CSV files, and not in a database, all the necessary functions are implemented, including CRUD operations, inventory update, logging and filters.

By doing this project, I understood well:
• How layered architecture works
• How to design models
• How to make a CSV-based “database”
• And how to connect everything with services

Project authors:
David Shengelia
Lado Maisuradze



ქართულად:

Warehouse Inventory Management System
პროექტის აღწერა

ჩვენი პროექტი არის Warehouse Inventory Management System პატარა, მაგრამ საკმაოდ ფუნქციური საწყობის მართვის პროგრამა, რომელიც შევქმენით C# ენის და .NET პლატფორმის გამოყენებით. პროექტში არ ვიყენებთ რეალურ მონაცემთა ბაზას, ამიტომ სრული ინფორმაცია ინახება CSV ფაილებში, რომელთა მართვაც ხდება სპეციალურად დაწერილი კლასებით.
პროექტის მთავარი იდეა არის ის, რომ შევქმნათ სისტემა, რომელსაც შეუძლია პროდუქციის აღრიცხვა, მარაგების განახლება, პროდუქციის ძებნა/ფილტრი და ყველა ოპერაციის ისტორიის შენახვა.

პროექტის დანიშნულება
პროექტი მიზნად ისახავს საწყობის მუშაობის გამარტივებას. პროგრამა იძლევა შესაძლებლობას:
1.	დავამატოთ ახალი პროდუქტი
2.	შევცვალოთ უკვე არსებული პროდუქტის მონაცემები
3.	წავშალოთ პროდუქტი (soft or hard delete)
4.	გავზარდოთ ან შევამციროთ მისი რაოდენობა
5.	ვნახოთ ლოგების ისტორია
6.	გავფილტროთ პროდუქტები კატეგორიით
7.	ვნახოთ დაბალი მარაგის მქონე ნივთები
8.	დავთვალოთ სხვადასხვა ტიპის სტატისტიკა



არქიტექტურა
პროექტი დავყავით რამდენიმე ფენად, რათა კოდი უფრო მოწესრიგებული ყოფილიყო.

1) Domain Layer - ძირითადად მოდელები
აქ არის მთავარი კლასები:
Product
ინახავს პროდუქტის სახელს, ფასს, რაოდენობას, კატეგორიას და ა.შ. აქვე გაწერილია ფუნქციები, როგორებიცაა:
•	მარაგის ზრდა/კლება
•	პროდუქტის ღირებულების გამოთვლა
•	დაბალი მარაგის შემოწმება
StockMovement
ეს არის კლასა, რომელიც ინახავს ინფორმაციას თუ რა მოხდა პროდუქტებთან: დაემატა, მოაკლდა, შეიქმნა, ან წაიშალა.
BaseEntity
საერთო კლასა ყველა ობიექტისთვის (Id, აქტიურობა, შექმნის თარიღი და ა.შ.).

2) Data Layer - CSV ფაილების მართვა
აქ არის CsvDataManager, რომელიც ჩვენ გამოვიყენე როგორც “ფაილური ბაზა”.
მას შეუძლია:
•	CSV ფაილების წაკითხვა
•	მონაცემების ჩაწერა
•	პროდუქციის და მოძრაობების კონვერტირება CSV–დან ობიექტებად და პირიქით
ეს ფენა ასრულებს mini-ORM-ის ფუნქციას CSV ფაილებზე.
3) Repository Layer – CRUD ლოგიკა
აქ გვაქვს გენერიკული BaseRepository, რომელიც შეიცავს:
•	Create
•	Read
•	Update
•	Delete (soft და hard)
ProductRepository და StockMovementRepository კი დამატებით შეიცავს უფრო კომპლექსურ ფუნქციებს, როგორიცაა:
•	ძებნა სახელით
•	კატეგორიით გაფილტვრა
•	დაბალი მარაგის პროდუქტები
•	მოძრაობების ფილტრი
•	სტატისტიკა

4) Services Layer – ბიზნეს ლოგიკა
ყველაზე აქტიური ფენა.
InventoryService
აქ არის მთელი საწყობის ლოგიკა:
•	პროდუქციის შექმნა
•	რედაქტირება
•	წაშლა
•	მარაგის ცვლილება
•	ისტორიაში ჩანაწერის დამატება
სერვისი ფაქტობრივად მართავს იმას, რა ხდება ნებისმიერ მოქმედებაზე.

CSV ფაილები
პროექტი იყენებს ორ CSV-ს:
products.csv - შენახულია პროდუქტების მთლიანი სია.
movements.csv - აქ ინახება, რა მოხდა პროდუქტებთან - დაემატა, მოაკლდა, წაიშალა და ა.შ.

დამატებითი ფუნქციები
პროექტს ასევე აქვს:
•	მთლიანი საწყობის ღირებულების გამოთვლა
•	ყველაზე ძვირადღირებული პროდუქტები
•	პროდუქტების დათვლა კატეგორიებით
•	მოძრაობების დათვლა ტიპების მიხედვით

საბოლოო შეფასება
ჯამში, პროექტი წარმოადგენს სრულყოფილ, სწრად და მოსახერხებელ სისტემას საწყობის მართვისთვის. მიუხედავად იმისა, რომ მონაცემები ინახება CSV ფაილებში და არა მონაცემთა ბაზაში, მაინც გაკეთებულია ყველა საჭირო ფუნქცია, მათ შორის CRUD ოპერაციები, მარაგის განახლება, ლოგირება და ფილტრები.

ამ პროექტის გაკეთებით კარგად გავიგე:
•	როგორ მუშაობს ფენოვანი არქიტექტურა
•	როგორ დავაპროექტო მოდელები
•	როგორ გავაკეთო CSV-ზე დაფუძნებული “ბაზა”
•	და როგორ შევკრა ყველაფერი სერვისებით


პროექტის ავტორები:
დავით შენგელია
ლადო მაისურაძე
