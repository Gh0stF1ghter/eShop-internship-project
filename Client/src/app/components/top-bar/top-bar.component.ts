import { Component, OnInit } from '@angular/core';
import { TypeService } from '../../services/type.service';
import typeModel from '../../models/typeModel';
import { AuthService } from '../../services/auth.service';
import { userRoles } from '../../constants/userRoles';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrl: './top-bar.component.css',
})
export class TopBarComponent implements OnInit {
  constructor(
    private typeService: TypeService,
    private authService: AuthService
  ) {}

  types: typeModel[] = [];
  isLogged: boolean = false;
  searchTerm: string | undefined;

  ngOnInit(): void {
    this.getTypes();
    this.isLogged = this.authService.isLogged();
  }

  getTypes() {
    this.typeService.getTypes().subscribe((types) => (this.types = types));
  }

  typesOpen = false;

  toggleTypes() {
    this.typesOpen = !this.typesOpen;
  }

  startSearch(input: string) {
    if (input.length > 3) {
      console.log(input);

      this.searchTerm = input;
    }
  }

  isAdmin() {
    return this.authService.getUserRole() === userRoles.admin
  }

  logOut() {
    this.authService.logOut();
    this.isLogged = this.authService.isLogged()
  }
}
