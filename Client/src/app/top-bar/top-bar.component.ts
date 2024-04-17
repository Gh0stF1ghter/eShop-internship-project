import { Component, OnInit } from '@angular/core';
import { TypeService } from '../services/type.service';
import typeModel from '../models/typeModel';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrl: './top-bar.component.css',
})
export class TopBarComponent implements OnInit {
  constructor(private typeService: TypeService) {}

  types: typeModel[] = [];
  
  ngOnInit(): void {
    this.getTypes();
  }

  getTypes() {
    this.typeService.getTypes().subscribe((types) => (this.types = types));
  }

  typesOpen = false;

  toggleTypes() {
    this.typesOpen = !this.typesOpen;
  }
}
