# Real-time Chat Application

## Overview
This document provides the technical documentation for the real-time chat application designed as part of the Sanlam Studios technical assessment. The application facilitates communication between multiple users with real-time message broadcasting and persistence.

---

## Solution Description
### Brief Description
The real-time chat application is built using a .NET Web API backend and a simple web-based client interface. Users can send and receive messages in real time, with chat history persisting across sessions.  Basic auth functionality with sign-up and sign-in is used for simplicity.

### Features
- Real-time message broadcasting to all connected clients.
- Persistent message storage to ensure chat history visibility.
- Web-based interface for sending and receiving messages.

### Technical Choices and Rationale
- **Backend**: .NET Web API (Version: .NET 8)
  - Chosen for its robustness and compatibility with modern development standards.
- **Frontend**: Web-based client using Angular 16
  - Simplifies implementation while providing a user-friendly interface.
- **Database**: SQL Server
  - Reliable and scalable relational database suitable for storing chat history.
- **Real-time Communication**: SignalR
  - Provides a seamless mechanism for real-time communication in .NET applications.
- **Source Control**: GitHub
  - Ensures version control and collaboration.

### Assumptions and Trade-offs
- **Assumption**: All users are anonymous, and user identification is session-based.
- **Trade-offs**:
  - Limited authentication to focus on core functionality.
  - Basic frontend design for quick implementation.

---

## Setup and Installation Instructions
### Prerequisites
- .NET SDK 8 or later
- Node.js and npm
- SQL Server
- Git

### Steps
1. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   cd chat-application
   ```
2. **Backend Setup**:
   - Navigate to the backend folder:
     ```bash
     cd backend
     ```
   - Configure the database connection string in `appsettings.json`.
   - Apply migrations:
     ```bash
     dotnet ef database update --project ChatApplication.Server
     ```
   - Run the backend:
     ```bash
     dotnet run
     ```
3. **Frontend Setup**:
   - Navigate to the frontend folder:
     ```bash
     cd chatapplication.client
     ```
   - Install dependencies:
     ```bash
     npm install
     ```
   - Start the frontend:
     ```bash
     ng serve
     ```
4. Open the application in your browser at `http://localhost:4200`.

---

## Technical Documentation

### System Architecture Diagram
<img width="460" alt="image" src="https://github.com/user-attachments/assets/c6e9e3ef-9527-47d0-b1b9-8de804d6d8ab" />


### Data Flow Diagram
1. User sends a message via the frontend.
2. Message is sent to the backend via SignalR.
3. Backend broadcasts the message to all connected clients.
4. Message is persisted in the PostgreSQL database.
5. On new client connections, chat history is fetched from the database.

### Database Schema Diagram
<img width="290" alt="image" src="https://github.com/user-attachments/assets/7d46625f-7c53-4105-94bd-bca2939a431a" />



### Component Diagram
<img width="362" alt="image" src="https://github.com/user-attachments/assets/f4c175fc-aba5-40f9-abee-e67b3c7c9d97" />


### Sequence Diagram

#### Basic Auth
<img width="307" alt="image" src="https://github.com/user-attachments/assets/942afecd-6b2a-43f2-a5e4-7693d9732253" />

#### Chat
<img width="400" alt="image" src="https://github.com/user-attachments/assets/d3cf1d58-57c6-4a18-b904-8236b48f17f7" />


---

## Known Limitations and Future Improvements
### Limitations
- No user authentication beyond anonymous sessions.
- Basic UI design.
- Limited error handling.

### Potential Improvements
- Add user authentication and profiles.
- Enhance UI with modern design principles.
- Introduce unit and integration tests.
- Implement rate limiting to prevent message spamming.

---

## Deployment Considerations


---

## What Would Be Done Differently in Production
- Implement solid Authentication including all necessary endpoints for seamless user management.
- Implement Unit Tests both on Backend and Frontend.
- Add logging and monitoring with tools like Serilog and Azure Monitor.
- Containerize the application using Docker for consistent deployments.
- Optimize real-time performance using WebSockets over SignalR if necessary.

