import { Component, Input, OnInit, Type } from '@angular/core';
import typeModel from '../../../models/typeModel';

@Component({
  selector: 'app-type-dropdown',
  templateUrl: './type-dropdown.component.html',
  styleUrl: './type-dropdown.component.css'
})
export class TypeDropdownComponent{
  @Input()types : typeModel[] = []
}
