# .NET Challenge

## Assignment

Simple browser-based chat application using .NET. 

It allows several users to talk in a chatroom and also to get stock quotes from an API using a specific command.


## Mandatory Features

- [x] Allow registered users to log in and talk with other users in a chatroom.
- [x] Allow users to post messages as commands into the chatroom with the following format **/stock=stock_code**.
- [x] Create a **decoupled** bot that will call an API using the stock_code as a parameter (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here `aapl.us` is the stock_code).
- [x] The bot should parse the received CSV file and then it should send a message back into the chatroom using a message broker like RabbitMQ. The message will be a stock quote using the following format: "APPL.US quote is $93.42 per share". The post owner will be the bot.
- [x] Have the chat messages ordered by their timestamps and show only the last 50 messages.
- [x] Unit test the functionality you prefer.

## Bonus (Optional)

- [ ] Have more than one chatroom.
- [x] Use .NET identity for users authentication
- [x] Handle messages that are not understood or any exceptions raised within the bot.
- [ ] Build an installer

## Technologies

- .NET 5.0
- ASP.NET Core Blazor WebAssembly
- ASP.NET Core Identity
- SignalR
- RabbitMQ
- EntityFramework Core
- SQL Server
- xUnit
- Moq

## Installation

- Install Nuget Packages
- Setup ```appsettings.json``` from ```Chat.Server``` project.
	- Change the connection strings. The EF approach is Code First, so you can just put any names for databases and they will be created.
	- Change (**if needed**) the host name or the name of the queues for RabbitMQ. *The default values are ok, but if you change them, also change from ```Bot``` project.*
- Setup ```appsettings.json``` from ```Bot``` project.
	- Change (**if needed**) the host name or the name of the queues for RabbitMQ. *The default values are ok, but if you change them, also change from ```Chat.Server``` project.*
	- Change (**if needed**) the uri of the webservice to get stock information. *The default value is ok.
- Run these two commands to create the two databases and their tables: 
	- ```Update-Database -context ChatAppContext```
	- ```Update-Database -context ApplicationDbContext```.
- Run RabbitMQ.
	- If you have it installed locally, run it on the standard port 5672.
	- If you don't have it installed locally and you have Docker, run this command:
		- ```docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.10-management```.
- Setup the solution to startup these two projects:
	- ```Chat.Server```
	- ```Bot```
- Run the solution!	

## Usage

You can use these two users that are created by default.

1. Username: ```user1@email.com```. Password: ```Password1!```.
2. Username: ```user2@email.com```. Password: ```Password2!```.

If you need more users or you don't want to use these you can create as many as you want.
Steps to create a user:
1. Go to Home page.
2. Click in Register link on the top bar.
3. Enter your information and click in Register button.
4. Click the link that emulates the email confirmation.
5. Click in Login link on the top bar.
6. Enter your new credentials.
7. Start chatting!