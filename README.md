# TradeHarbor

[![License](https://img.shields.io/badge/License-CC%20BY--NC%204.0-lightgrey.svg)](https://creativecommons.org/licenses/by-nc/4.0/)
[![Angular](https://img.shields.io/badge/Angular-15.0.0-green.svg)](https://angular.io/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Database](https://img.shields.io/badge/Database-MSSQL-blue.svg)](https://www.microsoft.com/en-us/sql-server)

Welcome to TradeHarbor, your go-to social hub for stock option enthusiasts and traders! StockCollab is a dynamic platform designed for individuals passionate about stock options, where traders and investors can come together to share insights, create comments, and collaborate on their trades.

## Table of Contents
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Screenshots](#screenshots)
- [Future Features](#future-features)

## Technologies Used

- Angular 10.0.0
- ASP.NET Core 8.0
- MSSQL
- Entity Framework 8.0
- Angular Material 15.0.0

## Features

1. **Trade Posts Creation:**
   - Users can create posts to share information about stock option trades.
   - Include details in their comments such as trade strategies, entry/exit points, and reasoning behind the trades.

2. **Comments and Feedback:**
   - Users can engage in discussions by adding comments to trade posts.
   - Ability to upvote or downvote posts to indicate agreement or disagreement with the trade analysis.

3. **User Interaction:**
   - Users can add friends within the platform.
   - Accept or decline friend requests from other users.
   - Build a network of like-minded traders to enhance collaboration.

4. **Notifications:**
   - Receive notifications for new comments and friend requests.
   - Stay informed about interactions and activities related to your account.

5. **Authentication:**
   - Secure API calls to the backend using JSON Web Token (JWT) authentication.
   - Protect user data and ensure the integrity of interactions with the server.

6. **Database Integration:**
   - MSSQL database for efficient data storage and retrieval.
   - Custom-designed stored procedures to handle database operations.

## Screenshots

Standard home screen:
<img src="/Files/tradeharbor-fe-4.JPG" width="1000"/>

Friends dialog on the friend request tab:
<img src="/Files/tradeharbor-fe-5.jpg" width="1000"/>

User interacting with comments:
<img src="/Files/tradeharbor-fe-6.jpg" width="1000"/>

## Future Features
- Trade posts need to include the option expiration date.
- Modifications to allow strikeprice to be stored in decimal. The database allows it but data transfers rounds it.
