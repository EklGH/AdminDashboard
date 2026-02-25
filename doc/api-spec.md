### API Specification

##### Contrat Backend/Frontend









##### Configuration :



Base : URL
Auth: JWT Bearer token
Auth header: Authorization: Bearer <access\_token>
Content-Type: application/json









##### === AUTH



**POST /api/auth/login**
Authenticate a user.



Request
{
"email": "admin@example.com",
"password": "password123"
}



Response – 200 OK
{
"user": {
"email": "admin@example.com"
},
"token": "jwt-access-token"
}



Errors
401 Unauthorized – invalid credentials







**POST /api/auth/register** (admin)
{
"email": "admin@example.com",
"password": "password123"
}



Response – 201 Created
{
"email": "admin@example.com"
}







**POST /api/auth/refresh**
Request
{
"refreshToken": "refresh-token"
}



Response – 200 OK
{
"token": "new-access-token"
}







##### === USER



**Model**
User {
email: string
}







**GET /api/users**
Returns all users (admin only).



Response
\[
{ "email": "admin@example.com" }
]







##### === PRODUCTS



**Model**
Product {
id: number
name: string
category: string
price: number
stock: number
}







**GET /api/products**
Returns all products.
Response – 200 OK
\[
{
"id": 1,
"name": "Laptop",
"category": "Electronics",
"price": 1200,
"stock": 15
}
]

Query params for pagination :
?page=1\&pageSize=10

Paginated Response
{
"items": \[...],
"page": 1,
"pageSize": 10,
"totalItems": 42
}







**POST /api/products**
Create a new product.
Request
{
"name": "Desk",
"category": "Furniture",
"price": 350,
"stock": 10
}

Response – 201 Created
{
"id": 6,
"name": "Desk",
"category": "Furniture",
"price": 350,
"stock": 10
}

Note: In frontend, id = 0 means create new product.







**PUT /api/products/{id}**
Update a product.



Request
{
"id": 1,
"name": "Laptop Pro",
"category": "Electronics",
"price": 1500,
"stock": 10
}



Response – 200 OK
{
"id": 1,
"name": "Laptop Pro",
"category": "Electronics",
"price": 1500,
"stock": 10
}



Rule:

Path parameter {id} is the source of truth. body.id must match path id.







**DELETE /api/products/{id}**
Delete a product.



Response – 204 No Content









##### === RESERVATIONS



**Model**
Reservation {
id: number
customer: string
date: string // YYYY-MM-DD
status: "Confirmed" | "Pending" | "Cancelled"
}







**GET /api/reservations**
Returns all reservations.



Response
\[
{
"id": 1,
"customer": "Alice Johnson",
"date": "2026-01-15",
"status": "Confirmed"
}



Note:

Frontend currently only reads reservations.

Create/update/delete endpoints are required for future use.







**POST /api/reservations**
Create reservation.
Request
{
"customer": "John Doe",
"date": "2026-02-01",
"status": "Pending"
}







**PUT /api/reservations/{id}**
Update reservation.







**DELETE /api/reservations/{id}**
Delete reservation.









##### === PAGINATION

###### (server-side – future-proof)





Supported on:
**GET /api/products
GET /api/reservations**



Query params :
?page=1\&pageSize=10



Response
{
"items": \[...],
"page": 1,
"pageSize": 10,
"totalItems": 42
}



Rule:

If pagination params are provided, response must use paginated format.

Otherwise, return simple array.



### 



##### === FITLERING \& SORTING

###### (GraphQL priority)





GraphQL Endpoint :
**POST /graphql**



Example Query :
query {
products(
page: 1
pageSize: 5
orderBy: PRICE\_DESC
category: "Electronics"
) {
items {
id
name
price
stock
}
totalItems
}
}







##### === DOCKER



API container :

* Exposes port 8080
* Uses ASP.NET Core (.NET 9)



PostgreSQL :

* Database name: admindashboard
* User: postgres
* Password: RealPostgresPassword32characters0123456789!









##### === BACKEND ACCEPTANCE CRITERIA



* API matches frontend types exactly
* No field renaming without frontend validation
* JSON only
* Consistent HTTP status codes
* JWT required for all routes except /auth/\*









##### === NOTES FOR BACKEND



* Frontend uses mocks
* Dates are strings (YYYY-MM-DD)
* CRUD rules:
  Products: id = 0 -> create
  PUT {id} -> path id is authoritative
* Reservations CRUD endpoints exist, but frontend currently reads only
* Pagination \& sorting ready
