# Project Proposal: QuestLog

---

## 1. Project Overview

Modern gamers own games across multiple platforms: Steam, consoles, mobile devices, and digital storefronts. This fragmentation leads to "backlog paralysis," where players struggle to keep track of what games they actually own, what they want to play next, and what they have already finished.

QuestLog is a lightweight, distraction-free web application that solves this issue by acting as a single, centralized digital shelf. Our application allows users to search a massive global catalog of games and seamlessly add them to their private collection, keeping their gaming goals organized in one clean interface.

The target audience consists of casual and enthusiast gamers who want a straightforward tracker without the clutter of social media feeds or complex store integrations. What makes QuestLog valuable is its simplicity: it combines the power of a live public database with a completely private personal tracking space, allowing users to effortlessly curate their hobby.

---

## 2. Project Scope

### What's IN

* **User Accounts:** Registration, login, and secure session management.
* **Public Game Search:** Live lookup of games from a global database.
* **"My Shelf" (Personal Library):** A private dashboard containing the user's added games.
* **Status Tracking:** Categorizing games into *Wishlist*, *Backlog*, *Playing*, *Completed*, or *Shelved*.
* **Personal Evaluation:** Allowing users to rate games (1 to 5 stars) and add custom notes.

### What's OUT

* **Direct File Uploads:** Users cannot upload image files for game covers; instead, cover art is automatically pulled from the public game database.
* **Account Syncing:** No automatic syncing with Steam, PlayStation Network, or Xbox Live accounts.
* **Social Features:** No friend lists, public profile sharing, or group forums.

---

## 3. App Features & User Stories

The Trello board represents these core features as individual cards, each containing a target user story:

* **Feature 1: User Registration**
* *User Action:* Users can create a new account.
* *User Story:* *"As a gamer, I want to create a secure account so that my personal game backlog is saved privately."*
* *Requirements:* Database, User Authentication.


* **Feature 2: Secure Login**
* *User Action:* Users can log into their existing accounts.
* *User Story:* *"As a returning user, I want to log in securely so that I can access my saved game list from any of my devices."*
* *Requirements:* User Authentication.


* **Feature 3: Game Discovery (Explore Tab)**
* *User Action:* Users can search for any game using a search bar.
* *User Story:* *"As a user looking to organize a new game, I want to search a global catalog so that I don't have to type in all the game details manually."*
* *Requirements:* External API Integration (RAWG).


* **Feature 4: Add to Library**
* *User Action:* Users can click a button to add a searched game to their personal shelf.
* *User Story:* *"As a player, I want to quickly add games to my list so that I can start organizing my collection immediately."*
* *Requirements:* Database, User Authentication.


* **Feature 5: Update Game Progress & Ratings (My Shelf Tab)**
* *User Action:* Users can edit their saved games to update progress status, add a star rating, or write custom notes.
* *User Story:* *"As a gamer who just finished a game, I want to mark it as 'Completed' and give it 5 stars so that I can look back on my accomplishments."*
* *Requirements:* Database.


* **Feature 6: Remove Games**
* *User Action:* Users can delete a game from their shelf.
* *User Story:* *"As a user, I want to remove games from my shelf if I added them by mistake or lost interest."*
* *Requirements:* Database.



---

## 4. Technical Considerations

* **Data Storage:** The system saves user login profiles and custom shelf details (the game's title, platform, status, star rating, personal notes, and a web link to its cover art).
* **User Accounts:** Secure registration and login are mandatory so that each user has a private, protected experience and cannot view or edit other users' collections.
* **External Services:** The application utilizes the free **RAWG API** to search for game metadata (titles, platforms, and cover art) in real time.
* **Device Compatibility:** Built with responsive design principles, the application automatically scales to look clean and work smoothly on smartphones, tablets, and desktop monitors.
* **Basic Security:** To protect user privacy, password data is securely hashed, and strict permissions ensure a user can only access, edit, or delete records associated with their own account.

---

## 5. Project Links

* **GitHub Repository:** [https://github.com/santiagopintus/cse325-team-project](https://www.google.com/search?q=https://github.com/santiagopintus/cse325-team-project)
* **Trello Board:** [https://trello.com/b/wXK7fJZf/cse325-team-project](https://trello.com/b/wXK7fJZf/cse325-team-project)