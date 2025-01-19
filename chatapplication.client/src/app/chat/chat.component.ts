import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { AuthService } from '../services/auth.service';
import { ChatService } from '../services/chat.service';

export interface Message {
  id: string;
  senderId: string;
  receiverId: string;
  content: string;
  timestamp: string; // Use `Date` if you want to parse it into a JavaScript Date object
}


export interface User {
  userId: string;
  userName: string;
  email: string;
  isOnline: boolean;
  lastSeen?: Date;
}


@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  private hubConnection!: HubConnection;
  messages: Message[] = [];
  users: User[] = [];
  currentMessage: string = '';
  currentUser: string;
  loading = true;
  selectedUser: any = null; // Tracks the selected user


  constructor(
    private chatService: ChatService,
    private authService: AuthService,
    private changeDetectorRef: ChangeDetectorRef
  ) {
    this.currentUser = this.authService.getCurrentUser();
  }

  async ngOnInit() {
    await this.initializeSignalRConnection();
    this.loadUsers();
    this.currentUser = this.authService.getCurrentUser();

  }

  async selectUser(user: User) {
    if (this.hubConnection.state !== 'Connected') {
      console.log('Connection State:', this.hubConnection.state);
      console.error('SignalR connection is not active.');
      return;
    }
  
    this.selectedUser = user;
  
    // Add listener for receiving messages
    this.hubConnection.on("LoadMessages", (messages: any) => {  // Note: Match the method name
      console.log("Received messages from SignalR:", messages);
      this.messages = messages;
      // Trigger change detection if needed
      this.changeDetectorRef.detectChanges();
    });
  
    try {
      console.log('Invoking LoadMessages with:', this.currentUser, user.userId);
      await this.hubConnection.invoke('LoadMessages', this.currentUser, user.userId);
      console.log('LoadMessages invoked successfully');
    } catch (err) {
      console.error('Error invoking LoadMessages:', err);
    }
  }
  
  

  ngOnDestroy() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  private async initializeSignalRConnection() {
    this.hubConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:7070/chatHub') // Explicitly specify the backend URL
    .withAutomaticReconnect()
      .build();

  // Add listeners
  this.setupSignalRListeners();


    try {
      await this.hubConnection.start();
      this.loading = false;
    } catch (err) {
      console.error('Error while starting SignalR connection:', err);
    }
  }

  private async loadUsers() {
    try {
      this.users = await this.chatService.getUsers().toPromise();
    } catch (err) {
      console.error('Error loading users:', err);
    }
  }

  logout() {
    console.log('User logged out');
    this.authService.logout();
  }

  private setupSignalRListeners() {
    // Listener for individual messages
    this.hubConnection.on('ReceiveMessage', (message: Message) => {
      console.log('Individual message received:', message);
      this.messages.push(message);
      this.changeDetectorRef.detectChanges();
    });

    // Listener for loading message history
    this.hubConnection.on('LoadMessages', (messages: Message[]) => {
      console.log('Message history received:', messages);
      this.messages = messages;
      this.changeDetectorRef.detectChanges();
    });

    // Add connection state logging
    this.hubConnection.onreconnecting((error) => {
      console.log('SignalR Reconnecting...', error);
    });

    this.hubConnection.onreconnected((connectionId) => {
      console.log('SignalR Reconnected with ID:', connectionId);
    });

    this.hubConnection.onclose((error) => {
      console.log('SignalR Connection closed:', error);
    });
  }

  async sendMessage() {
    if (!this.currentMessage.trim()) return;
    console.log('CurrentUser:', this.currentUser, typeof this.currentUser);
    console.log('SelectedUser:', this.selectedUser, typeof this.selectedUser);
    console.log('CurrentMessage:', this.currentMessage, typeof this.currentMessage);
    try {
      await this.hubConnection.invoke('SendMessage', this.currentUser, this.selectedUser.userId, this.currentMessage);
      this.currentMessage = '';
    } catch (err) {
      console.error('Error sending message:', err);
    }
  }
}