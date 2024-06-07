import { Component, OnInit } from '@angular/core';
import item from '../../../../models/itemModel';
import { ItemOfTypeService } from '../../../../services/item-of-type.service';

@Component({
  selector: 'app-admin-items',
  templateUrl: './admin-items.component.html',
  styleUrl: './admin-items.component.css',
})
export class AdminItemsComponent implements OnInit {
  items: item[] = [];

  constructor(private typeService: ItemOfTypeService) {}

  ngOnInit(): void {
    this.getTypes();
  }

  getTypes() {
    this.typeService.getItems().subscribe((response) => {
      if (response.body) {
        this.items = response.body;
      }
    });
  }
}
