Feature: ReplaceABook

As User,
I want to replace a book in my library
So that I can keep my collection up to date

@regression
@allure.owner:Dusan_Milic
@allure.link:https://dev.example.com/
@allure.issue:AUTH-123
@allure.tms:TMS-456
@allure.epic:BookStoreAPI
@allure.parentSuite:BookStoreAPI
@allure.suite:ReplaceABook
Scenario: Replace a book in the library
	Given User is created and authorized.
	When User send request to get all books sending GET request to "/BookStore/v1/Books". 
	Then User should receive a 200 OK and the list of books in the response
	When User add the first book from the book list from the previous response to the user's book list sending POST request to "/BookStore/v1/Books".
	Then User should receive a 201 Created and the book should be added to the user's book list.
	When User send request to get user by its id sending GET request to "/Account/v1/User/{userId}".
	Then User should receive a 200 OK and the user's book list contain only 1 book.
	And The book in the user's book list matches the "first" book title added in the previous step.
	When User replace the book from the user's book list with the second book from the book list from the previous response sending PUT request to "/BookStore/v1/Books".
	Then User should receive a 200 OK.
	When User send request to get user by its id sending GET request to "/Account/v1/User/{userId}".
	Then User should receive a 200 OK and the user's book list contain only 1 book.
	And The book in the user's book list matches the "second" book title added in the previous step.

