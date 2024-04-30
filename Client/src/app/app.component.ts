import { Component, OnInit } from '@angular/core';
import { SignalrService } from './services/signalr.service';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'Client';

  constructor(
    private signalRService: SignalrService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    let userId = this.authService.getUserId();
    if (userId) {
      this.signalRService.startConnection(userId);
      this.signalRService.addNotificationListener()
    } 
  }
}
