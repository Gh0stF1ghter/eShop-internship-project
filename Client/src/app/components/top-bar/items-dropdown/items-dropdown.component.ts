import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import item from '../../../models/itemModel';
import { ItemOfTypeService } from '../../../services/item-of-type.service';

@Component({
  selector: 'app-items-dropdown',
  templateUrl: './items-dropdown.component.html',
  styleUrl: './items-dropdown.component.css'
})
export class ItemsDropdownComponent implements OnChanges {
  items: item[] = []
  @Input() searchTerm: string | undefined

  constructor(private itemService: ItemOfTypeService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if(this.searchTerm)
      this.itemService.getItems(this.searchTerm).subscribe(response => {
        if(response.body) {
          console.log(response.body);
          
          this.items = response.body
        }
      })
  }

  getSearchItems(input: string) {
    if(this.searchTerm) {
      this.itemService.getItems(this.searchTerm)
    }
  }
}
