import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignUpComponent } from './sign-up/sign-up.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { ChatComponent } from './chat/chat.component';
import { AuthGuard } from './services/auth.guard';

const routes: Routes = [
  { 
    path: '', 
    redirectTo: '/sign-in', 
    pathMatch: 'full' 
  },
  { 
    path: 'sign-in', 
    component:  SignInComponent 
  },
  { 
    path: 'sign-up', 
    component:  SignUpComponent 
  },
  { 
    path: 'chat', 
    component: ChatComponent,
    canActivate: [AuthGuard] 
  },
  { 
    path: '**', 
    redirectTo: '/sign-in' 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
