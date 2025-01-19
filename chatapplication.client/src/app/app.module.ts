import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; 
import { AppComponent } from './app.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { JwtInterceptor } from '@auth0/angular-jwt';
import { AppRoutingModule } from './app-routing.module';
import { ChatComponent } from './chat/chat.component';

@NgModule({
  declarations: [
    AppComponent,
    SignInComponent,
    SignUpComponent,
    ChatComponent
  ],
  imports: [
    BrowserModule, 
    HttpClientModule, 
    FormsModule, AppRoutingModule  
  ],
  providers: [
  ],  bootstrap: [AppComponent]
})
export class AppModule { }
