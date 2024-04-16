import { Component } from '@angular/core';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrl: './top-bar.component.css'
})
export class TopBarComponent {
    typesOpen = false;

    toggleTypes() {
      this.typesOpen = !this.typesOpen;
    }
}