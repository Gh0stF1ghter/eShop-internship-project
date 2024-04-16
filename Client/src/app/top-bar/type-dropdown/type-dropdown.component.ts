import { Component, OnInit, Type } from '@angular/core';
import typeModel from '../../models/typeModel';
import { TypeService } from '../../services/type.service';

@Component({
  selector: 'app-type-dropdown',
  templateUrl: './type-dropdown.component.html',
  styleUrl: './type-dropdown.component.css'
})
export class TypeDropdownComponent implements OnInit {
  types : typeModel[] = []

  constructor(private typeService: TypeService) {}

  ngOnInit(): void {
    this.getTypes()
  }

  getTypes() {
    this.typeService.getTypes()
      .subscribe(types => this.types = types)
  }
}
